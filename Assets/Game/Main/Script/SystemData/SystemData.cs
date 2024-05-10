using Loyufei;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    [Serializable]
    public class ScreenMode 
    {
        public ScreenMode()
        {
            _FullScreenMode = FullScreenMode.ExclusiveFullScreen;
            _Resolution     = 0;
        }

        [Header("畫面設定")]
        public FullScreenMode _FullScreenMode;
        public int            _Resolution;

        public Resolution Resolution => Screen.resolutions[_Resolution];
    }

    [Serializable]
    public class VolumnRate 
    {
        public VolumnRate() 
        {
            _Volume = 100;
            _Mute   = false;
        }

        [Header("系統設定")]
        [Range(0, 100)]
        public int  _Volume;
        public bool _Mute;
    }

    public class SystemData : ISaveable
    {
        public SystemData()
        {
            _ScreenMode = new();
            _VolumnRate = new();
        }

        public ScreenMode _ScreenMode;
        public VolumnRate _VolumnRate;
    }
}