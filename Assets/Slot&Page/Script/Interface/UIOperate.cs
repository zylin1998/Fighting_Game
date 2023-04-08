using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Page
{
    public interface IUIUpdate
    {
        public void UpdateUI();

        public void UpdateUI(bool refresh);
    }

    public interface IUIState
    {
        public void UIState(bool state);

        public void UIState(bool state, System.Action onClose);
    }

    public interface IPageState
    {
        public string PageName { get; }
        public string GroupName { get; }
        public bool InitState { get; }
        public bool IsOpen { get; }

        public IUIState State { get; }
    }

    public interface IPageCollect
    {
        public string GroupName { get; }
        public IPageState Current { get; }
        public IEnumerable<IPageState> Pages { get; }

        public void Add(IPageState page);
        public bool Remove(IPageState page);
        public void OpenPage(string page);
        public void CloseAll();
    }
}