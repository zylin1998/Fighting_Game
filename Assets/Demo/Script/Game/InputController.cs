using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.InputSystem;
using Custom;
using Custom.Role;
using Custom.Events;

namespace FightingGameDemo
{
    public class InputController : MonoBehaviour, IInputClient, IBeginClient
    {
        public IRole Role { get; set; }

        private void Awake()
        {
            BeginClient.AddBegin(this);
        }

        #region IInputClient

        public void GetValue() 
        {
            var hori = InputClient.GetAxis("Horizontal");
            var vert = InputClient.GetAxis("Vertical");
            var jump = InputClient.GetKeyDown("Jump");
            var sprint = InputClient.GetKey("Sprint");
            var attack = InputClient.GetKeyDown("Attack");

            var direct = new Vector2(hori, vert);
            
            if (this.Role != null)
            {
                this.Role.Convert<IMoveAction>()?.Move(direct, sprint);

                if (jump) { this.Role.Convert<IJumpAction>()?.Jump(); }
                if (attack) { this.Role.Convert<IAttackAction>()?.Attack("Attack"); }
            }
        }

        #endregion

        #region IBeginAction

        public void BeforeBegin() 
        {
            InputClient.SetBasic(this);
        }

        public void BeginAction() 
        {
            this.Role = DemoBattleRule.Player;
        }

        #endregion
    }
}