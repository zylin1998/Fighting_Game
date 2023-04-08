using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Custom.Slot;

public class VolumnSlot : Slot
{
    [SerializeField]
    private Text _Rate;
    [SerializeField]
    private Toggle _Mute;
    [SerializeField]
    private Slider _Volumn;

    public Text Rate => this._Rate;
    public Toggle Mute => this._Mute;
    public Slider Volumn => this._Volumn;

    public override void SetSlot(ISlotContent content)
    {
        if (content is VolumnRate rate)
        {
            this._Content = rate;
            this._Name.text = this.Content.Name;
            
            if (this.Content is ISlotFilling filling) { filling.Filling(this); }

            this.UpdateSlot();
        }
    }

    public override void UpdateSlot()
    {
        if (this.Content is VolumnRate rate) 
        {
            this._Rate.text = string.Format("{0}", rate.Rate * 5);
        }
    }

    public override void ClearSlot()
    {
        base.ClearSlot();

        this._Rate.text = "0";
        this._Mute.isOn = false;
        this._Volumn.value = 0;
    }
}
