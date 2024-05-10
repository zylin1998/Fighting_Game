using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyufei.Inventory
{
    public struct PurchaseOverflowEvent : IItemRepositEvent
    {
        public PurchaseOverflowEvent(IItem item, int remain)
        {
            Item = item;
            Remain = remain;
        }

        public IItem Item { get; }
        public int Remain { get; }
    }

    public struct PurchaseAtEvent : IItemRepositEvent
    {
        public PurchaseAtEvent(string shopName, int index, IItem currency, int price, int amount)
        {
            Currency = currency;
            ShopName = shopName;
            Price = price;
            Index = index;
            Amount = amount;
        }

        public IItem Currency { get; }
        public string ShopName { get; }
        public int Index { get; }
        public int Price { get; }
        public int Amount { get; }
    }

    public struct ShopInfoRequest : IItemRepositEvent
    {
        public ShopInfoRequest(string shopName)
        {
            ShopName = shopName;
        }

        public string ShopName { get; }
    }

    public struct ShopInfoResponse : IItemRepositEvent
    {
        public ShopInfoResponse(string shopName, IEnumerable<(IItem item, IItem currency, int info, int count)> infos)
        {
            ShopName = shopName;
            Infos = infos.ToList();
        }

        public string ShopName { get; }

        public List<(IItem item, IItem currency, int info, int count)> Infos { get; }
    }
}
