using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei
{
    [Serializable]
    public class RepositoryBase<T, TReposit> : IRepository<T>
        where TReposit : IReposit<T>
    {
        public RepositoryBase()
        {
            _Reposits = new();
        }

        public RepositoryBase(int capacity)
        {
            _Reposits = new TReposit[capacity].Select(r => Activator.CreateInstance<TReposit>()).ToList();
        }

        public RepositoryBase(IEnumerable<TReposit> reposits) 
        {
            _Reposits = reposits.ToList();
        }

        [SerializeField]
        protected List<TReposit> _Reposits;

        public int Capacity => _Reposits.Count;

        public IReposit<T> Search(Func<IReposit<T>, bool> condition)
        {
            return _Reposits.Find(x => condition.Invoke(x));
        }

        public IReposit<T> SearchAt(int index)
        {
            if (index >= _Reposits.Count) { return default; }

            return _Reposits[index];
        }

        public IReposit<T> SearchAt(object identify)
        {
            return _Reposits.Find(x => x.Identity.Equals(identify));
        }

        public IEnumerable<IReposit<T>> SearchAll(Func<IReposit<T>, bool> condition)
        {
            return _Reposits.Where(x => condition.Invoke(x)).OfType<IReposit<T>>();
        }

        public IEnumerable<IReposit<T>> SearchAll()
        {
            return _Reposits.OfType<IReposit<T>>();
        }

        public void Sort(Comparison<IReposit<T>> comparison)
        {
            _Reposits.Sort((reposit1, reposit2) => comparison.Invoke(reposit1, reposit2));
        }

        public bool Insert(int index, IReposit<T> reposit, bool replace) 
        {
            if (index >= _Reposits.Count) { return false; }

            var target   = _Reposits[index];
            var isEmpty  = string.IsNullOrEmpty(target.Identity.To<string>());
            var preserve = isEmpty || replace;
            
            if (preserve) 
            {
                target.SetIdentify(reposit.Identity);
                
                target.Preserve(target.Data);
            }

            return preserve;
        }
    }

    [Serializable]
    public class RepositoryBase<T> : RepositoryBase<T, RepositBase<T>>
    {
        public RepositoryBase() : base()
        {

        }

        public RepositoryBase(int capacity) : base(capacity)
        {

        }

        public RepositoryBase(IEnumerable<RepositBase<T>> reposits) : base(reposits)
        {

        } 
    }
}
