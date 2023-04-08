using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Role
{
    public class WallSensor : MonoBehaviour
    {
        public Collider2D Collider { get; private set; }
        public IWallSensor Controller { get; private set; }


        private void Awake()
        {
            var parent = this.transform.parent;

            this.Collider = parent.GetComponent<Collider2D>();
            this.Controller = parent.GetComponent<IWallSensor>();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!collision.CompareTag("Ground")) { return; }

            if (collision.isTrigger) { return; }

            var point = new List<ContactPoint2D>();
            var count = this.Collider?.GetContacts(point);
            var wall = point.FindAll(p => p.normal.x != 0);
            var sides = 0f;
            
            wall.ForEach(c => sides += c.normal.x > 0 ? -1f : 1f);
            
            if (this.Controller != null && sides != 0)
            {
                var contact = IWallSensor.EContactDirect.None;
                
                if (sides < 0) { contact = IWallSensor.EContactDirect.Left; }
                if (sides > 0) { contact = IWallSensor.EContactDirect.Right; }
                
                this.Controller.Contact = contact;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.CompareTag("Ground")) { return; }

            if (this.Controller != null) { this.Controller.Contact = IWallSensor.EContactDirect.None; }
        }
    }
}