using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei
{
    public interface IAgent
    {
        public List<IApply>  Applies    { get; }

        public void Initialize();
    }

    public class Agent : IAgent
    {
        public Agent()
        {
            Applies = new();
        }

        public List<IApply> Applies { get; }

        public void Initialize()
        {
            Applies.ForEach(a => a.Initialize());
        }
    }
}
