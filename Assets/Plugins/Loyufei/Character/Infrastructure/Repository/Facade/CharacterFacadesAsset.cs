using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Loyufei.Character
{
    [CreateAssetMenu(fileName = "Character Facades Asset", menuName = "Loyufei/Character/Facades")]
    public class CharacterFacadesAsset : BindableEntityFormAsset<GameObject, FacadeEntity>
    {
        public override void BindToContainer(DiContainer container, object group = null)
        {
            container
                .Bind<CharacterFacadeHandler>()
                .AsCached()
                .WithArguments(group, this)
                .NonLazy();
        }
    }
}