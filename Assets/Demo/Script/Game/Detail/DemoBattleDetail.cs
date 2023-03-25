using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom;
using Custom.Role;
using Custom.Battle;

namespace FightingGameDemo
{
    [CreateAssetMenu(fileName = "Battle Detail", menuName = "Game/Battle/Battle Detail", order = 1)]
    public class DemoBattleDetail : BattleDetail
    {
        [SerializeField]
        private float _SpawnTime;
        [SerializeField]
        private TimeClient.CountDownTimer _ComboTimer;
        [SerializeField]
        private int _Combo;

        public int Combo => this._Combo;
        public TimeClient.CountDownTimer ComboTimer => this._ComboTimer;
        public System.Action<int> OnComboChange { get; set; }

        public void Initialize() 
        {
            this._Combo = 0;
            this._Rule.Reset();

            var player = BattleManager.Allies[0];
            var enemyHurt = BattleManager.BattleEvent.EEventType.EnemyHurt;
            BattleManager.AddEvent(enemyHurt, (role) => 
            {
                this._Combo++;
                this._ComboTimer.Refresh();
                this._ComboTimer.Start();
                this.OnComboChange?.Invoke(this._Combo);
            });
            BattleManager.SetCamera(player.transform);
            BattleManager.EventInvoke(BattleManager.BattleEvent.EEventType.PlayerHurt, player);
            BattleManager.EventInvoke(BattleManager.BattleEvent.EEventType.PlayerUpgrade, player);

            this._ComboTimer.EndCallBack += (time) => 
            { 
                this._Combo = 0;
            };
        }

        public override IEnumerator GameLoop()
        {
            this.Initialize();
            
            var list = this._Enemies;
            var enemies = BattleManager.Enemies;

            while (true)
            {
                var normal = list.Where(p => p.EnemyType == IBattleDetail.EEnemyType.Normal).ToList();
                var count = normal.Count;
                var enemy = normal[Random.Range(0, count)];

                var onScene = enemies.Count(e => !e.IsDead);

                var fulfilled = this._Rule.Fulfilled();
                var defeated = this._Rule.Defeated();

                if (this._Rule is DemoBattleRule rule) 
                {
                    var amount = rule.Score - rule.Current;
                    
                    if (amount > onScene) { BattleManager.SetEnemy(enemy); }
                }
                
                if (fulfilled || defeated) 
                {
                    break;
                }

                yield return new WaitForSeconds(this._SpawnTime);
            }

            var eventType = BattleManager.BattleEvent.EEventType.BattleEnd;

            BattleManager.EventInvoke(eventType);
            BattleManager.PlayerReward(this.Reward);

            if (enemies.Count > 0) { enemies.ForEach(e => RolePool.Recycle(e, IRole.ETeamState.Enemy)); }
        }
    }
}