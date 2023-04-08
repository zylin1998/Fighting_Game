using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Role;
using Custom.Events;
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
            var player = DemoBattleRule.Player;

            player.SetData<IGetExp>(this);
            
            EventManager.EventInvoke("Ally Upgrade", new RoleVariable(player));
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