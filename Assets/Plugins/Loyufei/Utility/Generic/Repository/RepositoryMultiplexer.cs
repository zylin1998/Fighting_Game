using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei
{
    #region Identified Repository
    
    [Serializable]
    public class IdentifiedRepository<T> : IdentifiedRepository<T, RepositBase<T>>
    {
        public IdentifiedRepository() : base()
        {

        }

        public IdentifiedRepository(string identify, int capacity) : base(identify, capacity) 
        {
            
        }

        public IdentifiedRepository(string identify, IEnumerable<RepositBase<T>> reposits) 
            : base(identify, reposits)
        {
            
        }
    }

    [Serializable]
    public class IdentifiedRepository<T, TReposit> : RepositoryBase<T, TReposit>, IIdentity
        where TReposit : IReposit<T>
    {
        public IdentifiedRepository() : base() 
        {

        }

        public IdentifiedRepository(string identify, int capacity) : base(capacity)
        {
            _Identity = identify;
        }

        public IdentifiedRepository(string identify, IEnumerable<TReposit> reposits) : base(reposits)
        {
            _Identity = identify;
        }

        [SerializeField]
        protected string _Identity;

        public object Identity => _Identity;
    }

    [Serializable]
    public class IdentifiedFlexibleRepository<T> : IdentifiedFlexibleRepository<T, RepositBase<T>>, IIdentity
    {
        public IdentifiedFlexibleRepository() : base() 
        {

        }

        public IdentifiedFlexibleRepository(string identify, int capacity) : base(identify, capacity)
        {
            
        }

        public IdentifiedFlexibleRepository(string identify, int capacity, bool limit, int maxCapacity) 
            : base(identify, capacity, limit, maxCapacity)
        {
            
        }

        public IdentifiedFlexibleRepository(string identify, IEnumerable<RepositBase<T>> reposits)
            : base(identify, reposits)
        {

        }

        public IdentifiedFlexibleRepository(string identify, IEnumerable<RepositBase<T>> reposits, bool limit, int maxCapacity)
            : base(identify, reposits, limit, maxCapacity)
        {
            
        }
    }

    [Serializable]
    public class IdentifiedFlexibleRepository<T, TReposit> : FlexibleRepositoryBase<T, TReposit>, IIdentity
        where TReposit : IReposit<T>
    {
        public IdentifiedFlexibleRepository() : base() 
        {

        }

        public IdentifiedFlexibleRepository(string identify, int capacity) : base(capacity)
        {
            _Identity = identify;
        }

        public IdentifiedFlexibleRepository(string identify, int capacity, bool limit, int maxCapacity)
            : base(capacity, limit, maxCapacity)
        {
            _Identity = identify;
        }

        public IdentifiedFlexibleRepository(string identify, IEnumerable<TReposit> reposits)
            : this(identify, reposits, false, 0)
        {

        }

        public IdentifiedFlexibleRepository(string identify, IEnumerable<TReposit> reposits, bool limit, int maxCapacity)
            : base(reposits, limit, maxCapacity)
        {
            _Identity = identify;
        }

        [SerializeField]
        protected string _Identity;

        public object Identity => _Identity;
    }

    #endregion

    #region Multi Repository

    public class MultiRepository<T> 
        : MultiplexerBase<IdentifiedRepository<T>>
    {
    }

    public class MultiRepository<T, TReposit> 
        : MultiplexerBase<IdentifiedRepository<T, TReposit>> 
        where TReposit : IReposit<T>
    {
    }

    public class MultiRepository<T, TReposit, TRepository> : MultiplexerBase<TRepository>
        where TReposit : IReposit<T>
        where TRepository : IdentifiedRepository<T, TReposit>
    {
    }

    public class MultiFlexibleRepository<T>
        : MultiplexerBase<IdentifiedFlexibleRepository<T>>
    {
    }

    public class MultiFlexibleRepository<T, TReposit>
        : MultiplexerBase<IdentifiedFlexibleRepository<T, TReposit>>
        where TReposit : IReposit<T>
    {
    }

    public class MultiFlexibleRepository<T, TReposit, TRepository> : MultiplexerBase<TRepository>
        where TReposit    : IReposit<T>
        where TRepository : IdentifiedFlexibleRepository<T, TReposit>
    {
    }

    #endregion

    #region Multi ScriptableObject Repository

    public class ScriptableObjectMultiRepository<T> 
        : ScriptableObjectMultiplexerBase<IdentifiedRepository<T>> 
    {
    }

    public class ScriptableObjectMultiRepository<T, TReposit> : 
        ScriptableObjectMultiplexerBase<IdentifiedRepository<T, TReposit>>
        where TReposit : IReposit<T>
    {
    }

    public class ScriptableObjectMultiRepository<T, TReposit, TRepository>
        : ScriptableObjectMultiplexerBase<TRepository>
        where TReposit : IReposit<T>
        where TRepository : IdentifiedRepository<T, TReposit>
    {
    }

    public class ScriptableObjectMultiFlexibleRepository<T>
        : ScriptableObjectMultiplexerBase<IdentifiedFlexibleRepository<T>>
    {
    }

    public class ScriptableObjectMultiFlexibleRepository<T, TReposit> :
        ScriptableObjectMultiplexerBase<IdentifiedFlexibleRepository<T, TReposit>>
        where TReposit : IReposit<T>
    {
    }

    public class ScriptableObjectMultiFlexibleRepository<T, TReposit, TRepository> 
        : ScriptableObjectMultiplexerBase<TRepository>
        where TReposit : IReposit<T>
        where TRepository : IdentifiedFlexibleRepository<T, TReposit>
    {
    }

    #endregion
}
