using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Role;

namespace Custom.Battle
{
    public class SkillLoop : MonoBehaviour
    {
        [SerializeField]
        private SkillLoopAsset _LoopAsset;
        
        public SkillLoopAsset LoopAsset => this._LoopAsset;

        public Enemy Role { get; private set; }

        private void Awake()
        {
            this.Role = this.GetComponent<Enemy>();
        }

        public IEnumerator Progress() 
        {
            var entry = this._LoopAsset[SkillLoopAsset.ELoopType.Entry].List;
            var loop = this._LoopAsset[SkillLoopAsset.ELoopType.Loop].List;

            yield return Loop(entry);

            while (!this.Role.IsDead && this.Role.Target != null)
            {
                yield return Loop(loop);
            }
        }

        public IEnumerator Loop(IEnumerable<string> list) 
        {
            var loop = new Queue<string>(list);

            while (loop.Count > 0)
            {
                yield return UseSkill(loop.Dequeue());

                yield return new WaitForSeconds(this.LoopAsset.SpaceTime);
            }
        }

        public IEnumerator UseSkill(string attack) 
        {
            var target = this.Role.Target;
            var collect = this.Role.AttackCollect;
            var skill = collect[attack];

            yield return Toward(target, skill.Detail.Distance);

            this.Role.Attack(attack);
        }

        public IEnumerator Toward(IRole target, float stopDistance) 
        {
            var targetPosi = target.Convert<Component>().transform;
            var rolePosi = this.transform;
            var distance = Vector3.Distance(rolePosi.position, targetPosi.position);

            while (Mathf.Abs(distance) > stopDistance) 
            {
                var direct = rolePosi.position.x > targetPosi.position.x ? Vector2.left : Vector2.right;
                
                this.Role.Move(direct, false);

                distance = Vector3.Distance(rolePosi.position, targetPosi.position);

                yield return null;
            }
        }
    }
}