using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Slot;

public class WindowSlotsController : SlotFieldController
{
    [SerializeField]
    private SettingCollect _Collect;

    protected override void Start()
    {
        this.SlotSetting += (slot) =>
        {
            if (slot is DropdownSlot dropdown)
            {
                
            }
        };

        base.Start();

        this.UpdateList();
    }

    public override void UpdateList()
    {
        int i = 0;
        this.SlotField.Slots.ForEach(s =>
        {
            if (s is DropdownSlot dropdown)
            {
                dropdown.SetSlot(_Collect[i]);
                i++;
            }
        });
    }
}