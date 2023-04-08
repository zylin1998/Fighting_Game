using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom
{
    [System.Serializable]
    public class PropertyList
    {
        [SerializeField]
        protected string _ListName;
        [SerializeField]
        protected List<Property> _Properties;

        public string ListName => this._ListName;
        public Property this[string name] => this._Properties.Find(p => p.PropertyName == name);

        #region Constructor

        public PropertyList() : this(string.Empty, new Property[0] { }) 
        {

        }

        public PropertyList(string name) : this(name, new Property[0] { }) 
        {

        }

        public PropertyList(string name, IEnumerable<Property> properties) 
        {
            this._ListName = name;
            this._Properties = new List<Property>(properties);
        }

        #endregion

        public void SetValue(IEnumerable<Property> properties) 
        {
            foreach (var property in properties) 
            {
                var temp = this[property.PropertyName];

                if (temp != null) { temp.SetValue(property); }

                else { this._Properties.Add(property); }
            }
        }

        public PropertyVariable ValueChange(PropertyVariable variable)
        {
            var value = 0f;

            var property = this[variable.ValueTarget];

            if (variable.ValueType == PropertyVariable.EValueType.Increase)
            {
                value = property.Increase(variable.Value);
            }

            if (variable.ValueType == PropertyVariable.EValueType.Reduce)
            {
                value = property.Reduce(variable.Value);
            }

            return new PropertyVariable(variable.ValueTarget, value, variable.ValueType, variable.From, variable.To);
        }
    }

    [System.Serializable]
    public class Property
    {
        [SerializeField]
        private string _PropertyName;
        [SerializeField]
        private float _Value;
        [SerializeField]
        private Vector2 _Range;

        public string PropertyName => this._PropertyName;
        public float Value => this._Value;
        public Vector2 Range => this._Range;

        public float Normalized => this._Range != Vector2.zero ? this._Value / this._Range.y : 1f;

        public Property(string name, float value, Vector2 range)
        {
            this._PropertyName = name;
            this._Value = value;
            this._Range = range;
        }

        public void SetValue(Property property) 
        {
            if (property.PropertyName == this._PropertyName) 
            {
                this.SetValue(property.Value, property.Range);
            }
        }

        public void SetValue(float value)
        {
            if (this._Range != Vector2.zero)
            {
                this._Value = Mathf.Clamp(value, this._Range.x, this._Range.y);
            }

            else this._Value = value;
        }

        public void SetValue(float value, Vector2 range) 
        {
            this._Value = value;
            this._Range = range;
        }

        public bool InRangeMax(float value)
        {
            if (this._Range != Vector2.zero)
            {
                return value <= this._Range.y;
            }

            return false;
        }

        public bool InRangeMin(float value)
        {
            if (this._Range != Vector2.zero)
            {
                return value >= this._Range.x;
            }

            return false;
        }

        public float Increase(float value) 
        {
            var reduce = this._Range.y - this._Value;
            var heal = Mathf.Min(reduce, value);

            this.SetValue(this._Value + heal);

            return heal;
        }

        public float Reduce(float value)
        {
            var reduce = Mathf.Min(this._Value, value);

            this.SetValue(this._Value - reduce);

            return reduce;
        }
    }

    [System.Serializable]
    public struct PropertyVariable : IVariable
    {
        [SerializeField]
        private string _ValueTarget;
        [SerializeField]
        private float _Value;
        [SerializeField]
        private Role.RoleBasic _From;
        [SerializeField]
        private Role.RoleBasic _To;
        [SerializeField]
        private EValueType _ValueType;

        public string ValueTarget => this._ValueTarget;
        public float Value => this._Value;
        public EValueType ValueType => this._ValueType;
        public Role.RoleBasic From => this._From;
        public Role.RoleBasic To => this._To;

        public PropertyVariable(string valueTarget, float value, EValueType valueType, Role.RoleBasic from, Role.RoleBasic to)
        {
            this._ValueTarget = valueTarget;
            this._Value = value;
            this._ValueType = valueType;
            this._From = from;
            this._To = to;
        }

        [System.Serializable]
        public enum EValueType
        {
            Reduce = 0,
            Increase = 1
        }
    }
}