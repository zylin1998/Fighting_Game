using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Role;

namespace FightingGameDemo.Role
{
    public class SwordMan : RoleBasic, IJumpAction
    {
        private void Awake()
        {
            this.Rigidbody = this.GetComponent<Rigidbody2D>();
            this.Animator = this.GetComponentInChildren<Animator>();

            if (this.Animator) { this.AnimCheck = new AnimationCheck(this.Animator); }

            this._TeamState = IRole.ETeamState.Ally;
            this.gameObject.layer = LayerMask.NameToLayer("Player");

            this.Data = RoleStorage.GetData(this.ID);

            this.Initialize();
        }

        #region IMoveAction

        public override void Move(Vector2 direct, bool sprint)
        {
            this.IsSprint = sprint;

            var hori = direct.x;
            var vert = direct.y;

            if (this.AttackState || this.IsDead) { return; }

            if (hori != 0)
            {
                var speed = IsSprint ? this._SprintSpeed : this._WalkSpeed;

                this.transform.Translate(new Vector3(this.ContactCheck(hori * speed * Time.deltaTime), 0, 0));
                this.Flip(hori * -1f);
                
                if (this.IsGround) { this.Animator.Play("Run"); }
            }

            if (hori == 0 && this.IsGround) { this.Animator.Play("Idle"); }
        }

        #endregion

        #region IJumpAction

        [SerializeField]
        private float _JumpForce;

        public float JumpForce => this._JumpForce;

        public void Jump() 
        {
            if (this.IsGround) 
            {
                this.IsGround = false;
                this.Animator.Play("Jump");
                this.Rigidbody.velocity = Vector2.zero;
                this.Rigidbody.AddForce(Vector2.up * this._JumpForce, ForceMode2D.Impulse);
            }
        }

        #endregion

        #region IHurtAction

        public override void Hurt() 
        {
            if (this.IsDead) { this.Death(); }
        }

        #endregion

        #region IDeathAction

        public override void Death()
        {
            this.gameObject.SetActive(false);
        }

        #endregion

        #region IFlipAction

        public override IFlipAction.EFlipSide FlipSide => this.transform.localScale.x > 0 ? IFlipAction.EFlipSide.Left : IFlipAction.EFlipSide.Right;

        public override void Flip(IFlipAction.EFlipSide flip)
        {
            var side = 0;

            if (flip == IFlipAction.EFlipSide.Left) { side = 1; }
            if (flip == IFlipAction.EFlipSide.Right) { side = -1; }

            this.Flip(side);
        }

        #endregion

        #region IPoolItem

        public override void Spawn<TData>(TData data)
        {
            this.gameObject.SetActive(true);
        }

        public override void Recycle()
        {
            this.gameObject.SetActive(false);
        }

        #endregion
    }
}