using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.Character
{
    public class CharacterStatEvent : CharacterEvent
    {
        public CharacterStatEvent(Mark mark, StatVariable variable, Action<VariableResponse> onResponse) 
            : base(mark)
        {
            Variable   = variable;
            OnResponse = onResponse;
        }

        public CharacterStatEvent(Mark mark, StatVariable variable, Action<VariableResponse> onResponse, float invokeTime) 
            : base(mark, invokeTime)
        {
            Variable   = variable;
            OnResponse = onResponse;
        }

        public StatVariable             Variable   { get; }
        public Action<VariableResponse> OnResponse { get; }
    }
}