using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Role
{
    [CreateAssetMenu(fileName = "Property Increase", menuName = "Property/Increase", order = 1)]
    public class PropertyIncrease : ScriptableObject
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
            private float _Limit;
            [SerializeField, Range(0f, 1f)]
            private float _FillAmount;

            public string PropertyName => this._PropertyName;
            public float Init => this._Init;
            public float Slope => this._Slope;
            public float Limit => this._Limit;
            public float FillAmount => this._FillAmount;

            public float GetValue(int value)
            {
                var temp = this._Init * (1f + this._Slope * 0.01f * (value - 1));

                if (this._Limit == 0) { return temp; }

                return temp <= this._Limit ? temp : this._Limit;
            }

            public FloatProperty GetProperty(int value) 
            {
                var pValue = this.GetValue(value);
                var pRange = new Vector2(0, pValue);

                return new FloatProperty(this._PropertyName, this._FillAmount * pValue, pRange);
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

            public FloatPropertyList GetProperty(int value) 
            {
                var properties = this._Properties.ConvertAll(c => c.GetProperty(value));

                return new FloatPropertyList(this._GroupName, properties);
            }
        }

        #endregion

        #region Value

        [System.Serializable]
        public struct Value
        {
            [SerializeField]
            private List<FloatPropertyList> _PropertyList;

            public List<FloatPropertyList> PropertyList => this._PropertyList;

            public Value(IEnumerable<FloatPropertyList> list) 
            {
                this._PropertyList = new List<FloatPropertyList>(list);
            }
        }

        #endregion
    }
}