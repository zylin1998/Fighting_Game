using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Role
{
    [CreateAssetMenu(fileName = "Role List", menuName = "Role Data/Role List", order = 1)]
    public class RoleList : ScriptableObject
    {
        #region Pair

        [System.Serializable]
        public class Pair 
        {
            [SerializeField]
            private GameObject _Prefab;
            [SerializeField]
            private PropertyIncrease _RoleData;

            public IRole Role => this._Prefab.GetComponent<IRole>();
            public GameObject Prefab => this._Prefab;
            public PropertyIncrease RoleData => this._RoleData;
        }

        #endregion

        [SerializeField]
        private List<Pair> _Roles;

        public IEnumerable<RoleStorage.RolePair> RolePairs => this._Roles.ConvertAll(c => new RoleStorage.RolePair(c));
    }
}