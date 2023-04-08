using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Page
{
    public class PageState : MonoBehaviour, IPageState, IUIState, IBeginClient
    {
        [SerializeField]
        private string _PageName;
        [SerializeField]
        private string _GroupName;
        [SerializeField]
        private bool _InitState;

        public string PageName => this._PageName;
        public string GroupName => this._GroupName;
        public bool InitState => this._InitState;
        public bool IsOpen => this.gameObject.activeSelf;

        public IUIState State => this;
        
        private void Awake()
        {
            BeginClient.AddBegin(this);    
        }

        #region IUIState

        public void UIState(bool state)
        {
            this.gameObject.SetActive(state);
        }

        public void UIState(bool state, System.Action onClose)
        {
            onClose.Invoke();

            this.UIState(state);
        }

        #endregion

        #region IBeginClient

        public void BeforeBegin() 
        {
            PageClient.AddPage(this);

            this.UIState(this._InitState);
        }

        public void BeginAction() 
        {

        }

        #endregion
    }
}