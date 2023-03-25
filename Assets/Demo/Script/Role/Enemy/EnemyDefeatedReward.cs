using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Battle;

namespace FightingGameDemo
{
    [CreateAssetMenu(fileName = "Enemy Defeated Reward", menuName = "Game/Reward/Enemy Defeated Reward", order = 1)]
    public class EnemyDefeatedReward : RewardAsset
    {
        [SerializeField]
        private int _Score;

        public int Score => this._Score;

        public override void GetReward()
        {
            BattleManager.CheckRule(this.Score);
        }
    }
}