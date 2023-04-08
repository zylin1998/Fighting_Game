using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Custom.Role
{
    public class RolePool : MonoBehaviour
    {
        public static Dictionary<string, ObjectPool<RoleBasic>> Roles { get; private set; }

        public static Transform PoolRoot { get; private set; }

        private void Awake()
        {
            Roles = new Dictionary<string, ObjectPool<RoleBasic>>();
            PoolRoot = this.transform;
        }

        public static void AddRoles(RequireRole requires) 
        {
            requires.RequireRoles.ForEach(r => AddRoles(r.Requires));
        }

        public static void AddRoles(IEnumerable<RequireRole.RequireAmount> list) 
        {
            list.ToList().ForEach(r => AddRole(r));
        }

        public static void AddRole(RequireRole.RequireAmount amount) 
        {
            var pair = RoleStorage.GetPair(amount.RoleName);
            var key = pair.ID;
            var value = pair.Role;
            
            if (value is IPoolItem)
            {
                var root = new GameObject(key).transform;
                
                root.SetParent(PoolRoot);
                
                var items = ObjectCreation.CreateObjects<RoleBasic, IRole>(amount.Amount, pair.Prefab, root, key);
                var pool = new ObjectPool<RoleBasic>(items);
                
                Roles.Add(key, pool);
            }
        }

        public static RoleBasic Spawn<TData>(string id, TData data) where TData : ISpawn
        {
            var pool = default(ObjectPool<RoleBasic>);

            if (Roles.TryGetValue(id, out pool)) { return pool.Spawn(data); }

            return null;
        }

        public static TRole Spawn<TRole, TData>(string name, TData data) where TRole : RoleBasic where TData : ISpawn
        {
            return Spawn(name, data) is TRole enemy ? enemy : default(TRole);
        }

        public static void Recycle(RoleBasic role) 
        {
            var pool = default(ObjectPool<RoleBasic>);

            if (Roles.TryGetValue(role.RoleName, out pool)) 
            { 
                pool.Recycle(role); 
            }
        }

        private void OnDestroy()
        {
            PoolRoot = null;

            Roles.Clear();
        }
    }
}