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
        public PropertyIncrease Data { get; protected set; }

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

        public virtual void Hurt<TProperty>(TProperty property) where TProperty : IProperty<float>
        {
            if (this.IsDead) { return; }

            var injure = this._RoleProperty["Health"].ValueChange(property, (p, v) => v / (1 + (p["Defend"].Value / 100)));

            if (property is IVariable variable)
            {
                EventManager.EventInvoke(string.Format("{0} Hurt", this._Team.TeamState), variable);
            }

            if (this.IsDead)
            {
                var death = property is IRoleGiver roleGiver ? roleGiver : default(IRoleGiver);

                this.Death(death);
            }
        }

        public virtual void Death<TRoleGiver>(TRoleGiver roleGiver) where TRoleGiver : IRoleGiver 
        {
            if (!this.IsDead) { return; }

            if (this._Team.TeamState == RoleTeam.ETeamState.Enemy)
            {
                EventManager.GetReward(this.Team.DefeatedReward);
            }

            var variable = roleGiver is IVariable v ? v : default(IVariable);

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

        public TProperty HealthChange<TProperty>(TProperty property) where TProperty :IProperty<float> 
        {
            return this._RoleProperty["Health"].ValueChange(property);
        }

        #endregion

        #region RoleState

        public virtual bool IsDead => this.RoleProperty["Health"]["HP"].Value == 0; 
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
                if (this._RoleProperty == null) { this._RoleProperty = new RoleProperty(); }

                this._RoleProperty.SetValue(property);
            }
        }

        #region IPoolItem

        public virtual void Spawn<TData>(TData data) where TData : ISpawn 
        {
            data.Spawn(this);
        }

        public abstract void Recycle();

        #endregion
    }

    #region IBuffer

    public interface IBuffer 
    {
        public RoleBasic Role { get; protected set; }
        public string PropertyName { get; }
        public EBuffType BuffType { get; }
        public CountDownTimer Timer { get; }

        public void Invoke() 
        {
            this.Timer.CycleCallBack = this.Cycle;

            this.Timer.Start();
        }

        public void SetRole(RoleBasic role) 
        {
            this.Role = role;
        }

        public void Cycle(TimeClient.DegreeTime time);

        [Serializable]
        public enum EBuffType 
        {
            None = 0,
            Buff = 1,
            Nurf = 2
        }
    }

    [SerializeField]
    public abstract class Buffer : IBuffer
    {
        [SerializeField]
        private RoleBasic _Role;
        [SerializeField]
        protected string _PropertyName;
        [SerializeField]
        protected IBuffer.EBuffType _BuffType;
        [SerializeField]
        protected CountDownTimer _Timer;

        RoleBasic IBuffer.Role 
        {
            get => this._Role; 

            set => this._Role = value; 
        }

        public string PropertyName => this._PropertyName;
        public IBuffer.EBuffType BuffType => this._BuffType;
        public CountDownTimer Timer => this._Timer;

        public abstract void Cycle(TimeClient.DegreeTime time);
    }

    #endregion

    #region Variable

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

    [Serializable]
    public struct RoleGiverVariable : IRoleGiver, IVariable
    {
        [SerializeField]
        private RoleBasic _From;
        [SerializeField]
        private RoleBasic _To;

        public RoleBasic From => this._From;
        public RoleBasic To => this._To;

        public RoleGiverVariable(IRoleGiver roleGiver) : this(roleGiver.From, roleGiver.To)
        {

        }

        public RoleGiverVariable(RoleBasic killed, RoleBasic slaved)
        {
            this._From = slaved;
            this._To = killed;
        }
    }

    [Serializable]
    public struct RolePropertyVariable : IProperty<float>, IRoleGiver, IVariable
    {
        [SerializeField]
        private string _Name;
        [SerializeField]
        private float _Value;
        [SerializeField]
        private RoleBasic _From;
        [SerializeField]
        private RoleBasic _To;
        [SerializeField]
        private EValueType _ValueType;

        public string Name => this._Name;
        public float Value => this._Value;
        public EValueType ValueType => this._ValueType;
        public RoleBasic From => this._From;
        public RoleBasic To => this._To;

        public RolePropertyVariable(string name, float value, EValueType valueType, RoleBasic from, RoleBasic to)
        {
            this._Name = name;
            this._Value = value;
            this._ValueType = valueType;
            this._From = from;
            this._To = to;
        }

        public RolePropertyVariable(IProperty<float> property, IRoleGiver giver) : this(property.Name, property.Value, property.ValueType, giver.From, giver.To)
        {

        }

        public void SetValue(float value)
        {
            this._Value = value;
        }
    }

    public interface IRoleGiver 
    {
        public RoleBasic From { get; }
        public RoleBasic To { get; }
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
        private List<FloatPropertyList> _PropertyLists;

        public List<FloatPropertyList> PropertyLists => this._PropertyLists;
        public FloatPropertyList this[string name] => this._PropertyLists.Find(p => p.Category == name);

        public RoleProperty() : this(new FloatPropertyList[0] { })
        {
            
        }

        public RoleProperty(IEnumerable<FloatPropertyList> list) 
        {
            this._PropertyLists = new List<FloatPropertyList>(list);
        }

        public void SetValue(RoleProperty property) 
        {
            property._PropertyLists.ForEach(p =>
            {
                var list = this[p.Category];

                if (list != null) { list.SetProperty(p); }

                else { this._PropertyLists.Add(p); }
            });
        }
    }
}