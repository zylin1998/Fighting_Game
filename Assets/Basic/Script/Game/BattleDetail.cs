using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Role;

namespace Custom.Battle
{
    public abstract class BattleDetail : ScriptableObject, IBattleDetail
    {
        [SerializeField]
        protected BattleRuleAsset _Rule;
        [SerializeField]
        protected RewardAsset _Reward;
        [SerializeField]
        private RequireRole _Requires;
        [SerializeField]
        protected List<IBattleDetail.RoleDetail> _Enemies = new List<IBattleDetail.RoleDetail>() 
        {
            new IBattleDetail.RoleDetail(IBattleDetail.EEnemyType.Normal)
        };

        public IReward Reward => this._Reward;
        public IBattleRule Rule => this._Rule;
        public RequireRole Requires => this._Requires;
        public List<IBattleDetail.RoleDetail> Enemies => this._Enemies;
        public IBattleDetail.RoleDetail this[string role] => this.Enemies.Find(e => e.Role == role);

        public abstract IEnumerator GameLoop();
    }

    public interface IBattleDetail
    {
        public IReward Reward { get; }
        public IBattleRule Rule { get; }
        public RequireRole Requires { get; }
        public List<RoleDetail> Enemies { get; }
        public RoleDetail this[string role] { get; }

        public IEnumerator GameLoop();

        [System.Serializable]
        public enum EEnemyType
        {
            None = 0,
            Normal = 1,
            Boss = 2
        }

        [System.Serializable]
        public struct RoleDetail
        {
            [SerializeField]
            private string _Role;
            [SerializeField]
            private EEnemyType _EnemyType;
            [SerializeField]
            private int _Level;

            public string Role => this._Role;
            public EEnemyType EnemyType => this._EnemyType;
            public int Level => this._Level;

            public RoleDetail(EEnemyType enemyType)  : this(string.Empty, enemyType, 0)
            {

            }

            public RoleDetail(string role, EEnemyType enemyType, int level) 
            {
                this._Role = role;
                this._EnemyType = enemyType;
                this._Level = level;
            }
        }
    }
}