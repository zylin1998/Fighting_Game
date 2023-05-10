using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom;
using Custom.Role;
using Custom.Events;

namespace FightingGameDemo
{
    [CreateAssetMenu(fileName = "EnemySpawn Action", menuName = "Event/Game Action/EnemySpawn Action", order = 2)]
    public class EnemySpawnAction : GameAction
    {
        [SerializeField]
        private CountDownTimer _Timer;
        [SerializeField]
        protected List<IGameDetail.RoleDetail> _Enemies = new List<IGameDetail.RoleDetail>()
        {
            new IGameDetail.RoleDetail(IGameDetail.EEnemyType.Normal)
        };

        private DemoBattleRule rule;

        public override void Initialize()
        {
            if (EventManager.Detail.Rule is DemoBattleRule rule) 
            {
                this.rule = rule;
            }

            this._Timer.EndCallBack += (time) =>
            {
                SpawnEnemy();

                this._Timer.Reset();
                this._Timer.Start();
            };
        }

        public override void Invoke<TVariable>(TVariable role)
        {
            EventManager.AddEvent("Battle End", (variable) => this._Timer.Stop());

            this._Timer.Start();
        }

        private List<IGameDetail.RoleDetail> normal;

        private void SpawnEnemy() 
        {
            var amount = rule.Score - rule.Current;
            var onScene = DemoBattleRule.Enemies
                .ConvertAll(c => (c.Team.DefeatedReward as EnemyDefeatedReward).Score)
                .Sum();
            
            if (amount > onScene) 
            {
                if (this.normal.Count == 0)
                {
                    this.normal = this._Enemies.FindAll(e => e.EnemyType == IGameDetail.EEnemyType.Normal);
                }

                var enemy = normal[Random.Range(0, normal.Count)];

                var spot = EventManager.CreateSpots[Random.Range(0, EventManager.CreateSpots.Count)];

                this.SetRole(enemy.Role, new EnemySpawn(enemy.Level, spot), RoleTeam.ETeamState.Enemy); 
            }
        }

        public void SetRole<TSpawn>(string id, TSpawn spawn, RoleTeam.ETeamState teamState) where TSpawn : ISpawn
        {
            var role = RolePool.Spawn(id, spawn);

            role.Team.SetTeam(teamState);

            if (role.Team.TeamState == RoleTeam.ETeamState.Enemy) 
            {
                role.Team.SetTarget(DemoBattleRule.Player);

                DemoBattleRule.Enemies.Add(role);
            }
        }
    }
}