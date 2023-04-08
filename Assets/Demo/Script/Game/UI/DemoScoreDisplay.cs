using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Custom;
using Custom.Events;

namespace FightingGameDemo
{
    public class DemoScoreDisplay : MonoBehaviour, IBeginClient
    {
        [SerializeField]
        private Text _Score;

        private string format = "{0, 3}/{1, -3}";

        private void Awake()
        {
            BeginClient.AddBegin(this);
        }

        public void ScoreUpdate() 
        {
            if (EventManager.Detail.Rule is DemoBattleRule rule) 
            {
                this._Score.text = string.Format(this.format, rule.Current, rule.Score);
            }
        }

        #region IBeginAction

        public void BeforeBegin() 
        {
            EventManager.AddEvent("Enemy Slaved", (role) => this.ScoreUpdate());

            this._Score.text = string.Format(this.format, 0, 0);
        }

        public void BeginAction()
        {
            this.ScoreUpdate();
        }

        #endregion

        private void OnDestroy()
        {
            EventManager.RemoveEvent("Enemy Slaved", (role) => this.ScoreUpdate());
        }
    }
}