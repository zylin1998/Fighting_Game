using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using Loyufei.DomainEvents;
using UnityEngine;

namespace Loyufei.Character
{
    public class CharacterService : AggregateRoot
    {
        public CharacterService(DiContainer container, DomainEventService service) : base(service)
        {
            Container = container;
            Pools     = new();
        }
        
        public DiContainer                                 Container { get; }
        public Dictionary<object, CharacterControllerPool> Pools     { get; }

        public virtual TController CreateController<TController>(
            Mark                             mark,
            CharacterFacadeSpawn.SpawnOption spawnOption
            ) where TController : ICharacterController 
        {
            var controller = GetPool<TController>().Spawn().To<TController>();

            controller.Mark = mark;

            FacadeCreate(mark, spawnOption);
            StatsCreate(mark);
            StateCreate(mark, controller);

            return controller;
        }

        public virtual void ReleaseController<TController>(TController controller) 
            where TController : ICharacterController
        {
            var mark = controller.Mark;

            FacadeRelease(mark);
            StatsRelease(mark);
            StateRelease(mark);

            GetPool<TController>().Despawn(controller);
        }

        public virtual void FacadeCreate(Mark mark, CharacterFacadeSpawn.SpawnOption spawnOption) 
        {
            this.SettleEvents(mark.Group, new CharacterFacadeSpawn(mark, spawnOption));
        }

        public virtual void FacadeRelease(Mark mark)
        {
            this.SettleEvents(mark.Group, new CharacterFacadeDespawn(mark));
        }

        public virtual void StatsCreate(Mark mark)
        {
            this.SettleEvents(mark.Group, new CharacterStatsCreate(mark));
        }

        public virtual void StatsRelease(Mark mark)
        {
            this.SettleEvents(mark.Group, new CharacterStatsRelease(mark));
        }

        public virtual void StateCreate(Mark mark, ICharacterController controller)
        {
            this.SettleEvents(mark.Group, new CharacterStateCreate(mark, controller));
        }

        public virtual void StateRelease(Mark mark)
        {
            this.SettleEvents(mark.Group, new CharacterStateRelease(mark));
        }

        public virtual void CalculatStatIncrease(Mark mark, StatVariable variable, Action<VariableResponse> onResponse) 
        {
            this.SettleEvents(mark.Group, new CalculateStatIncrease(mark, variable, onResponse));
        }
        
        public virtual void CalculatStatDecrease(Mark mark, StatVariable variable, Action<VariableResponse> onResponse)
        {
            this.SettleEvents(mark.Group, new CalculateStatDecrease(mark, variable, onResponse));
        }

        public virtual void StandardStatIncrease(Mark mark, StatVariable variable, Action<VariableResponse> onResponse)
        {
            this.SettleEvents(mark.Group, new StandardStatIncrease(mark, variable, onResponse));
        }

        public virtual void StandardStatDecrease(Mark mark, StatVariable variable, Action<VariableResponse> onResponse)
        {
            this.SettleEvents(mark.Group, new StandardStatDecrease(mark, variable, onResponse));
        }

        public virtual void StatFetch(Mark mark, StatVariable variable, Action<VariableResponse> onResponse)
        {
            this.SettleEvents(mark.Group, new CharacterStatFetch(mark, variable, onResponse));
        }

        #region Binding

        protected virtual CharacterControllerPool GetPool<TController>() where TController : ICharacterController
        {
            return Pools.GetorAdd(typeof(TController), BindingPool<TController>);
        }

        protected virtual CharacterControllerPool BindingPool<TController>() where TController : ICharacterController 
        {
            var id = (object)typeof(TController);

            Container
                .BindMemoryPool<ICharacterController, CharacterControllerPool>()
                .WithId(id)
                .To<TController>();

            return Container.ResolveId<CharacterControllerPool>(id);
        }

        #endregion
    }
}