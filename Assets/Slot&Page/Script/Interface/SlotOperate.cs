using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Custom.Slot
{
    public interface IContentDetail
    {
        public string GetDetail();
    }

    public interface ISlotContent
    {
        public Sprite Icon { get; }
        public string Name { get; }
        IContentDetail Detail { get; }
    }

    public interface ISlotFilling
    {
        public void Filling(ISlot slot);
    }

    public interface ISlot
    {
        public ISlotContent Content { get; }
        public EventTrigger EventTrigger { get; }

        public void SetSlot(ISlotContent content);
        public void UpdateSlot();
        public void ClearSlot();
    }

    public interface ISlotField
    {
        public Transform Content { get; }
        public List<ISlot> Slots { get; }
        public List<ISlotFieldController.SlotsCapacity> Capacity { get; }

        public void SetCapacity(IEnumerable<ISlotFieldController.SlotsCapacity> capacity);
        public void Initialize();
        public void Initialize(Action<ISlot> setting);
        public ISlot CreateSlot();
        public ISlot CreateSlot(GameObject prefab);
        public IEnumerable<ISlot> CreatList(int count);
        public IEnumerable<ISlot> CreatList(int count, GameObject prefab);
        public void ExpandList(int count);
        public void ExpandList(int count, GameObject prefab);
        public void ExpandList(int count, Action<ISlot> setting);
        public void ExpandList(int count, GameObject prefab, Action<ISlot> setting);

    }

    public interface IDetailField
    {
        public void SetDetail(ISlotContent content);
        public void UpdateDetail();
        public void Clear();
    }

    public interface ISlotFieldController
    {
        [Serializable]
        public class SlotsCapacity
        {
            public GameObject _SlotPrefab;
            public int _DefaultCount;
            public int _ExpandCount;

            public override string ToString()
            {
                return $"Default: {_DefaultCount}, Increase: {_ExpandCount}";
            }
        }

        public ISlotField SlotField { get; }
        public IDetailField DetailField { get; }
        public List<SlotsCapacity> Capacity { get; }
        public Action<ISlot> SlotSetting { get; set; }

        public void UpdateList();
        public void RefreshList(IEnumerable<ISlotContent> contents);
    }

    public interface IContentPool
    {
        public IEnumerable<ISlotContent> Contents { get; }

        public ISlotContent SearchByName(string name);
    }
}