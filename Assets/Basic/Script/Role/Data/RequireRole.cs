using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Role
{
    [CreateAssetMenu(fileName = "Require Roles", menuName = "Role Data/Require Roles", order = 1)]
    public class RequireRole : ScriptableObject
    {
        [System.Serializable]
        public struct RequireAmount
        {
            [SerializeField]
            private string _RoleName;
            [SerializeField]
            private int _Amount;

            public string RoleName => this._RoleName;
            public int Amount => this._Amount;

            public RequireAmount(string role, int amount) 
            {
                this._RoleName = role;
                this._Amount = amount;
            }
        }

        [System.Serializable]
        public struct Pair 
        {
            [SerializeField]
            private RoleTeam.ETeamState _Team;
            [SerializeField]
            private List<RequireAmount> _Requires;
            

            public RoleTeam.ETeamState Team => this._Team;
            public List<RequireAmount> Requires => this._Requires;

            public Pair(RoleTeam.ETeamState team) : this(team, new RequireAmount[0]) { }

            public Pair(RoleTeam.ETeamState team, IEnumerable<RequireAmount> requires) 
            {
                this._Team = team;
                this._Requires = new List<RequireAmount>(requires);
            }
        }

        [SerializeField]
        private List<Pair> _RequireRoles = new List<Pair>() { new Pair(RoleTeam.ETeamState.Ally), new Pair(RoleTeam.ETeamState.Enemy) };

        public List<Pair> RequireRoles => this._RequireRoles;
        public Pair Ally => this._RequireRoles.Find(r => r.Team == RoleTeam.ETeamState.Ally);
        public Pair Enemy => this._RequireRoles.Find(r => r.Team == RoleTeam.ETeamState.Enemy);
    }
}