using Assets.Scripts.GameEvents.DebugEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.GameEvents {
    public class EventsManager : MonoBehaviour
    {
        [SerializeField]
        private List<DebugEvent> debugEvents;

        Dictionary<string, SceneWeaverEvent> nameToEvent = new Dictionary<string, SceneWeaverEvent>();

        private void Awake()
        {
           foreach(var debugEvent in debugEvents)
            {
                nameToEvent[debugEvent.eventName] = debugEvent;
            }
        }

        public void PlayEvent(string name)
        {
            if (nameToEvent.ContainsKey(name))
            {
                this.nameToEvent[name].Execute();
            } else
            {
                Debug.Log($"event {name} not added to event manager list");
            }
        }
    }
}

