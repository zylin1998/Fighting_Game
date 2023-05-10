using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Custom;

namespace FightingGameDemo
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private CinemachineVirtualCameraBase _Camera;

        public CinemachineVirtualCameraBase Camera => this._Camera;

        private void Awake()
        {
            Singleton<CameraController>.CreateClient(new Instance(this), EDestroyType.Destroy);

            if (!this._Camera) 
            {
                this._Camera = FindObjectOfType<CinemachineVirtualCameraBase>();
            }
        }

        public void Follow(Transform follow) 
        {
            this._Camera.Follow = follow;
        }

        public void LookAt(Transform lookAt) 
        {
            this._Camera.LookAt = lookAt;
        }
    }
}