using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Loyufei;

namespace FightingGame.Player
{
    [Serializable]
    public class InventoryData : RepositoryBase<int,  RepositBase<int, int>>
    {
        public InventoryData() : base() { }

        public InventoryData(int capacity) : base(capacity) { }

        public InventoryData(IEnumerable<RepositBase<int, int>> reposits) : base(reposits) { }
    }
}