using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.Character
{
    public interface IEnemyFacade : ICharacterFacade
    {
        public Transform Target { get; set; }
    }
}