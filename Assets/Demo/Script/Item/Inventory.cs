using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Slot;

namespace Custom.Item
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField]
        protected List<ItemList> _ItemLists = new List<ItemList>()
        {
            new ItemList(EItemType.Item, ItemList.EOnItemEmpty.Clear),
            new ItemList(EItemType.Currency, ItemList.EOnItemEmpty.None)
        };
        [SerializeField]
        private EventCollect<Inventory> _Collection;

        public ItemList this[EItemType itemType] => this._ItemLists.Find(l => l.Category == itemType);
        public ItemProperty this[string name, EItemType itemType] => this[itemType]?.GetProperty<ItemProperty>(name);

        protected virtual void Awake()
        {
            if (Singleton<Inventory>.Exist) 
            {
                Destroy(this.gameObject);

                return;
            }

            Singleton<Inventory>.CreateClient(new Instance(this), EDestroyType.DontDestroy);

            EventCollect<Inventory>.AddEvent("OnItemChange", () => { });

            this._Collection = EventCollect<Inventory>.Collection;
        }

        public static bool AddItem(ItemBase item, int count)
        {
            if (!item || count == 0) { return false; }

            var list = Singleton<Inventory>.Instance[item.ItemType];

            if (list == null) { return false; }

            list.SetProperty(new ItemProperty(item, count));

            EventCollect<Inventory>.Invoke("OnItemChange");

            return true;
        }

        public static bool RemoveItem(ItemBase item, int count) 
        {
            if (!item || count == 0) { return false; }

            var list = Singleton<Inventory>.Instance[item.ItemType];

            if (list == null) { return false; }

            var result = list.RemoveValue(new ItemProperty(item, count));

            if (result) { EventCollect<Inventory>.Invoke("OnItemChange"); }

            return result;
        }

        public static bool Perchase(ItemBase item, int count) 
        {
            var price = item.PerchasePrice * count;
            var instance = Singleton<Inventory>.Instance;
            var currency = instance[item.Currency, EItemType.Currency];

            if (item.PerchasePrice <= 0) { return false; }

            if (currency.Reduce(price)) { return AddItem(item, count); }

            return false;
        }

        public static bool SoleOut(ItemBase item, int count) 
        {
            var instance = Singleton<Inventory>.Instance;

            if (item.SellPrice <= 0) { return false; }

            if (RemoveItem(item, count)) 
            {
                var price = item.SellPrice * count;
                var currency = instance[item.Currency, EItemType.Currency];

                return currency.Increase(price);
            }

            return false;
        }
    }

    #region Property List

    [System.Serializable]
    public class ItemList : PropertyList<EItemType, ItemBase> 
    {
        [SerializeField]
        private EItemType _Category;
        [SerializeField]
        private EOnItemEmpty _ItemEmpty;
        [SerializeField]
        private List<ItemProperty> _Properties;

        public override EItemType Category 
        {
            get => this._Category; 
            
            protected set => this._Category = value; 
        }

        public override List<Property<ItemBase>> Properties 
        {
            get => this._Properties.ConvertAll(c => c as Property<ItemBase>); 
            
            protected set => this._Properties = value.ConvertAll(c => c as ItemProperty); 
        }

        public EOnItemEmpty ItemEmpty => this._ItemEmpty;

        public ItemList(EItemType itemType) : this(itemType, new  ItemProperty[0], EOnItemEmpty.Delete) { }
        public ItemList(EItemType itemType, EOnItemEmpty itemEmpty) : this(itemType, new  ItemProperty[0], itemEmpty) { }
        public ItemList(EItemType itemType, IEnumerable<ItemProperty> properties) : this(itemType, properties, EOnItemEmpty.Delete) { }
        public ItemList(EItemType itemType, IEnumerable<ItemProperty> properties, EOnItemEmpty itemEmpty) : base(itemType, properties) 
        {
            this._ItemEmpty = itemEmpty;
        }


        public override void SetProperty(Property<ItemBase> property)
        {
            if (property is ItemProperty item) 
            {
                var temp = this[item.Name] as ItemProperty;

                if (temp == null) 
                {
                    temp = item;

                    var empty = this._Properties.Find(p => p.IsEmpty);

                    if (empty != null) { empty.SetValue(temp); }

                    else { this._Properties.Add(temp); }
                }

                else { temp.Increase(item.Count); }
            }
        }

        public override bool RemoveValue(Property<ItemBase> property)
        {
            if (property is ItemProperty item)
            {
                var temp = this[item.Name] as ItemProperty;

                if (temp == null) { return false; }

                if (temp.Count < item.Count) { return false; }

                var result = temp.Reduce(item.Count);

                if (temp.Count <= 0)
                {
                    if (this._ItemEmpty == EOnItemEmpty.Delete) { return this._Properties.Remove(temp); }
                    
                    if (this._ItemEmpty == EOnItemEmpty.Clear) { temp.Clear(); }
                }

                return result;
            }

            return false;
        }

        [System.Serializable]
        public enum EOnItemEmpty 
        {
            None = 0,
            Clear = 1,
            Delete = 2
        }
    }

    [System.Serializable]
    public class ItemProperty : Property<ItemBase>, ISlotContent
    {
        [SerializeField]
        private int _Count;
        
        public int Count => this._Count;
        public bool IsEmpty => this._Value == null;

        #region ISlot

        public Sprite Icon => this._Value != null ? this._Value.Icon : null;
        public IContentDetail Detail => this._Value != null ? this._Value.Detail : null;

        #endregion

        public ItemProperty(ItemBase item, int count) : base(item.Name, item) 
        {
            this._Count = count;
        }

        public override void SetValue<TProperty>(TProperty property)
        {
            if (property is ItemProperty item) 
            {
                this._Name = item.Name;
                this._Value = item.Value;
                this._Count = item.Count;
            }
        }

        public bool Increase(int count) 
        {
            if (count <= 0) { return false; }

            this._Count += count;

            return true;
        }

        public bool Reduce(int count)
        {
            if (count <= 0 || count > this.Count) { return false; }

            this._Count -= count;

            return true;
        }

        public void Clear() 
        {
            this._Name = string.Empty;
            this._Value = null;
            this._Count = 0;
        }
    }

    #endregion
}