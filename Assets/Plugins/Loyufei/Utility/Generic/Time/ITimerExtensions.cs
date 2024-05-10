using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei
{
    public static class ITimerExtensions
    {
        public static TimeSpan PassTimeAsTimeSpan(this ITimer self) 
        {
            return new TimeSpan((long)self.PassTime * 10000000);
        }
    }
}
