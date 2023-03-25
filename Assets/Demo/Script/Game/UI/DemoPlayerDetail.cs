using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Custom;
using Custom.Role;
using Custom.Battle;

namespace FightingGameDemo
{
    public class DemoPlayerDetail : MonoBehaviour, IBeginClient
    {
        [SerializeField]
        private Text _Level;
        [SerializeField]
        private Image _HPBar;
        [SerializeField]
        private Image _MPBar;

        private void Awake()
        {
            BeginClient.AddBegin(this);
        }

        public void BarUpdate(Image bar, RoleBasic role) 
        {
            if (role.Health is Health health) 
            {
                this._HPBar.fillAmount = health.NormalizeHP;
            }
        }

        public void LevelUpdate(RoleBasic role) 
        {
            this._Level.text = string.Format("Lv.{0, 3}", role.Level.Level);
        }

        #region IBeginClient

        public void BeforeBegin() 
        {
            BattleManager.AddEvent(BattleManager.BattleEvent.EEventType.PlayerHurt, (role) => this.BarUpdate(this._HPBar, role));
            BattleManager.AddEvent(BattleManager.BattleEvent.EEventType.PlayerUpgrade, (role) => this.LevelUpdate(role));
        }

        public void BeginAction() { }

        #endregion
    }
}