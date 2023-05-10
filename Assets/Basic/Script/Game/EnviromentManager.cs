using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Enviroment
{
    public class EnviromentManager : MonoBehaviour
    {
        public List<IEnviromentObject> EnviromentObjects { get; private set; }

        private void Awake()
        {
            this.EnviromentObjects = new List<IEnviromentObject>();
        }

        public static IEnumerable<TObject> GetObjects<TObject>()
        {
            var list = new List<TObject>();

            Singleton<EnviromentManager>.Instance.EnviromentObjects.ForEach(e =>
            {
                if(e is TObject obj) { list.Add(obj); }
            });

            return list;
        }

        public static void AddObject(IEnviromentObject enviromentObject) 
        {
            Singleton<EnviromentManager>.Instance.EnviromentObjects.Add(enviromentObject);
        }
    }

    public interface IEnviromentObject 
    {
        public string ObjectName { get; }
        public Transform Transform { get; }
    }
}