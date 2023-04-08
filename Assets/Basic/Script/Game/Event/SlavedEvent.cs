using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Role
{
    [CreateAssetMenu(fileName = "Slaved Event", menuName = "Event/Game Event/Slaved Event", order = 2)]
    public class SlavedEvent : GameEvent
    {
        public override void Invoke<TVariable>(TVariable variable)
        {
            var v = this.Converter<RoleSlaveVariable>(variable);

            var role = v.Slaved;
            
            if (!role) { return; }
            if (role.Team.TeamState == RoleTeam.ETeamState.None) { return; }
            if (!role.IsDead) { return; }

            if (role.Team.TeamState == RoleTeam.ETeamState.Enemy) 
            {
                Events.EventManager.GetReward(role.Team.DefeatedReward);
            }

            this.CallBack?.Invoke(v);
        }
    }
}
