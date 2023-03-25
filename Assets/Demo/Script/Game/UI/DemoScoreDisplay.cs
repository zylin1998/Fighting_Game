using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Custom;
using Custom.Battle;

namespace FightingGameDemo
{
    public class DemoScoreDisplay : MonoBehaviour, IBeginClient
    {
        [SerializeField]
        private Text _Score;

        private string format = "{0, 3}/{1, 3}";

        private void Awake()
        {
            BeginClient.AddBegin(this);
        }

        public void ScoreUpdate() 
        {
            if (BattleManager.Detail.Rule is DemoBattleRule rule) 
            {
                this._Score.text = string.Format(this.format, rule.Current, rule.Score);
            }
        }

        #region IBeginAction

        public void BeforeBegin() 
        {
            BattleManager.AddEvent(BattleManager.BattleEvent.EEventType.EnemySlaved, (role) => this.ScoreUpdate());
        }

        public void BeginAction()
        {
            this.ScoreUpdate();
        }

        #endregion
    }
}