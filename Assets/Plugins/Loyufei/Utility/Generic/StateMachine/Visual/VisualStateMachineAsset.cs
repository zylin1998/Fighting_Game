using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Loyufei
{
    [CreateAssetMenu(fileName = "Visual StateMachine", menuName = "Loyufei/State Machine/Visual StateMachine", order = 1)]
    public class VisualStateMachineAsset : ScriptableObject, IIdentity, IStateMachine
    {
        [Serializable]
        protected class Setting 
        {
            [SerializeField]
            protected string _InitState;
            [SerializeField]
            protected List<string> _IntegerReposits;
            [SerializeField]
            protected List<string> _FloatReposits;
            [SerializeField]
            protected List<string> _BooleanReposits;
            [SerializeField]
            protected List<VisualTransitionAsset> _Transitions;

            public string InitState => _InitState;

            public IEnumerable<IReposit> Reposits 
                => new List<IReposit>()
                        .Concat(_IntegerReposits.Select(r => new RepositBase<int>  (r, 0)))
                        .Concat(_FloatReposits.  Select(r => new RepositBase<float>(r, 0f)))
                        .Concat(_BooleanReposits.Select(r => new RepositBase<bool> (r, false)));

            public IEnumerable<VisualTransitionAsset> Transitions => _Transitions;
        }
        
        [SerializeField]
        protected string  _Identity;
        [SerializeField]
        protected Setting _Setting;

        #region StateMachine

        public StateInfo                            Current       { get; protected set; }
        public Dictionary<IState, List<Transition>> Transitions   { get; } = new();

        #endregion

        public DiContainer                          Container     { get; protected set; }
        public Dictionary<object, IState>           States        { get; } = new();
        public Dictionary<object, ISubCondition>    SubConditions { get; } = new();
        public Dictionary<object, IReposit>         Reposits      { get; protected set; }

        public object Identity => _Identity;

        public VisualStateAsset InitState 
            => _Setting.Transitions.FirstOrDefault(t => t.From.Identity.Equals(_Setting.InitState)).From;

        public void SetState(IState state)
        {
            if (Transitions.TryGetValue(state, out var transitions))
            {
                Current = new(state, transitions);

                Current.State.OnEnter();
            }
        }

        [Inject]
        public virtual void Construction(DiContainer container) 
        {
            Container = container;

            Reposits = _Setting.Reposits.ToDictionary(key => key.Identity);

            foreach (var transition in _Setting.Transitions)
            {
                this.AddTransition(transition);
            }

            SetState(States[_Setting.InitState]);
        }

        public virtual void Setup(params object[] args)
        {
            Transitions.Keys.ForEach(s => s.To<ISetup>().Setup(this));

            SubConditions.Values.ForEach(c => c.To<ISetup>()?.Setup(this));
        }

        public IReposit GetReposit(object identity)
        {
            return Reposits.TryGetValue(identity, out var reposit) ? reposit : default;
        }

        public void Binding(IEnumerable<RepositBinding> bindings)
        {
            bindings.ForEach(binding =>
            {
                var reposit = GetReposit(binding.Identity);

                binding.Bind(reposit);
            });
        }
    }
}