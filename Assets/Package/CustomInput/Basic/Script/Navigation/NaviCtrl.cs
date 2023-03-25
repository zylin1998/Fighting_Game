using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Custom.InputSystem;

namespace Custom.Navigation
{
    public class NaviCtrl : MonoBehaviour, INaviCtrl
    {
        [SerializeField]
        private Selectable _Selectable;
        [SerializeField]
        private Vector2Int _ID;
        [SerializeField]
        private INaviCollect.EEdgeType _EdgeType;

        private void Awake()
        {
            this._Selectable = this.GetComponent<Selectable>();
        }

        #region INaviCtrl

        public Selectable Selectable => this._Selectable;
        public Vector2Int ID { get => this._ID; private set => this._ID = value; }
        public INaviCollect.EEdgeType EdgeType { get => this._EdgeType; set => this._EdgeType = value; }
        public bool OnEdge => this._EdgeType != INaviCollect.EEdgeType.None;

        public INaviCtrl FindNavi(Vector2 direct)
        {
            Selectable selectable = null;

            if (direct.x == 1) { selectable = this.Selectable.FindSelectableOnRight(); }
            if (direct.x == -1) { selectable = this.Selectable.FindSelectableOnLeft(); }
            if (direct.y == 1) { selectable = this.Selectable.FindSelectableOnUp(); }
            if (direct.y == -1) { selectable = this.Selectable.FindSelectableOnDown(); }
            
            return selectable?.GetComponent<INaviCtrl>();
        }

        public void SetID(Vector2Int id)
        {
            this.ID = id;
        }

        public void Select()
        {
            this.Selectable?.Select();
        }

        #endregion
    }
}