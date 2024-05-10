using FightingGame.PlayerControl;
using Loyufei;
using Loyufei.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame.Character
{
    [CreateAssetMenu(fileName = "Chasing", menuName = "Fighting Game/State/Enemy/Chasing", order = 1)]
    public class Chasing : CharacterVisualStateAsset
    {
        public float           ScaleX      { get; protected set; }
        public IReposit<float> SprintSpeed { get; protected set; }
        public IReposit<float> Horizontal  { get; protected set; }
        public Rigidbody2D     Rigidbody   { get; protected set; }
        public IEnemyFacade    Enemy       { get; protected set; }

        public override void Setup(CharacterStateMachine stateMachine)
        {
            base.Setup(stateMachine);

            ScaleX      = Mathf.Abs(stateMachine.Root.localScale.x);
            SprintSpeed = stateMachine.CalculateStats.SearchAt("SprintSpeed").To<IReposit<float>>();
            Horizontal  = stateMachine.GetReposit("Move").To<IReposit<float>>();
            Rigidbody   = stateMachine.Facade.To<ICharacter2DFacade>().Rigidbody;
            Enemy       = Facade.To<IEnemyFacade>();
        }

        public override void Tick()
        {
            var hori = Mathf.Clamp(Enemy.Target.position.x - Root.position.x, -1f, 1f);

            Horizontal.Preserve(hori);

            Rigidbody.FixedFlip(Horizontal.Data, 0.1f, ScaleX, true);

            Rigidbody.SetFixedHorizontalVelocity2D(Horizontal.Data, SprintSpeed.Data);
        }

        public override void OnExit()
        {
            Rigidbody.velocity = Vector2.zero;
        }
    }
}