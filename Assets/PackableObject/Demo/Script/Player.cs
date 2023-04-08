using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Custom.DataPacked;

namespace PackableDemo.Player
{
    public class Player : MonoBehaviour, IPlayer, IPlayerLimit, IPlayerState
    {
        [SerializeField]
        private Text _UserNameText;
        [SerializeField]
        private Text _LevelText;
        [SerializeField]
        private Text _ExpText;
        [SerializeField]
        private Button _UpgradeButton;
        [SerializeField]
        private Button _SaveButton;

        private string id = "000000001";

        public bool HasCollect => this.Collect != null;
        public IPackableCollect Collect { get; private set; }

        #region IPlayer
        
        public int Level { get; private set; }
        public float Exp { get; private set; }

        #endregion

        #region IPlayerLimit

        public int MaxLevel { get; private set; }
        public float MaxExp => this.InitExp + (this.InitExp * (float)(this.Level - 1) / this.MaxLevel);
        public float InitExp { get; private set; }

        #endregion

        #region

        public bool IsMaxLevel { get; private set; }

        #endregion

        void Start()
        {
            this.Collect = DataStreaming.GetCollect(this.id);
            
            if (this.HasCollect) 
            {
                var player = this.Collect.GetData<IPlayer>();
                var limit = this.Collect.GetData<IPlayerLimit>();
                var state = this.Collect.GetData<IPlayerState>();

                this.Level = player.Level;
                this.Exp = player.Exp;
                this.MaxLevel = limit.MaxLevel;
                this.InitExp = limit.InitExp;
                this.IsMaxLevel = state.IsMaxLevel;

                this.UserNameText();
                this.LevelText();
                this.ExpText();
            }

            if (this._UpgradeButton) { this._UpgradeButton.onClick.AddListener(this.AddExp); }
            if (this._SaveButton) { this._SaveButton.onClick.AddListener(this.Save); }
        }

        public void AddExp() 
        {
            if (this.IsMaxLevel) { return; }

            var value = 100f + Random.Range(-10, 10);

            this.Exp += value;

            if (this.Exp >= this.MaxExp) 
            {
                var temp = this.Exp;

                this.Level += (int)(temp / this.MaxExp);
                this.Exp = temp % this.MaxExp;

                this.LevelText();
            }

            this.ExpText();

            this.Collect.SetPack<PlayerData>(new PlayerPack(this));
        }

        public void Save() 
        {
            DataStreaming.SaveData<UserData>(this.id, DataStreaming.ESaveLocate.Locate);
        }

        #region Change Text

        private void UserNameText() 
        {
            if (this._UserNameText) 
            {
                this._UserNameText.text = "Player";
            }
        }

        private void LevelText()
        {
            if (this._LevelText)
            {
                this._LevelText.text = string.Format("Level: {0}/{1}", this.Level, this.MaxLevel);
            }
        }

        private void ExpText()
        {
            if (this._ExpText)
            {
                if (this.IsMaxLevel) { this._ExpText.text = "Exp: Max"; return; }

                this._ExpText.text = string.Format("Exp: {0}/{1}", this.Exp, this.MaxExp);
            }
        }

        #endregion

    }

    #region Data Pack

    [System.Serializable]
    public struct PlayerPack : IPack, IPlayer 
    {
        public int Level { get; private set; }
        public float Exp { get; private set; }

        public PlayerPack(IPlayer player) 
        {
            this.Level = player.Level;
            this.Exp = player.Exp;
        }

        public IPack Packed<TData>(TData data) 
        {
            if (data is IPlayer player) 
            {
                this.Level = player.Level;
                this.Exp = player.Exp;

                return this;
            }

            return null;
        }
    }

    [System.Serializable]
    public struct LimitPack : IPack, IPlayerLimit
    {
        public int MaxLevel { get; private set; }
        public float InitExp { get; private set; }
        public float MaxExp => 0f;

        public LimitPack(IPlayerLimit limit)
        {
            this.MaxLevel = limit.MaxLevel;
            this.InitExp = limit.InitExp;
        }

        public IPack Packed<TData>(TData data)
        {
            if (data is IPlayerLimit limit)
            {
                this.MaxLevel = limit.MaxLevel;
                this.InitExp = limit.InitExp;

                return this;
            }

            return null;
        }

        public override string ToString()
        {
            return  string.Format("{0}\nMaxLevel: {1}\nInitExp: {2}", base.ToString(), MaxLevel, InitExp);
        }
    }

    #endregion

    public interface IPlayer 
    {
        public int Level { get; }
        public float Exp { get; }
    }

    public interface IPlayerLimit 
    {
        public int MaxLevel { get; }
        public float MaxExp { get; }
        public float InitExp { get; }
    }

    public interface IPlayerState 
    {
        public bool IsMaxLevel { get; }
    }
}