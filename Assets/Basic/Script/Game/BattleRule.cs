using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Battle
{
    public abstract class BattleRule : IBattleRule
    {
        public abstract IBattleRule.EBattleResult CheckRule<TValue>(TValue data);
        public abstract bool Fulfilled();
        public abstract bool Defeated();
        public abstract void Reset();
    }

    public abstract class BattleRuleAsset : ScriptableObject, IBattleRule
    {
        public abstract IBattleRule.EBattleResult CheckRule<TValue>(TValue data);
        public abstract bool Fulfilled();
        public abstract bool Defeated();
        public abstract void Reset();
    }

    public interface IBattleRule 
    {
        public EBattleResult CheckRule<TValue>(TValue data);
        public bool Fulfilled();
        public bool Defeated();
        public void Reset();

        [System.Serializable]
        public enum EBattleResult 
        {
            None = 0,
            Progress = 1,
            Fulfill = 2,
            Defeated = 3
        }
    }
}