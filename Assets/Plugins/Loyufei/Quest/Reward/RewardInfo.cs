using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.Quest
{
    [Serializable]
    public class RewardInfo : RepositBase<int>
    {
        public RewardInfo(string identity, int amount) : base(identity, amount)
        {

        }

        public int Amount => Data;
    }
}