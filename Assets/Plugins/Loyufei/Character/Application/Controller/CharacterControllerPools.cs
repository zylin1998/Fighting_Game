using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Loyufei.Character 
{
    public class CharacterControllerPool : MemoryPool<ICharacterController>
    {
        protected override void OnDespawned(ICharacterController item)
        {
            base.OnDespawned(item);

            item.Mark = default;
            item.ForEach(b => b.UnBind());
        }
    }
}