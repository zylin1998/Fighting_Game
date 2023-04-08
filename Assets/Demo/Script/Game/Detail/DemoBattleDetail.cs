using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom;
using Custom.Role;
using Custom.Events;

namespace FightingGameDemo
{
    [CreateAssetMenu(fileName = "Battle Detail", menuName = "Game/Battle/Battle Detail", order = 1)]
    public class DemoBattleDetail : GameDetail
    {
        public override void Initialize() 
        {
            RolePool.AddRoles(this._Requires);

            this._Rule.Initialize();
            
            this._GameActions.ForEach(a => a.Initialize());
        }

        public override IEnumerator GameLoop()
        {
            this._GameActions
                .FindAll(a => a.InvokeType == IGameAction.EInvokeType.Start)
                .ForEach(a => a.Invoke(new RoleVariable(DemoBattleRule.Player)));

            for (; !this.Rule.Fulfilled() && !this.Rule.Defeated();) 
            {
                yield return null;
            }
            
            EventManager.EventInvoke("Battle End");
            EventManager.GetReward(this.Reward);
        }
    }
}