using Loyufei.Character;
using Loyufei;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame.Character
{
    [CreateAssetMenu(fileName = "StandStill", menuName = "Fighting Game/State/Enemy/StandStill", order = 1)]
    public class StandStill : CharacterVisualStateAsset
    {
        [SerializeField]
        private float _Interval;

        public Timer    Timer { get; protected set; }
        public IReposit Stand { get; protected set; }

        public override void Setup(CharacterStateMachine stateMachine)
        {
            base.Setup(stateMachine);

            Timer = new(_Interval, false);

            Stand = stateMachine.GetReposit("StandStill");
        }

        public override void OnEnter()
        {
            base.OnEnter();

            Timer.Start();
        }

        public override void Tick()
        {
            base.Tick();

            Stand.Preserve(Timer.PassTime);
        }

        public override void OnExit()
        {
            base.OnExit();

            Timer.Stop();
            Timer.Reset();
        }
    }
}
