using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Custom.Slot;

[CreateAssetMenu(fileName = "Resolution", menuName = "Setting/Window/Resolution", order = 1)]
public class Resolutions : SlotContent, ISlotFilling
{
    [System.Serializable]
    public class Option
    {
        [SerializeField]
        private int _Width;
        [SerializeField]
        private int _Height;

        public int Width => this._Width;
        public int Height => this._Height;

        public string GetDescribe()
        {
            return string.Format("{0}x{1}", this._Width, this._Height);
        }
    }

    [SerializeField]
    private List<Option> _OptionList;

    public override IContentDetail Detail => throw new System.NotImplementedException();

    public void Filling(ISlot slot)
    {
        if (slot is DropdownSlot dropdown)
        {
            dropdown.Dropdown.options = _OptionList.ConvertAll(c => new Dropdown.OptionData(c.GetDescribe()));
        }
    }
}
