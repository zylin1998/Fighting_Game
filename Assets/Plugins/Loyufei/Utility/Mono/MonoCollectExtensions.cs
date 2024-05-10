using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei
{
    public static class MonoCollectExtensions
    {
        public static TComponent Get<TComponent>(this MonoCollect self) 
            where TComponent : Component
        {
            return self.GetAll<TComponent>().FirstOrDefault();
        }

        public static IEnumerable<TComponent> GetAll<TComponent>(this MonoCollect self) 
            where TComponent : Component
        {
            return self.Components.OfType<TComponent>();
        }
    }
}
