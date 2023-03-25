using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FightingGameDemo;

namespace Custom.Role
{
    public class RoleStorage : MonoBehaviour
    {
        private void Awake()
        {
            this.Dictionary = new Dictionary<string, RolePair>();
        }

        public Dictionary<string, RolePair> Dictionary { get; private set; }

        #region Add

        public static bool AddRole(IRole role, RoleData data) 
        {
            var prefab = role.Convert<Component>().gameObject;

            if (prefab == null) { return false; }

            return AddRole(new RolePair(role, data, prefab));
        }

        public static bool AddRole(RolePair pair) 
        {
            var dataID = pair.Data.ID;
            var roleID = pair.Role.RoleName;

            if(dataID != roleID) { return false; }

            return Singleton<RoleStorage>.Instance.Dictionary.TryAdd(roleID, pair);
        }

        public static int AddRoles(IEnumerable<RolePair> roles) 
        {
            var success = 0;

            foreach (RolePair role in roles) 
            {
                if (role.IsEmpty) { continue; }

                if (Singleton<RoleStorage>.Instance.Dictionary.TryAdd(role.ID, role)) { success++; }
            }

            return success;
        }

        #endregion

        #region Get

        public static IRole GetRole(string id) 
        {
            return GetPair(id).Role;
        }

        public static RoleData GetData(string id)
        {
            return GetPair(id).Data;
        }

        public static RolePair GetPair(string id) 
        {
            return Singleton<RoleStorage>.Instance.Dictionary.GetValueOrDefault(id);
        }

        #endregion

        public void OnDestroy()
        {
            Singleton<RoleStorage>.RemoveClient();
        }

        #region RolePair

        public struct RolePair 
        {
            public IRole Role { get; private set; }
            public RoleData Data { get; private set; }
            public GameObject Prefab { get; private set; }

            public string ID => this.Role?.RoleName;
            public bool IsEmpty => this.Role == null && this.Data == null && this.Prefab == null;

            public RolePair(RoleList.Pair pair) : this(pair.Role, pair.RoleData, pair.Prefab) { }

            public RolePair(IRole role, RoleData data, GameObject prefab) 
            {
                var correctID = role.RoleName == data.ID;

                this.Role = correctID ? role : null;
                this.Data = correctID ? data : null;
                this.Prefab = correctID ? prefab : null;
            }
        }

        #endregion
    }
}
