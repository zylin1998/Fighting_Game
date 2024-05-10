using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Loyufei.Character
{
    public interface ICharacterController : IEnumerable<RepositBinding>
    {
        public Mark Mark { get; set; }
    }

    public class CharacterController : ICharacterController 
    {
        public CharacterController() 
        {
            _Bindings = new();
        }

        public CharacterController(IEnumerable<RepositBinding> bindings) 
        {
            _Bindings = bindings.ToDictionary(k => k.Identity);
        }

        protected Dictionary<object, RepositBinding> _Bindings;

        public Mark Mark { get; set; }

        public IEnumerator<RepositBinding> GetEnumerator() 
        {
            return _Bindings.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}