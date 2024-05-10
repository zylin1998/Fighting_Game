using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.Character
{
    public class CalculateStat : RepositBase<float>, IAmountAdjust
    {
        public CalculateStat(string identity, float data, float standard) 
        {
            _Identity = identity;
            _Data     = data;
            Standard  = standard;
        }

        public CalculateStat(Stat stat)
        {
            _Identity = stat.Name;
            _Data     = stat.Value;
            Standard  = stat.Value;
        }

        public float Standard { get; protected set; }

        public override void Preserve(float data)
        {
            _Data = Mathf.Clamp(data, 0, Standard);
        }

        #region IAmountAdjust

        public float Amount { get; protected set; }

        public virtual bool Increase(float amount) 
        {
            Amount = Mathf.Clamp(amount, 0, Standard - Data);

            var preserve = Amount > 0;

            if (preserve) { Preserve(Data + Amount); }

            return preserve;
        }

        public virtual bool Decrease(float amount) 
        {
            Amount = Mathf.Clamp(amount, 0, Standard);

            var preserve = Amount > 0;

            if (preserve) { Preserve(Data - Amount); }

            return preserve;
        }

        #endregion

        public void Reset() 
        {
            Reset(Standard, true);
        }

        public void Reset(float standard, bool preserve = false) 
        {
            Standard = standard;

            if (preserve) 
            {
                Preserve(Standard); 
            }
        }
    }
}