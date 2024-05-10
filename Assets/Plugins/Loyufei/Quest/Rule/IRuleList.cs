using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei.Quest
{
    public interface IRuleList
    {
        public RuleMapping Mapping { get; }
    }

    public class RuleMapping : Dictionary<Type, List<IRule>>
    {
    }
}
