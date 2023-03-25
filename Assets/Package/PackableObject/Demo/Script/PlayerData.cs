using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.DataPacked;

namespace PackableDemo.Player
{
    [Serializable]
    public class PlayerData : Data, IPlayer, IPlayerLimit, IPlayerState
    {
        #region IPackableObject

        public override string ID => "Player";
        
        public override void SetData(IPack pack) 
        {
            if (pack is IPlayer player) 
            {
                this.Level = player.Level;
                this.Exp = player.Exp;
            }
        }

        public override void Initialize()
        {
            this.Level = 1;
            this.Exp = 0;

            this.MaxLevel = 50;
            this.InitExp = 100f;
        }

        public override void Initialize(IPack pack)
        {
            if (pack is IPlayer player)
            {
                this.Level = player.Level;
                this.Exp = player.Exp;
            }

            if (pack is IPlayerLimit limit)
            {
                this.MaxLevel = limit.MaxLevel;
                this.InitExp = limit.InitExp;
            }
        }

        public override TPack GetPack<TPack>()
        {
            var pack = Activator.CreateInstance<TPack>();

            if (pack is IPlayer player) { pack.Packed(player); }

            if (pack is IPlayerLimit limit) { pack.Packed(limit); }

            return pack;
        }

        #endregion

        #region IPlayer

        [SerializeField]
        private int _Level;
        [SerializeField]
        private float _Exp;

        public int Level { get => this._Level; private set => this._Level = value; }
        public float Exp { get => this._Exp; private set => this._Exp = value; }

        #endregion

        #region IPlayerLimit

        public int MaxLevel { get; private set; }
        public float InitExp { get; private set; }
        public float MaxExp => this.InitExp + (this.InitExp * (float)(this.Level - 1) / this.MaxLevel);

        #endregion

        #region IPlayerState

        public bool IsMaxLevel => this.Level >= this.MaxLevel;

        #endregion

        public override string ToString()
        {
            return string.Format("{0}\nMaxLevel: {1}\nInitExp: {2}", base.ToString(), MaxLevel, InitExp);
        }
    }
}