using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Role 
{
    public interface IRole
    {
        public string RoleName { get; }
        public RoleTeam Team { get; }
        public RoleProperty RoleProperty { get; }
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
