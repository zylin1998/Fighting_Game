using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Loyufei
{
    public class Timer : ITimer
    {
        #region Constructor

        public Timer() : this(1f, true)
        {

        }

        public Timer(float interval) : this(interval, true)
        {

        }

        public Timer(float interval, bool autoReset)
        {
            Interval  = interval;
            AutoReset = autoReset;
        }

        #endregion

        [SerializeField]
        protected float _PassTime;

        protected float _StartTime;
        protected bool  _Enable;

        protected List<Action<ITimer>> _CallBacks = new List<Action<ITimer>>();

        #region ITimer properties

        public float Interval  { get; set; }
        public bool  AutoReset { get; set; }
        
        public bool  Enable
        {
            get => _Enable;

            set
            {
                if (_Enable == value) { return; }

                _Enable = value;
                
                if (Enable) 
                {
                    EventSystem.current?.StartCoroutine(TimeTick(this));
                }
            }
        }
        
        public float PassTime  
        { 
            get => _PassTime; 
            
            protected set => _PassTime = value; 
        }

        public event Action<ITimer> Elapsed
        {
            add    => _CallBacks.Add(value);

            remove => _CallBacks.Remove(value);
        }

        #endregion

        #region public methods

        public void Start()
        {
            Enable = true;
        }

        public void Stop()
        {
            Enable = false;
        }

        public void Reset() 
        {
            PassTime = 0f;
        }

        #endregion

        #region Tick

        protected static IEnumerator TimeTick(Timer timer)
        {
            timer._StartTime = Time.realtimeSinceStartup;

            for (; ;) 
            {
                yield return new WaitForSeconds(timer.Interval);

                if (!timer.Enable) { break; }

                timer.PassTime = Time.realtimeSinceStartup - timer._StartTime;

                timer._CallBacks.ForEach(callBack => callBack.Invoke(timer));

                if (!timer.AutoReset) { timer.Enable = false; }
            }

            timer._StartTime = 0f;
        }

        #endregion
    }
}
