using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.InputSystem
{
    [CreateAssetMenu(fileName = "Axes List", menuName = "Custum Input/Axes List", order = 1)]
    public class AxesList : ScriptableObject, IAxesList
    {
        [SerializeField]
        private RuntimePlatform _Platform;
        [SerializeField]
        private List<InputAxes> _Axes;

        public RuntimePlatform Platform => this._Platform;
        public IEnumerable<IAxes> Axes => this._Axes;
        public IAxes this[string axes] => this._Axes.Find(f => f.AxesName == axes);
    }
}