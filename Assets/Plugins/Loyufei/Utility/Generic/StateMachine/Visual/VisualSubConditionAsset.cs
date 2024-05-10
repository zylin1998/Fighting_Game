using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Loyufei
{
    public abstract class VisualSubConditionAsset : ScriptableObject, ISetup, ISubCondition
    {
        public abstract void Setup(params object[] args);

        public abstract bool Condition();
    }
}
