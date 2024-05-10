using Loyufei;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    [CreateAssetMenu(fileName = "QuestInfo Asset", menuName = "Fighting Game/Quest/QuestInfo", order = 1)]
    public class QuestInfoAsset : EntityFormAsset<QuestInfo, Entity<int, QuestInfo>>
    {
        
    }
}