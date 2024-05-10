using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Loyufei.Character
{
    [CreateAssetMenu(fileName = "Character States", menuName = "Loyufei/Character/State Machine", order = 1)]
    public class CharacterStateMachine : 
        VisualStateMachineAsset, 
        ISetup<ICharacterFacade, CalculateStats>, 
        ITickable, 
        IFixedTickable
    {
        public Transform        Root           { get; protected set; }
        public ICharacterFacade Facade         { get; protected set; }
        public CalculateStats   CalculateStats { get; protected set; }

        public void Setup(ICharacterFacade facade, CalculateStats stats)
        {
            Root           = facade.To<Component>()?.transform;
            Facade         = facade;
            CalculateStats = stats;

            base.Setup(facade, stats);
        }

        public override void Setup(params object[] args)
        {
            if (args.Length < 2) { return; }
            
            var facade = args[0].To<ICharacterFacade>();
            var stats  = args[1].To<CalculateStats>();

            Setup(facade, stats);
        }

        public void Tick()
        {
            this.Transfer();
        }

        public void FixedTick()
        {
            StateMachineExtensions.Tick(this);
        }
    }
}