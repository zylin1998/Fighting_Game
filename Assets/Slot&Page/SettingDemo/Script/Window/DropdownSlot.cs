using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Custom.Slot;

public class DropdownSlot : Slot
{
    [SerializeField]
    private Dropdown _Dropdown;

    public Dropdown Dropdown 
    {
        get 
        {
            if (_Dropdown == null) { this._Dropdown = this.GetComponentInChildren<Dropdown>(); }

            return this._Dropdown;
        }
    }

    public override void SetSlot(ISlotContent content)
    {
        if (content is ISlotFilling filling) 
        {
            base.SetSlot(content);
        }
    }

    public override void UpdateSlot()
    {
        this._Name.text = this._Content.Name;

        if (this._Content is ISlotFilling filling) 
        {
            filling.Filling(this);
        }
    }

    public override void ClearSlot()
    {
        this._Name.text = "???";

        this._Dropdown.options = new List<Dropdown.OptionData>();
    }
}
