using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei
{
    public interface IMultiplexer
    {
        public IIdentity this[object identify]    { get; }
        public IIdentity this[IIdentity identify] { get; }

        public bool TryGet(object    identify, out IIdentity result);
        public bool TryGet(IIdentity identify, out IIdentity result);
    }

    public interface IMultiplexer<T> : IMultiplexer where T : IIdentity
    {
        public new T this[object identify] { get; }
        public new T this[IIdentity identify] { get; }

        public bool TryGet(object    identify, out T result);
        public bool TryGet(IIdentity identify, out T result);

        IIdentity IMultiplexer.this[object    identify] => this[identify];
        IIdentity IMultiplexer.this[IIdentity identify] => this[identify];

        bool IMultiplexer.TryGet(object       identify, out IIdentity result)
            => TryGet(identify, out result);
        bool IMultiplexer.TryGet(IIdentity identify, out IIdentity result) 
            => TryGet(identify, out result);
    }
}
