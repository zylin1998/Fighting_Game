using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.DomainEvents;

namespace Loyufei
{
    public static class IAgentExtensions
    {
        public static TApply CreateApply<TApply>(this IAgent self, params object[] args) 
            where TApply : IApply
        {
            var apply   = Activator.CreateInstance(typeof(TApply), args).To<TApply>();

            self.Applies.Add(apply);

            return apply;
        }

        public static List<IDomainEvent> SettleApplies(this IAgent self) 
        {
            var events = new List<IDomainEvent>();

            self.Applies.ForEach(apply =>
            {
                var e = apply.GetApplyEvent();

                if (!e.IsDefault()) { events.Add(e); }
            });

            return events;
        }
    }
}
