using Loyufei;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame.Player
{
    [Serializable]
    public class RateData
    {
        public RateData() 
        {
            _Rate = 1;
        }

        public RateData(int rate)
        {
            _Rate = rate;
        }

        public int _Rate;
    }
}