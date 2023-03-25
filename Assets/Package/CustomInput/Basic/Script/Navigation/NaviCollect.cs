using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Custom.InputSystem;

namespace Custom.Navigation
{
    public class NaviCollect : MonoBehaviour, INaviCollect
    {

        [SerializeField]
        private INaviCollect.NavigationSetting _Navigation;
        [SerializeField]
        private List<NaviCtrl> _Selectables;

        public INaviCollect.NavigationSetting Navigation => this._Navigation;
        public IEnumerable<INaviCtrl> Selectables => this._Selectables;

        private INaviCtrl _Final;

        public INaviCtrl First => this._Selectables.FirstOrDefault();
        public INaviCtrl Final { get => this._Final; private set => this._Final = value; }
        public INaviCtrl this[int x, int y] => this[new Vector2(x, y)];
        public INaviCtrl this[Vector2 id] => this._Selectables.Find(f => f.ID == id);

        public System.Action ExitCallBack { get; set; }

        public virtual void GetSelectables() 
        {
            this._Selectables = new List<NaviCtrl>(this.GetComponentsInChildren<NaviCtrl>());
        }

        public virtual void GetSelectables(System.Predicate<INaviCtrl> compare)
        {
            this._Selectables = new List<NaviCtrl>(this.GetComponentsInChildren<NaviCtrl>().Where(n => compare.Invoke(n)));
        }

        public virtual void SetNavigation() 
        {
            this._Navigation.Setting(this._Selectables);
        }

        public virtual void Select(INaviCollect.ESelectType selectType)
        {
            InputClient.SetCurrent(this);

            var select = selectType == INaviCollect.ESelectType.First || this.Final == null ? this.First : this.Final;

            select.Select();

            this.Final = select;
        }

        public virtual void FindNaviCtrl(Vector2Int direct) 
        {
            this.Navigation.FindNaviCtrl(ref this._Final, direct);
        }
    }
}
