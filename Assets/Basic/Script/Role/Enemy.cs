using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Battle;
using FightingGameDemo;

namespace Custom.Role
{
    public abstract class Enemy : RoleBasic, IEnemy
    {
        [SerializeField]
        protected RewardAsset _DefeatedReward;
        
        public IReward DefeatedReward => this._DefeatedReward;
        public IRole Target { get; protected set; }

        public virtual void SetTarget(IRole role)
        {
            this.Target = role;

            var skilLoop = this.GetComponent<SkillLoop>();

            if (skilLoop) { StartCoroutine(skilLoop.Progress()); }
        }
    }

    #region IEnemy Interface

    public interface IEnemy 
    {
        public IRole Target { get; }
        public IReward DefeatedReward { get; }

        public void SetTarget(IRole role);
    }

    #endregion

    #region Spawn

    [System.Serializable]
    public struct EnemySpawn
    {
        public int Level { get; private set; }
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }
        public IFlipAction.EFlipSide FlipSide { get; private set; }

        public EnemySpawn(int level, ISpot spot) : this(level, spot.Position, Quaternion.identity, spot.FlipSide) 
        {

        }

        public EnemySpawn(int level, Vector3 position, Quaternion rotation, IFlipAction.EFlipSide flipSide)
        {
            this.Level = level;
            this.Position = position;
            this.Rotation = rotation;
            this.FlipSide = flipSide;
        }
    }

    #endregion
}