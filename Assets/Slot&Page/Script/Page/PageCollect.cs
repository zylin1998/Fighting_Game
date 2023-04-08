using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Page
{
    public class PageCollect : MonoBehaviour, IPageCollect, IUIState, IBeginClient
    {
        [SerializeField]
        private string _GroupName;
        [SerializeField]
        private List<PageState> _Pages = new List<PageState>();

        public string GroupName => this._GroupName;
        public IPageState Current => this._Pages.Find(p => p.IsOpen);
        public IEnumerable<IPageState> Pages => this._Pages;

        private void Awake()
        {
            PageClient.AddGroup(this);
            BeginClient.AddBegin(this);
        }

        public void Add(IPageState page)
        {
            this._Pages.Add(page as PageState);
        }

        public bool Remove(IPageState page)
        {
            return this._Pages.Remove(page as PageState);
        }

        public void OpenPage(string page)
        {
            this.UIState(true);
            this._Pages.ForEach(p => p.State.UIState(p.PageName == page));
        }

        public void CloseAll()
        {
            this._Pages.ForEach(p => p.State.UIState(false));
            this.UIState(false);
        }

        public void OnDestroy()
        {
            PageClient.RemoveGroup(this.GroupName);
        }

        public void UIState(bool state)
        {
            this.gameObject.SetActive(state);
        }

        public void UIState(bool state, System.Action onClose)
        {
            if (!state) { onClose?.Invoke(); }
            this.UIState(state);
        }

        #region IBeginClient

        public void BeforeBegin()
        {
            this.UIState(false);
        }

        public void BeginAction()
        {

        }

        #endregion
    }
}