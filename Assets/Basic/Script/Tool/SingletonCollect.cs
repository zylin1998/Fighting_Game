using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Custom
{
    public class SingletonCollect : MonoBehaviour
    {
        public static List<SingletonList> List { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);

            if (List == null)
            {
                List = new List<SingletonList>()
                {
                      new SingletonList(EDestroyType.Destroy)
                    , new SingletonList(EDestroyType.DontDestroy)
                };
            }

            this._List = List;
        }

        [SerializeField]
        private List<SingletonList> _List;

        #region Instance Management

        internal static Instance AddInstance<TInstance>(Singleton<TInstance>.Demand demand) where TInstance : MonoBehaviour 
        {
            if (List == null) 
            {
                List = new List<SingletonList>()
                {
                      new SingletonList(EDestroyType.Destroy)
                    , new SingletonList(EDestroyType.DontDestroy)
                };
            }

            var destroy = demand.DestroyType;
            
            return List.Find(l => l.DestroyType == destroy).AddInstance(demand);
        }

        internal static bool AddInstance(Instance instance, EDestroyType destroyType) 
        {
            if (List == null)
            {
                List = new List<SingletonList>()
                {
                      new SingletonList(EDestroyType.Destroy)
                    , new SingletonList(EDestroyType.DontDestroy)
                };
            }

            return List.Find(l => l.DestroyType == destroyType).AddInstance(instance, destroyType); 
        }

        internal static bool RemoveInstance<TInstance>() where TInstance : MonoBehaviour
        {
            if (List == null) { return false; }

            var destroy = Singleton<TInstance>.DestroyType;

            return List.Find(l => l.DestroyType == destroy).RemoveInstance<TInstance>();
        }

        #endregion

        public static TInstance Instance<TInstance>() where TInstance : Component
        {
            foreach (SingletonList list in List) 
            {
                foreach (Instance instance in list.Instances) 
                {
                    if (instance.Component is TInstance component) 
                    {
                        return component;
                    }
                }
            }

            return default(TInstance);
        }

        #region Singleton List

        [System.Serializable]
        public class SingletonList 
        {
            [SerializeField]
            private EDestroyType _DestroyType;
            [SerializeField]
            private List<Instance> _Instances;

            public EDestroyType DestroyType => this._DestroyType;
            public List<Instance> Instances => this._Instances;

            public SingletonList(EDestroyType destroyType) 
            {
                this._DestroyType = destroyType;
                this._Instances = new List<Instance>();
            }

            internal Instance AddInstance<TSingleton>(Singleton<TSingleton>.Demand demand) where TSingleton : MonoBehaviour 
            {
                var exist = this._Instances.Exists(i => i.Component is TSingleton);

                Instance instance;

                if (!exist)
                {
                    var prefab = new GameObject(demand.Name, typeof(TSingleton));
                    var component = prefab.GetComponent<TSingleton>();

                    if (demand.DestroyType == EDestroyType.DontDestroy) 
                    {
                        DontDestroyOnLoad(prefab);
                    }

                    instance = new Instance(prefab, component);

                    this.Instances.Add(instance);
                }

                else
                {
                    instance = this._Instances.Find(i => i.Component is TSingleton);
                }

                return instance;
            }

            internal bool AddInstance(Instance instance, EDestroyType destroyType) 
            {
                if (instance.IsEmpty) { return false; }

                var contains = this._Instances.Contains(instance);

                if (!contains) 
                {
                    if (destroyType == EDestroyType.DontDestroy)
                    {
                        DontDestroyOnLoad(instance.Prefab);
                    }

                    this._Instances.Add(instance);
                }

                return false;
            }

            internal bool RemoveInstance<TSingleton>() where TSingleton : MonoBehaviour
            {
                var ins = this._Instances.Find(e => e.Component is TSingleton);

                return this._Instances.Remove(ins);
            }
        }

        #endregion
    }

    #region Singleton

    public class Singleton<TInstance> where TInstance : MonoBehaviour
    {
        private static Instance instance;

        public static TInstance Instance
        {
            get
            {
                if (!Exist) { CreateClient(); }

                return instance.GetInstance<TInstance>();
            }
        }

        public static bool Exist => !instance.IsEmpty;
        public static EDestroyType DestroyType { get; private set; }

        #region CreateClient

        public static void CreateClient()
        {
            CreateClient(typeof(TInstance).Name, EDestroyType.Destroy);
        }

        public static void CreateClient(EDestroyType destroyType)
        {
            CreateClient(typeof(TInstance).Name, destroyType);
        }

        public static void CreateClient(string name)
        {
            CreateClient(name, EDestroyType.Destroy);
        }

        public static void CreateClient(string name, EDestroyType destroyType)
        {
            if (!Exist)
            {
                DestroyType = destroyType;

                var demand = new Demand(name, destroyType);

                instance = SingletonCollect.AddInstance(demand);
            }
        }

        public static void CreateClient(Instance instance, EDestroyType destroyType) 
        {
            var add = SingletonCollect.AddInstance(instance, destroyType);

            if (add) 
            {
                Singleton<TInstance>.instance = instance;
                Singleton<TInstance>.DestroyType = destroyType;
            }
        }

        #endregion

        public static void RemoveClient()
        {
            instance.Clear();
            DestroyType = EDestroyType.Destroy;

            SingletonCollect.RemoveInstance<TInstance>();
        }

        internal struct Demand 
        {
            public string Name { get; private set; }
            public EDestroyType DestroyType { get; private set; }

            public Demand(string name, EDestroyType destroyType) 
            {
                this.Name = name;
                this.DestroyType = destroyType;
            }
        }
    }

    #endregion

    #region Singleton Instance

    [System.Serializable]
    public struct Instance
    {
        [SerializeField]
        private GameObject _Prefab;
        [SerializeField]
        private Component _Component;

        public GameObject Prefab => this._Prefab;
        public Component Component => this._Component;

        public bool IsEmpty => this._Prefab == null || this._Component == null;

        public Instance(GameObject prefab, Component instance)
        {
            this._Prefab = prefab;
            this._Component = instance;
        }

        public TInstance GetInstance<TInstance>()
        {
            return this._Component is TInstance instance ? instance : default(TInstance);
        }

        public bool CheckType<TInstance>() 
        {
            return this._Component is TInstance;
        }

        public void Clear() 
        {
            this._Prefab = null;
            this._Component = null;
        }
    }

    #endregion

    #region Destroy Type

    [System.Serializable]
    public enum EDestroyType
    {
        Destroy = 0,
        DontDestroy = 1
    }

    #endregion
}