using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Role
{
    [CreateAssetMenu(fileName = "Role Data", menuName = "Role Data/Role", order = 1)]
    public class RoleData : ScriptableObject
    {
        [SerializeField]
        private string _ID;
        [SerializeField]
        private List<IncreaseGroup> _PropetyGroups;

        public string ID => this._ID;
        public List<IncreaseGroup> PropertyGroups => this._PropetyGroups;

        public IncreaseGroup this[string name] => this._PropetyGroups.Find(p => p.GroupName == name);

        public Value GetValue(int value)
        {
            return new Value(this._PropetyGroups.ConvertAll(c => c.GetProperty(value)));
        }

        #region IncreaseValue

        [System.Serializable]
        public class IncreaseValue
        {
            [SerializeField]
            private string _PropertyName;
            [SerializeField]
            private float _Init;
            [SerializeField]
            private float _Slope;
            [SerializeField]
            private bool _FillAmount;

            public string PropertyName => this._PropertyName;
            public float Init => this._Init;
            public float Slope => this._Slope;
            public bool FillAmount => this._FillAmount;

            public float GetValue(int value) => this._Init * (1f + this._Slope * 0.01f * (value - 1));

            public Property GetProperty(int value) 
            {
                var pValue = this.GetValue(value);
                var pRange = new Vector2(0, pValue);

                return new Property(this._PropertyName, this._FillAmount ? pValue : 0, pRange);
            }
        }

        [System.Serializable]
        public class IncreaseGroup 
        {
            [SerializeField]
            private string _GroupName;
            [SerializeField]
            private List<IncreaseValue> _Properties;

            public string GroupName => this._GroupName;
            public List<IncreaseValue> Properties => this._Properties;
            public IncreaseValue this[string name] => this._Properties.Find(p => p.PropertyName == name);

            public PropertyList GetProperty(int value) 
            {
                var properties = this._Properties.ConvertAll(c => c.GetProperty(value));

                return new PropertyList(this._GroupName, properties);
            }
        }

        #endregion

        #region Value

        [System.Serializable]
        public struct Value
        {
            [SerializeField]
            private List<PropertyList> _PropertyList;

            public List<PropertyList> PropertyList => this._PropertyList;

            public Value(IEnumerable<PropertyList> list) 
            {
                this._PropertyList = new List<PropertyList>(list);
            }
        }

        #endregion
    }
}