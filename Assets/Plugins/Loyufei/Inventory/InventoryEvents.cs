using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Loyufei.DomainEvents;
using UnityEngine;

namespace Loyufei.Inventory
{
    public interface IItemRepositEvent : IDomainEvent 
    {
        float IDomainEvent.InvokeTime => Time.realtimeSinceStartup;
    }

    #region Increase

    public struct ItemIncreaseEvent : IItemRepositEvent
    {
        public ItemIncreaseEvent(object identify, int amount)
        {
            Identify = identify;
            Amount   = amount;
        }

        public object Identify { get; }
        public int    Amount   { get; }
    }

    public struct ItemIncreaseAtEvent : IItemRepositEvent
    {
        public ItemIncreaseAtEvent(object identify, int amount, int index)
        {
            Identify = identify;
            Amount   = amount;
            Index    = index;
        }

        public object Identify { get; }
        public int    Amount   { get; }
        public int    Index    { get; }
    }

    public struct IncreaseOverflowEvent : IItemRepositEvent
    {
        public IncreaseOverflowEvent(IItem item, int remain)
        {
            Item   = item;
            Remain = remain;
        }

        public IItem  Item   { get; }
        public int    Remain { get; }
    }

    #endregion

    #region Decrease

    public struct ItemDecreaseEvent : IItemRepositEvent
    {
        public ItemDecreaseEvent(object identify, int amount)
        {
            Identify = identify;
            Amount   = amount;
        }

        public object Identify { get; }
        public int    Amount   { get; }
    }

    public struct ItemDecreaseAtEvent : IItemRepositEvent
    {
        public ItemDecreaseAtEvent(object identify, int amount, int index)
        {
            Identify = identify;
            Amount   = amount;
            Index    = index;
        }

        public object Identify { get; }
        public int    Amount   { get; }
        public int    Index    { get; }
    }

    public struct DecreaseOverflowEvent : IItemRepositEvent
    {
        public DecreaseOverflowEvent(IItem item, int remain)
        {
            Item   = item;
            Remain = remain;
        }

        public IItem  Item   { get; }
        public int    Remain { get; }
    }

    #endregion

    #region SearchInfo

    public struct InventoryInfoRequest : IItemRepositEvent
    {
        public InventoryInfoRequest(object category, object source)
        {
            Category = category;
            Source   = source;
        }

        public object Category { get; }
        public object Source   { get; }
    }

    public struct InventoryInfoResponse : IItemRepositEvent
    {
        public InventoryInfoResponse(IEnumerable<(int index, IItem item, int count)> itemInfos, object category)
        {
            Category  = category;
            ItemInfos = itemInfos.ToList();
            Capacity  = ItemInfos.Count;
        }

        public object Category { get; }
        public int    Capacity { get; }
        public List<(int index, IItem item, int count)> ItemInfos { get; }
    }

    public struct SearchItemRequest : IItemRepositEvent
    {
        public SearchItemRequest(object identity)
        {
            Identify = identity;
        }

        public object Identify { get; }
    }

    public struct SearchItemResponse : IItemRepositEvent
    {
        public SearchItemResponse(IItem item, int count)
        {
            Item   = item;
            Count  = count;
        }

        public IItem  Item   { get; }
        public int    Count  { get; }
    }

    #endregion
}
