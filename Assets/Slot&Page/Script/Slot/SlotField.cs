using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Slot
{
    public abstract class SlotField : MonoBehaviour, ISlotField
    {
        [SerializeField]
        protected Transform _Content;

        protected List<ISlot> _Slots = new List<ISlot>();

        protected List<ISlotFieldController.SlotsCapacity> _Capacity;

        #region ISlotField

        public Transform Content => this._Content;
        public List<ISlot> Slots => this._Slots;

        public List<ISlotFieldController.SlotsCapacity> Capacity => this._Capacity;

        public virtual void Initialize()
        {
            this._Capacity.ForEach(c => this.ExpandList(c._DefaultCount, c._SlotPrefab));
        }

        public virtual void Initialize(System.Action<ISlot> setting)
        {
            this._Capacity.ForEach(c => this.ExpandList(c._DefaultCount, c._SlotPrefab, setting));
        }

        public virtual void SetCapacity(IEnumerable<ISlotFieldController.SlotsCapacity> capacity)
        {
            this._Capacity = capacity.ToList();
        }

        public virtual ISlot CreateSlot()
        {
            return this.CreateSlot(this._Capacity.FirstOrDefault()._SlotPrefab);
        }

        public virtual ISlot CreateSlot(GameObject prefab)
        {
            return Instantiate(prefab, this._Content).GetComponent<ISlot>();
        }

        public virtual IEnumerable<ISlot> CreatList(int count)
        {
            return new ISlot[count].Select(s => s = this.CreateSlot());
        }

        public virtual IEnumerable<ISlot> CreatList(int count, GameObject prefab)
        {
            return new ISlot[count].Select(s => s = this.CreateSlot(prefab));
        }

        public virtual void ExpandList(int count)
        {
            this.ExpandList(count, (slot) => { });
        }

        public virtual void ExpandList(int count, GameObject prefab)
        {
            this.ExpandList(count, prefab, (slot) => { });
        }

        public virtual void ExpandList(int count, System.Action<ISlot> setting)
        {
            var expand = new List<ISlot>(this.CreatList(count));

            expand.ForEach(e => setting.Invoke(e));

            this._Slots.AddRange(expand);
        }

        public virtual void ExpandList(int count, GameObject prefab, System.Action<ISlot> setting)
        {
            var expand = new List<ISlot>(this.CreatList(count, prefab));

            expand.ForEach(e => setting?.Invoke(e));

            this._Slots.AddRange(expand);
        }

        #endregion
    }
}