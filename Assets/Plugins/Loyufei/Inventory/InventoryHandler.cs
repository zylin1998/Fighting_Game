using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.DomainEvents;
using Zenject;

namespace Loyufei.Inventory
{
    public class InventoryHandler : AggregateRoot
    {
        public IMultiplexer Storage  { get; }
        public IMultiplexer ItemList { get; }

        public InventoryHandler(
            IMultiplexer    storage, 
            IMultiplexer    itemList, 
            DomainEventService service) : base(service)
        {
            Storage  = storage;
            ItemList = itemList;

            service.Register<ItemIncreaseEvent>(Increase);
            service.Register<ItemDecreaseEvent>(Decrease);
            service.Register<ItemIncreaseAtEvent>(IncreaseAt);
            service.Register<ItemDecreaseAtEvent>(DecreaseAt);
            service.Register<SearchItemRequest>(SearchItem);
            service.Register<InventoryInfoRequest>(InventoryInfo);
        }

        public bool Verify(object uuid, out IItem item)
        {
            item = ItemList.To<ISearch<string, IItem>>().Search(uuid.To<string>());

            return !item.IsDefault();
        }

        public void SearchItem(SearchItemRequest request) 
        {
            var identify = request.Identify;

            if (!Verify(identify, out var item)) { return; }

            var repository = Storage[item.Category].To<IRepository>();
            var reposits   = repository
                .SearchAll(r => r.Identity == identify)
                .OfType<IReposit<int>>();
            var sum        = reposits.Any() ? reposits.Sum(r => r.Data) : 0;

            this.SettleEvents(new SearchItemResponse(item, sum));
        }

        public void InventoryInfo(InventoryInfoRequest request)
        {
            var category = request.Category;
            var reposits = Storage[category]
                .To<IRepository>()
                .SearchAll(r => true)
                .OfType<IReposit<int>>();
            var capacity = reposits.Count();
            var search   = ItemList.To<ISearch<string, IItem>>();
            var index    = 0;
            var infos    = reposits
                .Select(r => (index++, search.Search(r.Identity.To<string>()), r.Data));

            this.SettleEvents(new InventoryInfoResponse(infos, category));
        }

        public void IncreaseAt(ItemIncreaseAtEvent increaseEvent)
        {
            var identify = increaseEvent.Identify;
            var amount   = increaseEvent.Amount;
            var index    = increaseEvent.Index;

            if (!Verify(identify, out var item)) { return; }

            var repository = Storage[item.Category].To<IRepository>();
            var reposit    = repository.SearchAt(index).To<IReposit<int>>();

            if (reposit.Identity.IsDefault()) { reposit.SetIdentify(identify); }

            var isLimit  = item.StackLimit >= 1;
            var preserve = isLimit ? Mathf.Clamp(amount, 0, item.StackLimit - reposit.Data) : amount;

            reposit.Preserve(reposit.Data + preserve);

            if (preserve < amount)
            {
                AddEvent(new IncreaseOverflowEvent(item, amount - preserve));
            }

            DomainEventService.Post(this);
        }

        public void DecreaseAt(ItemDecreaseAtEvent increaseEvent)
        {
            var identify = increaseEvent.Identify;
            var amount   = increaseEvent.Amount;
            var index    = increaseEvent.Index;

            if (!Verify(identify, out var item)) { return; }

            var repository = Storage[item.Category].To<IRepository>();
            var reposit    = repository.SearchAt(index).To<IReposit<int>>();

            if (reposit.Identity.IsDefault()) { return; }

            var reduce   = Mathf.Clamp(amount, 0, reposit.Data);
            var preserve = reposit.Data - reduce;

            reposit.Preserve(preserve);

            if (reposit.Data <= 0) { reposit.Release(); }

            if (preserve < amount)
            {
                AddEvent(new DecreaseOverflowEvent(item, amount - preserve));
            }

            DomainEventService.Post(this);
        }

        public void Increase(ItemIncreaseEvent increaseEvent)
        {
            var identify = increaseEvent.Identify;
            var amount   = increaseEvent.Amount;

            if (!Verify(identify, out var item)) { return; }

            var repository = Storage[item.Category].To<IRepository>();
            var islimit    = item.StackLimit >= 1;
            var datas      = repository
                .SearchAll(data => data.Identity == identify)
                .OfType<IReposit<int>>()
                .Where(data => islimit ? data.Data < item.StackLimit : true);

            var expectStorageSpace = GetExpectStorageSpace(amount, item.StackLimit);
            var extraSpaceAsking   = expectStorageSpace - datas.Count();

            var emptyDatas = repository.SearchAll(r => r.Identity.IsDefault());
            var overRange  = extraSpaceAsking - emptyDatas.Count();

            if (overRange > 0 && repository is IFlexibleRepository<IReposit<int>> flexible)
            {
                emptyDatas = emptyDatas.Concat(flexible.Create(overRange));
            }

            var reposits   = datas.Concat(emptyDatas.OfType<IReposit<int>>());
            var remain     = amount;
            
            foreach (var reposit in reposits)
            {
                if (remain <= 0) { break; }

                var preserve = islimit ? Mathf.Clamp(remain, 0, item.StackLimit - reposit.Data) : remain;

                if (reposit.Identity.IsDefault()) { reposit.SetIdentify(identify); }

                reposit.Preserve(reposit.Data + preserve);

                remain -= preserve;
            }

            if (remain > 0)
            {
                AddEvent(new IncreaseOverflowEvent(item, remain));
            }

            DomainEventService.Post(this);
        }

        public void Decrease(ItemDecreaseEvent decreaseEvent)
        {
            var identify = decreaseEvent.Identify;
            var amount   = decreaseEvent.Amount;

            if (!Verify(identify, out var item)) { return; }

            var repository = Storage[item.Category].To<IRepository>();
            var datas      = repository
                .SearchAll(data => data.Identity == identify)
                .OfType<IReposit<int>>();
            var remain     = Mathf.Abs(amount);

            foreach (var data in datas.Reverse())
            {
                if (remain <= 0) { break; }

                var value   = data.Data;
                var takeOut = Mathf.Clamp(remain, 0, value);

                data.Preserve(value - takeOut);

                remain -= takeOut;

                if (data.Data <= 0) { data.Release(); }
            }

            if (remain > 0)
            {
                AddEvent(new DecreaseOverflowEvent(item, remain));
            }

            DomainEventService.Post(this);
        }

        protected static int GetExpectStorageSpace(int count, int limit)
        {
            if (limit <= 0) { return 1; }

            return count / limit + ((count % limit) > 0 ? 1 : 0);
        }
    }
}