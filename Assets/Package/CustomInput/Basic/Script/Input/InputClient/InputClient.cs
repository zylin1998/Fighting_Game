using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.InputSystem
{
    public class InputClient : MonoBehaviour
    {
        private void Awake()
        {
            Platform = Application.platform;
        }

        public static IInputList InputList { get; private set; }
        public static IAxesList AxesList { get; private set; }
        public static IInputClient Basic { get; private set; }
        public static RuntimePlatform Platform { get; private set; }

        public static UICtrlService UICtrl { get; private set; }
        
        public bool Pause => Current == null;

        private static IInputClient current;

        public static IInputClient Current 
        {
            get 
            {
                if (current == null) { current = Basic; }

                return current;
            }
        }

        private void Update()
        {
            if (!Pause) { Current.GetValue(); }
        }

        #region Client Event
        
        public static void SetBasic(IInputClient client) 
        {
            Basic = client;
        }

        public static void SetCurrent(IInputClient client) 
        {
            current = client;
        }

        public static void SetCurrent(Navigation.INaviCollect client)
        {
            if (UICtrl == null) { UICtrl = new UICtrlService(); }

            UICtrl.SetCurrent(client);
        }

        public static void ClearCurrent() 
        {
            current = null;
        }

        public static void SetPlatform(RuntimePlatform platform) 
        {
            Platform = platform;
        }

        public static void SetInput(IInputList inputList) 
        {
            InputList = inputList;
            AxesList = inputList[Platform];
        }

        public static void Clear() 
        {
            Basic = null;
            current = null;
        }

        #endregion

        #region GetValue

        public static TAxes GetAxes<TAxes>(string name) where TAxes : IAxes
        {
            if (AxesList == null) { return default(TAxes); }

            var axes = AxesList[name];

            return axes is TAxes result ? result : default(TAxes);
        }

        public static float GetAxis(string name) 
        {
            if (AxesList == null) { return 0; }

            return AxesList[name].Value.Axis;
        }

        public static bool GetKeyDown(string name)
        {
            if (AxesList == null) { return false; }

            return AxesList[name].Value.GetKeyDown;
        }

        public static bool GetKey(string name) 
        {
            if (AxesList == null) { return false; }

            return AxesList[name].Value.GetKey;
        }

        public static bool GetKeyUp(string name)
        {
            if (AxesList == null) { return false; }

            return AxesList[name].Value.GetKeyUp;
        }

        #endregion

        public void OnDestroy()
        {
            Basic = null;
            current = null;
            AxesList = null;
            InputList = null;
            Platform = RuntimePlatform.WindowsEditor;
            UICtrl = null;

            Singleton<InputClient>.RemoveClient();
        }

        #region UICtrlService

        public class UICtrlService : IInputClient
        {
            public float HoldMax { get; private set; }
            public float HoldMin { get; private set; }
            public float Hold { get; private set; }
            public float HoldTime { get; private set; }

            public UICtrlService() 
            {
                this.HoldMax = 0.5f;
                this.HoldMin = 0.1f;
                this.Hold = this.HoldMax;
                this.HoldTime = 0f;
            }

            #region IInputClient

            public void GetValue()
            {
                var x = (int)GetAxis("Horizontal");
                var y = (int)GetAxis("Vertical");

                this.Move(new Vector2Int(x, y));

                if (GetKeyDown("Attack"))
                {
                    this.Current.Final.Select();
                }

                if (GetKeyDown("Jump")) 
                {
                    this.Current.ExitCallBack?.Invoke();
                }
            }

            #endregion

            public void Move(Vector2Int direct) 
            {
                if (direct != Vector2.zero)
                {
                    if (this.HoldTime == 0f)
                    {
                        Current.FindNaviCtrl(direct);
                    }

                    this.HoldTime += Time.deltaTime;

                    if (HoldTime >= Hold)
                    {
                        Hold = this.HoldMin;
                        this.HoldTime = 0f;
                    }
                }

                if (direct == Vector2.zero)
                {
                    this.Hold = this.HoldMax;
                    this.HoldTime = 0f;
                }
            }

            public Navigation.INaviCollect Current { get; private set; }

            public void SetCurrent(Navigation.INaviCollect client)
            {
                this.Current = client;
                InputClient.SetCurrent(this);
            }
        }

        #endregion
    }
}