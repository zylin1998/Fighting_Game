using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.Inventory
{
    [CreateAssetMenu(fileName = "Item List", menuName = "Loyufei/Inventory/Item List", order = 1)]
    public class ItemList : ScriptableObject, IMultiplexer<IForm<IItem>>, ISearch<string, IItem>, IIdentity
    {
        [SerializeField]
        private string _Identity;
        [SerializeField]
        private List<FormBase<Item, IItem>> _Forms;

        public object Identity => _Identity;

        public IForm<IItem> this[object identify] 
            => _Forms.FirstOrDefault(form => form.Identity.Equals(identify)).To<IForm<IItem>>();

        public IForm<IItem> this[IIdentity identify]
            => _Forms.FirstOrDefault(form => form.Identity.Equals(identify.Identity)).To<IForm<IItem>>();

        public bool TryGet(object identify, out IForm<IItem> result)
        {
            result = this[identify];

            return !result.IsDefault();
        }

        public bool TryGet(IIdentity identify, out IForm<IItem> result)
        {
            result = this[identify];

            return !result.IsDefault();
        }

        public IItem Search(string input) 
        {
            foreach (var form in _Forms) 
            {
                var item = form[input];

                if (!item.IsDefault()) { return item; }
            }

            return default;
        }
    }
}