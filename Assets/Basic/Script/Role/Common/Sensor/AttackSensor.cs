using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Role
{
    public class AttackSensor : MonoBehaviour
    {
        private float time = 0f;
        private IAttackAction controller;

        public Collider2D Collider { get; private set; }
        public AttackDetail Detail { get; private set; }

        public float Damage => this.controller.CurrentDamage;

        private void Awake()
        {
            this.Collider = this.GetComponent<Collider2D>();
        }

        private void Start()
        {
            if (this.Collider) { this.Collider.enabled = false; }
        }

        private void Update()
        {
            this.time += Time.deltaTime;

            var role = this.Detail.Role;
            var enableTime = this.Detail.EnableTime;

            if (this.time < enableTime.Start && role.Convert<IHurtAction>().HurtState) { Destroy(this); }
            if (this.time >= enableTime.Start) { this.Collider.enabled = true; }
            if (this.time >= enableTime.End) { this.Collider.enabled = false; }
            
            if (this.time >= this.Detail.ExistTime) { Destroy(this.gameObject); }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var role = collision.GetComponent<RoleBasic>();

            if (role == null) { return; }

            if (role.TeamState == this.Detail.Role.TeamState) { return; }
            
            if (!role.IsDead) 
            {
                var damage = this.Damage;
                var healthTarget = HealthVariable.EValueTarget.HP;
                var healthType = HealthVariable.EValueType.Reduce;
                
                var variable = new HealthVariable(damage, healthTarget, healthType, this.Detail.Role);

                Battle.BattleManager.RoleHealthCal(variable, role);
            }
        }

        public void SetData(AttackDetail detail)
        {
            this.Detail = detail;
            
            this.controller = this.Detail.Role.Convert<IAttackAction>();
        }
    }

    #region Attack Detail

    [System.Serializable]
    public struct AttackDetail 
    {
        [SerializeField]
        private float _ExistTime;
        [SerializeField]
        private float _Distance;
        [SerializeField]
        private ColliderEnableTime _EnableTime;

        public float ExistTime => this._ExistTime;
        public float Distance => this._Distance;
        public ColliderEnableTime EnableTime => this._EnableTime;
        public IRole Role { get; private set; }

        public AttackDetail(float exist, float distance, ColliderEnableTime enableTime, IRole role) 
        {
            this._ExistTime = exist;
            this._Distance = distance;
            this._EnableTime = enableTime;
            this.Role = role;
        }

        public void SetRole(IRole role) { this.Role = role; }

        public override string ToString()
        {
            return string.Format("{0}\nExistTime: {1}\nEnableTime: {2}\nRole: {3}", base.ToString(), this.ExistTime, this.EnableTime.ToString(), this.Role);
        }
    }

    #endregion

    #region Collider Enable Time

    [System.Serializable]
    public struct ColliderEnableTime 
    {
        [SerializeField]
        private float _Start;
        [SerializeField]
        private float _End;

        public float Start => this._Start;
        public float End => this._End;

        public ColliderEnableTime(float start, float end) 
        {
            this._Start = start;
            this._End = end;
        }

        public override string ToString()
        {
            return string.Format("Start: {0}, End: {1}", this.Start, this.End);
        }
    }

    #endregion

    #region Variable

    [System.Serializable]
    public struct HealthVariable 
    {
        [SerializeField]
        private float _Value;
        [SerializeField]
        private EValueTarget _ValueTarget;
        [SerializeField]
        private EValueType _ValueType;

        public float Value => this._Value;
        public EValueTarget ValueTarget => this._ValueTarget;
        public EValueType ValueType => this._ValueType;

        public RoleBasic Role { get; private set; }

        public HealthVariable(float value, EValueTarget valueTarget, EValueType valueType, IRole role) : this(value, valueTarget, valueType, role.Convert<RoleBasic>()) 
        {

        }

        public HealthVariable(float value, EValueTarget valueTarget, EValueType valueType, RoleBasic role) 
        {
            this._Value = value;
            this._ValueTarget = valueTarget;
            this._ValueType = valueType;
            this.Role = role;
        }

        [System.Serializable]
        public enum EValueTarget 
        {
            HP = 0,
            MP = 1
        }

        [System.Serializable]
        public enum EValueType 
        {
            Reduce = 0,
            Increase = 1
        }
    }

    #endregion
}