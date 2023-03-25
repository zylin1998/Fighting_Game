using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Role 
{
    public interface IMoveAction
    {
        public float WalkSpeed { get; }
        public float SprintSpeed { get; }
        public bool IsSprint { get; set; }

        public void Move(Vector2 direct) => this.Move(direct, false);
        public void Move(Vector2 direct, bool sprint);
    }

    public interface IJumpAction
    {
        public float JumpForce { get; }

        public void Jump();
    }

    public interface IHurtAction
    {
        public bool HurtState { get; }

        public void Hurt();
    }

    public interface IDeathAction 
    {
        public void Death();
    }

    public interface IAttackAction
    {
        public float CurrentDamage { get; }
        public AttackCollect AttackCollect { get; }

        public void Attack(string attack);
    }

    public interface ISkillLoop 
    {

    }

    public interface IFlipAction
    {
        [System.Serializable]
        public enum EFlipSide
        {
            Left = 0,
            Right = 1
        }

        public EFlipSide FlipSide { get; }

        public void Flip(EFlipSide flip);
        public void Flip(float flip);
    }

    public class AnimationCheck
    {
        public Animator Animator { get; set; }

        public AnimationCheck() : this(null) { }

        public AnimationCheck(Animator animator) 
        {
            this.Animator = animator;
        }

        public IEnumerator WaitAnimationEnd(string stateName, System.Action onEnd)
        {
            while (!this.Animator.GetCurrentAnimatorStateInfo(0).IsName(stateName)) { yield return null; }

            while (this.Animator.GetCurrentAnimatorStateInfo(0).IsName(stateName)) { yield return null; }

            onEnd.Invoke();
        }

        public IEnumerator AnimationEvent(string stateName, System.Action onStart, System.Action onEnd)
        {
            while (!this.Animator.GetCurrentAnimatorStateInfo(0).IsName(stateName)) { yield return null; }

            onStart.Invoke();

            var animState = this.Animator.GetCurrentAnimatorStateInfo(0);

            while (animState.IsName(stateName)) 
            {
                if (!animState.loop && animState.normalizedTime >= 1) { break; }

                animState = this.Animator.GetCurrentAnimatorStateInfo(0);

                yield return null; 
            }

            onEnd.Invoke();
        }

        public AnimatorStateInfo AnimationState(string attack) => this.AnimationState(0, attack);

        public AnimatorStateInfo AnimationState(int layer, string attack) 
        {
            if (!this.Animator) { return default(AnimatorStateInfo); }

            var state = this.Animator.GetCurrentAnimatorStateInfo(layer);
            
            return state.IsName(attack) ? state : default(AnimatorStateInfo);
        }
    }
}
