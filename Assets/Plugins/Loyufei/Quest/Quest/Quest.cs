using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Loyufei.DomainEvents;

namespace Loyufei.Quest
{
    public abstract class QuestRoot : AggregateRoot, IQuest
    {
        public QuestRoot(DomainEventService service) : base(service) 
        {
            
        }

        public virtual string Chapter   
        { 
            get; 
            
            protected set; 
        }

        protected IQuestInfo _QuestInfo;

        public virtual IQuestInfo QuestInfo 
        { 
            get => _QuestInfo; 
            
            protected set => _QuestInfo = value; 
        }

        public abstract IQuestResult GetResult(EQuestState state);
    }
}