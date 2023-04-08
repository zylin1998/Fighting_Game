using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Role;
namespace Custom
{
    [CreateAssetMenu(fileName = "Game Event", menuName = "Event/Game Event/Basic Event", order = 2)]
    public class GameEvent : ScriptableObject
    {
        [SerializeField]
        protected string _EventName;

        public string EventName => this._EventName;
        public Action<IVariable> CallBack { get; set; }
        
        public virtual void Invoke<TVariable>(TVariable variable) where TVariable : IVariable 
        {
            this.CallBack?.Invoke(variable);
        }
        
        public virtual TObject Converter<TObject>(object obj) 
        {
            return obj is TObject result ? result : default(TObject);
        }
    }
}