using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Slot;

public class VolumnSlotsCtrl : SlotFieldController
{
    [SerializeField]
    private SettingCollect _Collect;

    protected override void Start()
    {
        this.SlotSetting += (slot) =>
        {
            if (slot is VolumnSlot volumn)
            {
                if (volumn.Content is VolumnRate rate) 
                {
                    volumn.Volumn.onValueChanged.AddListener((v) => 
                    {
                        rate.SetRate(System.Convert.ToInt32(v));
                        volumn.UpdateSlot();
                    });
                    volumn.Mute.onValueChanged.AddListener((m) => rate.SetMute(m));
                }
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
            if (s is VolumnSlot volumn)
            {
                volumn.SetSlot(_Collect[i]);
                volumn.UpdateSlot();
                this.SlotSetting.Invoke(volumn);
                i++;
            }
        });
    }
}
