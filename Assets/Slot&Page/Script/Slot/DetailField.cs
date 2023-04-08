using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Slot
{
    public abstract class DetailField : MonoBehaviour, IDetailField
    {
        public ISlotContent Content { get; protected set; }

        public abstract void SetDetail(ISlotContent content);

        public abstract void UpdateDetail();

        public abstract void Clear();
    }
}