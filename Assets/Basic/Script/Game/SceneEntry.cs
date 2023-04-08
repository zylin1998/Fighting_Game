using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom
{
    public abstract class SceneEntry : MonoBehaviour
    {
        protected virtual void Awake()
        {
            BeginClient.SetBeginType(BeginClient.EBeginType.Command);
        }

        protected virtual void Start()
        {
            this.Load();

            this.Entry();
        }

        public abstract void Load();

        public virtual void Entry() 
        {
            BeginClient.Ready();
        }
    }
}