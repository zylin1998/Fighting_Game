using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.DomainEvents;

namespace Loyufei.Inventory
{
    public class ShopHandler : AggregateRoot
    {
        public IMultiplexer ItemShops          { get; }
        public IMultiplexer ShopRepositories   { get; }
        public IMultiplexer ItemList           { get; }
        public IRepository  CurrencyRepository { get; }
        
        public ShopHandler(
            IMultiplexer       itemShops,
            IMultiplexer       shopRepositories,
            IMultiplexer       itemList,
            IRepository        currencyRepository,
            DomainEventService service) : base(service)
        {
            ItemShops          = itemShops;
            ShopRepositories   = shopRepositories;
            ItemList           = itemList;
            CurrencyRepository = currencyRepository;
            
            DomainEventService.Register<PurchaseAtEvent>(PurchaseAt);
            DomainEventService.Register<ShopInfoRequest>(ShopInfo);
        }

        public void ShopInfo(ShopInfoRequest request) 
        {
            var shopName   = request.ShopName;
            var repository = ShopRepositories[shopName].To<IRepository>();
            var shop       = ItemShops[shopName].To<IForm>();
            var search     = ItemList.To<ISearch<string, IItem>>();

            var purchases = repository
                .SearchAll(r => true)
                .Select(reposit =>
                {
                    var id        = (string)reposit.Identity;
                    var item      = search.Search(id);
                    var priceInfo = shop[id].To<Purchase>().Price;
                    var currency  = search.Search(priceInfo.Identify);
                    var price     = priceInfo.Price;
                    var count     = repository
                        .SearchAt(id)
                        .To<IReposit<int>>()
                        .Data;

                    return (item, currency, price, count);
                });
            
            this.SettleEvents(new ShopInfoResponse(shopName, purchases));
        }

        public bool Verify(string identify, out IItem item) 
        {
            item = ItemList
                .To<ISearch<string, IItem>>()
                .Search(identify);

            return !item.IsDefault();
        }

        public void PurchaseAt(PurchaseAtEvent purchaseAtEvent) 
        {
            var currency = purchaseAtEvent.Currency;
            var shopName = purchaseAtEvent.ShopName;
            var index    = purchaseAtEvent.Index;
            var price    = purchaseAtEvent.Price;
            var amount   = purchaseAtEvent.Amount;
            
            var repository = ShopRepositories[shopName]
                .To<IRepository>();
            var ireposit    = repository
                .SearchAt(index)
                .To<IReposit<int>>();
            var creposit    = CurrencyRepository
                .SearchAt((string)currency.Identity)
                .To<IReposit<int>>()
                .Data;
            
            if (!Verify((string)ireposit.Identity, out var item)) { return; }

            var reduce   = Mathf.Clamp(amount, 0, ireposit.Data);
            var submit   = Mathf.Clamp(reduce, 0, creposit / price);
            var preserve = ireposit.Data - submit;

            ireposit.Preserve(preserve);
            
            if(ireposit.Data == 0) { repository.Sort(Sort); }

            AddEvent(new ItemIncreaseEvent((string)item.Identity  , submit));
            AddEvent(new ItemDecreaseEvent((string)currency.Identity , submit * price));
            
            if (preserve < amount)
            {
                AddEvent(new PurchaseOverflowEvent(item, amount - preserve));
            }

            DomainEventService.Post(this);
        }

        protected static int Sort(IIdentity i1, IIdentity i2) 
        {
            var data1  = i1.To<IReposit<int>>().Data;
            var data2  = i2.To<IReposit<int>>().Data;
            var id1    = (string)i1.Identity;
            var id2    = (string)i2.Identity;

            if (data1 == 0 && data2 > 0) { return 1; }

            if (data1 == data2) { return string.Compare(id1, id2); }

            return 0;
        }
    }
}