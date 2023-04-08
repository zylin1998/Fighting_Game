using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Slot;

public class TextSlotsCtrl : SlotFieldController
{
    [SerializeField]
    private SettingCollect _Collect;

    protected override void Start()
    {
        this.SlotSetting += (slot) =>
        {
            if (slot is SliderSlot ss && ss.Content is SliderContent sc)
            {
                ss.Slider.onValueChanged.AddListener((v) =>
                {
                    sc.SetRate(v);
                    ss.UpdateSlot();
                });
            }

            if (slot is PickOneSlot ps && ps.Content is PickOneRate pr) 
            {
                void Event(PickOneRate.ERate r)
                {
                    pr.SetRate(r);
                    ps.UpdateSlot();
                }

                ps.Left.onValueChanged.AddListener((v) =>
                {
                    if (v) { Event(PickOneRate.ERate.Left); }
                });
                ps.Right.onValueChanged.AddListener((v) =>
                {
                    if (v) { Event(PickOneRate.ERate.Right); }
                });
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
            s.SetSlot(_Collect[i]);
            this.SlotSetting.Invoke(s);
            i++;
        });
    }
}
