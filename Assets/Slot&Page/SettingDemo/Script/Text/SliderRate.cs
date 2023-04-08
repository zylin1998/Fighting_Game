using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Slot;

public class SliderRate : SlotContent
{
    [SerializeField, Range(0, 20)]
    private int _Rate;

    public int Rate => this._Rate;

    public override IContentDetail Detail => throw new System.NotImplementedException();

    public void Filling(ISlot slot)
    {
        if (slot is SliderSlot slider)
        {
            slider.Slider.value = this._Rate;
        }
    }

    public void SetRate(int value)
    {
        this._Rate = value;
    }
}
