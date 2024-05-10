using System.Collections;
using System.Collections.Generic;

namespace Loyufei.Character
{
    public class StatVariable
    {
        public StatVariable(string statId, float variable)
        {
            StatId   = statId;
            Variable = variable;
        }

        public string StatId   { get; }
        public float  Variable { get; }
    }
}