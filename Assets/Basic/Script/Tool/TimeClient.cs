using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom
{
    public class TimeClient : MonoBehaviour
    {
        public static TTimer GetTimer<TTimer>(float time) where TTimer : ITimer
        {
            var obj = System.Activator.CreateInstance(typeof(TTimer), new object[1] { time });

            if (obj is TTimer timer) { return timer; }

            return default(TTimer);
        }

        #region DegreeTime

        [System.Serializable]
        public struct DegreeTime
        {
            [SerializeField]
            private int _Hour;
            [SerializeField]
            private int _Minute;
            [SerializeField]
            private float _Second;
            
            public int Hour => this._Hour;
            public int Minute => this._Minute;
            public float Second => this._Second;

            public float SecondOnly { get; private set; }

            public DegreeTime(float time) 
            {
                this.SecondOnly = time;
                
                var minutes = (int)(time / 60);
                
                this._Hour = minutes / 60;
                this._Minute = minutes % 60;
                this._Second = time % 60;
            }

            public string GetTime(string format) 
            {
                var finalFormate = string.IsNullOrEmpty(format) ? "{0, 2}:{1, 2}:{2, 2}, Total:{3}s" : format;

                return string.Format(finalFormate, this.Hour, this.Minute, this.Second, this.SecondOnly);
            }
        }

        #endregion

        public void OnDestroy()
        {
            Singleton<TimeClient>.RemoveClient();
        }

        #region ITimer

        public interface ITimer 
        {
            public DegreeTime InitTime { get; }
            public float Frequency { get; set; }
            public bool Pause { get; set; }

            public IEnumerator Count();
            public void Start();
            public void Stop();
            public void Reset();
        }

        public interface ITimerAsset 
        {
            public ITimer Timer { get; }
        }

        #endregion
    }
}