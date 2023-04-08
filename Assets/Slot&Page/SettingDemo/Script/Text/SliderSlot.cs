using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Custom.Slot;

public class SliderSlot : Slot
{
    [SerializeField]
    private Text _Rate;
    [SerializeField]
    private Slider _Slider;

    public Text Rate => this._Rate;
    public Slider Slider => _Slider;

    public override void SetSlot(ISlotContent content)
    {
        if (content is SliderContent slider) 
        {
            this._Content = slider;
            this.Name.text = slider.Name;

            if (slider is ISlotFilling filling) { filling.Filling(this); }

            this.UpdateSlot();
        }
    }

    public override void UpdateSlot()
    {
        if (this._Content is SliderContent slider)
        {
            this._Rate.text = string.Format("{0}", slider.Rate * 5);
        }
    }

    public override void ClearSlot()
    {
        this._Content = null;
        this._Name.text = "???";
        this._Rate.text = "0";
    }
}
