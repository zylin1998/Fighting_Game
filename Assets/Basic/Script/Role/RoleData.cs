using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Role
{
    [CreateAssetMenu(fileName = "Role Data", menuName = "Role Data/Role", order = 1)]
    public class RoleData : ScriptableObject
    {
        [SerializeField]
        private string _RoleName;
        [SerializeField]
        private IncreaseValue _Exp;
        [SerializeField]
        private IncreaseValue _HP;
        [SerializeField]
        private IncreaseValue _MP;
        [SerializeField]
        private IncreaseValue _Damage;
        [SerializeField]
        private IncreaseValue _Defend;

        public string ID => this._RoleName;
        public IncreaseValue Exp => this._Exp;
        public IncreaseValue HP => this._HP;
        public IncreaseValue MP => this._MP;
        public IncreaseValue Damage => this._Damage;
        public IncreaseValue Defend => this._Defend;

        public Value GetValue(int value)
        {
            var exp = this._Exp.GetValue(value);
            var hp = this._HP.GetValue(value);
            var mp = this._MP.GetValue(value);
            var damage = this._Damage.GetValue(value);
            var defend = this._Defend.GetValue(value);
            
            return new Value(value, exp, hp, mp, damage, defend);
        }

        #region IncreaseValue

        [System.Serializable]
        public class IncreaseValue
        {
            [SerializeField]
            private float _Init;
            [SerializeField]
            private float _Slope;

            public float Init => this._Init;
            public float Slope => this._Slope;

            public float GetValue(int value) => this._Init * (1f + this._Slope * 0.01f * (value - 1)); 
        }

        #endregion

        #region Value

        [System.Serializable]
        public struct Value : IHealth, ILevel
        {
            public int Level { get; private set; }
            public float Exp { get; private set; }
            public float HP { get; private set; }
            public float MP { get; private set; }
            public float Damage { get; private set; }
            public float Defend { get; private set; }

            public Value(int level, float exp, float hp, float mp, float damage, float defend) 
            {
                this.Level = level;
                this.Exp = exp;
                this.HP = hp;
                this.MP = mp;
                this.Damage = damage;
                this.Defend = defend;
            }
        }

        #endregion
    }
}