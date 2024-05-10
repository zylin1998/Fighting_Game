using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.Character;
using Loyufei;

namespace FightingGame.Character
{
    [CreateAssetMenu(fileName = "Attack", menuName = "Fighting Game/State/SubCondition/Attack", order = 1)]
    public class AttackSubCondition : SubConditionAsset
    {
        public IReposit Attack { get; protected set; }

        public override void Setup(CharacterStateMachine stateMachine)
        {
            Attack = stateMachine.GetReposit("Attack");
        }

        public override bool Condition()
        {
            return Attack.Data.Equals(true);
        }
    }
}
