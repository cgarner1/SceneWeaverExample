using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GameEvents

{
    public enum SceneWeaverEventType
    {
        Audio,
        GameBG,
        Character,
        Transition
    }

   
    public abstract class SceneWeaverEvent : ScriptableObject
    {
        
        public string eventName;
        public SceneWeaverEventType eventType { get; set; }
        public delegate void EventAction();
        public EventAction OnExecute;

        public void Execute()
        {
            OnExecute?.Invoke();
        }
    }
}
