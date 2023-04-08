using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Custom.Slot;

public class SampleController : SlotFieldController
{
    [SerializeField]
    private SamplePool pool;

    protected override void Start()
    {
        this.SlotSetting += (slot) =>
        {
            if (slot is SampleSlot s)
            {
                EventTrigger.Entry pointEnter = new EventTrigger.Entry();
                pointEnter.eventID = EventTriggerType.PointerEnter;
                pointEnter.callback.AddListener((data) => this._DtailField.SetDetail(s.Content));

                s.EventTrigger.triggers.Add(pointEnter);
            }
        };

        base.Start();

        this.RefreshList(pool.Contents);
        this.UpdateList();
    }
}
