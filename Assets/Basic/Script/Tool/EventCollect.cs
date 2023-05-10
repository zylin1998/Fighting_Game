using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom
{
    [Serializable]
    public class EventCollect<TTarget>
    {
        [SerializeField]
        private List<EventProperty> _Events;

        public List<EventProperty> Events => this._Events;

        public static EventCollect<TTarget> Collection { get; private set; }

        public EventCollect()
        {
            this._Events = new List<EventProperty>();
        }

        private static void Init() 
        {
            if (Collection == null) { Collection = new EventCollect<TTarget>(); }
        }

        public static void AddEvent(string name, Action value) 
        {
            Init();

            var property = Collection.Events.Find(p => p.Name == name);

            if (property != null) { property.SetValue(value); }

            else Collection.Events.Add(new EventProperty(name, value));
        }

        public static void RemoveEvent(string name, Action value)
        {
            if (Collection.Events == null) { return; }

            var property = Collection.Events.Find(p => p.Name == name);

            if (property != null) { property.RemoveValue(value); }
        }

        public static void Invoke(string eventName) 
        {
            Collection.Events.Find(e => e.Name == eventName)?.Value.Invoke();
        }

        #region Event Property

        [Serializable]
        public class EventProperty : Property<Action> 
        {
            public EventProperty(string name, Action value) : base(name, value) { }

            public override void SetValue(Action value) 
            {
                this._Value += value;
            }

            public void RemoveValue(Action value) 
            {
                this._Value -= value;
            }

            public void Clear()
            {
                this._Value = () => { };
            }
        }

        #endregion
    }

    [Serializable]
    public class EventCollect<TTarget, TInvoke>
    {
        [SerializeField]
        private List<EventProperty> _Events;

        public List<EventProperty> Events => this._Events;

        public static EventCollect<TTarget, TInvoke> Collection { get; private set; }

        public EventCollect()
        {
            this._Events = new List<EventProperty>();
        }

        private static void Init()
        {
            if (Collection == null) { Collection = new EventCollect<TTarget, TInvoke>(); }
        }
        
        public static void AddEvent(string name, Action<TInvoke> value)
        {
            Init();

            var property = Collection.Events.Find(p => p.Name == name);

            if (property != null) { property.SetValue(value); }

            else Collection.Events.Add(new EventProperty(name, value));
        }

        public static void RemoveEvent(string name, Action<TInvoke> value)
        {
            if (Collection.Events == null) { return; }

            var property = Collection.Events.Find(p => p.Name == name);

            if (property != null) { property.RemoveValue(value); }
        }

        public static void Invoke(string name, TInvoke invoke)
        {
            Collection.Events.Find(e => e.Name == name)?.Value.Invoke(invoke);
        }

        #region Event Property

        [Serializable]
        public class EventProperty : Property<Action<TInvoke>>
        {
            public EventProperty(string name, Action<TInvoke> value) : base(name, value) { }

            public override void SetValue(Action<TInvoke> value)
            {
                this._Value += value;
            }

            public void RemoveValue(Action<TInvoke> value)
            {
                this._Value -= value;
            }

            public void Clear() 
            {
                this._Value = (invoke) => { };
            }
        }

        #endregion
    }
}