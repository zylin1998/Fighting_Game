using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Custom.DataPacked
{
    public class DataStreaming : MonoBehaviour
    {
        #region Singleton

        public static DataStreaming Instance { get; private set; }
        public static bool Exist => Instance != null;

        private void Awake()
        {
            if (Exist) { return; }

            Instance = this;

            this.Path = new StreamPath();
            this.Dictionary = new Dictionary<string, IPackableCollect>();
        }

        #endregion

        public StreamPath Path { get; set; }
        public Dictionary<string, IPackableCollect> Dictionary { get; private set; }

        public static void CreateClient()
        {
            if (!Exist)
            {
                DontDestroyOnLoad(new GameObject("DataStreaming", typeof(DataStreaming)));
            }
        }

        #region Get Collect

        public static IPackableCollect GetCollect(string id) 
        {
            if (!Exist) { CreateClient(); }

            IPackableCollect collect;

            if (Instance.Dictionary.TryGetValue(id, out collect)) { return collect; }

            return null;
        }

        public static T GetCollect<T>(string id) where T : IPackableCollect
        {
            var collect = GetCollect(id);

            return collect is T result ? result : default(T);
        }

        #endregion

        #region Streaming

        public static void SaveData<T>(string id, ESaveLocate saveLocate) where T : IPackableCollect 
        {
            if (!Exist) { CreateClient(); }

            var data = GetCollect<T>(id);

            if (data != null) 
            {
                SaveData(data);
            }
        }

        public static void SaveData<T>(T data) where T : IPackableCollect
        {
            if (!Exist) { CreateClient(); }

            if (data.SaveLocate == ESaveLocate.None) { return; }

            if (data.SaveLocate == ESaveLocate.Locate) { SaveLocate(data, Instance.Path.Locate, data.ID); }
        }

        public static T LoadData<T>(string fileName, ESaveLocate saveLocate) 
        {
            if (!Exist) { CreateClient(); }

            if (saveLocate == ESaveLocate.None) { return default(T); }

            if (saveLocate == ESaveLocate.Locate) 
            {
                return LoadLocate<T>(System.IO.Path.Combine(Instance.Path.Locate, fileName)); 
            }

            return default(T);
        }

        private static void SaveLocate<T>(T data, string path, string fileName)
        {
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
            
            path = System.IO.Path.Combine(path, fileName + ".json");

            string json = JsonUtility.ToJson(data, true);

            if (!File.Exists(path)) { File.Create(path).Dispose(); }

            File.WriteAllText(path, json);
        }

        private static T LoadLocate<T>(string fileName)
        {
            string path = fileName + ".json";
            
            if (!File.Exists(path)) { return default(T); }

            string json = File.ReadAllText(path);

            T data = JsonUtility.FromJson<T>(json);

            return data;
        }

        public static void RequireData<TData>(string id, ESaveLocate saveLocate) where TData : IPackableCollect 
        {
            if (!Exist) { CreateClient(); }

            if (saveLocate == ESaveLocate.None) { return; }

            var temp = LoadData<TData>(id, saveLocate);
            
            if (temp == null) 
            {
                var _object = System.Activator.CreateInstance(typeof(TData), new object[1] { id });
                
                if (_object is TData data) 
                {
                    temp = data;
                    SaveData(temp);
                }
            }
            
            if (temp != null) { Instance.Dictionary.Add(temp.ID, temp); }
        }

        #endregion

        [System.Serializable]
        public enum ESaveLocate 
        { 
            None = 0,
            Locate = 1,
            Server = 2
        }

        [System.Serializable]
        public class StreamPath 
        {
            public string Web { get; set; }
            public string Locate { get; set; }
        }
    }
}