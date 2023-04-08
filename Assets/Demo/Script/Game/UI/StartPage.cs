using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Custom;
using Custom.Page;
using Custom.InputSystem;

namespace FightingGameDemo
{
    public class StartPage : MonoBehaviour, IInputClient
    {
        [SerializeField]
        private Button _StartButton;

        public Button StartButton => this._StartButton;

        private void Start()
        {
            this._StartButton.onClick.AddListener(() =>
            {
                this.StartEvent();
                PageClient.OpenPage("Demo", "Detail");
                InputClient.ClearCurrent();
            });

            InputClient.SetCurrent(this);
        }

        public void StartEvent()
        {
            BeginClient.Begin();
        }

        #region IInputClient

        public void GetValue() 
        {
            if (InputClient.GetKeyDown("Attack")) 
            {
                this.StartButton.onClick.Invoke();
            }
        }

        #endregion
    }
}