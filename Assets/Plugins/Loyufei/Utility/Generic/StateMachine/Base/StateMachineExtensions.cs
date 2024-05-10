using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Codice.CM.Common.CmCallContext;

namespace Loyufei
{
    public static class StateMachineExtensions
    {
        public static Transition Transfer(this StateInfo self)
        {
            return self.Transitions.FirstOrDefault(t => t.Condition.Invoke());
        }

        public static void Tick(this IStateMachine self)
        {
            var state = self.Current.State;

            state.Tick();
        }

        public static void TickAndTransfer(this IStateMachine self, params object[] args)
        {
            var state = self.Current.State;

            state.Tick();

            var transition = self.Current.Transfer();

            if (!transition.IsDefault())
            {
                state.OnExit();

                self.SetState(transition.To);
            }
        }

        public static void Transfer(this IStateMachine self)
        {
            var state      = self.Current.State;
            var transition = self.Current.Transfer();

            if (!transition.IsDefault())
            {
                state.OnExit();

                self.SetState(transition.To);
            }
        }

        public static void AddTransition(this IStateMachine self, IState from, IState to, Func<bool> condition)
        {
            self.AddTransition(from, new Transition(to, condition));
        }

        public static void AddTransition(this IStateMachine self, IState from, Transition transition)
        {
            var transitions = self.Transitions.GetorAdd(from, () => new());

            transitions.Add(transition);
        }
    }
}
