using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom;
using Custom.Role;
using Custom.Battle;
using Custom.InputSystem;

namespace FightingGameDemo
{
    public class GM : MonoBehaviour
    {
        private void Awake()
        {
            if (Singleton<GM>.Exist) 
            {
                Destroy(this.gameObject);
                
                return; 
            }

            Singleton<GM>.CreateClient(new Instance(this.gameObject, this), EDestroyType.DontDestroy);

            Singleton<TimeClient>.CreateClient(EDestroyType.DontDestroy);
            Singleton<RoleStorage>.CreateClient(EDestroyType.DontDestroy);
            Singleton<InputClient>.CreateClient(EDestroyType.DontDestroy);

            if (this._Ally) { RoleStorage.AddRoles(this._Ally.RolePairs); }
            if (this._Enemy) { RoleStorage.AddRoles(this._Enemy.RolePairs); }

            InputClient.SetPlatform(RuntimePlatform.WindowsPlayer);
            if (this._InputList) { InputClient.SetInput(this._InputList); }

            BattleManager.Detail = this._BattleDetail;
        }

        [SerializeField]
        private DemoBattleDetail _BattleDetail;
        [SerializeField]
        private RoleList _Ally;
        [SerializeField]
        private RoleList _Enemy;
        [SerializeField]
        private InputList _InputList;

        public void OnDestroy()
        {
            Singleton<GM>.RemoveClient();
        }
    }

}