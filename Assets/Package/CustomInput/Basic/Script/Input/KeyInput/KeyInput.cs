using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.InputSystem
{
    [CreateAssetMenu(fileName = "Key Axes", menuName = "Custum Input/Input Axes/Key Axes", order = 1)]
    public class KeyInput : InputAxes, IKeyAxes
    {
        [SerializeField]
        private KeyCode _Positive;
        [SerializeField]
        private KeyCode _Negative;

        #region IAxes

        private float _Axis;

        public override float Axis
        {
            get
            {
                if (this._Axis == 0 && Input.GetKey(this.Positive)) { this._Axis = 1f; }
                if (this._Axis == 1 && !Input.GetKey(this.Positive)) { this._Axis = 0f; }

                if (this._Axis == 0 && Input.GetKey(this.Negative)) { this._Axis = -1f; }
                if (this._Axis == -1 && !Input.GetKey(this.Negative)) { this._Axis = 0f; }

                return this._Axis;
            }
        }

        public override bool GetKeyDown => Input.GetKeyDown(this.Positive);
        public override bool GetKey => Input.GetKey(this.Positive);
        public override bool GetKeyUp => Input.GetKeyUp(this.Positive);
        
        #endregion

        #region IKeyAxes

        public KeyCode Positive => this._Positive;
        public KeyCode Negative => this._Negative;

        public void SetAxes(IKeyAxes.EAxesState axesState, KeyCode keyCode)
        {
            if (axesState == IKeyAxes.EAxesState.Positive) { this._Positive = keyCode; }

            if (axesState == IKeyAxes.EAxesState.Negative) { this._Negative = keyCode; }
        }

        #endregion
    }
}