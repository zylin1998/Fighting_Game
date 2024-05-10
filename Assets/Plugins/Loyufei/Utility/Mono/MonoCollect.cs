using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei
{
    public class MonoCollect : MonoBehaviour
    {
        [SerializeField]
        private List<Component> _Components = new List<Component>();

        public List<Component> Components => _Components;
    }
}
