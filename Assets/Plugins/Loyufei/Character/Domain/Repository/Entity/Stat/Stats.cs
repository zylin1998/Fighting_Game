using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Loyufei.Character
{
    [CreateAssetMenu(fileName = "Character Stats", menuName = "Loyufei/Character/Stats", order = 1)]
    public class Stats : ScriptableObject, IEntity<IEnumerable<Stat>>, IExtract
    {
        [SerializeField]
        private string     _Identity;
        [SerializeField]
        private List<Stat> _Stats;

        public IEnumerable<Stat> Data     => _Stats;
        public object            Identity => _Identity;

        public object Extract() 
        {
            return new CalculateStats(_Identity, _Stats.Select(s => new CalculateStat(s)));
        }
    }
}