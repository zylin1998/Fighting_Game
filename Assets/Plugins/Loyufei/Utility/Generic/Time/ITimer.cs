using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Loyufei
{
    public interface ITimer
    {
        public float PassTime  { get; }
        public float Interval  { get; set; }
        public bool  AutoReset { get; set; }
        public bool  Enable    { get; set; }

        public event Action<ITimer> Elapsed;

        public static ITimer operator +(ITimer timer, Action<ITimer> action) 
        {
            timer.Elapsed += action;

            return timer;
        }

        public static ITimer operator -(ITimer timer, Action<ITimer> action)
        {
            timer.Elapsed -= action;

            return timer;
        }
    }
}
