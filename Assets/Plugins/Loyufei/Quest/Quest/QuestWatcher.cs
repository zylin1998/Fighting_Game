using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Loyufei.DomainEvents;

namespace Loyufei.Quest
{
    public class QuestWatcher : AggregateRoot
    {
        public QuestWatcher(DomainEventService service, IRuleList rules) : base(service)
        {
            Mapping = rules.Mapping;

            service.Register<QuestEvent>(SetEvent);
        }

        public RuleMapping Mapping { get; }

        public void SetEvent(QuestEvent e) 
        {
            var questState = CheckRule(e.Quest);

            if (questState == EQuestState.Fulfilled || questState == EQuestState.Defeated) 
            {
                this.SettleEvents(e.Quest.GetResult(questState));
            }
        }

        public EQuestState CheckRule(IQuest quest) 
        {
            var interfaces = quest.GetType().GetInterfaces();
            
            foreach (var iface in interfaces)
            {
                var rules = Mapping.GetorReturn(iface, () => new());

                foreach (var rule in rules) 
                {
                    var result = rule.CheckRule(quest);

                    if (result == EQuestState.Fulfilled) { return EQuestState.Fulfilled; }
                    if (result == EQuestState.Defeated ) { return EQuestState.Defeated ; }
                }
            }

            return EQuestState.Progress;
        }
    }
}
