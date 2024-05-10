using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei
{
    [Serializable]
    public class VisualTransitionAsset : ISetup
    {
        [Serializable]
        public struct ToState
        {
            [SerializeField]
            private VisualStateAsset _To;
            [SerializeField]
            private List<VisualSubConditionAsset> _SubConditions;

            public VisualStateAsset To => _To;

            public IEnumerable<VisualSubConditionAsset> SubConditions => _SubConditions;
        }

        [SerializeField]
        private VisualStateAsset _From;
        [SerializeField]
        private List<ToState> _ToStates;

        public VisualStateAsset          From     => _From;
        public IEnumerable<ToState> ToStates => _ToStates;

        public void Setup(params object[] args)
        {
            From.To<ISetup>()?.Setup(args);
        }
    }
}
