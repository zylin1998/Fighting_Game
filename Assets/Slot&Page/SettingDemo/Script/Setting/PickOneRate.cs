using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Custom.Slot;

[CreateAssetMenu(fileName = "PickOne Rate", menuName = "Setting/Text/PickOne", order = 1)]
public class PickOneRate : SlotContent, ISlotFilling
{
    [System.Serializable]
    public enum ERate 
    {
        None = 0,
        Left = 1,
        Right = 2
    }

    [SerializeField]
    private string _Left;
    [SerializeField]
    private string _Right;
    [SerializeField]
    private ERate _Rate;

    public string Left => this._Left;
    public string Right => this._Right;
    public ERate Rate => this._Rate;

    public override IContentDetail Detail => throw new System.NotImplementedException();

    public void Filling(ISlot slot) 
    {
        if (slot is PickOneSlot pick) 
        {
            pick.Left.isOn = this._Rate == ERate.Left;
            pick.Right.isOn = this._Rate == ERate.Right;
            pick.Left.GetComponentInChildren<Text>().text = this._Left;
            pick.Right.GetComponentInChildren<Text>().text = this._Right;
        }
    }

    public void SetRate(ERate value) 
    {
        this._Rate = value;
    }
}
