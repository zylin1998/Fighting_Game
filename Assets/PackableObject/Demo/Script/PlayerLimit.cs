using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.DataPacked;

namespace PackableDemo.Player
{
    [System.Serializable]
    public class PlayerLimit : IPlayerLimit
    {
        [SerializeField]
        private int _MaxLevel;
        [SerializeField]
        private float _InitExp;

        public int MaxLevel => this._MaxLevel;
        public float InitExp => this._InitExp;
        public float MaxExp => 0f;

        public IPack Packed => new LimitPack(this);
    }
}