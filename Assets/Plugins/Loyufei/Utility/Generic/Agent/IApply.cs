using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Loyufei.DomainEvents;

namespace Loyufei
{
    public interface IApply : IIdentity
    {
        public IDomainEvent GetApplyEvent();

        public void Initialize();
    }
}
