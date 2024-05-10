using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.Character;
using Loyufei;

namespace FightingGame.Character
{
    [CreateAssetMenu(fileName = "Move", menuName = "Fighting Game/State/SubCondition/Move", order = 1)]
    public class MoveSubCondition : SubConditionAsset
    {
        [SerializeField]
        private bool _IsZero;

        public IReposit Move { get; protected set; }

        public override void Setup(CharacterStateMachine stateMachine)
        {
            Move = stateMachine.GetReposit("Move");
        }

        public override bool Condition()
        {
            var isZero = Equals(Move.Data, 0f);

            return Equals(_IsZero, isZero);
        }
    }
}
