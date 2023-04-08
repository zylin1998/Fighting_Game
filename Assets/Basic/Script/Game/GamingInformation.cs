using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom
{
    public abstract class GamingInformation : MonoBehaviour, IBeginClient
    {
        protected void Awake()
        {
            BeginClient.AddBegin(this);
        }

        public abstract void SetInformation(IVariable variable);

        public abstract void BeforeBegin();

        public abstract void BeginAction();
    }
}