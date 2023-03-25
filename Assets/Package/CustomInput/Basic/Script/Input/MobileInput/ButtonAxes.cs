using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.InputSystem
{
    [CreateAssetMenu(fileName = "Button Axes", menuName = "Custum Input/Input Axes/Button Axes", order = 1)]
    public class ButtonAxes : InputAxes, IButtonAxes
    {
        public IMobileButton MobileButton { get; private set; }

        public void SetButton(IMobileButton mobileButton) 
        {
            this.MobileButton = mobileButton;
        }

        public override float Axis => 0f;
        public override bool GetKeyDown => this.MobileButton.GetKeyDown;
        public override bool GetKey => this.MobileButton.GetKey;
        public override bool GetKeyUp => this.MobileButton.GetKeyUp;
    }
}