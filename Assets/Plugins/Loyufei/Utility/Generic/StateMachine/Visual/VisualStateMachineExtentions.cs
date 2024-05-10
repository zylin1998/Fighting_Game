using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.ComponentModel;

namespace Loyufei
{
    public static class VisualStateMachineExtensions
    {
        public static void AddTransition(this VisualStateMachine self, VisualTransitionAsset transition)
        {
            var from       = transition.From;
            var states     = self.States;
            var conditions = self.SubConditions;
            var state      = states.GetorAdd(from.Identity, () 
                => self.Container.TryResolveorBindId(from, from.GetHashCode()));

            transition.ToStates.ForEach(next =>
            {
                var to            = states.GetorAdd(next.To.Identity, () 
                    => self.Container.TryResolveorBindId(next.To, next.To.GetHashCode()));
                var subConditions = next.SubConditions
                    .Select(c => conditions.GetorAdd(c, () 
                        => self.Container.TryResolveorBindId(c, c.GetHashCode())))
                    .ToList();
                var transition    = new VisualTransition(to, subConditions);

                self.AddTransition(state, transition);
            });
        }

        public static void AddTransition(this VisualStateMachineAsset self, VisualTransitionAsset transition)
        {
            var from       = transition.From;
            var states     = self.States;
            var conditions = self.SubConditions;
            var state      = states.GetorAdd(from.Identity, ()
                => self.Container.TryResolveorBindId(from, from.GetHashCode()));

            transition.ToStates.ForEach(next =>
            {
                var to = states.GetorAdd(next.To.Identity, ()
                    => self.Container.TryResolveorBindId(next.To, next.To.GetHashCode()));
                var subConditions = next.SubConditions
                    .Select(c => conditions.GetorAdd(c, ()
                        => self.Container.TryResolveorBindId(c, c.GetHashCode())))
                    .ToList();
                var transition = new VisualTransition(to, subConditions);

                self.AddTransition(state, transition);
            });
        }

        public static void SetState(this VisualStateMachine self, VisualStateAsset visualState)
        {
            var identity = visualState.Identity;
            var state    = self.States[identity];

            self.SetState(state);
        }
        /*
        public static T BindandResolveScriptableObject<T>(this T self, DiContainer container, object identifier = null) 
            where T : ScriptableObject
        {
            var type = self.GetType();
            
            container
                .Bind<T>()
                .WithId(identifier)
                .To(type)
                .FromScriptableObject(self)
                .AsTransient();

            return container.TryResolveId<T>(identifier);
        }

        public static T TryResolveorBind<T>(this T self, DiContainer container, object identifier = null) 
            where T : ScriptableObject
        {
            return container.TryResolveId<T>(identifier) ?? self.BindandResolveScriptableObject(container, identifier);
        }
        */
        public static T TryResolveorBindId<T>(this DiContainer self, T bind,  object identifier = null) 
            where T : ScriptableObject
        {
            return self.TryResolveId<T>(identifier) ?? self.BindandResolve(bind, identifier);
        }

        public static T BindandResolve<T>(this DiContainer self, T bind, object identifier = null) 
            where T : ScriptableObject
        {
            var type = bind.GetType();

            self
                .Bind<T>()
                .WithId(identifier)
                .To(type)
                .FromScriptableObject(bind)
                .AsTransient();

            return self.ResolveId<T>(identifier);
        }
    }
}
