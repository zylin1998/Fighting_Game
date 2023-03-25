using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Role
{
    [CreateAssetMenu(fileName = "Attack Asset", menuName = "Role Data/Action/Attack Asset", order = 1)]
    public class AttackAsset : ScriptableObject
    {
        [SerializeField]
        private string _RoleName;
        [SerializeField]
        private string _AttackName;
        [SerializeField]
        private GameObject _Prefab;
        [SerializeField]
        private AttackDetail _Detail;

        public string RoleName => this._RoleName;
        public string AttackName => this._AttackName;
        public AttackDetail Detail => this._Detail;

        public void Invoke(Transform root, IRole role)
        {
            var detail = new AttackDetail(this.Detail.ExistTime, this.Detail.Distance, this.Detail.EnableTime, role);

            ObjectCreation.CreateObject<AttackSensor>(this._Prefab, root, this.name)?.SetData(detail);
        }
    }
}