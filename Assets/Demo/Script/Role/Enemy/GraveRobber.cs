using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Role;
using Custom.Battle;

namespace FightingGameDemo.Role
{
    public class GraveRobber : Enemy
    {
        private void Awake()
        {
            this.Rigidbody = this.GetComponent<Rigidbody2D>();
            this.Animator = this.GetComponentInChildren<Animator>();

            if (this.Animator) { this.AnimCheck = new AnimationCheck(this.Animator); }

            this._TeamState = IRole.ETeamState.Enemy;
            this.gameObject.layer = LayerMask.NameToLayer("Enemy");
        }

        #region IMoveAction

        public override void Move(Vector2 direct, bool sprint)
        {
            if (this.AttackState) { return; }
            if (this.IsDead) { return; }
            if (this.HurtState) { return; }
            if (!this.IsGround) { return; }

            var hori = direct.x;
            var vert = direct.y;
            
            if (hori != 0)
            {
                var speed = this.IsSprint ? this._SprintSpeed : this._WalkSpeed;

                this.transform.Translate(new Vector3(this.ContactCheck(hori * speed * Time.deltaTime), 0, 0));
                this.Flip(hori);

                this.Animator.Play(this.IsSprint ? "Run" : "Walk");
            }

            if (hori == 0) { this.Animator.Play("Idle"); }
        }

        #endregion

        #region IHurtAction

        public override void Hurt()
        {
            if (this.IsDead) { this.Death(); }

            if (this.HurtState) { return; }

            var animState = "Hurt";
            this.Animator.Play(animState);
            this.HurtState = true;

            var enumerator = AnimCheck?.AnimationEvent(
                  animState
                , () => { }
                , () => this.HurtState = false);

            StartCoroutine(enumerator);
        }

        #endregion

        #region IAttackAction

        public override void Attack(string attack)
        {
            if (this.HurtState) { return; }

            base.Attack(attack);
        }

        #endregion

        #region IDeathAction

        public override void Death() 
        {
            if (!this.IsDead) { return; }

            var animState = "Death";
            this.Animator.Play(animState);
            this.HurtState = true;

            var enumerator = AnimCheck?.AnimationEvent(
                  animState
                , () => BattleManager.RoleSlaved(this, this._TeamState)
                , () => RolePool.Recycle(this, IRole.ETeamState.Enemy));

            StartCoroutine(enumerator);
        }

        #endregion

        #region IPoolItem

        public override void Spawn<TData>(TData data)
        {
            if (data is EnemySpawn spawn) 
            {
                if (!this.Data) { this.Data = RoleStorage.GetData(this.ID); }

                var value = this.Data.GetValue(spawn.Level);

                this._Level = new LevelState(value, value, this.Data.Exp);
                this._Health = new Health(value, value);
                
                this.transform.position = spawn.Position;
                this.transform.rotation = spawn.Rotation;
                this.Flip(spawn.FlipSide);
                this.gameObject.SetActive(true);
            }
        }

        public override void Recycle()
        {
            this.AttackState = false;
            this.HurtState = false;
            this.SetTarget(null);
            this.gameObject.SetActive(false);
        }

        #endregion

        #region IFlipAction

        public override IFlipAction.EFlipSide FlipSide => this.transform.localScale.x > 0 ? IFlipAction.EFlipSide.Right : IFlipAction.EFlipSide.Left;

        public override void Flip(IFlipAction.EFlipSide flip)
        {
            var side = 0;

            if (flip == IFlipAction.EFlipSide.Left) { side = -1; }
            if (flip == IFlipAction.EFlipSide.Right) { side = 1; }

            this.Flip(side);
        }

        #endregion
    }
}