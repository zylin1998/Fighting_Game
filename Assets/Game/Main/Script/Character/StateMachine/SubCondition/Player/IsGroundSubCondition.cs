using Loyufei;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.Character;

namespace FightingGame.Character 
{
    [CreateAssetMenu(fileName = "Is Ground", menuName = "Fighting Game/State/SubCondition/Is Ground", order = 1)]
    public class IsGroundSubCondition : SubConditionAsset
    {
        [SerializeField]
        private bool _IsGround;

        public Rigidbody2D Rigidbody { get; protected set; }

        protected LayerMask _GroundMask;

        public override void Setup(CharacterStateMachine stateMachine)
        {
            _GroundMask = LayerMask.NameToLayer("Ground");
            
            Rigidbody = stateMachine.Facade.To<ICharacter2DFacade>().Rigidbody;
        }

        public override bool Condition()
        {
            Rigidbody.GroundCheck(0.01f, _GroundMask, out var isGround);

            return isGround && _IsGround;
        }
    }
}