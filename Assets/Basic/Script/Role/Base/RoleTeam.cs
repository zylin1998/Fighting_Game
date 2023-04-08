using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Events;

namespace Custom.Role
{
    public class RoleTeam : MonoBehaviour
    {
        [SerializeField]
        private ETeamState _TeamState;
        [SerializeField]
        private RewardAsset _DefeatedReward;


        public ETeamState TeamState => this._TeamState;
        public IReward DefeatedReward => this._TeamState == ETeamState.Enemy ? this._DefeatedReward : default(IReward);

        public IRole Target { get; private set; } 
    
        public void SetTeam(ETeamState teamState) 
        {
            this._TeamState = teamState;
        }

        public virtual void SetTarget(IRole role)
        {
            if (this._TeamState == ETeamState.Enemy)
            {
                this.Target = role;

                var action = this.GetComponent<ActionLoop>();

                if (action) { StartCoroutine(action.Progress()); }
            }
        }

        [System.Serializable]
        public enum ETeamState
        {
            None = 0,
            Ally = 1,
            Enemy = 2,
            NPC = 3
        }
    }
}