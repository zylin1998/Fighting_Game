using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei.Quest
{
    public enum EQuestState 
    {
        None      = 0,
        Progress  = 1,
        Fulfilled = 2,
        Defeated  = 3,
    }

    public interface IRule
    {
        public EQuestState CheckRule(IQuest quest);
    }

    public interface IRule<Pending> : IRule
    {
        public EQuestState CheckRule(Pending pending);

        EQuestState IRule.CheckRule(IQuest quest)
            => quest is Pending pending ? CheckRule(pending) : EQuestState.None;
    }
}
