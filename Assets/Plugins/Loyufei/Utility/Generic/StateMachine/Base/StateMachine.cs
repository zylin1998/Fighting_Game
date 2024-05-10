using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei
{
    public class StateMachine : IStateMachine
    {
        public StateMachine()
        {
            Transitions = new();
        }

        public StateInfo Current { get; protected set; }
        public Dictionary<IState, List<Transition>> Transitions { get; }

        public void SetState(IState state)
        {
            if (Transitions.TryGetValue(state, out var transitions))
            {
                Current = new(state, transitions);

                Current.State.OnEnter();
            }
        }
    }
}
