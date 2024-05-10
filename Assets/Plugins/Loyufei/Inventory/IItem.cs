using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.DomainEvents;

namespace Loyufei.Inventory
{
    public interface IItem : IIdentity
    {
        public string Name       { get; }
        public string Category   { get; }
        public Sprite Icon       { get; }
        public int    StackLimit { get; }
        public IDomainEvent OnUse      { get; }
    }

    public interface IPerchase 
    {
        public int PerchasePrice { get; }
    }

    public interface ISell 
    {
        public int SellingPrice { get; }
    }

    public interface IEnhance 
    {
        public void Enhance();
    }
}