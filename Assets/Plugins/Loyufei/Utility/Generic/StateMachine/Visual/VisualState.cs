using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyufei
{
    public abstract class VisualState : IState, ISetup
    {
        public abstract void OnEnter();

        public abstract void OnExit();

        public abstract void Tick();

        public abstract void Setup(params object[] args);
    }
}
