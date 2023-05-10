using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom;
using Custom.Item;
using Custom.Role;

namespace FightingGameDemo
{
    [CreateAssetMenu(fileName = "Health Item", menuName = "Items/Health Item", order = 1)]
    public class HealthItem : ItemBase
    {
        [SerializeField]
        private HealthItemDetail _Detail;

        protected override ItemDetail itemDetail => this._Detail;

        public override IEnumerable<IItemUsed.ItemEffect> ItemEffects => new IItemUsed.ItemEffect[1] { this._Detail.ItemEffect };

        public override void Used()
        {
            this._Detail.ItemEffect.Invoke();
        }

        [System.Serializable]
        public class HealthItemDetail : ItemDetail
        {
            [SerializeField]
            private HealInstance _ItemEffect;

            public HealInstance ItemEffect => this._ItemEffect;

            public override string GetDetail()
            {
                var variable = this._ItemEffect.Variable;

                var parameter = new object[6]
                {
                    this._Name,
                    this._PerchasePrice,
                    this._SellPrice,
                    variable.Name,
                    variable.Value,
                    variable.ValueType
                };

                var format = !string.IsNullOrEmpty(this._Format) ? this._Format : "{0}\n{1}\n{2}\n{3}\n{4}\n{5}";

                return string.Format(format, parameter);
            }
        }
    }

    [System.Serializable]
    public class HealInstance : IItemUsed.ItemEffect
    {
        [SerializeField]
        private FloatVariable _Variable;

        public FloatVariable Variable => this._Variable;

        public override void Invoke()
        {
            var role = DemoBattleRule.Player;

            var roleGiver = new RoleGiverVariable(role, role);

            role.HealthChange(new RolePropertyVariable(this._Variable, roleGiver));
        }
    }
}