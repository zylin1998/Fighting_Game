using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.Quest
{
    public abstract class ScriptableObjectRuleBase : ScriptableObject, IRule 
    {
        public abstract EQuestState CheckRule(IQuest quest);
    }

    public abstract class ScriptableObjectRuleBase<TPending> 
        : ScriptableObjectRuleBase, IRule<TPending>
    {
        public abstract EQuestState CheckRule(TPending pending);

        public override EQuestState CheckRule(IQuest quest)
            => quest is TPending pending ? CheckRule(pending) : EQuestState.None;
    }
}
