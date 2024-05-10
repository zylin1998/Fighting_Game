using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.Character
{
    public class StandardStatDecrease : CharacterStatEvent
    {
        public StandardStatDecrease(Mark mark, StatVariable variable, Action<VariableResponse> onResponse) 
            : base(mark, variable, onResponse)
        {

        }

        public StandardStatDecrease(Mark mark, StatVariable variable, Action<VariableResponse> onResponse, float invokeTime)
            : base(mark, variable, onResponse, invokeTime)
        {

        }
    }
}