using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Custom.Slot;
using Custom.Page;

public class PageSlotsCtrl : SlotFieldController
{
    [SerializeField]
    private string[] pages = { "Window", "Volume", "Text" };

    protected override void Start()
    {
        this.SlotSetting += (slot) =>
        {
            if (slot is PageSlot page)
            {
                EventTrigger.Entry click = new EventTrigger.Entry();
                click.eventID = EventTriggerType.PointerClick;
                click.callback.AddListener((data) =>
                {
                    PageClient.OpenPage(page);
                });

                page.EventTrigger.triggers.Add(click);
            }
        };

        base.Start();

        this.UpdateList();
    }

    public override void UpdateList()
    {
        var group = this.GetComponent<IPageState>().GroupName;

        int i = 0;
        this.SlotField.Slots.ForEach(s => 
        {
            if (s is PageSlot page) 
            {
                page.Page = pages[i];
                page.Group = group;
                page.UpdateSlot();
                
                i++;
            }
        });
    }
}
