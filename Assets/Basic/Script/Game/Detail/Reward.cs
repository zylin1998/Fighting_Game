using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Events
{
    public abstract class RewardAsset : ScriptableObject, IReward
    {
        public abstract void GetReward();
    }

    public interface IReward
    {
        public void GetReward();
    }

    public interface IGetExp
    {
        public float Exp { get; }
    }
}