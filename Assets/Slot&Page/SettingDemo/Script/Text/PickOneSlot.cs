using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Custom.Slot;

public class PickOneSlot : Slot
{
    
    [SerializeField]
    private Toggle _Left;
    [SerializeField]
    private Toggle _Right;

    public Toggle Left => this._Left;
    public Toggle Right => this._Right;

    public override void SetSlot(ISlotContent content)
    {
        if (content is PickOneRate rate) 
        {
            this.Content = rate;
            this._Name.text = content.Name;

            if (rate is ISlotFilling filling) { filling.Filling(this); }

            this.UpdateSlot();
        }
    }

    public override void UpdateSlot()
    {
        if (this.Content is PickOneRate rate)
        {
            if (rate.Rate == PickOneRate.ERate.Left) { this._Right.isOn = false; }
            if (rate.Rate == PickOneRate.ERate.Right) { this._Left.isOn = false; }
        }
    }

    public override void ClearSlot()
    {
        base.ClearSlot();

        this._Left.isOn = false;
        this._Right.isOn = false;

        this._Left.GetComponentInChildren<Text>().text = "???";
        this._Right.GetComponentInChildren<Text>().text = "???";
    }
}
