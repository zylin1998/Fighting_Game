using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.DomainEvents;
using Zenject;

namespace Loyufei.Character
{
    using EntityForm = IEntityForm<GameObject, FacadeEntity>;

    public class CharacterFacadeHandler
    {
        public CharacterFacadeHandler(
            DiContainer           container,
            EntityForm            entityForm,
            DomainEventService    service, 
            object                group = null)
        {
            Group     = group;
            Entities  = entityForm;
            Container = container;
            Facades   = new();
            Pools     = new();

            EventRegister(service);
        }

        public object                            Group     { get; }
        public EntityForm                        Entities  { get; }
        public DiContainer                       Container { get; }
        public Dictionary<int, ICharacterFacade> Facades   { get; }
        public Dictionary<object, FacadePool>    Pools     { get; }

        protected void EventRegister(DomainEventService service) 
        {
            service.Register<CharacterFacadeSpawn>  (this.Spawn  , Group);
            service.Register<CharacterFacadeDespawn>(this.Despawn, Group);
        }
        
        public FacadePool AddPool(GameObject prefab)
        {
            var facade = prefab.GetComponent<ICharacterFacade>();

            Debug.Assert(facade != null);

            var type  = facade.GetType();
            var hash  = Guid.NewGuid().GetHashCode();

            Container
                .BindMemoryPool<ICharacterFacade, FacadePool>()
                .WithId(hash)
                .To(type)
                .FromComponentInNewPrefab(prefab);

            return Container.ResolveId<FacadePool>(hash);
        }

        public void Despawn(Mark mark) 
        {
            var hash = mark.GuidHash;
            
            if (Facades.TryGetValue(hash, out var facade))
            {
                Pools[mark.CharacterID].Despawn(facade);

                Facades.Remove(hash);
            }
        }

        public void Spawn(Mark mark, Transform parent, Vector3 position, Quaternion rotation) 
        {
            var pool      = Pools.GetorAdd(mark.CharacterID, () => AddPool(Entities[mark.CharacterID].Data));
            var facade    = pool.Spawn(mark);
            var hash      = mark.GuidHash;
            var component = facade.To<Component>();
            
            facade.Mark = mark;
            
            if (component) 
            {
                component.gameObject.layer = mark.LayerMask;
                component.transform.SetParent(parent);
                component.transform.SetPositionAndRotation(position, rotation);
            }

            Facades.Add(hash, facade);
        }

        public class FacadePool : MemoryPool<Mark, ICharacterFacade>
        {
            protected override void OnDespawned(ICharacterFacade facade)
            {
                var component = facade.To<Component>();

                facade.Mark = default;

                component.transform.position = Vector3.zero;
                component.gameObject.SetActive(false);
                component.gameObject.layer = 0;
            }

            protected override void Reinitialize(Mark mark, ICharacterFacade facade)
            {
                facade.Mark = mark;

                var component = facade.To<Component>();

                component.gameObject.SetActive(true);
            }
        }
    }
}