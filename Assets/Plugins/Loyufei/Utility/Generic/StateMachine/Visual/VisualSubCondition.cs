using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyufei
{
    public abstract class VisualSubCondition : ISubCondition, ISetup
    {
        public abstract void Setup(params object[] args);

        public abstract bool Condition();
    }
}
