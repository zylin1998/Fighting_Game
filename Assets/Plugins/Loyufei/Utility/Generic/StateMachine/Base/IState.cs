using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei
{
    public interface IState
    {
        public void Tick();
        public void OnEnter();
        public void OnExit();
    }
}