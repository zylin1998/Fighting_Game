using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyufei.Character
{
    public class CharacterStateRelease : CharacterEvent
    {
        public CharacterStateRelease(Mark mark) 
            : base(mark)
        {

        }

        public CharacterStateRelease(Mark mark, float invokeTime) 
            : base(mark, invokeTime)
        {

        }
    }
}
