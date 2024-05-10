using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using Loyufei.DomainEvents;

namespace Loyufei.Character
{
    using EntityForm = IEntityForm<CharacterStateMachine, Entity<string, CharacterStateMachine>>;

    public class CharacterStateHandler
    {
        public CharacterStateHandler(
            DiContainer                  container,
            EntityForm                   stateSetting,
            DomainEventService           service,
            List<CharacterFacadeHandler> facadeHandlers,
            List<CharacterStatsHandler>  statsHandlers,
            object                       group = null)
        {
            StateMachines = new();

            Group         = group;
            Container     = container;
            StateSettings = stateSetting;
            StatsHandler  = statsHandlers
                .FirstOrDefault(h => Equals(h.Group, Group));
            FacadeHandler = facadeHandlers
                .FirstOrDefault(h => Equals(h.Group, Group));
            
            TickableManager = container.Resolve<TickableManager>();
            
            EventRegister(service);
        }

        public object                 Group           { get; }
        public DiContainer            Container       { get; }
        public TickableManager        TickableManager { get; }
        public EntityForm             StateSettings   { get; }
        public CharacterFacadeHandler FacadeHandler   { get; }
        public CharacterStatsHandler  StatsHandler    { get; }

        public Dictionary<int, CharacterStateMachine> StateMachines { get; }

        protected virtual void EventRegister(DomainEventService service)
        {
            service.Register<CharacterStateCreate> (this.CharacterStatesCreate , Group);
            service.Register<CharacterStateRelease>(this.CharacterStatesRelease, Group);
        }

        public void CharacterStatesCreate(int guidHash, object identity, IEnumerable<RepositBinding> bindings)
        {
            var visualStates = StateSettings[identity]
                .Data;
            var stateMachine = Container.TryResolveorBindId(visualStates);
            
            var facade       = FacadeHandler.Facades.GetorReturn(guidHash, () => default);
            var stats        = StatsHandler[guidHash];
            
            stateMachine.Setup(facade, stats);

            stateMachine.Binding(bindings);
            
            BindToContainer(stateMachine);

            StateMachines.Add(guidHash, stateMachine);
        }

        public void CharacterStatesRelease(int guidHash)
        {
            if (StateMachines.TryGetValue(guidHash, out var stateMachine))
            {
                UnbindFromContainer(stateMachine);

                StateMachines.Remove(guidHash);
            }
        }

        public void BindToContainer(CharacterStateMachine stateMachine) 
        {
            TickableManager.Add(stateMachine);
            TickableManager.AddFixed(stateMachine);
        }

        public void UnbindFromContainer(CharacterStateMachine stateMachine)
        {
            TickableManager.Remove(stateMachine);
            TickableManager.RemoveFixed(stateMachine);
        }
    }
}
