using Loyufei;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FightingGame.Player
{
    [Serializable]
    public class QuestData : RepositoryBase<bool, RepositBase<int, bool>> 
    {
        public QuestData() : base() { }

        public QuestData(int capacity) : base(capacity) { }

        public QuestData(IEnumerable<RepositBase<int, bool>> reposits) : base(reposits) { }
    }
}