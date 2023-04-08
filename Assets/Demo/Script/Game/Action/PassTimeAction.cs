using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom;
using Custom.Events;

namespace FightingGameDemo
{
    [CreateAssetMenu(fileName = "PassTime Action", menuName = "Event/Game Action/PassTime Action", order = 2)]
    public class PassTimeAction : GameAction
    {
        [SerializeField]
        private Timer _Timer;

        public Timer Timer => this._Timer;
        public TimeClient.DegreeTime GameTime => this._Timer.TotalTime;

        public override void Initialize()
        {
            this._Timer = new Timer(0f);

            EventManager.AddEvent("Battle End", (role) => this.Timer.Stop());
        }

        public override void Invoke<TVariable>(TVariable role)
        {
            this.Timer.Start();
        }

        private void OnDestroy()
        {
            EventManager.RemoveEvent("Battle End", (role) => this.Timer.Stop());
        }
    }
}