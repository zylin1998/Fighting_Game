using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.Character
{
    [Serializable]
    public class FacadeEntity : Entity<string, GameObject>
    {
        public FacadeEntity(string identity, GameObject data) : base(identity, data)
        {

        }
    }
}