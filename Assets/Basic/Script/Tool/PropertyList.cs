using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom
{
    [System.Serializable]
    public class PropertyList<TRetrieve, TValue>
    {
        public virtual TRetrieve Category { get; protected set; }
        public virtual List<Property<TValue>> Properties { get; protected set; }
        public virtual Property<TValue> this[string name] => this.Properties.Find(p => p.Name == name);

        #region Constructor

        public PropertyList() : this(default(TRetrieve), new Property<TValue>[0] { }) 
        {

        }

        public PropertyList(TRetrieve category) : this(category, new Property<TValue>[0] { }) 
        {

        }

        public PropertyList(TRetrieve category, IEnumerable<Property<TValue>> properties) 
        {
            this.Category = category;
            this.Properties = new List<Property<TValue>>(properties);
        }

        #endregion

        public virtual void SetProperty(Property<TValue> property) 
        {
            var temp = this[property.Name];

            if (temp != null) { temp.SetValue(property); }

            else { this.Properties.Add(property); }
        }

        public void SetProperty(IEnumerable<Property<TValue>> properties)
        {
            foreach (var property in properties) 
            {
                this.SetProperty(property);
            }
        }

        public void SetProperty(PropertyList<TRetrieve, TValue> propertyList) 
        {
            this.SetProperty(propertyList.Properties);
        }

        public TProperty GetProperty<TProperty>(string name) where TProperty : Property<TValue> 
        {
            if(this[name] is TProperty property) { return property; }

            return default(TProperty);
        }

        public virtual bool RemoveValue(Property<TValue> property) 
        {
            var p = this[property.Name];

            if (p == null) { return false; }

            return this.Properties.Remove(p);
        }

        public TData ValueChange<TData>(TData variable) where TData : IProperty<TValue>
        {
            return ValueChange(variable, (p, v) => v);
        }

        public delegate TOutput Evaluate<TPropertyList, TOutput>(TPropertyList list, TOutput value) where TPropertyList : PropertyList<TRetrieve, TValue>;

        public virtual TData ValueChange<TData>(TData variable, Evaluate<PropertyList<TRetrieve, TValue>, TValue> evaluate) where TData : IProperty<TValue>
        {
            var value = evaluate != null ? evaluate.Invoke(this, variable.Value) : variable.Value;

            variable.SetValue(value);

            return variable;
        }
    }

    [System.Serializable]
    public class Property<TValue> 
    {
        [SerializeField]
        protected string _Name;
        [SerializeField]
        protected TValue _Value;

        public string Name => this._Name;
        public TValue Value => this._Value;

        public Property(string name, TValue value) 
        {
            this._Name = name;
            this._Value = value;
        }

        public virtual void SetValue<TProperty>(TProperty property) where TProperty : Property<TValue> 
        {
            if (this._Name == property._Name)
            {
                this._Value = property._Value;
            }
        }

        public virtual void SetValue(TValue value) 
        {
            this._Value = value;
        }
    }

    #region Float Property

    [System.Serializable]
    public class FloatPropertyList : PropertyList<string, float>
    {
        [SerializeField]
        private string _Category;
        [SerializeField]
        private List<FloatProperty> _Properties;

        public override string Category 
        {
            get => this._Category; 
            
            protected set => this._Category = value; 
        }

        public override List<Property<float>> Properties 
        { 
            get => this._Properties.ConvertAll(c => c as Property<float>);

            protected set => this._Properties = value.ConvertAll(c => c as FloatProperty); 
        }

        public FloatPropertyList(string name, IEnumerable<FloatProperty> properties) : base(name, properties) { }

        public override void SetProperty(Property<float> property)
        {
            if (property is FloatProperty p)
            {
                var temp = this[p.Name];

                if (temp != null) { temp.SetValue(p); }

                else { this._Properties.Add(p); }
            }
        }

        public override bool RemoveValue(Property<float> property)
        {
            if (property is FloatProperty p)
            {
                var temp = this[p.Name];

                if (temp == null) { return false; }

                return this.Properties.Remove(p);
            }

            return false;
        }

        public override TData ValueChange<TData>(TData variable, Evaluate<PropertyList<string, float>, float> evaluate)
        {
            var value = 0f;

            var property = this[variable.Name] as FloatProperty;
            var temp = evaluate != null ? evaluate.Invoke(this, variable.Value) : variable.Value;

            if (variable.ValueType == EValueType.Increase)
            {
                value = property.Increase(temp);
            }

            if (variable.ValueType == EValueType.Reduce)
            {
                value = property.Reduce(temp);
            }

            variable.SetValue(value);

            return variable;
        }
    }

    [System.Serializable]
    public class FloatProperty : Property<float>
    {
        [SerializeField]
        private Vector2 _Range;

        public Vector2 Range => this._Range;

        public float Normalized => this._Range != Vector2.zero ? this._Value / this._Range.y : 1f;

        public FloatProperty(string name, float value, Vector2 range) : base(name, value)
        {
            this._Range = range;
        }

        public override void SetValue<TProperty>(TProperty property)
        {
            var p = property is FloatProperty temp ? temp : null;

            if (p == null) { return; }

            if (p.Name == this._Name) 
            {
                this.SetValue(p.Value, p.Range);
            }
        }

        public override void SetValue(float value)
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

    #endregion

    public interface IProperty<TValue>
    {
        public string Name { get; }
        public TValue Value { get; }
        public EValueType ValueType { get; }

        public void SetValue(TValue value);
    }

    [System.Serializable]
    public enum EValueType
    {
        Reduce = 0,
        Increase = 1
    }

    [System.Serializable]
    public struct FloatVariable : IProperty<float>, IVariable
    {
        [SerializeField]
        private string _Name;
        [SerializeField]
        private float _Value;
        [SerializeField]
        private EValueType _ValueType;

        public string Name => this._Name;
        public float Value => this._Value;
        public EValueType ValueType => this._ValueType;
        
        public FloatVariable(string valueTarget, float value, EValueType valueType)
        {
            this._Name = valueTarget;
            this._Value = value;
            this._ValueType = valueType;
        }

        public void SetValue(float value) 
        {
            this._Value = value;
        }
    }
}