using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Custom;
using Custom.Page;
using Custom.Events;
using Custom.InputSystem;

namespace FightingGameDemo  
{
    public class DemoEndPage : MonoBehaviour, IBeginClient, IInputClient
    {
        [SerializeField]
        private Text _Result;
        [SerializeField]
        private Text _Score;
        [SerializeField]
        private Text _MaxCombo;
        [SerializeField]
        private Text _PassTime;
        [SerializeField]
        private Text _Reward;

        private IGameDetail detail;
        private DemoBattleRule rule;

        private void Awake()
        {
            BeginClient.AddBegin(this);
        }

        public void ResultUpdate() 
        {
            InputClient.SetCurrent(this);

            if (this.detail != null) 
            {
                PageClient.OpenPage("Demo", "End");

                var result = string.Empty;
                var score = this.rule.Current;
                var combo = this.detail.GetAction<ComboAction>().Combo;
                var passTime = this.detail.GetAction<PassTimeAction>().GameTime.GetTime("{1}:{2, 4:F2}");
                var reward = 0f;

                if (rule.Fulfilled()) 
                {
                    result = "Win";

                    reward = (this.detail.Reward as DemoReward).Exp;
                }

                if (rule.Defeated()) { result = "Lose"; }

                this._Result.text = string.Format("{0}", result);
                this._Score.text = string.Format("Score: {0}", score);
                this._MaxCombo.text = string.Format("Combo: {0}", combo);
                this._PassTime.text = string.Format("PassTime: {0}", passTime);
                this._Reward.text = string.Format("Reward: {0}Exp", reward);
            }
        }

        public void BeforeBegin() 
        {
            if (EventManager.Detail is DemoBattleDetail detail)
            {
                this.detail = detail;

                EventManager.AddEvent("Battle End", (variable) => this.ResultUpdate());

                if (this.detail.Rule is DemoBattleRule rule)
                {
                    this.rule = rule;
                }
            }
            
            this.gameObject.SetActive(false);
        }

        public void BeginAction() 
        {
            
            
        }

        public void GetValue() 
        {

        }

        private void OnDestroy()
        {
            EventManager.RemoveEvent("Battle End", (role) => this.ResultUpdate());
        }
    }
}