using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.Quest
{
    [CreateAssetMenu(fileName = "Rule List", menuName = "Loyufei/Quest/Rule List", order = 1)]
    public class ScriptableObjectRuleList : ScriptableObject, IRuleList
    {
        [SerializeField]
        private List<ScriptableObjectRuleBase> _Rules;

        public RuleMapping Mapping => _Rules.TypeMapping<IRule, RuleMapping>(rule =>
        {
            var ruleGeneric = rule.GetType().GetInterfaces().First(type => type.Name == "IRule`1");

            return ruleGeneric?.GetGenericArguments().First();
        });
    }
}