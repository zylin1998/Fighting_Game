using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Item;
using Custom.Role;
using Custom.Events;

namespace FightingGameDemo
{
    [CreateAssetMenu(fileName = "Exp Item", menuName = "Items/Exp Item", order = 1)]
    public class EXPItem : ItemBase
    {
        [SerializeField]
        private ExpDetail _ItemDetail;

        protected override ItemDetail itemDetail => this._ItemDetail;

        public override IEnumerable<IItemUsed.ItemEffect> ItemEffects => null;

        public override void PickUp()
        {
            var player = DemoBattleRule.Player;

            player.SetData<IGetExp>(this._ItemDetail);

            EventManager.EventInvoke("Ally Upgrade", new RoleVariable(player));
        }

        public override void PickUp(int count) 
        {
            for(int i = 0; i < count; i++) { this.PickUp(); }
        }

        public override void Used() { }
        public override bool Perchase(int price) => false;
        public override bool SoldOut(int price) => false;

        [System.Serializable]
        public class ExpDetail : ItemDetail, IGetExp
        {
            [SerializeField]
            private float _Exp;

            public float Exp => this._Exp;

            public override string GetDetail()
            {
                var parameter = new object[4]
                {
                    this._Name,
                    this._PerchasePrice,
                    this._SellPrice,
                    this._Exp
                };

                var format = !string.IsNullOrEmpty(this._Format) ? this._Format : "{0}\n{1}\n{2}\n{3}";

                return string.Format(format, parameter);
            }
        }
    }
}