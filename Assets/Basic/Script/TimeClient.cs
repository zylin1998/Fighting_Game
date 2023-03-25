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

        #region Timer

        [System.Serializable]
        public class Timer : ITimer
        {
            [SerializeField]
            private float _InitTime;
            [SerializeField]
            private float _PassTime;
            [SerializeField]
            private float _TotalTime;
            [SerializeField]
            private float _Frequency;
            [SerializeField]
            private bool _Pause;

            public DegreeTime InitTime => new DegreeTime(this._InitTime);
            public DegreeTime PassTime => new DegreeTime(this._PassTime);
            public DegreeTime TotalTime => new DegreeTime(this._TotalTime);

            public float Frequency
            {
                get => this._Frequency;

                set => this._Frequency = value;
            }

            private Coroutine Coroutine { get; set; }

            public List<float> TimeCapture { get; private set; }

            public bool Pause
            {
                get => this._Pause;

                set
                {
                    this._Pause = value;

                    if (!this._Pause) { this.Start(); }
                }
            }

            public IEnumerator Count()
            {
                while (true)
                {
                    if (this._Pause) { break; }

                    this._PassTime += this._Frequency <= 0 ? Time.deltaTime : this._Frequency;
                    this._TotalTime = this._InitTime + this._PassTime;

                    yield return this._Frequency <= 0 ? null : new WaitForSeconds(this._Frequency);
                }

                this.Coroutine = null;
            }

            public float Capture() 
            {
                var time = this._TotalTime;

                this.TimeCapture.Add(time);

                return time;
            }

            public void Start()
            {
                if (this.Coroutine == null)
                {
                    this.Coroutine = Singleton<TimeClient>.Instance.StartCoroutine(this.Count());
                }
            }

            public void Reset()
            {
                this._PassTime = 0f;
                this._TotalTime = this._InitTime;
                this._Pause = false;
                this.TimeCapture = new List<float>();
            }

            public void Stop() 
            {
                this._Pause = true;
            }

            public Timer(float initTime) 
            {
                this._InitTime = initTime;
                this._PassTime = 0f;
                this._TotalTime = initTime;
                this._Pause = false;
                this.TimeCapture = new List<float>();
            }
        }

        #endregion

        #region CountDown

        [System.Serializable]
        public class CountDownTimer : ITimer
        {
            [SerializeField]
            private float _InitTime;
            [SerializeField]
            private float _LeftTime = 0f;
            [SerializeField]
            private float _Frequency;
            [SerializeField]
            private bool _Pause;

            public DegreeTime InitTime => new DegreeTime(this._InitTime);
            public DegreeTime LeftTime => new DegreeTime(this._LeftTime);

            public float Frequency 
            { 
                get => this._Frequency;

                set => this._Frequency = value;
            }

            public System.Action<DegreeTime> EndCallBack { get; set; }

            private Coroutine Coroutine { get; set; }

            public bool Pause
            {
                get => this._Pause;

                set
                {
                    this._Pause = value;

                    if (!this._Pause) { this.Start(); }
                }
            }

            public IEnumerator Count()
            {
                while (true)
                {
                    if (this._Pause) { break; }

                    var time = this._Frequency <= 0 ? Time.deltaTime : this._Frequency;

                    this._LeftTime = Mathf.Clamp(this._LeftTime - time, 0f, this._InitTime);
                    
                    if (this._LeftTime <= 0f) 
                    {
                        this.EndCallBack.Invoke(this.LeftTime);

                        break; 
                    }

                    yield return this._Frequency <= 0 ? null : new WaitForSeconds(this._Frequency);
                }

                this.Coroutine = null;
            }

            public void Start() 
            {
                this._Pause = false;

                if (this.Coroutine == null)
                {
                    this.Coroutine = Singleton<TimeClient>.Instance.StartCoroutine(this.Count());
                }
            }

            public void Stop() 
            {
                this._Pause = true;
            }

            public void Reset() 
            {
                this._LeftTime = this._InitTime;
                this._Pause = false;
            }

            public void Refresh() 
            {
                this._LeftTime = this._InitTime;
            }

            public CountDownTimer(float time) 
            {
                this._InitTime = time;
                this._LeftTime = time;

                this._Pause = false;

                this.EndCallBack = (time) => { };
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

        #endregion
    }
}