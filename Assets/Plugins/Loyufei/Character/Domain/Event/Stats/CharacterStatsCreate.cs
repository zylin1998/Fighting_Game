using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.Character
{
    public class CharacterStatsCreate : CharacterEvent
    {
        public CharacterStatsCreate(Mark mark) 
            : base(mark)
        {

        }

        public CharacterStatsCreate(Mark mark, float invokeTime) 
            : base(mark, invokeTime)
        {

        }
    }
}