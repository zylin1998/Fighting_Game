using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom
{
    public static class ObjectCreation
    {
        public static TValue CreateObject<TValue>(GameObject prefab, Transform root, string name)
        {
            var newObj = GameObject.Instantiate(prefab, root);
            var value = newObj.GetComponent<TValue>();

            if (!string.IsNullOrEmpty(name)) { newObj.name = name; }

            if (value == null) { GameObject.Destroy(newObj); }

            return value;
        }
        public static TValue CreateObject<TValue>(GameObject prefab, Transform root) => CreateObject<TValue>(prefab, root, string.Empty);
        public static TValue CreateObject<TValue>(GameObject prefab) => CreateObject<TValue>(prefab, null, string.Empty);
        public static TValue CreateObject<TValue, TBasic>(GameObject prefab, Transform root, string name)
        {
            var newObj = GameObject.Instantiate(prefab, root);
            var basic = newObj.GetComponent<TBasic>();

            if (!string.IsNullOrEmpty(name)) { newObj.name = name; }

            if (basic is TValue output) { return output; }

            GameObject.Destroy(newObj);

            return default(TValue);
        }
        public static TValue CreateObject<TValue, TBasic>(GameObject prefab, Transform root) => CreateObject<TValue, TBasic>(prefab, root, string.Empty);
        public static TValue CreateObject<TValue, TBasic>(GameObject prefab) => CreateObject<TValue, TBasic>(prefab, null, string.Empty);
        public static IEnumerable<TValue> CreateObjects<TValue>(int count, GameObject prefab, Transform root, string name)
        {
            var list = new List<TValue>();

            for (int i = 0; i < count; i++)
            {
                var value = CreateObject<TValue>(prefab, root, name);

                if (value != null) { list.Add(value); }
            }

            return list;
        }
        public static IEnumerable<TValue> CreateObjects<TValue>(int count, GameObject prefab, Transform root) => CreateObjects<TValue>(count, prefab, root, string.Empty);
        public static IEnumerable<TValue> CreateObjects<TValue>(int count, GameObject prefab) => CreateObjects<TValue>(count, prefab, null, string.Empty);
        public static IEnumerable<TValue> CreateObjects<TValue, TBasic>(int count, GameObject prefab, Transform root, string name)
        {
            var list = new List<TValue>();

            for (int i = 0; i < count; i++)
            {
                var value = CreateObject<TValue, TBasic>(prefab, root, name);

                if (value != null) { list.Add(value); }
            }

            return list;
        }
        public static IEnumerable<TValue> CreateObjects<TValue, TBasic>(int count, GameObject prefab, Transform root) => CreateObjects<TValue, TBasic>(count, prefab, root, string.Empty);
        public static IEnumerable<TValue> CreateObjects<TValue, TBasic>(int count, GameObject prefab) => CreateObjects<TValue, TBasic>(count, prefab, null, string.Empty);
    }
}