using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Events;
using Custom.DataPacked;

namespace Custom.Role
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(RoleTeam))]
    public abstract class RoleBasic : MonoBehaviour, IRole, ILevelState, ILiveState, IGroundSensor, IWallSensor, IMoveAction, IAttackAction, IHurtAction, IDeathAction, IFlipAction, IPoolItem
    {
        public Rigidbody2D Rigidbody { get; protected set; }
        public Animator Animator { get; protected set; }
        public AnimationCheck AnimCheck { get; protected set; }
        public RoleData Data { get; protected set; }

        #region IRole

        [SerializeField]
        protected string _RoleName;
        [SerializeField]
        protected RoleTeam _Team;
        [SerializeField]
        protected RoleProperty _RoleProperty;

        public string RoleName => this._RoleName;
        public RoleTeam Team => this._Team;
        public RoleProperty RoleProperty => this._RoleProperty;

        #endregion

        #region Action

        [SerializeField]
        protected float _WalkSpeed;
        [SerializeField]
        protected float _SprintSpeed;
        [SerializeField]
        protected AttackCollect _AttackCollect;

        public bool IsSprint { get; set; }
        public virtual float WalkSpeed => this._WalkSpeed;
        public virtual float SprintSpeed => this._SprintSpeed;
        public virtual float CurrentDamage => this.RoleProperty["Health"]["Damage"].Value;
        public AttackCollect AttackCollect => this._AttackCollect;

        public abstract void Move(Vector2 direct, bool sprint);

        public virtual void Hurt(PropertyVariable variable) 
        {
            if (this.IsDead) { return; }

            EventManager.EventInvoke(string.Format("{0} Hurt", this._Team.TeamState), variable);

            if (this.IsDead) { this.Death(new RoleSlaveVariable(variable.To, variable.From)); }
        }

        public virtual void Death(RoleSlaveVariable variable) 
        {
            if (!this.IsDead) { return; }

            var animState = "Death";
            this.Animator.Play(animState);

            var enumerator = AnimCheck?.AnimationEvent(
                  animState
                , () => EventManager.EventInvoke(string.Format("{0} Slaved", this._Team.TeamState), variable)
                , () => RolePool.Recycle(this));

            StartCoroutine(enumerator);
        }

        public virtual void Attack(string attack) 
        {
            if (this.AttackState || this.IsDead) { return; }

            var asset = this._AttackCollect[attack];

            this.Animator.Play(asset.AttackName);
            this.AttackState = true;

            var enumerator = AnimCheck?.AnimationEvent(
                  asset.AttackName
                , () => asset.Invoke(this.transform, this)
                , () => this.AttackState = false);

            StartCoroutine(enumerator);
        }

        #endregion

        #region RoleState

        public bool IsDead => this.RoleProperty["Health"]["HP"].Value == 0; 
        public virtual bool IsMaxLevel { get; }
        public virtual bool AttackState { get; protected set; }
        public virtual bool HurtState { get; protected set; }

        #endregion

        #region Sensor

        #region IGroundSensor

        public bool IsGround { get; set; }
        public virtual float VerticalVelocity => this.Rigidbody.velocity.y;

        public virtual void Land()
        {
            
        }

        #endregion

        #region IWallSensor

        public IWallSensor.EContactDirect Contact { get; set; }

        public virtual float ContactCheck(float speed)
        {
            if (this.Contact == IWallSensor.EContactDirect.Left && speed < 0f) { return 0f; }
            if (this.Contact == IWallSensor.EContactDirect.Right && speed > 0f) { return 0f; }

            return speed;
        }

        #endregion

        #region IFlipAction

        public virtual IFlipAction.EFlipSide FlipSide { get; }

        public virtual void Flip(IFlipAction.EFlipSide flip)
        {

        }

        public virtual void Flip(float flip)
        {
            var scale = transform.localScale;

            if (scale.x * flip < 0)
            {
                scale.x *= -1f;
                transform.localScale = scale;
            }
        }

        #endregion

        #endregion

        #region IPackableObject

        public string ID => this._RoleName;
        
        public virtual void Initialize()
        {
            var value = this.Data.GetValue(1);

            this._RoleProperty = new RoleProperty(value.PropertyList);
        }

        public virtual void SetData<TData>(TData data)
        {
            if (data is IGetExp getExp) 
            {
                this.RoleProperty["Level"]["Level"].SetValue(getExp.Exp);
            }

            if (data is RoleProperty property) 
            {
                this._RoleProperty = property;
            }
        }

        #endregion

        #region IPoolItem

        public virtual void Spawn<TData>(TData data) where TData : ISpawn 
        {
            data.Spawn(this);
        }
        public abstract void Recycle();

        #endregion
    }

    #region IPack

    [Serializable]
    public struct HealthPack : IPack, IHealth
    {
        [SerializeField]
        private float _HP;
        [SerializeField]
        private float _MP;
        [SerializeField]
        private float _Damage;
        [SerializeField]
        private float _Defend;

        public float HP => this._HP;
        public float MP => this._MP;
        public float Damage => this._Damage;
        public float Defend => this._Defend;
        public bool IsDead => this.HP == 0;

        #region Construction

        public HealthPack(IHealth health) : this(health.HP, health.MP, health.Damage, health.Defend)
        {

        }

        public HealthPack(float hp, float mp, float damage, float defend) 
        {
            this._HP = hp;
            this._MP = mp;
            this._Damage = damage;
            this._Defend = defend;
        }

        #endregion

        public void SetHP(float value) { this._HP = value; }
        public void SetMP(float value) { this._MP = value; }
        public void SetDamage(float value) { this._Damage = value; }
        public void SetDefend(float value) { this._Defend = value; }

        #region IPack

        public IPack Packed<TData>(TData data)
        {
            if (data is IHealth health) 
            {
                this._HP = health.HP;
                this._MP = health.MP;
                this._Damage = health.Damage;
                this._Defend = health.Defend;
                return this;
            }

            return null;
        }

        #endregion
    }

    [Serializable]
    public struct LevelPack : IPack, ILevel 
    {
        [SerializeField]
        private int _Level;
        [SerializeField]
        private float _Exp;

        public int Level => this._Level;
        public float Exp => this._Exp;

        #region Construction

        public LevelPack(ILevel level) : this(level.Level, level.Exp)
        {

        }

        public LevelPack(int level, float exp) 
        {
            this._Level = level;
            this._Exp = exp;
        }

        #endregion

        #region IPack

        public IPack Packed<TData>(TData data) 
        {
            if (data is ILevel level) 
            {
                this._Level = level.Level;
                this._Exp = level.Exp;

                return this;
            }

            return null;
        }

        #endregion

        public void SetLevel(int value) { this._Level = value; }
        public void SetExp(int value) { this._Exp = value; }
    }

    #endregion

    #region IHealthBuffer

    public interface IHealthBuffer 
    {
        public EBuffType BuffType { get; }
        public CountDownTimer Timer { get; }

        [Serializable]
        public enum EBuffType 
        {
            None = 0,
            Buff = 1,
            Nurf = 2
        }
    }

    #endregion

    #region Variable

    [Serializable]
    public struct RoleSlaveVariable : IVariable
    {
        [SerializeField]
        private RoleBasic _Slaved;
        [SerializeField]
        private RoleBasic _Killed;

        public RoleBasic Slaved => this._Slaved;
        public RoleBasic Killed => this._Killed;

        public RoleSlaveVariable(RoleBasic slaved, RoleBasic killed)
        {
            this._Slaved = slaved;
            this._Killed = killed;
        }
    }

    [SerializeField]
    public struct RoleVariable : IVariable
    {
        [SerializeField]
        private RoleBasic _Role;

        public RoleBasic Role => this._Role;

        public RoleVariable(RoleBasic role)
        {
            this._Role = role;
        }
    }

    #endregion

    #region Spawn

    [Serializable]
    public struct EnemySpawn : ISpawn
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

        public void Spawn<TItem>(TItem item) where TItem : IPoolItem 
        {
            if (item is RoleBasic role) 
            {
                var value = role.Data.GetValue(this.Level);

                role.SetData(new RoleProperty(value.PropertyList));

                role.transform.position = this.Position;
                role.transform.rotation = this.Rotation;
                role.Flip(this.FlipSide);
                role.gameObject.SetActive(true);

                role.Team.SetTeam(RoleTeam.ETeamState.Enemy);
                role.gameObject.layer = LayerMask.NameToLayer("Enemy");
            }
        }
    }

    #endregion

    [Serializable]
    public class RoleProperty 
    {
        [SerializeField]
        private List<PropertyList> _PropertyLists;

        public List<PropertyList> PropertyLists => this._PropertyLists;
        public PropertyList this[string name] => this._PropertyLists.Find(p => p.ListName == name);

        public RoleProperty(IEnumerable<PropertyList> list) 
        {
            this._PropertyLists = new List<PropertyList>(list);
        }
    }
}