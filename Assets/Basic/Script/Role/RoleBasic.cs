using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Battle;
using Custom.DataPacked;

namespace Custom.Role
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class RoleBasic : MonoBehaviour, IRole, ILevelState, ILiveState, IGroundSensor, IWallSensor, IMoveAction, IAttackAction, IHurtAction, IDeathAction, IFlipAction, IPackableObject, IPoolItem
    {
        public Rigidbody2D Rigidbody { get; protected set; }
        public Animator Animator { get; protected set; }
        public AnimationCheck AnimCheck { get; protected set; }
        public RoleData Data { get; protected set; }

        #region IRole

        [SerializeField]
        protected string _RoleName;
        [SerializeField]
        protected IRole.ETeamState _TeamState;
        [SerializeField]
        protected LevelState _Level;
        [SerializeField]
        protected Health _Health;

        public string RoleName => this._RoleName;
        public IRole.ETeamState TeamState => this._TeamState;
        public ILevel Level => this._Level;
        public IHealth Health => this._Health;

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
        public virtual float CurrentDamage => this.Health.Damage;
        public AttackCollect AttackCollect => this._AttackCollect;

        public abstract void Move(Vector2 direct, bool sprint);
        public abstract void Hurt();
        public abstract void Death();

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

        public bool IsDead => this.Health.HP == 0; 
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

            this._Health = new Health(value, new HealthPack(value));
            this._Level = new LevelState(1, value.Level, 0, value.Exp, this.Data.Exp);
        }

        public virtual void Initialize(IPack pack)
        {
            if (pack is ILevel level) 
            {
                var value = this.Data.GetValue(level.Level);

                this._Level = new LevelState(level.Level, 100, level.Exp, value.Exp, this.Data.Exp);
            }

            if (pack is IHealth health && this.Level.Level > 0) 
            {
                var value = this.Data.GetValue(this.Level.Level);

                this._Health = new Health(health, value);
            }
        }

        public virtual void SetData(IPack pack)
        {
            if (pack is IGetExp getExp) 
            {
                this._Level.GetExp(getExp);
            }
        }

        public TPack GetPack<TPack>() where TPack : IPack
        {
            var pack = Activator.CreateInstance<TPack>();
            
            if (pack is ILevel) { pack.Packed(this.Level); }

            return pack;
        }

        #endregion

        #region IPoolItem

        public abstract void Spawn<TData>(TData data);
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

    #region Health

    [Serializable]
    public class Health : IHealth
    {
        [SerializeField]
        private float _HP;
        [SerializeField]
        private float _MP;
        [SerializeField]
        private float _Damage;
        [SerializeField]
        private float _Defend;

        public IHealth Default { get; private set; }

        public float HP => this._HP;
        public float MP => this._HP;
        public float Damage => this._Damage;
        public float Defend => this._Defend;
        public float NormalizeHP => this._HP / this.Default.HP;
        public float NormalizeMP => this._MP / this.Default.MP;

        public Health(IHealth health, IHealth Default) : this(health.HP, health.MP, health.Damage, health.Defend, Default)
        {
            
        }

        public Health(float hp, float mp, float damage, float defend, IHealth Default) 
        {
            this._HP = hp;
            this._MP = mp;
            this._Damage = damage;
            this._Defend = defend;

            this.Default = Default;
        }

        public float Injured(float hp) 
        {
            var damage = Mathf.Min(this._HP, hp);

            this._HP -= damage;

            return damage;
        }

        public float Reduced(float mp)
        {
            var cost = Mathf.Min(this._MP, mp);

            this._MP -= cost;

            return cost;
        }

        public float Healed(float hp) 
        {
            var injure = this.Default.HP - this._HP;
            var heal = Mathf.Min(injure, hp);

            this._HP += heal;

            return heal;
        }

        public float Increased(float mp)
        {
            var reduce = this.Default.MP - this._MP;
            var increase = Mathf.Min(reduce, mp);

            this._HP += increase;

            return increase;
        }

        public bool Cost(float cost, out float consume) 
        {
            var higher = cost >= this._MP;

            consume = higher ? cost : 0;

            return higher;
        }

        public HealthVariable ValueChange(HealthVariable variable) 
        {
            var value = 0f;

            if (variable.ValueTarget == HealthVariable.EValueTarget.HP)
            {
                if (variable.ValueType == HealthVariable.EValueType.Increase) { value = this.Healed(variable.Value); }

                if (variable.ValueType == HealthVariable.EValueType.Reduce) { value = this.Injured(variable.Value); }
            }

            if (variable.ValueTarget == HealthVariable.EValueTarget.MP)
            {
                if (variable.ValueType == HealthVariable.EValueType.Increase) { value = this.Increased(variable.Value); }

                if (variable.ValueType == HealthVariable.EValueType.Reduce) { value = this.Reduced(variable.Value); }
            }

            return new HealthVariable(value, variable.ValueTarget, variable.ValueType, variable.Role);
        }
    }

    #endregion

    #region IHealthBuffer

    public interface IHealthBuffer 
    {
        public EBuffType BuffType { get; }
        public TimeClient.CountDownTimer Timer { get; }

        [Serializable]
        public enum EBuffType 
        {
            None = 0,
            Buff = 1,
            Nurf = 2
        }
    }

    #endregion

    #region Level

    [Serializable]
    public class LevelState : ILevel
    {
        [SerializeField]
        private int _Level;
        [SerializeField]
        private float _Exp;

        public int MaxLevel { get; private set; }
        public float MaxExp { get; private set; }
        public RoleData.IncreaseValue Increase { get; private set; }

        public int Level => this._Level;
        public float Exp => this._Exp;

        public LevelState(ILevel current, ILevel max, RoleData.IncreaseValue increase) : this(current.Level, max.Level, current.Exp, max.Exp, increase)
        {

        }

        public LevelState(int level, int maxLvl, float exp, float maxExp, RoleData.IncreaseValue increase) 
        {
            this._Level = level;
            this.MaxLevel = maxLvl;
            this._Exp = exp;
            this.MaxExp = maxExp;
            this.Increase = increase;
        }

        public bool GetExp(IGetExp value) 
        {
            var temp = this._Exp + value.Exp;

            if (this._Exp >= this.MaxExp) 
            {
                this._Level += (int)(temp / this.MaxExp);
                this._Exp = temp % this.MaxExp;

                this.MaxExp = this.Increase.GetValue(this._Level);

                return true;
            }

            else 
            {
                this._Exp = temp;

                return false;
            }
        }
    }

    #endregion
}