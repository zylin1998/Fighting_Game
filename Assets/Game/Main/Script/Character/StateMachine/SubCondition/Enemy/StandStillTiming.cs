using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.Character;
using Loyufei;

namespace FightingGame.Character
{
    [CreateAssetMenu(fileName = "StandStillTiming", menuName = "Fighting Game/State/SubCondition/StandStillTiming", order = 1)]
    public class StandStillTiming : SubConditionAsset
    {
        [SerializeField]
        private float _StandStillTime;

        public IReposit StandStill { get; protected set; }

        public override void Setup(CharacterStateMachine stateMachine)
        {
            StandStill = stateMachine.GetReposit("StandStill");
        }

        public override bool Condition()
        {
            return (float)StandStill.Data >= _StandStillTime;
        }
    }
}