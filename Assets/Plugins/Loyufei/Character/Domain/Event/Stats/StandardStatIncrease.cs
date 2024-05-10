using System;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei.Character
{
    public class StandardStatIncrease : CharacterStatEvent
    {
        public StandardStatIncrease(Mark mark, StatVariable variable, Action<VariableResponse> onResponse) 
            : base(mark, variable, onResponse)
        {

        }

        public StandardStatIncrease(Mark mark, StatVariable variable, Action<VariableResponse> onResponse, float invokeTime)
            : base(mark, variable, onResponse, invokeTime)
        {

        }
    }
}