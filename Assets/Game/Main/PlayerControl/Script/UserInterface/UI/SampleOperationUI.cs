using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Loyufei;

namespace FightingGame.PlayerControl.Sample
{
    public class SampleOperationUI : ITickable, IInitializable
    {
        public SampleOperationUI(SampleOperation operation, SampleOperationFacade facade)
        {
            Operation = operation;
            Facade    = facade;

            Operation.PlusCallBack  += Facade.UpdateSlot;
            Operation.MinusCallBack += Facade.UpdateSlot;
        }

        public PlayerControl2D       Player    { get; private set; }
        public SampleOperation       Operation { get; private set; }
        public SampleOperationFacade Facade    { get; private set; }

        public void Initialize()
        {
            Player = Operation.CreatePlayer();
            
            Operation.BindCamera();

            Facade.ForEach(f =>
            {
                Operation.Fetch(f.Variable.StatId, Facade.UpdateSlot);

                f.Plus .AddListener(() => Operation.Plus(f.Variable));

                f.Minus.AddListener(() => Operation.Minus(f.Variable));
            });
        }

        public void Tick() 
        {
            GetInput();
        }

        #region GetInput

        public void GetInput()
        {
            GetDirection();
            GetJump();
            GetSprint();
            GetDash();
            GetAttack();
        }

        public virtual void GetDirection()
        {
            var x = Input.GetAxis("Horizontal");

            Player.Direction = new Vector2(x, 0);
        }

        public virtual void GetJump()
        {
            Player.Jump = Input.GetKeyDown(KeyCode.Space);
        }

        public virtual void GetSprint()
        {
            Player.Sprint = Input.GetKey(KeyCode.LeftControl);
        }

        public virtual void GetDash()
        {
            Player.Dash = Input.GetKeyDown(KeyCode.LeftShift);
        }

        public virtual void GetAttack()
        {
            Player.Attack = Input.GetKeyDown(KeyCode.Q);
        }

        #endregion
    }
}