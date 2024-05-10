using Loyufei;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Loyufei
{
    [CreateAssetMenu(fileName = "RepositSubCondition", menuName = "Loyufei/State Machine/RepositSubCondition", order = 1)]
    public class RepositSubCondition : VisualSubConditionAsset
    {
        [SerializeField]
        private List<RepositBase<int>> _IntegerReposits;
        [SerializeField]
        private List<RepositBase<float>> _FloatReposits;
        [SerializeField]
        private List<RepositBase<bool>> _BooleanReposits;

        public override bool Condition()
        {
            throw new NotImplementedException();
        }

        public override void Setup(params object[] args)
        {
            throw new NotImplementedException();
        }

        public class SubCondition : VisualSubCondition, ISetup<VisualStateMachine>
        {
            public class Binding
            {
                public Binding(IReposit sample)
                {
                    Sample = sample;
                    Target = default;
                }

                public IReposit Sample { get; }
                public IReposit Target { get; private set; }

                public void Bind(IReposit target)
                {
                    Target = target;
                }

                public bool Comparison()
                {
                    return Target.IsDefault() ? false : Target.Data.Equals(Sample.Data);
                }
            }

            public SubCondition(IEnumerable<IReposit> reposits)
            {
                Bindings = reposits.Select(r => new Binding(r)).ToList();
            }

            public List<Binding> Bindings { get; }

            public void Setup(VisualStateMachine stateMachine)
            {
                Bindings.ForEach(b => b.Bind(stateMachine.GetReposit(b.Sample.Identity)));
            }

            public override void Setup(params object[] args)
            {
                Setup(args.FirstOrDefault()?.To<VisualStateMachine>());
            }

            public override bool Condition()
            {
                return Bindings.All(b => b.Comparison());
            }
        }
    }
}
