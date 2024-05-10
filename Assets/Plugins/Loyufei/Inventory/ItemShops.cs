using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Loyufei.Inventory
{
    [CreateAssetMenu(fileName = "Item Shops", menuName = "Loyufei/Inventory/Item Shops", order = 1)]
    public class ItemShops : ScriptableObject, IMultiplexer<ItemShops.PurchaseForm>, IExtract
    {
        [SerializeField]
        private string         _Identity;
        [SerializeField]
        private string         _RepositoryIdentify;
        [SerializeField]
        private PurchaseForm[] _Forms;
        
        public string Identity => _Identity;

        public PurchaseForm this[object identify] 
            => _Forms.FirstOrDefault(f => f.Identity.Equals(identify));

        public PurchaseForm this[IIdentity identify]
            => _Forms.FirstOrDefault(f => f.Identity.Equals(identify.Identity));

        public bool TryGet(object identify, out PurchaseForm result)
        {
            result = this[identify];

            return !result.IsDefault();
        }

        public bool TryGet(IIdentity identify, out PurchaseForm result)
        {
            result = this[identify];

            return !result.IsDefault();
        }

        public object Extract()
        {
            return new PurchaseMultiplexer(_RepositoryIdentify, _Forms.Select(f => f.Extract().To<PurchaseRepository>()));
        }

        [Serializable]
        public class PurchaseForm : FormBase<Purchase>, IExtract
        {
            public PurchaseForm(string identify, IEnumerable<Purchase> items) : base(identify, items)
            {

            }

            public object Extract() 
            {
                return new PurchaseRepository(_Identity, _Items.Select(i => ((string)i.Identity, i.Count)));
            }
        }

        [Serializable]
        public class PurchaseRepository : IdentifiedRepository<int> 
        {
            public PurchaseRepository(string identify, IEnumerable<(string identify, int count)> items) 
            {
                _Reposits = items
                    .Select(i => new RepositBase<int>(i.identify, i.count))
                    .ToList();

                _Identity = identify;
            }
        }

        [Serializable]
        public class PurchaseMultiplexer : MultiplexerBase<PurchaseRepository>, IIdentity
        {
            [SerializeField]
            private string _Identity;

            public object Identity => _Identity;

            public PurchaseMultiplexer (string identity, IEnumerable<PurchaseRepository> repositories) 
            {
                _Identity = identity;
                _Elements = repositories.ToArray();
            }
        }
    }

    [Serializable]
    public class PriceInfo 
    {
        public string Identify;
        public int    Price;
    }

    [Serializable]
    public class Purchase : IIdentity
    {
        [SerializeField]
        private string    _Identity;
        [SerializeField]
        private PriceInfo _Price;
        [SerializeField]
        private int       _Count;

        public object    Identity => _Identity;
        public PriceInfo Price    => _Price;
        public int       Count    => _Count;
    }
}