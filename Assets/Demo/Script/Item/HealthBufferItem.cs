using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Item;
using Custom.Role;
using Custom;

namespace FightingGameDemo
{
    [CreateAssetMenu(fileName = "Health Buffer Item", menuName = "Items/Health Buffer Item", order = 1)]
    public class HealthBufferItem : ItemBase
    {
        [SerializeField]
        private HealthBufferDetail _ItemDetail;
        protected override ItemDetail itemDetail => throw new System.NotImplementedException();
        public override IEnumerable<IItemUsed.ItemEffect> ItemEffects => throw new System.NotImplementedException();

        public override void Used()
        {
            this._ItemDetail.ItemEffect.Invoke();
        }

        [System.Serializable]
        public class HealthBufferDetail : ItemDetail
        {
            [SerializeField]
            private HealthBufferEffect _ItemEffect;

            public HealthBufferEffect ItemEffect => this._ItemEffect;

            public override string GetDetail()
            {
                var variable = this._ItemEffect.HealthBuffer.Variable;

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
    public class HealthBufferEffect : IItemUsed.ItemEffect
    {
        [SerializeField]
        private HealthBuffer _HealthBuffer;

        public HealthBuffer HealthBuffer => this._HealthBuffer;

        public override void Invoke()
        {
            var role = DemoBattleRule.Player;

            (this._HealthBuffer as IBuffer).SetRole(role);

            //Set Buffer
        }
    }
    
    [System.Serializable]
    public class HealthBuffer : Buffer
    {
        [SerializeField]
        private FloatVariable _Variable;

        public FloatVariable Variable => this._Variable;

        public override void Cycle(TimeClient.DegreeTime time)
        {
            var target = this._Variable.Name;
            var value = this._Variable.Value * (this._Timer.Frequency / this._Timer.InitTime.SecondOnly);
            var type = this._Variable.ValueType;
            var role = (this as IBuffer).Role;

            var variable = new FloatVariable(target, value, type);
            var giver = new RoleGiverVariable(role, role);

            role.HealthChange(new RolePropertyVariable(variable, giver));
        }
    }
}