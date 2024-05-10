using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.Character
{
    public interface ICharacter2DFacade : ICharacterFacade
    {
        public Rigidbody2D Rigidbody   { get; }
        public Transform   CameraFocus { get; }
    }
}