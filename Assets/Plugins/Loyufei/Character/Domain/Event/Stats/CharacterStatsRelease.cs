using System.Collections;
using System.Collections.Generic;

namespace Loyufei.Character
{
    public class CharacterStatsRelease : CharacterEvent
    {
        public CharacterStatsRelease(Mark mark) 
            : base(mark)
        {

        }

        public CharacterStatsRelease(Mark mark, float invokeTime) 
            : base(mark, invokeTime)
        {

        }
    }
}