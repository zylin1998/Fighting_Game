using System;
using System.Collections;
using System.Collections.Generic;
using Loyufei;

namespace FightingGame.Player
{
    [Serializable]
    public class PlayerSave : SaveSystem
    {
        public override ISaveable Saveable 
        {
            get 
            {
                if (_Saveable.IsDefault())
                {
                    _Saveable = this.Load<PlayerData>() ?? new PlayerData(new(2), new(10));
                }

                return _Saveable;
            }  
        }
    }
}