using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Slot;

namespace Custom.Item
{
    public abstract class ItemBase : ScriptableObject, ISlotContent, IItemUsed, ITrade
    {
        [SerializeField]
        private Sprite _Icon;
        [SerializeField]
        private EItemType _ItemType;
        
        protected abstract ItemDetail itemDetail { get; }

        public Sprite Icon => this._Icon;
        public EItemType ItemType => this._ItemType;
        public string Name => this.itemDetail.Name;
        public string Currency => this.itemDetail.Currency;
        public int PerchasePrice => this.itemDetail.PerchasePrice;
        public int SellPrice => this.itemDetail.SellPrice;
        public IContentDetail Detail => this.itemDetail;
        public abstract IEnumerable<IItemUsed.ItemEffect> ItemEffects { get; }

        public virtual void PickUp() 
        {
            Inventory.AddItem(this, 1);
        }

        public virtual void PickUp(int count)
        {
            Inventory.AddItem(this, 1);
        }

        public virtual void Used() 
        {
            new List<IItemUsed.ItemEffect>(this.ItemEffects).ForEach(e => e.Invoke());

            Inventory.RemoveItem(this, 1);
        }

        public virtual bool Perchase(int count)
        {
            return Inventory.Perchase(this, count);
        }

        public virtual bool SoldOut(int count)
        {
            return Inventory.SoleOut(this, count);
        }

        [System.Serializable]
        public class ItemDetail : IContentDetail
        {
            [SerializeField]
            protected string _Name;
            [SerializeField]
            protected string _Currency;
            [SerializeField]
            protected int _PerchasePrice;
            [SerializeField]
            protected int _SellPrice;
            [SerializeField]
            protected string _Format;

            public string Name => this._Name;
            public string Currency => this._Currency;
            public int PerchasePrice => this._PerchasePrice;
            public int SellPrice => this._SellPrice;

            public virtual string GetDetail()
            {
                var parameters = new object[3] { this._Name, this._PerchasePrice, this._SellPrice };

                var format = !string.IsNullOrEmpty(this._Format) ? this._Format : "{0}\n{1}\n{2}";

                return string.Format(format, parameters);
            }
        }
    }

    public interface IItemUsed 
    {
        [System.Serializable]
        public abstract class ItemEffect 
        {
            public abstract void Invoke();
        }

        public IEnumerable<ItemEffect> ItemEffects { get; }

        public void Used();
    }

    public interface ITrade
    {
        public string Currency { get; }
        public int SellPrice { get; }
        public int PerchasePrice { get; }

        public bool Perchase(int price);
        public bool SoldOut(int price);
    }

    [System.Serializable]
    public enum EItemType 
    {
        Item = 0,
        Currency = 1,
        Parameter = 2
    }
}