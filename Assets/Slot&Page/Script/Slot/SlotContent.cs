using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Slot
{
    public abstract class SlotContent : ScriptableObject, ISlotContent
    {
        [System.Serializable]
        public abstract class Describe : IContentDetail
        {
            public virtual string GetDetail()
            {
                return "Basic Describe";
            }
        }

        [SerializeField]
        private Sprite _Icon;
        [SerializeField]
        private string _Name;

        public Sprite Icon => this._Icon;
        public string Name => this._Name;
        public abstract IContentDetail Detail { get; }
    }
}