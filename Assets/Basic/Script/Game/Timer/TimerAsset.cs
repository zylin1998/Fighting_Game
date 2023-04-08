using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom
{
    [CreateAssetMenu(fileName = "Timer Asset", menuName = "Event/Timer/Timer Asset", order = 2)]
    public class TimerAsset : ScriptableObject, TimeClient.ITimerAsset
    {
        [SerializeField]
        private Timer _Timer;

        public TimeClient.ITimer Timer => this._Timer;
    }

    #region Timer

    [System.Serializable]
    public class Timer : TimeClient.ITimer
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

        public TimeClient.DegreeTime InitTime => new TimeClient.DegreeTime(this._InitTime);
        public TimeClient.DegreeTime PassTime => new TimeClient.DegreeTime(this._PassTime);
        public TimeClient.DegreeTime TotalTime => new TimeClient.DegreeTime(this._TotalTime);

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
            for (; this._Pause == false;) 
            {
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
            this.TimeCapture.Clear();
        }

        public void Stop()
        {
            this._Pause = true;
        }

        public void SetInitTime(float time) 
        {
            this._InitTime = time;
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
}