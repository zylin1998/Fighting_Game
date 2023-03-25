using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Custom.Navigation
{
    public interface INaviCtrl 
    {
        public Selectable Selectable { get; }
        public Vector2Int ID { get; }
        public INaviCollect.EEdgeType EdgeType { get; set; }
        public bool OnEdge { get; }

        public INaviCtrl FindNavi(Vector2 direct);
        public void SetID(Vector2Int id);
        public void Select();
    }

    public interface INaviCollect
    {
        [Serializable, Flags]
        public enum ENaviType
        {
            None = 0,
            Horizontal = 1,
            Vertical = 2,
            Automatic = 3
        }

        [Serializable]
        public enum EAxis
        {
            Horizontal = 0,
            Vertical = 1
        }

        [Serializable, Flags]
        public enum EEdgeType
        {
            None = 0,
            Top = 1,
            Buttom = 2,
            Left = 4,
            Right = 8
        }

        [Serializable]
        public enum ESelectType
        {
            First = 0,
            Final = 1,
        }

        [Serializable]
        public struct NaviArea
        {
            [SerializeField]
            private NaviCollect _Up;
            [SerializeField]
            private NaviCollect _Down;
            [SerializeField]
            private NaviCollect _Left;
            [SerializeField]
            private NaviCollect _Right;

            public INaviCollect Up => this._Up;
            public INaviCollect Down => this._Down;
            public INaviCollect Left => this._Left;
            public INaviCollect Right => this._Right;

            public INaviCollect GetNavi(Vector2Int direct)
            {
                INaviCollect navi = null;

                if (direct.y == 1) { navi = this._Up; }
                if (direct.y == -1) { navi = this._Down; }
                if (direct.x == -1) { navi = this._Left; }
                if (direct.x == 1) { navi = this._Right; }

                return navi;
            }
        }

        [Serializable]
        public class NavigationSetting
        {
            [SerializeField]
            private ENaviType _NaviType;
            [SerializeField]
            private EAxis _StartAxis;
            [SerializeField]
            private Vector2Int _Range;
            [SerializeField]
            private NaviArea _Navigation;
            [SerializeField]
            private bool _WrapAround;

            public ENaviType NaviType => this._NaviType;
            public EAxis StartAxis => this._StartAxis;
            public NaviArea Navigation => this._Navigation;
            public Vector2Int Range => this._Range;
            public bool WrapAround => this._WrapAround;

            #region Navigation Setting

            public void Setting(IEnumerable<INaviCtrl> navis)
            {
                var list = new List<INaviCtrl>(navis);

                if (this._NaviType == ENaviType.Horizontal) { this._Range = new Vector2Int(list.Count, 0); }
                if (this._NaviType == ENaviType.Vertical) { this._Range = new Vector2Int(0, list.Count); }

                SetID(list);
                SetNavi(list);

                if (!WrapAround) { SetEdgeType(list); }
            }

            private void SetID(List<INaviCtrl> navis)
            {
                int i = this._StartAxis == EAxis.Horizontal ? this._Range.y : this._Range.x;
                int j = this._StartAxis == EAxis.Horizontal ? this._Range.x : this._Range.y;

                for (int y = 0; y < i; y++)
                {
                    for (int x = 0; x < j; x++)
                    {
                        var n = navis[y * j + x];

                        if (this._StartAxis == EAxis.Horizontal) { n?.SetID(new Vector2Int(x, y)); }
                        if (this._StartAxis == EAxis.Vertical) { n?.SetID(new Vector2Int(y, x)); }
                    }
                }
            }

            private void SetNavi(List<INaviCtrl> navis)
            {
                navis.ForEach(s =>
                {
                    if (s is INaviCtrl n)
                    {
                        var id = n.ID;
                        var navi = new UnityEngine.UI.Navigation();

                        navi.mode = UnityEngine.UI.Navigation.Mode.Explicit;

                        if (id.y - 1 >= 0) { navi.selectOnUp = navis.Find(n => n.ID == id - Vector2Int.up).Selectable; }
                        if (id.y + 1 < this._Range.y) { navi.selectOnDown = navis.Find(n => n.ID == id - Vector2Int.down).Selectable; }
                        if (id.x - 1 >= 0) { navi.selectOnLeft = navis.Find(n => n.ID == id + Vector2Int.left).Selectable; }
                        if (id.x + 1 < this._Range.x) { navi.selectOnRight = navis.Find(n => n.ID == id + Vector2Int.right).Selectable; }

                        if (WrapAround)
                        {
                            if (this._NaviType.HasFlag(ENaviType.Vertical))
                            {
                                if (id.y == 0) { navi.selectOnUp = navis.Find(n => n.ID == new Vector2(id.x, this._Range.y - 1)).Selectable; }
                                if (id.y == this._Range.y - 1) { navi.selectOnDown = navis.Find(n => n.ID == new Vector2(id.x, 0)).Selectable; }
                            }
                            if (this._NaviType.HasFlag(ENaviType.Horizontal))
                            {
                                if (id.x == 0) { navi.selectOnLeft = navis.Find(n => n.ID == new Vector2(this._Range.x - 1, id.y)).Selectable; }
                                if (id.x == this._Range.x - 1) { navi.selectOnRight = navis.Find(n => n.ID == new Vector2(0, id.y)).Selectable; }
                            }
                        }

                        n.Selectable.navigation = navi;
                    }
                });
            }

            private void SetEdgeType(List<INaviCtrl> navis)
            {
                navis.ForEach(s =>
                {
                    var id = s.ID;
                    var edge = INaviCollect.EEdgeType.None;

                    if (id.x == 0) { edge += System.Convert.ToInt32(EEdgeType.Left); }
                    if (id.x == this._Range.x - 1) { edge += System.Convert.ToInt32(EEdgeType.Right); }
                    if (id.y == 0) { edge += System.Convert.ToInt32(EEdgeType.Top); }
                    if (id.y == this._Range.y - 1) { edge += System.Convert.ToInt32(EEdgeType.Buttom); }

                    s.EdgeType = edge;
                });
            }

            #endregion

            public void FindNaviCtrl(ref INaviCtrl navi, Vector2Int direct)
            {
                bool HasFlag(ref INaviCtrl n, EEdgeType e) => n.EdgeType.HasFlag(e);

                if (navi.OnEdge && !WrapAround)
                {
                    if (direct.y == 1 && HasFlag(ref navi, EEdgeType.Top)) { this.Navigation.Up?.Select(ESelectType.Final); return; }
                    if (direct.y == -1 && HasFlag(ref navi, EEdgeType.Buttom)) { this.Navigation.Down?.Select(ESelectType.Final); return; }
                    if (direct.x == 1 && HasFlag(ref navi, EEdgeType.Right)) { this.Navigation.Right?.Select(ESelectType.Final); return; }
                    if (direct.x == -1 && HasFlag(ref navi, EEdgeType.Left)) { this.Navigation.Left?.Select(ESelectType.Final); return; }
                }

                var next = navi?.FindNavi(direct);

                if (next != null)
                {
                    navi = next;
                    navi.Select();
                }
            }
        }

        public NavigationSetting Navigation { get; }
        public IEnumerable<INaviCtrl> Selectables { get; }

        public INaviCtrl First { get; }
        public INaviCtrl Final { get; }
        public INaviCtrl this[int x, int y] { get; }
        public INaviCtrl this[Vector2 id] { get; }

        public Action ExitCallBack { get; set; }

        public void GetSelectables();
        public void GetSelectables(Predicate<INaviCtrl> compare);
        public void SetNavigation();
        public void Select(ESelectType selectType);
        public void FindNaviCtrl(Vector2Int direct);
    }

    public interface IEventCollect
    {
        public EventTrigger EventTrigger { get; }

        public void AddEvent(EventTriggerType eventType, UnityAction<BaseEventData> call) 
        {
            var triggers = this.EventTrigger.triggers;

            bool hasTrigger = triggers.Exists(e => e.eventID == eventType);

            var entry = hasTrigger ? triggers.Find(e => e.eventID == eventType) : new EventTrigger.Entry();
            entry.eventID = eventType;
            entry.callback.AddListener(call);

            if (!hasTrigger) { triggers.Add(entry); }
        }

        public void Click(PointerEventData data) 
        {
            this.EventTrigger.OnPointerClick(data);
        }
    }
}
