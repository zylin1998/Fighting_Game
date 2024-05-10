using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Loyufei.DomainEvents;


namespace Loyufei.Quest
{
    #region Quest Operate Events

    public struct QuestStart : IDomainEvent
    {
        public QuestStart(object source)
        {
            InvokeTime = Time.realtimeSinceStartup;
            Source = source;
        }

        public float InvokeTime { get; }
        public object Source { get; }
    }

    public struct QuestPause : IDomainEvent
    {
        public QuestPause(object source)
        {
            InvokeTime = Time.realtimeSinceStartup;
            Source = source;
        }

        public float InvokeTime { get; }
        public object Source { get; }
    }

    public struct QuestStop : IDomainEvent
    {
        public QuestStop(object source)
        {
            InvokeTime = Time.realtimeSinceStartup;
            Source     = source;
        }

        public float InvokeTime { get; }
        public object Source { get; }
    }

    #endregion
}
