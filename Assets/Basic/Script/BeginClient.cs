using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom
{
    public interface IBeginClient
    {
        public void BeforeBegin();

        public void BeginAction();
    }

    public class BeginClient : MonoBehaviour
    {
        private void Awake()
        {
            this.Begins = new List<IBeginClient>();
        }

        [SerializeField]
        private EBeginType _BeginType = EBeginType.Start;

        public List<IBeginClient> Begins { get; private set; }

        public EBeginType BeginType => this._BeginType; 

        private void Start()
        {
            if (this._BeginType == EBeginType.Start) 
            {
                Ready();
                Begin();
            }
        }

        public static void SetBeginType(EBeginType beginType) 
        {
            Singleton<BeginClient>.Instance._BeginType = beginType;
        }

        public static void Ready() 
        {
            Singleton<BeginClient>.Instance.Begins.ForEach(b => b.BeforeBegin());
        }

        public static void Begin() 
        {
            Singleton<BeginClient>.Instance.Begins.ForEach(b => b.BeginAction());
        }

        public static void AddBegin(IBeginClient begin)
        {
            Singleton<BeginClient>.Instance.Begins.Add(begin);
        }

        public static void RemoveBegin(IBeginClient begin)
        {
            Singleton<BeginClient>.Instance.Begins.Remove(begin);
        }

        private void OnDestroy()
        {
            Singleton<BeginClient>.RemoveClient();
        }

        [System.Serializable]
        public enum EBeginType 
        {
            None = 0,
            Start = 1,
            Command = 2
        }
    }
}