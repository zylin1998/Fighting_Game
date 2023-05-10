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
            if (this._Ally) { RoleStorage.AddRole(this._Ally.RolePairs); }
            if (this._Enemy) { RoleStorage.AddRole(this._Enemy.RolePairs); }

            if (this._InputList) { InputClient.SetInput(this._InputList); }

            if (this._BattleDetail) { EventManager.Detail = this._BattleDetail; }
            if (this._GameEventCollect) { EventManager.GameEventCollect = this._GameEventCollect; }
        }

        public override void Entry()
        {
            base.Entry();

            PageClient.OpenGroup("Demo", "Start");
        }
    }
}