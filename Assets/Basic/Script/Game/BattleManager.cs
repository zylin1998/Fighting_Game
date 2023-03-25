using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Custom.Role;

namespace Custom.Battle
{
    public class BattleManager : MonoBehaviour, IBeginClient
    {
        [SerializeField]
        private CinemachineVirtualCamera _Camera;
        [SerializeField]
        private TimeClient.Timer _Timer;
        
        private static CinemachineVirtualCamera Camera { get; set; }

        public static IBattleDetail Detail { get; set; }
        public static List<RoleBasic> Allies { get; private set; }
        public static List<RoleBasic> Enemies { get; private set; }
        public static List<CreateSpot> CreateSpots { get; set; }
        public static Dictionary<BattleEvent.EEventType, BattleEvent> BattleEvents { get; private set; } 
        
        private void Awake()
        {
            BeginClient.AddBegin(this);

            Camera = this._Camera;
            Allies = new List<RoleBasic>();
            Enemies = new List<RoleBasic>();
            BattleEvents = new Dictionary<BattleEvent.EEventType, BattleEvent>();

            CreateSpots = this.GetComponentsInChildren<CreateSpot>().ToList();
        }

        public void BeforeBegin() 
        {
            var player = LayerMask.NameToLayer("Player");
            var enemy = LayerMask.NameToLayer("Enemy");
            
            Physics2D.IgnoreLayerCollision(player, enemy);
            Physics2D.IgnoreLayerCollision(player, player);
            Physics2D.IgnoreLayerCollision(enemy, enemy);

            RolePool.AddRoles(Detail.Requires);

            Allies = Detail
                .Requires
                .Ally
                .Requires
                .ConvertAll(r => RolePool.Spawn(r.RoleName, 0, IRole.ETeamState.Ally));
            
            this._Timer = TimeClient.GetTimer<TimeClient.Timer>(0);

            AddEvent(BattleEvent.EEventType.BattleEnd, (role) => { this._Timer.Stop(); });
        }

        public void BeginAction() 
        {
            StartCoroutine(Detail.GameLoop());

            this._Timer.Start();
        }

        #region Set Enemy

        public static void SetEnemy(IBattleDetail.RoleDetail detail) 
        {
            var spot = CreateSpots[UnityEngine.Random.Range(0, CreateSpots.Count)];
            var spawn = new EnemySpawn(detail.Level, spot);

            var enemy = RolePool.Spawn<Enemy, EnemySpawn>(detail.Role, spawn, IRole.ETeamState.Enemy);

            if (enemy) 
            {
                enemy.SetTarget(Allies.FirstOrDefault());
                Enemies.Add(enemy);
            }
        }

        public static void SetEnemy(string enemy) 
        {
            SetEnemy(Detail[enemy]);
        }

        #endregion

        public static void CheckRule<TValue>(TValue value) 
        {
            Detail.Rule.CheckRule(value);
        }

        public static void PlayerReward(IReward reward) 
        {
            if (reward != null) { reward.GetReward(); }
        }

        public static void SetCamera(Transform follow) 
        {
            Camera.Follow = follow;
        }

        #region Battle Event

        public static void AddEvent(BattleEvent.EEventType eventType, Action<RoleBasic> callBack) 
        {
            if (eventType == BattleEvent.EEventType.None) { return; }

            var battleEvent = GetEvent(eventType);

            if (battleEvent != null)
            {
                battleEvent.CallBack += callBack;
            }

            else 
            {
                BattleEvents.Add(eventType, new BattleEvent(eventType, callBack));
            }
        }

        public static void RemoveEvent(BattleEvent.EEventType eventType, Action<RoleBasic> callBack) 
        {
            if (eventType == BattleEvent.EEventType.None) { return; }
            if (callBack == null) { return; }

            var battleEvent = GetEvent(eventType);

            if (battleEvent != null)
            {
                battleEvent.CallBack -= callBack;
            }
        }

        private static BattleEvent GetEvent(BattleEvent.EEventType eventType) 
        {
            BattleEvent battleEvent = null;

            BattleEvents.TryGetValue(eventType, out battleEvent);

            return battleEvent;
        }

        public static void EventInvoke(BattleEvent.EEventType eventType) 
        {
            EventInvoke(eventType, null);
        }

        public static void EventInvoke(BattleEvent.EEventType eventType, RoleBasic role) 
        {
            var battleEvent = GetEvent(eventType);

            if (battleEvent != null)
            {
                battleEvent.CallBack.Invoke(role);
            }
        }

        public static void RoleSlaved<TRole>(TRole role, IRole.ETeamState teamState) where TRole : RoleBasic
        {
            if (teamState == IRole.ETeamState.None || role == null) { return; }

            var list = teamState == IRole.ETeamState.Ally ? Allies : Enemies;
            var eventType = teamState == IRole.ETeamState.Ally ? BattleEvent.EEventType.PlayerSlaved : BattleEvent.EEventType.EnemySlaved;

            list.Remove(role);

            if (teamState == IRole.ETeamState.Enemy && role is IEnemy enemy) 
            {
                PlayerReward(enemy.DefeatedReward);
            }

            EventInvoke(eventType, role);
        }

        public static void RoleHealthCal(HealthVariable variable, RoleBasic role) 
        {
            if (!role) { return; }
            if (role.TeamState == IRole.ETeamState.None) { return; }
            if (role.IsDead) { return; }

            if (role.Health is Health health) 
            {
                health.ValueChange(variable);
                
                var eventType = role.TeamState == IRole.ETeamState.Ally ? BattleEvent.EEventType.PlayerHurt : BattleEvent.EEventType.EnemyHurt;
                
                if (variable.ValueTarget == HealthVariable.EValueTarget.HP) 
                {
                    role.Hurt();

                    EventInvoke(eventType, role); 
                }
            }
        }

        #endregion

        private void OnDestroy()
        {
            Detail = null;

            Allies.Clear();
            Enemies.Clear();
            CreateSpots.Clear();
            BattleEvents.Clear();
        }

        [Serializable]
        public class BattleEvent 
        {
            [SerializeField]
            private EEventType _EventType;
            
            public Action<RoleBasic> CallBack { get; set; }

            public EEventType EventType => this._EventType;

            public BattleEvent() : this(EEventType.None, (role) => { })
            {

            }

            public BattleEvent(EEventType eventType) : this(eventType, (role) => { })
            {

            }

            public BattleEvent(EEventType eventType, Action<RoleBasic> callBack) 
            {
                this._EventType = eventType;
                this.CallBack = callBack;
            }

            [Serializable]
            public enum EEventType 
            {
                None = 0,
                PlayerSlaved = 1,
                EnemySlaved = 2,
                PlayerHurt = 3,
                EnemyHurt = 4,
                BattleEnd = 5,
                PlayerUpgrade = 6
            }
        }
    }
}