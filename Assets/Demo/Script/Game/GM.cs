using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom;
using Custom.Role;
using Custom.Page;
using Custom.InputSystem;

namespace FightingGameDemo
{
    public class GM : MonoBehaviour
    {
        private void Awake()
        {
            if (Singleton<GM>.Exist) 
            {
                Destroy(this.gameObject);
                
                return; 
            }

            DontDestroyOnLoad(new GameObject("SingletonCollect", typeof(SingletonCollect)));

            Singleton<GM>.CreateClient(new Instance(this.gameObject, this), EDestroyType.DontDestroy);
            
            Singleton<TimeClient>.CreateClient(EDestroyType.DontDestroy);
            Singleton<RoleStorage>.CreateClient(EDestroyType.DontDestroy);
            Singleton<InputClient>.CreateClient(EDestroyType.DontDestroy);
            Singleton<PageClient>.CreateClient(EDestroyType.DontDestroy);
        }

        public void OnDestroy()
        {
            Singleton<GM>.RemoveClient();
        }
    }
}