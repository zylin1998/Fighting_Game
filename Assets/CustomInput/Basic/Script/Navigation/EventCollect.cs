using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Custom.Navigation
{
    [RequireComponent(typeof(EventTrigger))]
    public class EventCollect : MonoBehaviour, IEventCollect
    {
        [SerializeField]
        private EventTrigger _EventTrigger;

        public EventTrigger EventTrigger => this._EventTrigger;

        private void Awake()
        {
            this._EventTrigger = this.GetComponent<EventTrigger>();
        }
    }
}