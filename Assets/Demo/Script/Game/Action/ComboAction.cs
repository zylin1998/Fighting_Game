using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom;
using Custom.Events;

namespace FightingGameDemo
{
    [CreateAssetMenu(fileName = "Combo Action", menuName = "Event/Game Action/Combo Action", order = 2)]
    public class ComboAction : GameAction
    {
        [SerializeField]
        private CountDownTimer _Timer;
        
        public int Current { get; private set; }
        public int Combo { get; private set; }
        public CountDownTimer Timer => this._Timer;
        public System.Action<int> OnComboChange { get; set; }

        public override void Initialize()
        {
            this.Current = 0;
            this.Combo = 0;

            EventManager.AddEvent("Enemy Hurt", this.Invoke);

            this._Timer.EndCallBack += (time) =>
            {
                this.Current = 0;
            };
        }

        public override void Invoke<TVariable>(TVariable variable)
        {
            this.Current++;

            if (this.Combo <= this.Current)
            {
                this.Combo = this.Current;
            }

            this._Timer.Refresh();
            this._Timer.Start();
            this.OnComboChange?.Invoke(this.Current);
        }

        private void OnDestroy()
        {
            EventManager.RemoveEvent("Enemy Hurt", this.Invoke);
        }
    }
}