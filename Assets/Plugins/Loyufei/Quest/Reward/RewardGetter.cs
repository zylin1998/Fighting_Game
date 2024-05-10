using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Loyufei.DomainEvents;

namespace Loyufei.Quest
{
    public abstract class RewardGetter<TQuestResult> : AggregateRoot, IRewardGetter<TQuestResult> 
        where TQuestResult : IQuestResult
    {
        public RewardGetter(DomainEventService service) : base(service) 
        {
            service.Register<TQuestResult>(GetResult);
        }

        public abstract void GetResult(TQuestResult questResult);
    }
}
