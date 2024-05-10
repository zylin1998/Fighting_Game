using System;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei.Character
{
    public class CalculateStatIncrease : CharacterStatEvent
    {
        public CalculateStatIncrease(Mark mark, StatVariable variable, Action<VariableResponse> onResponse) 
            : base(mark, variable, onResponse)
        {

        }

        public CalculateStatIncrease(Mark mark, StatVariable variable, Action<VariableResponse> onResponse, float invokeTime)
            : base(mark, variable, onResponse, invokeTime)
        {

        }
    }
}