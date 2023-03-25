using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Role 
{
    public interface IRole 
    {
        [System.Serializable]
        public enum ETeamState 
        {
            None = 0,
            Ally = 1,
            Enemy = 2
        }

        public string RoleName { get; }
        public IRole.ETeamState TeamState { get; }
        public ILevel Level { get; }
        public IHealth Health { get; }
        public Animator Animator { get; }
        
        public TConvert Convert<TConvert>()
        {
            if (this is TConvert action) { return action; }

            return default(TConvert);
        }
    }

    #region RoleState

    public interface ILevelState
    {
        public bool IsMaxLevel { get; }
    }

    public interface ILiveState
    {
        public bool IsDead { get; }
    }

    public interface IAttackState 
    {
        public bool AttackState { get; }
    }

    #endregion

    #region Sensor

    public interface IGroundSensor 
    {
        public bool IsGround { get; set; }
        public float VerticalVelocity { get; }

        public void Land();
    }

    public interface IWallSensor 
    {
        [System.Serializable]
        public enum EContactDirect 
        {
            None = 0,
            Left = 1,
            Right = 2,
        }

        public EContactDirect Contact { get; set; }

        public float ContactCheck(float speed);
    }

    public interface ITargetSensor 
    {

    }

    #endregion
}
