using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Slot;

[CreateAssetMenu(fileName = "Sample Item", menuName = "Items/Sample", order = 1)]
public class SampleItem : SlotContent
{
    [System.Serializable]
    public class SampleDescribe : Describe
    {
        public string _Func;

        public override string GetDetail() 
        {
            return string.Format("{0}", _Func);
        }
    }

    [SerializeField]
    private SampleDescribe _Detail;

    public override IContentDetail Detail => this._Detail;
}
