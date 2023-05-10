using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Custom.Slot
{
    [RequireComponent(typeof(EventTrigger))]
    public abstract class Slot : MonoBehaviour, ISlot
    {
        [SerializeField]
        protected Image _Image;
        [SerializeField]
        protected Text _Name;

        private EventTrigger _EventTrigger;

        public Image Image
        {
            get
            {
                if (this._Image == null) { this._Image = this.GetComponentInChildren<Image>(); }

                return _Image;
            }
        }
        public Text Name
        {
            get
            {
                if (this._Name == null) { this._Name = this.GetComponentInChildren<Text>(); }

                return _Name;
            }
        }

        /// <summary>
        /// Islot ¤¶­±¹ê§@
        /// </summary>
        #region ISlot

        public virtual ISlotContent Content { get; protected set; }

        public EventTrigger EventTrigger
        {
            get
            {
                if (this._EventTrigger == null) { this._EventTrigger = this.GetComponent<EventTrigger>(); }

                return _EventTrigger;
            }
        }

        public virtual void SetSlot(ISlotContent content)
        {
            this.Content = content;

            this.UpdateSlot();
        }

        public virtual void UpdateSlot()
        {
            if (this.Content != null)
            {
                this.Image.sprite = this.Content.Icon;
                this.Name.text = this.Content.Name;
            }
        }

        public virtual void ClearSlot()
        {
            this.Image.sprite = null;
            this.Name.text = "None";

            this.Content = null;
        }

        #endregion
    }
}