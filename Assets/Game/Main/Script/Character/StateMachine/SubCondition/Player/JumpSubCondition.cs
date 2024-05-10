using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.Character;
using Loyufei;

namespace FightingGame.Character
{
    [CreateAssetMenu(fileName = "Jump", menuName = "Fighting Game/State/SubCondition/Jump", order = 1)]
    public class JumpSubCondition : SubConditionAsset
    {
        public IReposit Jump { get; protected set; }

        public override void Setup(CharacterStateMachine stateMachine)
        {
            Jump = stateMachine.GetReposit("Jump");
        }

        public override bool Condition()
        {
            return Jump.Data.Equals(true);
        }
    }
}
