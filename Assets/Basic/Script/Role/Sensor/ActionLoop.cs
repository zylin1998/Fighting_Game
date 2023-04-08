using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Role;

namespace Custom.Role
{
    public class ActionLoop : MonoBehaviour
    {
        [SerializeField]
        private ActionLoopAsset _LoopAsset;
        
        public ActionLoopAsset LoopAsset => this._LoopAsset;

        public RoleBasic Role { get; private set; }

        private void Awake()
        {
            this.Role = this.GetComponent<RoleBasic>();
        }

        public IEnumerator Progress() 
        {
            var entry = this._LoopAsset[ActionLoopAsset.ELoopType.Entry].List;
            var loop = this._LoopAsset[ActionLoopAsset.ELoopType.Loop].List;

            yield return Loop(entry);

            while (!this.Role.IsDead && this.Role.Team.Target != null)
            {
                yield return Loop(loop);
            }
        }

        public IEnumerator Loop(IEnumerable<string> list) 
        {
            var loop = new Queue<string>(list);

            while (loop.Count > 0)
            {
                yield return Attack(loop.Dequeue());

                yield return new WaitForSeconds(this.LoopAsset.SpaceTime);
            }
        }

        public IEnumerator Attack(string attack) 
        {
            var target = this.Role.Team.Target;
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