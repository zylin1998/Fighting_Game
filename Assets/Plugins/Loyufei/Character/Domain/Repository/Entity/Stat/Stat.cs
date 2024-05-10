using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.Character
{
    [Serializable]
    public class Stat
    {
        [SerializeField]
        private string _Name;
        [SerializeField, Min(0f)]
        private float  _Value;

        public string Name  => _Name;
        public float  Value => Mathf.Clamp(_Value, 0, _Value);
    }
}