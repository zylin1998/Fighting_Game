using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Custom.InputSystem
{
    public struct AxesValue
    {
        public string AxesName { get; private set; }
        public float Axis { get; private set; }
        public bool GetKeyDown { get; private set; }
        public bool GetKey { get; private set; }
        public bool GetKeyUp { get; private set; }

        public AxesValue(IAxes axes)
        {
            this.AxesName = axes.AxesName;
            this.Axis = axes.Axis;
            this.GetKeyDown = axes.GetKeyDown;
            this.GetKey = axes.GetKey;
            this.GetKeyUp = axes.GetKeyUp;
        }
    }

    public interface IButtonAxes
    {
        public IMobileButton MobileButton { get; }

        public void SetButton(IMobileButton mobileButton);
    }

    public interface IMobileButton 
    {
        public string AxesName { get; }
        public Navigation.IEventCollect EventCollect { get; }
        public bool GetKeyDown { get; }
        public bool GetKey { get; }
        public bool GetKeyUp { get; }

        public void Initialize();
    }

    public interface IVJoyStick 
    {
        public float Angle { get; }
        public bool IsOnDrag { get; }

        public void OnDrag(PointerEventData eventData);
    }

    public interface IJoyStickAxes 
    {
        [System.Serializable]
        public enum EAxis
        {
            Horizontal,
            Vertical
        }

        public EAxis AxisType { get; }
        public float Degree { get; }

        public void SetJoyStick(IVJoyStick joyStick);
    }

    public interface IKeyAxes 
    {
        [System.Serializable]
        public enum EAxesState
        {
            Positive = 0,
            Negative = 1
        }

        public KeyCode Positive { get; }
        public KeyCode Negative { get; }

        public void SetAxes(EAxesState axesState, KeyCode keyCode);
    }

    public interface IAxes
    {
        public string AxesName { get; }
        
        public float Axis { get; }
        public bool GetKeyDown { get; }
        public bool GetKey { get; }
        public bool GetKeyUp { get; }
        public AxesValue Value { get; }
    }

    public interface IAxesList 
    {
        public List<RuntimePlatform> Platform { get; }
        public IEnumerable<IAxes> Axes { get; }
        public IAxes this[string axes] { get; }
        public bool CheckPlatform => this.Platform.Contains(Application.platform);
    }

    public interface IInputList 
    {
        public IEnumerable<IAxesList> List { get; }
        public IAxesList this[RuntimePlatform platform] { get; }
    }

    public interface IInputClient 
    {
        public bool IsCurrent => InputClient.Current == this;

        public void GetValue();
    }
}