using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Sound.Models;


namespace Assets.Scripts.Sound.Implementation
{
    public class AudioController : MonoBehaviour
    { 
        [SerializeField] private List<Models.Sound> SoundList;
        Dictionary<string, Models.Sound> soundNameToSound = new Dictionary<string, Models.Sound>();
        // Dictionary<AudioChannel, List<Models.Sound>> channelToSounds = new Dictionary<AudioChannel, List<Models.Sound>>();
        
        
        void Awake()
        {
            foreach(Models.Sound sound in SoundList)
            {
                soundNameToSound[sound.soundName] = sound;
            }

        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Play an audio clip one time.
        /// </summary>
        /// <param name="clip"></param>
        public void Play(string soundName)
        {
            if (!soundNameToSound.ContainsKey(soundName))
            {
                Debug.Log($"No sound object registered for {soundName}. Failed to play.");
                return;
            }

            Models.Sound soundToPlay = soundNameToSound[soundName];

            if(soundToPlay.channel is null)
            {
                Debug.Log($"Sound {soundName} is not associated with an audio channel. Failed to play.");
            } else
            {
                soundToPlay.channel.Play(soundToPlay);
            }
        }

        /// <summary>
        /// Stop all sounds on a specific channel.
        /// </summary>
        public void StopAudioInChannel(string channelName)
        {

        }

        /// <summary>
        /// play a sound repeatedly until we request it to stop playing
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="textSpeed"></param>
        public void PlayRepeating(AudioClip clip, float textSpeed)
        {

        }

        /// <summary>
        /// Adds an audio clip to a specified channel. Creates sound channelif it doesn't exist.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="clip"></param>
        public void AddSoundToChannel(string name, AudioClip clip)
        {

        }
    }
}

