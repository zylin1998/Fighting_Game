using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei;
using Loyufei.Character;

namespace FightingGame.PlayerControl
{
    public interface IAttackFacade : IFacade
    {
        public float AwakeTime { get; }
        public float SleepTime { get; }
    }

    [CreateAssetMenu(fileName = "Attack", menuName = "Fighting Game/State/Player/Attack", order = 1)]
    public class Attack : CharacterVisualStateAsset
    {
        [SerializeField]
        private GameObject _Prefab;

        private GameObject    _Instance;
        private IAttackFacade _AttackFacade;

        public override void Setup(CharacterStateMachine stateMachine)
        {
            base.Setup(stateMachine);

            _AttackFacade = stateMachine.Container
                .InstantiatePrefabForComponent<IAttackFacade>(_Prefab, Root);
            _Instance     = _AttackFacade.To<Component>().gameObject;
            _Instance
                .SetActive(false);

            Debug.Assert(!_AttackFacade.IsDefault());
        }

        public override void Tick()
        {
            var animState = Facade.Animator.GetCurrentAnimatorStateInfo(0);

            if (!animState.IsName(_AnimationStateName)) { return; }

            var normalizeTime = animState.normalizedTime;
            var awake = normalizeTime >= _AttackFacade.AwakeTime && normalizeTime <= _AttackFacade.SleepTime;

            if (awake != _Instance.activeSelf)
            {
                _Instance.SetActive(awake);
            }
        }
    } 
}