using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.Character;
using Loyufei;

namespace FightingGame.Character
{
    [CreateAssetMenu(fileName = "Animation State", menuName = "Fighting Game/State/SubCondition/Animation State", order = 1)]
    public class AnimStateSubConditionInfo : SubConditionAsset
    {
        [SerializeField, Range(0, 1f)]
        private float _TurnState;

        public Animator Animator { get; protected set; }

        public override void Setup(CharacterStateMachine stateMachine)
        {
            Animator = stateMachine.Facade.Animator;
        }

        public override bool Condition()
        {
            return Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= _TurnState;
        }
    }
}
