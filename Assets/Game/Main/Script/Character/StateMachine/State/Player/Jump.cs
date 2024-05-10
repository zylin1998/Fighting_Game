using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei;
using Loyufei.Character;
 
namespace FightingGame.PlayerControl
{
    [CreateAssetMenu(fileName = "Jump", menuName = "Fighting Game/State/Player/Jump", order = 1)]
    public class Jump : CharacterVisualStateAsset
    {
        public float           ScaleX { get; protected set; }
        public IReposit<float> MoveSpeed { get; protected set; }
        public IReposit<float> JumpForce { get; protected set; }
        public IReposit<float> Horizontal { get; protected set; }
        public Rigidbody2D     Rigidbody { get; protected set; }

        public override void Setup(CharacterStateMachine stateMachine)
        {
            base.Setup(stateMachine);

            ScaleX     = Mathf.Abs(stateMachine.Root.localScale.x);
            MoveSpeed  = stateMachine.CalculateStats.SearchAt("MoveSpeed").To<IReposit<float>>();
            JumpForce  = stateMachine.CalculateStats.SearchAt("JumpForce").To<IReposit<float>>();
            Horizontal = stateMachine.GetReposit("Move").To<IReposit<float>>();
            Rigidbody  = Facade.To<ICharacter2DFacade>().Rigidbody;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            Rigidbody.AddForce(new Vector2(0, JumpForce.Data), ForceMode2D.Impulse);
        }

        public override void Tick()
        {
            Rigidbody.FixedFlip(Horizontal.Data, 0.1f, ScaleX, true);

            Rigidbody.SetFixedHorizontalVelocity2D(Horizontal.Data, MoveSpeed.Data);
        }
    }
}
