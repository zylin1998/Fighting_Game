using Loyufei.DomainEvents;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei.Character 
{
    public class CharacterEvent : DomainEventBase
    {
        public CharacterEvent(Mark mark) 
            : base() 
        {
            Mark = mark;
        }

        public CharacterEvent(Mark mark, float invokeTime) 
            : base(invokeTime)
        {
            Mark = mark;
        }

        public Mark Mark { get; }
    }
}
