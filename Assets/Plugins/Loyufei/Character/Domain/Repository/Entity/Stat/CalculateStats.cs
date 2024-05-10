using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.Character
{
    public class CalculateStats : IdentifiedRepository<float, CalculateStat>
    {
        public CalculateStats(string identity, IEnumerable<CalculateStat> stats) : base(identity, stats)
        {

        }
    }
}