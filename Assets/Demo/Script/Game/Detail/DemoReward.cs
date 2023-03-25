using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Battle;
using Custom.DataPacked;

namespace FightingGameDemo
{
    [CreateAssetMenu(fileName = "Battle Reward", menuName = "Game/Reward/Battle Reward", order = 1)]
    public class DemoReward : RewardAsset, IPack, IGetExp
    {
        [SerializeField]
        private float _Exp;

        public float Exp => this._Exp;

        public override void GetReward()
        {
            BattleManager.Allies.ForEach(p => 
            {
                if (p.Level is Custom.Role.LevelState level) 
                {
                    if (level.GetExp(this)) 
                    {
                        BattleManager.EventInvoke(BattleManager.BattleEvent.EEventType.PlayerUpgrade, p);
                    }
                }
            });
        }

        public IPack Packed<TData>(TData data)
        {
            if (data is IGetExp exp)
            {
                this._Exp = exp.Exp;
            }

            return this;
        }
    }
}