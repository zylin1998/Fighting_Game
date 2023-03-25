using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.InputSystem
{
    [CreateAssetMenu(fileName = "Input List", menuName = "Custum Input/Input List", order = 1)]
    public class InputList : ScriptableObject, IInputList
    {
        [SerializeField]
        private List<AxesList> _List;

        public IEnumerable<IAxesList> List => this._List;
        public IAxesList this[RuntimePlatform platform] => this._List.Find(f => f.Platform == platform);
    }
}