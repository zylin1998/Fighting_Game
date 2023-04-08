using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Slot;

public class PageSlot : Slot
{
    [SerializeField]
    private string _Page;
    [SerializeField]
    private string _Group;

    public string Page { get => this._Page; set => this._Page = value; }
    public string Group { get => this._Group; set => this._Group = value; }

    public override void SetSlot(ISlotContent content)
    {
        this.UpdateSlot();
    }

    public override void UpdateSlot()
    {
        this._Name.text = this.Page;
    }

    public override void ClearSlot()
    {
        this._Name.text = "???";
    }
}
