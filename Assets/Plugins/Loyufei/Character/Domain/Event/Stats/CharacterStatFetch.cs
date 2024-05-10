using System;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei.Character
{
    public class CharacterStatFetch : CharacterStatEvent
    {
        public CharacterStatFetch(Mark mark, StatVariable variable, Action<VariableResponse> onResponse) 
            : base(mark, variable, onResponse)
        {

        }

        public CharacterStatFetch(Mark mark, StatVariable variable, Action<VariableResponse> onResponse, float invokeTime) 
            : base(mark, variable, onResponse, invokeTime)
        {

        }
    }
}