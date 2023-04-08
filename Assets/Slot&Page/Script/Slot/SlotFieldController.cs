using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Slot
{
    public abstract class SlotFieldController : MonoBehaviour, ISlotFieldController
    {
        [SerializeField]
        protected SlotField _SlotField;
        [SerializeField]
        protected DetailField _DtailField;
        [SerializeField]
        protected List<ISlotFieldController.SlotsCapacity> _Capacity;

        public ISlotField SlotField => this._SlotField;
        public IDetailField DetailField => this.DetailField;
        private List<ISlot> slots => this.SlotField.Slots;
        public List<ISlotFieldController.SlotsCapacity> Capacity => this._Capacity;

        public System.Action<ISlot> SlotSetting { get; set; }

        protected virtual void Start()
        {
            this.SlotField.SetCapacity(this._Capacity);
            this.SlotField.Initialize(this.SlotSetting);
        }

        public virtual void RefreshList(IEnumerable<ISlotContent> contents)
        {
            var list = contents.ToList();

            if (list.Count >= slots.Count)
            {
                this.SlotField.ExpandList(this._Capacity.FirstOrDefault()._ExpandCount, this.SlotSetting);
            }

            int i = 0;
            this.slots.ForEach(s =>
            {
                if (i < list.Count) s.SetSlot(list[i]);

                else s.ClearSlot();

                i++;
            });
        }

        public virtual void UpdateList()
        {
            this.SlotField.Slots.ForEach(s => s.UpdateSlot());
        }
    }
}