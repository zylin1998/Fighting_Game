using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei 
{
    public interface IValue 
    {
        public object Data { get; }
    }

    public interface IValue <TValue> : IValue 
    {
        public new TValue Data { get; }

        object IValue.Data => Data;
    }
}