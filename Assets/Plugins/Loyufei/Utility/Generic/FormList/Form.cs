using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei
{
    [Serializable]
    public class FormBase<TItem> : IForm<TItem> where TItem : IIdentity
    {
        public FormBase() 
        {
            _Identity = string.Empty;
            _Items    = new();
        }

        public FormBase(string identify, IEnumerable<TItem> items)
        {
            _Identity = identify;
            _Items    = items.ToList();
        }

        [SerializeField]
        protected string      _Identity;
        [SerializeField]
        protected List<TItem> _Items;

        public object Identity => _Identity;

        public TItem this[string uuid]
            => _Items.First(item => (string)item.Identity == uuid);

        public IEnumerable<TOutput> GetInfos<TOutput>(Func<TItem, TOutput> predicate) 
        {
            return _Items.Select(item => predicate(item));
        }
    }

    [Serializable]
    public class FormBase<TItem, TItemBase> : IForm<TItemBase> 
        where TItemBase : IIdentity
        where TItem     : TItemBase
    {
        public FormBase(string category, IEnumerable<TItem> items)
        {
            _Identity = category;

            _Items = items.ToList();
        }

        [SerializeField]
        protected string _Identity;
        [SerializeField]
        protected List<TItem> _Items;

        public object Identity => _Identity;

        public TItemBase this[string identify]
            => _Items.FirstOrDefault(item => item.Identity.Equals(identify));

        public IEnumerable<TOutput> GetInfos<TOutput>(Func<TItemBase, TOutput> predicate)
        {
            return _Items.Select(item => predicate(item));
        }
    }
}
