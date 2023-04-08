using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom;
using Custom.Role;
using Custom.Events;

namespace FightingGameDemo
{
    public class DemoInformation : GamingInformation
    {
        [SerializeField]
        private float _CauseDamage;
        [SerializeField]
        private float _RecieveDamage;
        [SerializeField]
        private int _EnemySlaved;
        [SerializeField]
        private int _Combo;
        [SerializeField]
        private TimeClient.DegreeTime _PassTime;

        public float CauseDamage => this._CauseDamage;
        public float RecieveDamage => this._RecieveDamage;
        public int EnemySlaved => this._EnemySlaved;
        public int Combo => this._Combo;
        public TimeClient.DegreeTime PassTime => this._PassTime;



        public override void SetInformation(IVariable variable)
        {
            
        }

        #region IBeginClient

        public override void BeforeBegin() 
        {
            Singleton<DemoInformation>.CreateClient(new Instance(this.gameObject, this), EDestroyType.Destroy);

            EventManager.AddEvent("Battle End", (variable) =>
            {
                this._PassTime = EventManager.Detail.GetAction<PassTimeAction>().GameTime;
                this._Combo = EventManager.Detail.GetAction<ComboAction>().Combo;
            });

            EventManager.AddEvent("Ally Hurt", (variable) =>
            {
                if (variable is PropertyVariable health) 
                {
                    if (health.ValueTarget == "HP" && health.ValueType == PropertyVariable.EValueType.Reduce) 
                    {
                        this._RecieveDamage += health.Value;
                    }
                }
            });

            EventManager.AddEvent("Enemy Hurt", (variable) =>
            {
                if (variable is PropertyVariable health)
                {
                    if (health.ValueTarget == "HP" && health.ValueType == PropertyVariable.EValueType.Reduce)
                    {
                        this._CauseDamage += health.Value;
                    }
                }
            });

            EventManager.AddEvent("Enemy Slaved", (variable) => this._EnemySlaved++);
        }

        public override void BeginAction() 
        {

        }

        #endregion
    }
}