using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.DomainEvents;

namespace Loyufei.Character 
{
    public static class CharacterServiceExtensions
    {
        public static TController CreateController<TController>(
            this CharacterService self,
                 string           characterId,
                 LayerMask        layerMask,
                 Transform        parent,
                 Vector3          position,
                 Quaternion       rotation,
                 object           group = null
            ) where TController : ICharacterController
        {
            var spawnOption = new CharacterFacadeSpawn.SpawnOption(parent, position, rotation);
            var mark        = new Mark(layerMask, characterId, Guid.NewGuid().GetHashCode(), group);

            return self.CreateController<TController>(mark, spawnOption);
        }

        public static TController CreateController<TController>(
            this CharacterService self,
                 string           characterId,
                 LayerMask        layerMask,
                 object           group = null
            ) where TController : ICharacterController
        {
            return self.CreateController<TController>(
                    characterId,
                    layerMask,
                    null,
                    Vector3.zero,
                    Quaternion.identity, 
                    group);
        }

        public static TController CreateController<TController>(
            this CharacterService self,
                 string           characterId,
                 LayerMask        layerMask,
                 Transform        parent,
                 object           group = null
            ) where TController : ICharacterController
        {
            return self.CreateController<TController>(
                    characterId,
                    layerMask,
                    parent,
                    Vector3.zero,
                    Quaternion.identity,
                    group);
        }

        public static TController CreateController<TController>(
            this CharacterService self,
                 string           characterId,
                 LayerMask        layerMask,
                 Vector3          position,
                 Quaternion       rotation,
                 object           group = null
            ) where TController : ICharacterController
        {
            return self.CreateController<TController>(
                    characterId,
                    layerMask,
                    null,
                    position,
                    rotation,
                    group);
        }

        public static void StatFetch(this CharacterService self, Mark mark, string statId, Action<VariableResponse> onResponse) 
        {
            self.StatFetch(mark, new StatVariable(statId, 0f), onResponse);
        }
    }
}