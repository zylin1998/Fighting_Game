using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Custom.Slot;
using Custom.Item;

namespace FightingGameDemo
{
    public class RewardItemSlot : Slot
    {
        [SerializeField]
        private Text _Count;
        [SerializeField]
        private ItemProperty _Item;

        public override ISlotContent Content 
        { 
            get => this._Item;

            protected set
            {
                if (value is ItemProperty item)
                {
                    this._Item = item;

                    this.UpdateSlot();
                }
            }
        }

        public Text Count => this._Count;

        public override void SetSlot(ISlotContent content)
        {
            this.Content = content;
        }

        public override void UpdateSlot()
        {
            if (this._Item != null) 
            {
                this._Image.sprite = this._Item.Icon;
                this._Count.text = string.Format("{0}",this._Item.Count);

                this.gameObject.SetActive(true);
            }

            else { this.gameObject.SetActive(false); }
        }
    }
}