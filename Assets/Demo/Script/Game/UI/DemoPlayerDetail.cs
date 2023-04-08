using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Custom;
using Custom.Role;
using Custom.Events;

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
            this._HPBar.fillAmount = role.RoleProperty["Health"]["HP"].Normalized;
        }

        public void LevelUpdate(RoleBasic role) 
        {
            this._Level.text = string.Format("Lv.{0, 3}", role.RoleProperty["Level"]["Level"].Value);
        }

        #region IBeginClient

        public void BeforeBegin() 
        {
            EventManager.AddEvent("Ally Hurt", (variable) => 
            {
                if (variable is PropertyVariable health)
                {
                    this.BarUpdate(this._HPBar, health.To);
                }
            });
            EventManager.AddEvent("Ally Upgrade", (variable) =>
            {
                if (variable is PropertyVariable health)
                {
                    this.LevelUpdate(health.To);
                }
            });

            this._Level.text = string.Format("Lv.{0, 3}", 1);
        }

        public void BeginAction() 
        {
            LevelUpdate(DemoBattleRule.Player);
        }

        #endregion

    }
}