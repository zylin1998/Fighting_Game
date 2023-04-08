using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.InputSystem
{
    [CreateAssetMenu(fileName = "JoyStick Axes", menuName = "Custum Input/Input Axes/JoyStick Axes", order = 1)]
    public class JoyStickAxes : InputAxes, IJoyStickAxes
    {
        [SerializeField]
        private IJoyStickAxes.EAxis _AxisType;
        [SerializeField]
        private float _Degree;

        #region IJoyStickAxes

        public IJoyStickAxes.EAxis AxisType => this._AxisType;
        public float Degree => this._Degree;

        public IVJoyStick JoyStick { get; private set; }

        public void SetJoyStick(IVJoyStick joyStick) 
        {
            this.JoyStick = joyStick;
        }

        #endregion

        #region IAxes

        private float _Axis;

        public override float Axis 
        {
            get 
            {
                if (!this.JoyStick.IsOnDrag) { return 0; }

                if (this._AxisType == IJoyStickAxes.EAxis.Vertical) 
                {
                    if (Mathf.Abs(this.JoyStick.Angle - 90f) <= this._Degree) { this._Axis = 1f; }
                    else if (Mathf.Abs(this.JoyStick.Angle + 90f) <= this._Degree) { this._Axis = -1f; }
                    else { this._Axis = 0; }
                }
                if (this._AxisType == IJoyStickAxes.EAxis.Horizontal) 
                {
                    if (Mathf.Abs(this.JoyStick.Angle - 0f) <= this._Degree) { this._Axis = 1f; }
                    else if (180f - Mathf.Abs(this.JoyStick.Angle) <= this._Degree) { this._Axis = -1f; }
                    else { this._Axis = 0; }
                }

                return this._Axis;
            }
        }

        public override bool GetKeyDown => false;
        public override bool GetKey => false;
        public override bool GetKeyUp => false;
        
        #endregion
    }
}