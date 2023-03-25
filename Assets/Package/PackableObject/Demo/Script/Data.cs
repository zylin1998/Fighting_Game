using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.DataPacked;

namespace PackableDemo
{
    public abstract class Data : IPackableObject
    {
        public abstract string ID { get; }
        
        public abstract void SetData(IPack pack);
        public abstract void Initialize();
        public abstract void Initialize(IPack pack);
        public abstract TPack GetPack<TPack>() where TPack : IPack;

    }
}