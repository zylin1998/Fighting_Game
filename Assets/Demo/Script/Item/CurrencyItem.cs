using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Item;

namespace FightingGameDemo
{
    [CreateAssetMenu(fileName = "Currency Item", menuName = "Items/Currency Item", order = 1)]
    public class CurrencyItem : ItemBase
    {
        [SerializeField]
        private ItemDetail _Detail;

        protected override ItemDetail itemDetail => this._Detail;

        public override IEnumerable<IItemUsed.ItemEffect> ItemEffects => null;

        public override void Used() { }
        public override bool Perchase(int price) => false;
        public override bool SoldOut(int price) => false;
    }
}