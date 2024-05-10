using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.Character
{
    public class CharacterVisualStateAsset : VisualStateAsset, ISetup<CharacterStateMachine>
    {
        [SerializeField]
        protected string _AnimationStateName;

        public ICharacterFacade Facade { get; protected set; }
        public Transform        Root   { get; protected set; }

        #region IState

        public override void OnEnter()
        {
            Facade?.Animator?.Play(_AnimationStateName);
        }

        public override void OnExit()
        {

        }

        public override void Tick()
        {

        }

        #endregion

        #region ISetup

        public virtual void Setup(CharacterStateMachine stateMachine)
        {
            Facade = stateMachine.Facade;
            Root   = stateMachine.Root;
        }

        public override void Setup(params object[] args)
        {
            Setup(args[0].To<CharacterStateMachine>());
        }

        #endregion
    }
}