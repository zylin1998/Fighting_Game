using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom;
using Custom.Page;
using Custom.Role;
using Custom.Events;
using Custom.InputSystem;

namespace FightingGameDemo
{
    public class DemoSceneEntry : SceneEntry
    {
        [SerializeField]
        private DemoBattleDetail _BattleDetail;
        [SerializeField]
        private GameEventCollect _GameEventCollect;
        [SerializeField]
        private RoleList _Ally;
        [SerializeField]
        private RoleList _Enemy;
        [SerializeField]
        private InputList _InputList;

        public override void Load()
        {
            if (this._Ally) { RoleStorage.AddRoles(this._Ally.RolePairs); }
            if (this._Enemy) { RoleStorage.AddRoles(this._Enemy.RolePairs); }

            if (this._InputList) { InputClient.SetInput(this._InputList); }

            EventManager.Detail = this._BattleDetail;
            EventManager.GameEventCollect = this._GameEventCollect;
        }

        public override void Entry()
        {
            base.Entry();

            PageClient.OpenGroup("Demo", "Start");
        }
    }
}