using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.Character;
using System.Linq;
using Loyufei;

namespace FightingGame.Character
{
    [CreateAssetMenu(fileName = "FoundTarget", menuName = "Fighting Game/State/SubCondition/FoundTarget", order = 1)]
    public class FoundTarget : SubConditionAsset
    {
        [SerializeField]
        private float _SearchRadius;
        [SerializeField]
        private bool  _IsFound;

        public Transform    Root   { get; protected set; }
        public Mark         Mark   { get; protected set; }
        public IEnemyFacade Facade { get; protected set; }

        public override bool Condition()
        {
            var result = false;

            foreach (var collider in Physics2D.OverlapCircleAll(Root.position, _SearchRadius))
            {
                var mark = collider.GetComponent<ICharacterFacade>()?.Mark;

                if (mark.IsDefault()) { continue; }

                result = Equals(Mark.LayerMask, mark.LayerMask);

                if (result)
                {
                    Facade.Target = collider.transform;

                    break;
                }
            }

            return result && _IsFound;
        }

        public override void Setup(CharacterStateMachine stateMachine)
        {
            Root   = stateMachine.Root;
            Mark   = stateMachine.Facade.Mark;
            Facade = stateMachine.Facade.To<IEnemyFacade>();
        }
    }
}