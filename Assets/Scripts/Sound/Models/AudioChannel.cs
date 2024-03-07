using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Sound.Models
{
    public class AudioChannel : MonoBehaviour
    {
        public string Name;
        [Range(0f, 1f)]
        public float volumeModifier =1f;
        [Range(0f, 2f)]
        public float pitchModifier = 1f;
        public AudioSource audioSource;

        void Awake()
        {
            this.audioSource = GetComponent<AudioSource>();
        }

        public void Play(Models.Sound sound) {
            // todo -> only update volume and pitch when actually needed.
            this.audioSource.volume = sound.volume * this.volumeModifier;
            this.audioSource.pitch = sound.pitch * this.pitchModifier;
            this.audioSource.clip = sound.clip;
            this.audioSource.Play();
        }

        public void StopAllAudioOnChannel()
        {
            throw new NotImplementedException("have not implemented stopAllAudio for a channel... Guess I'll only need this during a pause screen?");
        }

        public void PlayRepeating(Models.Sound sound, float repeatInterval)
        {
            throw new NotImplementedException("seems like music is the only thing that requires this...");
        }


    }
}
