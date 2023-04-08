using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom
{
    public abstract class GameAction : ScriptableObject, IGameAction
    {
        [SerializeField]
        private IGameAction.EInvokeType _InvokeType;

        public IGameAction.EInvokeType InvokeType => this._InvokeType;

        public abstract void Initialize();
        public abstract void Invoke<TVariable>(TVariable role) where TVariable : IVariable;
    }

    public interface IGameAction
    {
        public EInvokeType InvokeType { get; }

        public void Initialize();
        public void Invoke<TVariable>(TVariable role) where TVariable : IVariable;

        [System.Serializable]
        public enum EInvokeType 
        {
            Start = 0,
            Event = 1
        }
    }

    public interface IVariable 
    {

    }
}