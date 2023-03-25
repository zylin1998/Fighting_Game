using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Custom.Role
{
    public class RolePool : MonoBehaviour, IBeginClient
    {
        private static Transform allyRoot;
        private static Transform enemyRoot;

        public static Dictionary<string, ObjectPool<RoleBasic>> Ally { get; private set; }
        public static Dictionary<string, ObjectPool<RoleBasic>> Enemy { get; private set; }

        private void Awake()
        {
            BeginClient.AddBegin(this);

            Ally = new Dictionary<string, ObjectPool<RoleBasic>>();
            Enemy = new Dictionary<string, ObjectPool<RoleBasic>>();

            allyRoot = new GameObject("Ally").transform;
            enemyRoot = new GameObject("Enemy").transform;

            allyRoot.SetParent(this.transform);
            enemyRoot.SetParent(this.transform);
        }

        public void BeforeBegin() { }

        public void BeginAction() { }

        public static void AddRoles(RequireRole requires) 
        {
            requires.RequireRoles.ForEach(r => AddRoles(r.Requires, r.Team));
        }

        public static void AddRoles(IEnumerable<RequireRole.RequireAmount> list, IRole.ETeamState teamState) 
        {
            list.ToList().ForEach(r => AddRole(r, teamState));
        }

        public static void AddRole(RequireRole.RequireAmount amount, IRole.ETeamState teamState) 
        {
            if (teamState == IRole.ETeamState.None) { return; }

            var pair = RoleStorage.GetPair(amount.RoleName);
            var key = pair.ID;
            var value = pair.Role;
            var parent = teamState == IRole.ETeamState.Ally ? allyRoot : enemyRoot;
            var dictionary = teamState == IRole.ETeamState.Ally ? Ally : Enemy;

            if (value is IPoolItem)
            {
                var items = ObjectCreation.CreateObjects<RoleBasic, IRole>(amount.Amount, pair.Prefab, parent, key);
                var pool = new ObjectPool<RoleBasic>(items);
                
                dictionary.Add(key, pool);
            }
        }

        public static RoleBasic Spawn<TData>(string id, TData data, IRole.ETeamState teamState) 
        {
            if (teamState == IRole.ETeamState.None) { return null; }

            var dictionary = teamState == IRole.ETeamState.Ally ? Ally : Enemy;
            var pool = default(ObjectPool<RoleBasic>);

            if (dictionary.TryGetValue(id, out pool)) { return pool.Spawn(data); }

            return null;
        }

        public static TRole Spawn<TRole, TData>(string name, TData data, IRole.ETeamState teamState) where TRole : RoleBasic
        {
            return Spawn(name, data, teamState) is TRole enemy ? enemy : default(TRole);
        }

        public static void Recycle(RoleBasic role, IRole.ETeamState teamState) 
        {
            var dictionary = teamState == IRole.ETeamState.Ally ? Ally : Enemy;
            var pool = default(ObjectPool<RoleBasic>);

            if (dictionary.TryGetValue(role.RoleName, out pool)) 
            { 
                pool.Recycle(role); 
            }
        }

        private void OnDestroy()
        {
            allyRoot = null;
            enemyRoot = null;

            Ally.Clear();
            Enemy.Clear();
        }
    }
}