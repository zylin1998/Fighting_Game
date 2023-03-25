using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Custom;
using Custom.Battle;

namespace FightingGameDemo
{
    public class DemoComboDisplay : MonoBehaviour, IBeginClient
    {
        [SerializeField]
        private Text _Combo;

        private DemoBattleDetail detail;

        private void Awake()
        {
            BeginClient.AddBegin(this);
        }

        public void ComboUpdate(int combo) 
        {
            this.gameObject.SetActive(true);

            this._Combo.text = string.Format("{0}", combo);
        }

        #region IBeginClient

        public void BeforeBegin()
        {
            if (BattleManager.Detail is DemoBattleDetail detail)
            {
                this.detail = detail;
                this.detail.OnComboChange += (combo) => this.ComboUpdate(combo);
                this.detail.ComboTimer.EndCallBack += (time) => this.gameObject.SetActive(false);
            }
        }

        public void BeginAction() 
        {
            this.gameObject.SetActive(false);
        }

        #endregion
    }
}