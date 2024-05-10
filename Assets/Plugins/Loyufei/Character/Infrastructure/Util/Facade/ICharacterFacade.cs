using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.Character
{
    public interface ICharacterFacade : IFacade
    {
        public Animator Animator { get; }
        public Mark     Mark     { get; set; }
    }
}
