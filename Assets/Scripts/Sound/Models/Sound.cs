using UnityEngine;

namespace Assets.Scripts.Sound.Models
{
    [System.Serializable]
    public class Sound
    {
        
        public AudioChannel channel;
        public string soundName;
        public AudioClip clip;

        public float volume;
        public float pitch;
    }
}
