//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using UnityEngine;

namespace FPSBuilder.Core.Managers
{
    /// <summary>
    /// AudioEmitter categories (used to set the volume modifier).
    /// </summary>
    public enum AudioCategory
    {
        SFx,
        Voice,
        Music
    }

    /// <summary>
    /// AudioEmitter is an AudioSource preset used by the AudioManager, making it quick and less manual for setting up, through code.
    /// </summary>
    public class AudioEmitter
    {
        private readonly AudioCategory m_Category;
        private readonly AudioSource m_Source;

        /// <summary>
        /// Is the clip playing right now? (Read Only)
        /// </summary>
        public bool IsPlaying => m_Source.isPlaying;

        /// <summary>
        /// Creates a new AudioEmitter.
        /// </summary>
        /// <param name="parent">Parent which the AudioEmitter will be child of.</param>
        /// <param name="name">The name of this AudioEmitter.</param>
        /// <param name="category">The AudioEmitter category (SFx, Voice or Music).</param>
        /// <param name="minDistance">Within the min distance this AudioEmitter will cease to grow louder in volume.</param>
        /// <param name="maxDistance">MaxDistance is the distance a sound stops attenuating at.</param>
        /// <param name="spatialBlend">Sets how much this AudioEmitter is affected by 3D spatialisation calculations (attenuation, doppler etc). 0.0 makes the sound full 2D, 1.0 makes it full 3D.</param>
        public AudioEmitter(Transform parent, string name, AudioCategory category, float minDistance, float maxDistance, float spatialBlend)
        {
            m_Category = category;

            GameObject go = new GameObject(name);
            go.transform.SetParent(parent);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            m_Source = go.AddComponent<AudioSource>();

            m_Source.rolloffMode = AudioRolloffMode.Linear;
            m_Source.minDistance = minDistance;
            m_Source.maxDistance = maxDistance;
            m_Source.spatialBlend = spatialBlend;

            m_Source.outputAudioMixerGroup = (category == AudioCategory.Music) ? AudioManager.Instance.MusicMixer : AudioManager.Instance.SFxMixer;
        }

        /// <summary>
        /// Plays an AudioClip in the AudioEmitter if it's not being already used.
        /// </summary>
        /// <param name="clip">The AudioClip to be played.</param>
        /// <param name="volume">The AudioEmitter volume.</param>
        public void Play(AudioClip clip, float volume)
        {
            if (!clip)
            {
                Debug.LogError("AudioManager: AudioClip '" + clip + "' was not found.");
                return;
            }
            
            if (m_Source.isPlaying && m_Source.clip == clip)
                return;

            m_Source.clip = clip;
            m_Source.volume = GetVolume(volume);
            m_Source.Play();
        }

        /// <summary>
        /// Forces the AudioEmitter to stop playing the current AudioClip and play immediately the requested sound.
        /// </summary>
        /// <param name="clip">The AudioClip to be played.</param>
        /// <param name="volume">The AudioEmitter volume.</param>
        public void ForcePlay(AudioClip clip, float volume)
        {
            if (!clip)
            {
                Debug.LogError("AudioManager: AudioClip '" + clip + "' was not found.");
                return;
            }

            m_Source.clip = clip;
            m_Source.volume = GetVolume(volume);
            m_Source.Play();
        }

        /// <summary>
        /// Immediately stop playing the current AudioClip.
        /// </summary>
        public void Stop()
        {
            m_Source.Stop();
        }

        /// <summary>
        /// Calculate the AudioEmitter volume by using the formula: (volume = 1 - value / startValue).
        /// Useful when you need to start playing a sound at certain point. e.g when the character 
        /// vitality is below certain point, it will increase the volume as the vitality gets lower.
        /// </summary>
        /// <param name="startValue">The volume will be calculated from this value.</param>
        /// <param name="value">The current value to be calculated the volume.</param>
        /// <param name="maxVolume">The AudioEmitter maximum volume.</param>
        public float CalculateVolumeByPercent(float startValue, float value, float maxVolume)
        {
            float vol = 1 - value / startValue;
            m_Source.volume = GetVolume(Mathf.Clamp(vol, 0, maxVolume));
            return m_Source.volume;
        }

        /// <summary>
        /// Returns the volume value adjusted by its category.
        /// </summary>
        /// <param name="volume">Reference volume.</param>
        /// <returns></returns>
        private float GetVolume(float volume)
        {
            switch (m_Category)
            {
                case AudioCategory.SFx:
                return volume * AudioManager.Instance.SFxVolume;
                case AudioCategory.Voice:
                return volume * AudioManager.Instance.VoiceVolume;
                case AudioCategory.Music:
                return volume * AudioManager.Instance.MusicVolume;
                default:
                return 0;
            }
        }
    }
}