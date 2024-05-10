using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei;
using Loyufei.Character;

namespace FightingGame.PlayerControl
{
    [CreateAssetMenu(fileName = "Sprint", menuName = "Fighting Game/State/Player/Sprint", order = 1)]
    public class Sprint : CharacterVisualStateAsset
    {
        public float           ScaleX      { get; protected set; }
        public IReposit<float> SprintSpeed { get; protected set; }
        public IReposit<float> Horizontal  { get; protected set; }
        public Rigidbody2D     Rigidbody   { get; protected set; }

        public override void Setup(CharacterStateMachine stateMachine)
        {
            base.Setup(stateMachine);

            ScaleX      = Mathf.Abs(stateMachine.Root.localScale.x);
            SprintSpeed = stateMachine.CalculateStats.SearchAt("SprintSpeed").To<IReposit<float>>();
            Horizontal  = stateMachine.GetReposit("Move").To<IReposit<float>>();
            Rigidbody   = stateMachine.Facade.To<ICharacter2DFacade>().Rigidbody;
        }

        public override void Tick()
        {
            Rigidbody.FixedFlip(Horizontal.Data, 0.1f, ScaleX, true);

            Rigidbody.SetFixedHorizontalVelocity2D(Horizontal.Data, SprintSpeed.Data);
        }

        public override void OnExit()
        {
            Rigidbody.velocity = Vector2.zero;
        }
    }
}