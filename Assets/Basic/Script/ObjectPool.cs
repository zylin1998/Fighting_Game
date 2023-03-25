using System;
using System.Collections;
using System.Collections.Generic;

namespace Custom
{
    public interface IPoolItem
    {
        public void Spawn<Tdata>(Tdata spawn);

        public void Recycle();
    }

    public class ObjectPool<TItem> where TItem : IPoolItem
    {
        public Queue<TItem> Pool { get; private set; }
        public bool IsEmpty => this.Pool.Count <= 0;

        public ObjectPool()
        {
            this.Pool = new Queue<TItem>();
        }

        public ObjectPool(IEnumerable<TItem> items) : this()
        {
            this.Recycle(items);
        }

        public TItem Spawn<TData>(TData data)
        {
            TItem item;
            
            if (this.Pool.TryDequeue(out item))
            {
                item.Spawn(data);
            }

            return item;
        }

        public void Recycle(TItem item)
        {
            if (item != null)
            {
                this.Pool.Enqueue(item);

                item.Recycle();
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
