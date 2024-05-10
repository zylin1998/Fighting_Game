using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Loyufei.Character
{
    public class CharacterStateCreate : CharacterEvent
    {
        public CharacterStateCreate(Mark mark, IEnumerable<RepositBinding> bindingss) 
            : base(mark)
        {
            Bindings = bindingss;
        }

        public CharacterStateCreate(Mark mark, IEnumerable<RepositBinding> bindingss, float invokeTime) 
            : base(mark, invokeTime)
        {
            Bindings = bindingss;
        }

        public IEnumerable<RepositBinding> Bindings { get; }
    }
}
