using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.Character;
using Loyufei;

namespace FightingGame.Character
{
    [CreateAssetMenu(fileName = "IsSprint", menuName = "Fighting Game/State/SubCondition/IsSprint", order = 1)]
    public class IsSprintCondition : SubConditionAsset
    {
        [SerializeField]
        private bool _IsSprint;

        public IReposit Sprint { get; protected set; }

        public override void Setup(CharacterStateMachine stateMachine)
        {
            Sprint = stateMachine.GetReposit("Sprint");
        }

        public override bool Condition()
        {
            return Equals(Sprint.Data, _IsSprint);
        }
    }
}