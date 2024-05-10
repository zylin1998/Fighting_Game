using System.Collections;
using System.Collections.Generic;

namespace Loyufei.Character
{
    public class CharacterFacadeDespawn : CharacterEvent
    {
        public CharacterFacadeDespawn(Mark mark)
            : base(mark)
        {

        }

        public CharacterFacadeDespawn(Mark mark, float invokeTime)
            : base(mark, invokeTime)
        {

        }
    }
}