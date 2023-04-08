using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Role
{
    [CreateAssetMenu(fileName = "Health Event", menuName = "Event/Game Event/Health Event", order = 2)]
    public class HealthEvent : GameEvent
    {
        public override void Invoke<TVariable>(TVariable variable)
        {
            var v = this.Converter<PropertyVariable>(variable);

            var role = v.To;
            
            if (!role) { return; }
            if (role.Team.TeamState == RoleTeam.ETeamState.None) { return; }
            if (role.IsDead) { return; }

            var health = role.RoleProperty["Health"];

            if (health != null)
            {
                health.ValueChange(v);

                this.CallBack?.Invoke(v);
            }

        }
    }
}