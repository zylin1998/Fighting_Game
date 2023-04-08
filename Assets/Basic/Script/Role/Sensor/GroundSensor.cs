using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Role
{
    public class GroundSensor : MonoBehaviour
    {
        public IGroundSensor Controller { get; private set; }

        private void Awake()
        {
            this.Controller = this.transform.parent.GetComponent<IGroundSensor>();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Ground"))
            {
                var isGround = this.Controller.IsGround;
                var velocity = this.Controller.VerticalVelocity;

                if (velocity <= 0 && !isGround)
                {
                    this.Controller.Land();
                    this.Controller.IsGround = true;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.CompareTag("Ground")) { return; }
            
            if (this.Controller != null) { this.Controller.IsGround = false; }
        }
    }
}