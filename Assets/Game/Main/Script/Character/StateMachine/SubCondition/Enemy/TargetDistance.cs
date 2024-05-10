using Loyufei.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei;

namespace FightingGame.Character
{
    public class TargetDistance : SubConditionAsset
    {
        [SerializeField]
        private float _Distance;

        public IEnemyFacade Enemy    { get; protected set; }
        public Transform    Root     { get; protected set; }

        public override bool Condition()
        {
            return Mathf.Abs(Enemy.Target.position.x - Root.position.x) <= _Distance;
        }

        public override void Setup(CharacterStateMachine stateMachine)
        {
            Enemy = stateMachine.Facade.To<IEnemyFacade>();
        }
    }
}