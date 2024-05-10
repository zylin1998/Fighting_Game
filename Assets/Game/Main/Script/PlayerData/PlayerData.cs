using Loyufei;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame.Player
{
    [Serializable]
    public class PlayerData : ISaveable
    {
        public PlayerData(InventoryData inventory, QuestData quest) 
        {
            _Inventory = inventory;
            _Quest     = quest;
        }

        public InventoryData _Inventory;
        public QuestData     _Quest;
    }
}