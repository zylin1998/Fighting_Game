using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei
{
    public abstract class VisualStateAsset : ScriptableObject, IIdentity, IState, ISetup
    {
        [SerializeField]
        protected string _Identity;

        public object Identity => _Identity;

        public abstract void OnEnter();

        public abstract void OnExit();

        public abstract void Tick();

        public abstract void Setup(params object[] args);
    }
}