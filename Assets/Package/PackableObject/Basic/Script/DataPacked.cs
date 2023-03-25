using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Custom.DataPacked
{
    public interface IPack 
    {
        public IPack Packed<TData>(TData data);

        public void UnPacked<TPack, TData>(TData data, Action<TPack, TData> unpack) 
        {
            if (this is TPack pack) { unpack.Invoke(pack, data); }
        }
    }

    public interface IPackableObject 
    {
        public string ID { get; }
        
        public void SetData(IPack pack);

        public void Initialize();

        public void Initialize(IPack pack);

        public TPack GetPack<TPack>() where TPack : IPack;
    }

    public interface IPackableCollect 
    {
        public string ID { get; }
        public DataStreaming.ESaveLocate SaveLocate { get; }

        public IEnumerable<IPackableObject> Packables { get; }
        public Dictionary<string,IPackableObject> Dictionary { get; }

        public TData GetData<TData>() 
        {
            foreach (KeyValuePair<string, IPackableObject> value in this.Dictionary) 
            {
                if (value.Value is TData data) 
                {
                    return data;
                }
            }

            return default(TData);
        }

        public void SetPacks(IEnumerable<IPack> packs)
        {
            foreach (IPack pack in packs)
            {
                foreach (IPackableObject packable in this.Packables)
                {
                    packable.Initialize(pack);
                }
            }
        }

        public void SetPack<T>(IPack pack) where T : IPackableObject
        {
            var packable = this.Packables.ToList().Find(p => p is T result);

            if (packable is T result) 
            {
                result.SetData(pack);
            }
        }
    }
}