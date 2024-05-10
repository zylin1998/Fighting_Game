using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.Character 
{
    public class Mark
    {
        public Mark(LayerMask layerMask, object characterId, int guidHash, object group = null)
        {
            LayerMask   = layerMask;
            CharacterID = characterId;
            GuidHash    = guidHash;
            Group       = group;
        }

        public LayerMask LayerMask   { get; }
        public object    CharacterID { get; }
        public int       GuidHash    { get; }
        public object    Group       { get; }
    }
}