using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Custom;
using Custom.Events;

namespace FightingGameDemo
{
    public class DemoComboDisplay : MonoBehaviour, IBeginClient
    {
        [SerializeField]
        private Text _Combo;

        private IGameDetail detail;

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
            if (EventManager.Detail is DemoBattleDetail detail)
            {
                this.detail = detail;
                var combo = this.detail.GetAction<ComboAction>();

                combo.OnComboChange += (combo) => this.ComboUpdate(combo);
                combo.Timer.EndCallBack += (time) => this.gameObject.SetActive(false);
            }

            this.gameObject.SetActive(false);
        }

        public void BeginAction() 
        {
            
        }

        #endregion
    }
}