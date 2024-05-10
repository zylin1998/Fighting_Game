using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.DomainEvents;

namespace Loyufei.Inventory
{
    public abstract class Item : ScriptableObject, IItem
    {
        [SerializeField]
        protected string _Identity;
        [SerializeField]
        protected string _Name;
        [SerializeField]
        protected string _Category;
        [SerializeField]
        protected Sprite _Icon;
        [SerializeField]
        protected int    _StackLimit;
        
        public object Identity   => _Identity;
        public string Name       => _Name;
        public string Category   => _Category;
        public Sprite Icon       => _Icon;
        public int    StackLimit => _StackLimit;

        public abstract IDomainEvent OnUse { get; }
    }
}