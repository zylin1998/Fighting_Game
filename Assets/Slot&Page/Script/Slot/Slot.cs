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
        protected SlotContent _Content;

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

        public ISlotContent Content => this._Content;

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
            this._Content = content as SlotContent;

            this.UpdateSlot();
        }

        public virtual void UpdateSlot()
        {
            if (this._Content)
            {
                this.Image.sprite = this._Content.Icon;
                this.Name.text = this._Content.Name;
            }
        }

        public virtual void ClearSlot()
        {
            this._Content = null;

            this.Image.sprite = null;
            this.Name.text = "None";
        }

        #endregion
    }
}