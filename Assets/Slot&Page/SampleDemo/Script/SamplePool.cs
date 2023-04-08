using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Slot;

[CreateAssetMenu(fileName = "Sample Pool", menuName = "Content Pool/ Sample", order = 1)]
public class SamplePool : ContentPool
{
    [SerializeField]
    private List<SampleItem> _Contents;

    public override IEnumerable<ISlotContent> Contents => this._Contents;
}
