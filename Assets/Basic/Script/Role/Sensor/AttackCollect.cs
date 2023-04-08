using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Role
{
    [CreateAssetMenu(fileName = "Attack Collect", menuName = "Role Data/Action/Attack Collect", order = 1)]
    public class AttackCollect : ScriptableObject
    {
        [SerializeField]
        private List<AttackAsset> _Assets;

        public AttackAsset this[string attackName] => this._Assets.Find(a => a.AttackName == attackName);
    }
}