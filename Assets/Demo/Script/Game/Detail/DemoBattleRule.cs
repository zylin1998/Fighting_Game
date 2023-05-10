using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom;
using Custom.Role;
using Custom.Events;

namespace FightingGameDemo
{
    [CreateAssetMenu(fileName = "Demo Rule", menuName = "Game/Battle/Detail/Rule", order = 1)]
    public class DemoBattleRule : GameRuleAsset
    {
        [SerializeField]
        private int _Score;
        [SerializeField]
        private int _Current;

        public int Score => this._Score;
        public int Current => this._Current;

        public static RoleBasic Player { get; set; }
        public static List<RoleBasic> Enemies { get; set; }

        public override void Initialize()
        {
            Enemies = new List<RoleBasic>();

            Player = RolePool.Spawn("Sword Man", default(ISpawn));

            Singleton<CameraController>.Instance.Follow(Player.transform);

            EventManager.AddEvent("Enemy Slaved", (variable) =>
            {
                if (variable is IRoleGiver slave)
                {
                    Enemies.Remove(slave.To);
                }
            });

            this.Reset();
        }

        public override IGameRule.EBattleResult CheckRule<TValue>(TValue data)
        {
            if (data is int s) 
            {
                this._Current = Mathf.Clamp(this._Current + s, 0, this._Score);
            }

            if (this.Fulfilled()) { return IGameRule.EBattleResult.Fulfill; }
            if (this.Defeated()) { return IGameRule.EBattleResult.Defeated; }

            return IGameRule.EBattleResult.Progress;
        }

        public override bool Fulfilled()
        {
            return this._Current >= this._Score;
        }

        public override bool Defeated()
        {
            return Player.IsDead;
        }

        public override void Reset() 
        {
            this._Current = 0;
        }
    }
}