using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Loyufei.Character
{
    [CreateAssetMenu(fileName = "Character Stats Asset", menuName = "Loyufei/Character/Stats Forms", order = 1)]
    public class CharacterStatsAsset : BindableEntityFormAsset<IEnumerable<Stat>, Stats>
    {
        public override void BindToContainer(DiContainer container, object group = null) 
        {
            container
                .Bind<CharacterStatsHandler>()
                .AsCached()
                .WithArguments(group, this)
                .NonLazy();
        }
    }
}