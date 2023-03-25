using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Role;
using Custom.Battle;

namespace FightingGameDemo
{
    [CreateAssetMenu(fileName = "Demo Rule", menuName = "Game/Battle/Detail/Rule", order = 1)]
    public class DemoBattleRule : BattleRuleAsset
    {
        [SerializeField]
        private int _Score;

        private int current;

        public int Score => this._Score;
        public int Current => this.current;

        public override IBattleRule.EBattleResult CheckRule<TValue>(TValue data)
        {
            if (data is int s) 
            {
                this.current = Mathf.Clamp(this.current + s, 0, this._Score);
            }

            if (this.Fulfilled()) { return IBattleRule.EBattleResult.Fulfill; }
            if (this.Defeated()) { return IBattleRule.EBattleResult.Defeated; }

            return IBattleRule.EBattleResult.Progress;
        }

        public override bool Fulfilled()
        {
            return this.current >= this._Score;
        }

        public override bool Defeated()
        {
            return BattleManager.Allies.All(p => p.IsDead);
        }

        public override void Reset() 
        {
            this.current = 0;
        }
    }
}