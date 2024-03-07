using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GameEvents.DebugEvents
{
    [CreateAssetMenu(fileName = "DebugEvent", menuName= "SceneWeaverEvents/DebugEventBasic")]
    public class DebugEvent : SceneWeaverEvent
    {
        public string text;

        public void OnEnable ()
        {
            base.OnExecute = () =>
            {
                Debug.Log(text);
            };
        }
    }
}
