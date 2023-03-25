using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.DataPacked;

namespace PackableDemo
{
    public class PlayerDemo : MonoBehaviour
    {
        [SerializeField]
        private string _ID = "000000001";
        [SerializeField]
        private Player.PlayerLimit _PlayerLimit;

        private void Awake()
        {
            var packs = new IPack[1] { this._PlayerLimit.Packed };

            DataStreaming.CreateClient();
            DataStreaming.Instance.Path.Locate = Path.Combine(Application.dataPath, "Save");
            DataStreaming.RequireData<UserData>(this._ID, DataStreaming.ESaveLocate.Locate);
            DataStreaming.GetCollect(this._ID).SetPacks(packs);
        }
    }
}