using System.Collections;
using System.Collections.Generic;

namespace Loyufei
{
    public class Value<TData> : IValue<TData>
    {
        public Value(TData data) 
        {
            Data = data;
        }    

        public TData Data { get; }
    }
}