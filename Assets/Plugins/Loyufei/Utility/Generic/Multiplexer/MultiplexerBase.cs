using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei
{
    public class MultiplexerBase<T> : IMultiplexer<T> where T : IIdentity
    {
        [SerializeField]
        public T[] _Elements;

        public T this[object identity] 
            => _Elements.FirstOrDefault(element => element.Identity.Equals(identity));

        public T this[IIdentity identity]
            => _Elements.FirstOrDefault(element => element.Identity.Equals(identity.Identity));

        public bool TryGet(object identify, out T result)
        {
            result = this[identify];

            return !result.IsDefault();
        }

        public bool TryGet(IIdentity identify, out T result)
        {
            result = this[identify];

            return !result.IsDefault();
        }
    }

    public class ScriptableObjectMultiplexerBase<T> : ScriptableObject,  IMultiplexer<T> where T : IIdentity
    {
        [SerializeField]
        public T[] _Elements;

        public T this[object identity]
            => _Elements.FirstOrDefault(element => element.Identity.Equals(identity));

        public T this[IIdentity identity]
            => _Elements.FirstOrDefault(element => element.Identity.Equals(identity.Identity));

        public bool TryGet(object identify, out T result)
        {
            result = this[identify];

            return !result.IsDefault();
        }

        public bool TryGet(IIdentity identify, out T result)
        {
            result = this[identify];

            return !result.IsDefault();
        }
    }
}
