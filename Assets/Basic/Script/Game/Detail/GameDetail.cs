using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Role;

namespace Custom.Events
{
    public abstract class GameDetail : ScriptableObject, IGameDetail
    {
        [SerializeField]
        protected GameRuleAsset _Rule;
        [SerializeField]
        protected RewardAsset _Reward;
        [SerializeField]
        protected RequireRole _Requires;
        [SerializeField]
        protected List<IGameDetail.RoleDetail> _Enemies = new List<IGameDetail.RoleDetail>() 
        {
            new IGameDetail.RoleDetail(IGameDetail.EEnemyType.Normal)
        };
        [SerializeField]
        protected List<GameAction> _GameActions;

        public IReward Reward => this._Reward;
        public IGameRule Rule => this._Rule;
        public RequireRole Requires => this._Requires;
        public List<IGameDetail.RoleDetail> Enemies => this._Enemies;
        public IGameDetail.RoleDetail this[string role] => this.Enemies.Find(e => e.Role == role);
        public IEnumerable<IGameAction> GameActions => this._GameActions;

        public abstract void Initialize();
        public abstract IEnumerator GameLoop();
    }

    public interface IGameDetail
    {
        public IEnumerable<IGameAction> GameActions { get; }

        public IReward Reward { get; }
        public IGameRule Rule { get; }
        public RequireRole Requires { get; }
        public List<RoleDetail> Enemies { get; }
        public RoleDetail this[string role] { get; }

        public void Initialize();
        public IEnumerator GameLoop();

        public TAction GetAction<TAction>() where TAction : IGameAction
        {
            var action = new List<IGameAction>(this.GameActions).Find(a => a is TAction);

            return action is TAction result ? result : default(TAction);
        }

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