using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Slot;

[CreateAssetMenu(fileName = "Setting Collection", menuName = "Setting/Collection/Basic", order = 1)]
public class SettingCollect : SlotContent
{
    [SerializeField]
    private List<SlotContent> _Setting;

    public override IContentDetail Detail => throw new System.NotImplementedException();

    public ISlotContent this[int num] => _Setting[num];
    public ISlotContent this[string name] => _Setting.Find(f => f.Name == name);
}
