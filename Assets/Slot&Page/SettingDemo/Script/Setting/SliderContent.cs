using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Slot;

[CreateAssetMenu(fileName = "Slider Rate", menuName = "Setting/Text/Slider", order = 1)]
public class SliderContent : SlotContent, ISlotFilling
{
    [SerializeField]
    private Rate _Range;
    [SerializeField, Range(0, 20)]
    private float _Rate;

    public float Rate => this._Rate;
    public Rate Range => this._Range;

    public override IContentDetail Detail => throw new System.NotImplementedException();

    public void Filling(ISlot slot) 
    {
        if (slot is SliderSlot slider)
        {
            slider.Name.text = this.Name;
            slider.Slider.value = this._Rate;
        }
    }

    public void SetRate(float value) 
    {
        this._Rate = value;
    }
}
