using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Slot
{
    public abstract class ContentPool : ScriptableObject, IContentPool
    {
        public abstract IEnumerable<ISlotContent> Contents { get; }

        public virtual ISlotContent SearchByName(string name)
        {
            return this.Contents.ToList().Find(c => c.Name == name);
        }
    } 
}