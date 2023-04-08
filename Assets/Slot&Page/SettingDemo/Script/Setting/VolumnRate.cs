using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Slot;

[CreateAssetMenu(fileName = "Volumn Rate", menuName = "Setting/Volumn/Rate", order = 1)]
public class VolumnRate : SlotContent, ISlotFilling
{
    [SerializeField, Range(0, 20)]
    private int _Rate;
    [SerializeField]
    private bool _Mute;

    public int Rate => this._Rate;
    public bool Mute => this._Mute;

    public override IContentDetail Detail => throw new System.NotImplementedException();

    public void Filling(ISlot slot) 
    {
        if (slot is VolumnSlot volumn) 
        {
            volumn.Volumn.value = this._Rate;
            volumn.Mute.isOn = this._Mute;
        }
    }

    public void SetRate(int value) 
    {
        this._Rate = value;
    }

    public void SetMute(bool value) 
    {
        this._Mute = value;
    }
}
