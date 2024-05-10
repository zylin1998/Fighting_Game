using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.Inventory
{
    [CreateAssetMenu(fileName = "Item Storage", menuName = "Loyufei/Inventory/Item Storage", order = 1)]
    public class ItemStorage : ScriptableObject, IMultiplexer<IForm<ItemStorage.StoreData>>, IIdentity
    {
        [Serializable]
        public class StoreData : IIdentity 
        {
            [SerializeField]
            protected string _Identity;
            [SerializeField]
            protected int    _Count;

            public object Identity => _Identity;
            public int    Count    => _Count;
        }

        [Serializable]
        public class Form : FormBase<StoreData>, IExtract
        {
            public Form(string identity, IEnumerable<StoreData> items) : base(identity, items)
            {
                
            }

            public object Extract() 
            {
                var identity = _Identity;
                var reposits = _Items.Select(i => new RepositBase<int>((string)i.Identity, i.Count));

                return new Repository(identity, reposits); 
            }
        }

        [Serializable]
        public class Repository : IdentifiedRepository<int> 
        {
            public Repository(string identify, IEnumerable<RepositBase<int>> reposits) 
            {
                _Identity = identify;
                _Reposits = reposits.ToList();
            }
        }

        [Serializable]
        public class Repositories : MultiRepository<int>, IIdentity
        {
            public Repositories(string identify, IEnumerable<Repository> reposits)
            {
                _Identity = identify;
                _Elements = reposits.ToArray();
            }
            
            [SerializeField]
            private string _Identity;

            public object Identity => _Identity;
        }

        [SerializeField]
        protected string     _Identity;
        [SerializeField]
        protected List<Form> _Forms;

        public object Identity => _Identity;

        public IForm<StoreData> this[IIdentity identify] 
            => _Forms.FirstOrDefault(f => f.Identity.Equals(identify.Identity));

        public IForm<StoreData> this[object    identify]
            => _Forms.FirstOrDefault(f => f.Identity.Equals(identify));

        public bool TryGet(object identify, out IForm<StoreData> result)
        {
            result = this[identify];

            return !result.IsDefault();
        }

        public bool TryGet(IIdentity identify, out IForm<StoreData> result)
        {
            result = this[identify];

            return !result.IsDefault();
        }

        public object Extract() 
        {
            return new Repositories(_Identity, _Forms
                .Select(f => f.Extract().To<Repository>()));
        }
    }
}