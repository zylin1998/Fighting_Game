using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Custom.Slot;

public class SampleDetail : DetailField
{
    [SerializeField]
    private Text _Name;
    [SerializeField]
    private Text _Describe;

    public override void SetDetail(ISlotContent content)
    {
        if (content is SampleItem sample) 
        {
            this.Content = sample;

            UpdateDetail();
        }
    }

    public override void UpdateDetail()
    {
        if (this.Content != null)
        {
            this._Name.text = this.Content.Name;
            this._Describe.text = this.Content.Detail.GetDetail();
        }
    }

    public override void Clear()
    {
        this.Content = null;

        this._Name.text = "???";
        this._Describe.text = "???";
    }
}
