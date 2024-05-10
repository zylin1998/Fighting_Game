using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei
{
    public interface IForm : IIdentity
    {
        public object this[string uuid] { get; }

        public IEnumerable<TOutput> GetInfos<TOutput>(Func<IIdentity, TOutput> predicate);
    }

    public interface IForm<TItem> : IForm where TItem : IIdentity
    {
        public new TItem this[string uuid] { get; }

        object IForm.this[string uuid] => this[uuid];

        public IEnumerable<TOutput> GetInfos<TOutput>(Func<TItem, TOutput> predicate);

        IEnumerable<TOutput> IForm.GetInfos<TOutput>(Func<IIdentity, TOutput> predicate)
            => GetInfos(item => predicate(item));
    }
}
