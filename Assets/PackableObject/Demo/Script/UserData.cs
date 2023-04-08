using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.DataPacked;
using PackableDemo.Player;

namespace PackableDemo
{
    [System.Serializable]
    public class UserData : IPackableCollect
    {
        [SerializeField]
        private string _ID;
        [SerializeField]
        private PlayerData _PlayerData;
        
        public UserData() : this("EmptyID", new Data[1] { new PlayerData() })
        {
            
        }

        public UserData(string id) : this(id, new Data[1] { new PlayerData() })
        {

        }

        public UserData(string id, IEnumerable<IPackableObject> packables) 
        {
            var list = packables.ToList().ConvertAll(c => new KeyValuePair<string, IPackableObject>(c.ID, c));

            this.ID = id;
            this.Packables = packables;
            this.Dictionary = new Dictionary<string, IPackableObject>(list);

            this._PlayerData = (this as IPackableCollect).GetData<PlayerData>();
            this._PlayerData.Initialize();
        }

        #region IPackableCollect

        public string ID { get => this._ID; private set => this._ID = value; }
        public DataStreaming.ESaveLocate SaveLocate => DataStreaming.ESaveLocate.Locate;

        public IEnumerable<IPackableObject> Packables { get; private set; }
        public Dictionary<string, IPackableObject> Dictionary { get; private set; }

        #endregion
    }
}