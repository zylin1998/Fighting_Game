using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom
{
    [CreateAssetMenu(fileName = "Event Collect", menuName = "Event/Game Event/Event Collect", order = 2)]
    public class GameEventCollect : ScriptableObject
    {
        [SerializeField]
        private List<GameEvent> _GameEvents;

        public GameEvent this[string eventName] => this._GameEvents.Find(e => e.EventName == eventName);

        public bool TryGetEvent(string eventName, out GameEvent gameEvent) 
        {
            gameEvent = this._GameEvents.Find(e => e.EventName == eventName);

            return gameEvent != null;
        }

        public bool TryGetEvent<TEvent>(string eventName, out TEvent gameEvent) where TEvent : GameEvent
        {
            var temp = this._GameEvents.Find(e => e.EventName == eventName);

            gameEvent = temp is TEvent result ? result : default(TEvent);

            return gameEvent != null;
        }
    }
}