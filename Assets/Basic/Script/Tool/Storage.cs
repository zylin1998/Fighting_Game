using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom
{
    public class Storage<TKey, TPreserve> : MonoBehaviour
    {
        protected void Awake()
        {
            this.Dictionary = new Dictionary<TKey, TPreserve>();
        }

        public Dictionary<TKey, TPreserve> Dictionary { get; protected set; }

        public virtual bool Preserve(KeyValuePair<TKey, TPreserve> preserve)
        {
            return this.Dictionary.TryAdd(preserve.Key, preserve.Value);
        }

        public virtual int Preserve(IEnumerable<KeyValuePair<TKey, TPreserve>> preserves)
        {
            int success = 0;

            foreach (var preserve in preserves)
            {
                if (this.Preserve(preserve)) { success++; }
            }

            return success;
        }

        public virtual TPreserve GetPreserve(TKey key)
        {
            return this.Dictionary.GetValueOrDefault(key);
        }
    }
}