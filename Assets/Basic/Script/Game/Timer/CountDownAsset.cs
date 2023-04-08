using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom
{
    [CreateAssetMenu(fileName = "CountDowm Asset", menuName = "Event/Timer/CountDowm Asset", order = 2)]
    public class CountDownAsset : ScriptableObject, TimeClient.ITimerAsset
    {
        [SerializeField]
        private CountDownTimer _Timer;

        public TimeClient.ITimer Timer => this._Timer;
    }

    #region CountDown

    [System.Serializable]
    public class CountDownTimer : TimeClient.ITimer
    {
        [SerializeField]
        private float _InitTime;
        [SerializeField]
        private float _LeftTime = 0f;
        [SerializeField]
        private float _Frequency;
        [SerializeField]
        private bool _Pause;

        public TimeClient.DegreeTime InitTime => new TimeClient.DegreeTime(this._InitTime);
        public TimeClient.DegreeTime LeftTime => new TimeClient.DegreeTime(this._LeftTime);

        public float Frequency
        {
            get => this._Frequency;

            set => this._Frequency = value;
        }

        public System.Action<TimeClient.DegreeTime> EndCallBack { get; set; }

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
            for (; this._LeftTime > 0;) 
            {
                if (this._Pause) { break; }

                var time = this._Frequency <= 0 ? Time.deltaTime : this._Frequency;
                
                this._LeftTime = Mathf.Clamp(this._LeftTime - time, 0f, this._InitTime);

                yield return this._Frequency <= 0 ? null : new WaitForSeconds(this._Frequency);
            }

            this.Coroutine = null;

            if (this._LeftTime <= 0f)
            {
                this.EndCallBack.Invoke(this.LeftTime);
            }
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
}