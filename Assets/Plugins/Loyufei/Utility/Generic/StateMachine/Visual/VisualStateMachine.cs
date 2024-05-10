using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Loyufei.Character;
using UnityEngine;
using Zenject;

namespace Loyufei
{
    public class VisualStateMachine : StateMachine, ISetup
    {
        public VisualStateMachine(IEnumerable<IReposit> reposits, DiContainer container) : base()
        {
            States        = new();
            SubConditions = new();
            Reposits      = reposits
                .ToDictionary(
                    key   => key.Identity, 
                    value => (IReposit)value.To<ICloneable>().Clone());
            Container     = container;
        }

        public DiContainer                       Container     { get; }
        public Dictionary<object, IReposit>      Reposits      { get; }
        public Dictionary<object, IState>        States        { get; }
        public Dictionary<object, ISubCondition> SubConditions { get; }

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
