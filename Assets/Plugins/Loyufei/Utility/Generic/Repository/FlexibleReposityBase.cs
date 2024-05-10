﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei
{
    public class FlexibleRepositoryBase<T> : FlexibleRepositoryBase<T, RepositBase<T>>
    {
        public FlexibleRepositoryBase() : base()
        {
            
        }

        public FlexibleRepositoryBase(int capacity) : this(capacity, false, 0)
        {
            
        }

        public FlexibleRepositoryBase(int capacity, bool limited, int maxCapacity) 
            : base(capacity, limited, maxCapacity)
        {
            
        }

        public FlexibleRepositoryBase(IEnumerable<RepositBase<T>> reposits, bool limited, int maxCapacity)
            : base(reposits, limited, maxCapacity)
        {
            
        }
    }

    public class FlexibleRepositoryBase<T, TReposit> : RepositoryBase<T, TReposit>
        , IFlexibleRepository<T>
        where TReposit : IReposit<T>
    {
        public FlexibleRepositoryBase() : base()
        {

        }

        public FlexibleRepositoryBase(int capacity) : base(capacity)
        {

        }

        public FlexibleRepositoryBase(int capacity, bool limited, int maxCapacity) : base(capacity)
        {
            _Limited = limited;
            _MaxCapacity = maxCapacity;
        }

        public FlexibleRepositoryBase(IEnumerable<TReposit> reposits, bool limited, int maxCapacity) 
            : base(reposits)
        {
            _Limited = limited;
            _MaxCapacity = maxCapacity;
        }

        [SerializeField]
        protected bool _Limited;
        [SerializeField]
        protected int _MaxCapacity;
        
        public bool Limited  => _Limited;
        
        public IEnumerable<IReposit<T>> Create(int amount)
        {
            var limit = _MaxCapacity - Capacity;
            var count = Limited ? Mathf.Clamp(amount, 0, limit) : amount;
            var expand = new TReposit[count]
                .Select(reposit => Activator.CreateInstance<TReposit>());

            _Reposits.AddRange(expand);

            return _Reposits.GetRange(count, count).OfType<IReposit<T>>();
        }
    }
}
