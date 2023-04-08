using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Custom.Role;

namespace Custom.Events
{
    public class EventManager : MonoBehaviour, IBeginClient
    {
        public static IGameDetail Detail { get; set; }
        public static List<CreateSpot> CreateSpots { get; set; }
        public static GameEventCollect GameEventCollect { get; set; }
        
        private void Awake()
        {
            BeginClient.AddBegin(this);

            CreateSpots = this.GetComponentsInChildren<CreateSpot>().ToList();
        }

        public void BeforeBegin() 
        {
            Detail.Initialize();
        }

        public void BeginAction() 
        {
            StartCoroutine(Detail.GameLoop());
        }

        public static void CheckRule<TValue>(TValue value) 
        {
            Detail.Rule.CheckRule(value);
        }

        public static void GetReward(IReward reward) 
        {
            if (reward != null) { reward.GetReward(); }
        }

        #region Battle Event

        public static void AddEvent(string eventType, Action<IVariable> callBack) 
        {
            if (string.IsNullOrEmpty(eventType)) { return; }

            var gameEvent = GetEvent(eventType);

            if (gameEvent != null)
            {
                gameEvent.CallBack += callBack;
            }
        }

        public static void RemoveEvent(string eventType, Action<IVariable> callBack) 
        {
            if (string.IsNullOrEmpty(eventType)) { return; }
            if (callBack == null) { return; }
            if (!GameEventCollect) { return; }

            var gameEvent = GetEvent(eventType);

            if (gameEvent != null)
            {
                gameEvent.CallBack -= callBack;
            }
        }

        private static GameEvent GetEvent(string eventType) 
        {
            GameEvent gameEvent;

            GameEventCollect.TryGetEvent(eventType, out gameEvent);

            return gameEvent;
        }

        public static void EventInvoke(string eventType) 
        {
            EventInvoke(eventType, null);
        }

        public static void EventInvoke(string eventType, IVariable variable) 
        {
            var gameEvent = GetEvent(eventType);
            
            if (gameEvent != null)
            {
                gameEvent.Invoke(variable);
            }
        }

        #endregion

        private void OnDestroy()
        {
            Detail = null;
            GameEventCollect = null;

            CreateSpots.Clear();
        }
    }
}