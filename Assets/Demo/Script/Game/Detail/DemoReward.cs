using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Item;
using Custom;
using Custom.Events;

namespace FightingGameDemo
{
    [CreateAssetMenu(fileName = "Battle Reward", menuName = "Game/Reward/Battle Reward", order = 1)]
    public class DemoReward : RewardAsset
    {
        [SerializeField]
        private List<ItemProperty> _Items;

        public List<ItemProperty> Items => this._Items;

        public override void GetReward()
        {
            this._Items.ForEach(i => Inventory.AddItem(i.Value, i.Count));
        }

        public TItem GetItem<TItem>() where TItem : ItemBase => this._Items.Find(i => i is TItem) as TItem;
    }
}