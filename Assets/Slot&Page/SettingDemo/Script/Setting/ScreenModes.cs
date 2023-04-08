using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Custom.Slot;

[CreateAssetMenu(fileName = "Screen Mode", menuName = "Setting/Window/Screen Mode", order = 1)]
public class ScreenModes : SlotContent, ISlotFilling
{
    [System.Serializable]
    public class Option
    {
        [SerializeField]
        private FullScreenMode _ScreenMode;
        [SerializeField]
        private string _Display;

        public FullScreenMode ScreenMode => _ScreenMode;

        public string GetDescribe()
        {
            return string.Format("{0}", _Display);
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
