using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Events
{
    public abstract class GameRule : IGameRule
    {
        public abstract void Initialize();
        public abstract IGameRule.EBattleResult CheckRule<TValue>(TValue data);
        public abstract bool Fulfilled();
        public abstract bool Defeated();
        public abstract void Reset();
    }

    public abstract class GameRuleAsset : ScriptableObject, IGameRule
    {
        public abstract void Initialize();
        public abstract IGameRule.EBattleResult CheckRule<TValue>(TValue data);
        public abstract bool Fulfilled();
        public abstract bool Defeated();
        public abstract void Reset();
    }

    public interface IGameRule
    {
        public void Initialize();
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