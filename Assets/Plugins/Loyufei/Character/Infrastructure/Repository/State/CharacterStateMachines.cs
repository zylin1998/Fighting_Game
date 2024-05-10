using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Loyufei.Character
{
    [CreateAssetMenu(fileName = "Character State Settings", menuName = "Loyufei/Character/State/Settings")]
    public class CharacterStateMachines : BindableEntityFormAsset<CharacterStateMachine, Entity<string, CharacterStateMachine>>
    {
        public override void BindToContainer(DiContainer container, object group = null) 
        {
            container
                .Bind<CharacterStateHandler>()
                .AsCached()
                .WithArguments(group, this)
                .NonLazy();
        }
    }
}
