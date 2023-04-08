using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom
{
    public interface ISpawn 
    {
        public void Spawn<TItem>(TItem item) where TItem : IPoolItem;
    }

    public interface IPoolItem
    {
        public void Spawn<TData>(TData data) where TData : ISpawn;

        public void Recycle();
    }

    public class ObjectPool<TItem> where TItem : IPoolItem
    {
        public List<TItem> Pool { get; private set; }
        public bool IsEmpty => this.Pool.Count <= 0;
        public Transform Root { get; private set; }

        public ObjectPool()
        {
            this.Pool = new List<TItem>();
        }

        public ObjectPool(IEnumerable<TItem> items) : this()
        {
            this.Recycle(items);
        }

        public ObjectPool(IEnumerable<TItem> items, Transform root) : this()
        {
            this.Root = root;

            this.Recycle(items);
        }

        public TItem Spawn<TData>(TData data) where TData : ISpawn
        {
            var item = default(TItem);
            
            if (this.Pool.Count >= 1)
            {
                item = this.Pool[0];
                item.Spawn(data);

                this.Pool.Remove(item);
            }

            return item;
        }

        public TItem Spawn<TData>(TData data, Predicate<TItem> predicate) where TData : ISpawn
        {
            var item = default(TItem);

            if (this.Pool.Count >= 1)
            {
                item = this.Pool.Find(i => predicate.Invoke(i));
                item.Spawn(data);

                this.Pool.Remove(item);
            }

            return item;
        }

        public void Recycle(TItem item)
        {
            if (item != null)
            {
                this.Pool.Add(item);

                item.Recycle();

                if (this.Root && item is Component component) 
                {
                    component.transform.SetParent(this.Root);
                }
            }
        }

        public void Recycle(IEnumerable<TItem> items)
        {
            foreach (TItem item in items)
            {
                this.Recycle(item);
            }
        }
    }
}
