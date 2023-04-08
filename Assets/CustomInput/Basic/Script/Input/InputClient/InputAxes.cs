using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.InputSystem
{
    public abstract class InputAxes : ScriptableObject, IAxes
    {
        [SerializeField]
        protected string _Name;

        public string AxesName => this._Name;

        public abstract float Axis { get; }
        public abstract bool GetKeyDown { get; }
        public abstract bool GetKey { get; }
        public abstract bool GetKeyUp { get; }
        public AxesValue Value => new AxesValue(this);
    }
}
