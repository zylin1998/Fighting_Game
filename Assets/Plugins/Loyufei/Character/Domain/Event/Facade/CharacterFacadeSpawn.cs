using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.Character 
{
    public class CharacterFacadeSpawn : CharacterEvent 
    {
        public struct SpawnOption 
        {
            public SpawnOption(Transform parent, Vector3 position, Quaternion rotation) 
            {
                Parent   = parent;
                Position = position;
                Rotation = rotation;
            }

            public Transform  Parent   { get; }
            public Vector3    Position { get; }
            public Quaternion Rotation { get; }
        }

        public CharacterFacadeSpawn(Mark mark, SpawnOption option)
            : base(mark)
        {
            Option = option;
        }

        public CharacterFacadeSpawn(Mark mark, SpawnOption option, float invokeTime)
            : base(mark, invokeTime)
        {
            Option = option;
        }

        public SpawnOption Option { get; }
    }
}