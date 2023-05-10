using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Slot;

public class SampleSlot : Slot
{ 
    public override void SetSlot(ISlotContent content)
    {
        if (content is SampleItem sample) 
        {
            this.Content = sample;
        }

        this.UpdateSlot();
    }

    public override void UpdateSlot()
    {
        base.UpdateSlot();

        this.gameObject.SetActive(this.Content != null);
    }
}
