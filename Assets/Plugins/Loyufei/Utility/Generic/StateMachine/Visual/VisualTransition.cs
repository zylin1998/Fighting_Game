using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyufei
{
    public class VisualTransition : Transition
    {
        public VisualTransition(IState to, IEnumerable<ISubCondition> subconditions)
        {
            To = to;
            Subconditions = subconditions.ToList();
            Condition = () => Subconditions.All(c => c.Condition());
        }

        public List<ISubCondition> Subconditions { get; }
    }
}
