using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyufei
{
    public interface IStateMachine
    {
        public StateInfo Current { get; }
        public Dictionary<IState, List<Transition>> Transitions { get; }

        public void SetState(IState state);
    }

    #region Data Structure

    public struct StateInfo
    {
        public IState           State { get; }
        public List<Transition> Transitions { get; }

        public StateInfo(IState state, IEnumerable<Transition> transitions)
        {
            State       = state;
            Transitions = transitions.ToList();
        }
    }

    public class Transition
    {
        public Transition() 
        {
            To        = default;
            Condition = () => false;
        }

        public Transition(IState to, Func<bool> condition)
        {
            To        = to;
            Condition = condition;
        }

        public IState     To        { get; protected set; }
        public Func<bool> Condition { get; protected set; }
    }

    #endregion
}
