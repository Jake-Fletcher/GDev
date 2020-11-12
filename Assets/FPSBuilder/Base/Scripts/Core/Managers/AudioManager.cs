//=========== Copyright (c) GameBuilders, All rights reserved. ================
//
// Purpose: The Audio Manager Script controls all AudioSources used by the character,
//          grouping them in different categories, SFx, Music and Voice.
//
//=============================================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

#pragma warning disable CS0649

namespace FPSBuilder.Core.Managers
{
    /// <summary>
    /// The Audio Manager Script controls all AudioSources used by the character.
    /// </summary>
    [AddComponentMenu("FPS Builder/Managers/Audio Manager"), DisallowMultipleComponent]
    public sealed class AudioManager : Singleton<AudioManager>
    {
        /// <summary>
        /// Defines the Sound Effects volume, e.g explosions, environment and weapons.
        /// </summary>
        [SerializeField]
        [Range(0, 1)]
        [Tooltip("Defines the Sound Effects volume, e.g explosions, environment and weapons.")]
        private float m_SfxVolume = 1.0f;

        /// <summary>
        /// Defines the Voice volume, e.g character responses and CutScenes.
        /// </summary>
        [SerializeField]
        [Range(0, 1)]
        [Tooltip("Defines the Voice volume, e.g character responses and CutScenes.")]
        private float m_VoiceVolume = 1.0f;

        /// <summary>
        /// Defines the Music volume.
        /// </summary>
        [SerializeField]
        [Range(0, 1)]
        [Tooltip("Defines the Music volume.")]
        private float m_MusicVolume = 1.0f;

        /// <summary>
        /// The SFx Mixer is used to apply effects such echoing and deafness to SFx sources.
        /// </summary>
        [SerializeField]
        [Required]
        [Tooltip("The SFx Mixer is used to apply effects such echoing and deafness to SFx sources.")]
        private AudioMixerGroup m_SfxMixer;

        /// <summary>
        /// The Music Mixer is not affect by any king of effect, for this reason, it is perfect for all Music Sources in game.
        /// </summary>
        [SerializeField]
        [Required]
        [Tooltip("The Music Mixer is not affect by any king of effect, for this reason, it is perfect for all Music Sources in game.")]
        private AudioMixerGroup m_MusicMixer;

        /// <summary>
        /// Dictionary of AudioEmitter registered in this character.
        /// </summary>
        private readonly Dictionary<string, AudioEmitter> m_Sources = new Dictionary<string, AudioEmitter>();

        /// <summary>
        /// AudioSources Pool.
        /// </summary>
        private readonly List<AudioSource> m_InstancedSources = new List<AudioSource>();

        /// <summary>
        /// Maximum Pool Size.
        /// </summary>
        private const int k_MaxInstancedSources = 32;

        /// <summary>
        /// Last instance index used.
        /// </summary>
        private int m_LastPlayedIndex;

        #region PROPERTIES

        /// <summary>
        /// Defines the Sound Effects volume, e.g explosions, environment and weapons.
        /// </summary>
        public float SFxVolume
        {
            get => m_SfxVolume;
            set => m_SfxVolume = Mathf.Clamp01(value);
        }

        /// <summary>
        /// Defines the Voice volume, e.g character responses and CutScenes.
        /// </summary>
        public float VoiceVolume
        {
            get => m_VoiceVolume;
            set => m_VoiceVolume = Mathf.Clamp01(value);
        }

        /// <summary>
        /// Defines the Music volume.
        /// </summary>
        public float MusicVolume
        {
            get => m_MusicVolume;
            set => m_MusicVolume = Mathf.Clamp01(value);
        }

        /// <summary>
        /// Returns the SFx AudioMixerGroup. (Read Only)
        /// </summary>
        public AudioMixerGroup SFxMixer => m_SfxMixer;

        /// <summary>
        /// Returns the Music AudioMixerGroup. (Read Only)
        /// </summary>
        public AudioMixerGroup MusicMixer => m_MusicMixer;

        #endregion

        /// <summary>
        /// Register a new AudioEmitter and returns its reference.
        /// </summary>
        /// <param name="sourceName">The name of the AudioEmitter.</param>
        /// <param name="parent">Parent which the AudioEmitter will be child of.</param>
        /// <param name="category">The AudioEmitter category (SFx, Voice or Music).</param>
        /// <param name="minDistance">Within the Min distance the AudioEmitter will cease to grow louder in volume.</param>
        /// <param name="maxDistance">MaxDistance is the distance a sound stops attenuating at.</param>
        /// <param name="spatialBlend">Sets how much this AudioEmitter is affected by 3D spatialisation calculations (attenuation, doppler etc). 0.0 makes the sound full 2D, 1.0 makes it full 3D.</param>
        /// <returns></returns>
        public AudioEmitter RegisterSource(string sourceName = "Generic Source", Transform parent = null, AudioCategory category = AudioCategory.SFx, float minDistance = 1, float maxDistance = 3, float spatialBlend = 0.3f)
        {
            if (ContainsSource(sourceName))
                return m_Sources[sourceName];

            AudioEmitter audioSource = new AudioEmitter(parent, sourceName, category, minDistance, maxDistance, spatialBlend);
            m_Sources.Add(sourceName, audioSource);

            return m_Sources[sourceName];
        }

        /// <summary>
        /// Returns true if the AudioEmitter exists in the Sources List, otherwise false.
        /// </summary>
        private bool ContainsSource(string source)
        {
            return m_Sources.ContainsKey(source);
        }

        /// <summary>
        /// Search and return an AudioEmitter by the given name.
        /// </summary>
        /// <param name="sourceName">The name of the desired AudioEmitter.</param>
        /// <returns>The first AudioEmitter found with the name.</returns>
        public AudioEmitter GetSource(string sourceName)
        {
            return m_Sources.ContainsKey(sourceName) ? m_Sources[sourceName] : null;
        }

        /// <summary>
        /// Plays an AudioClip in the AudioEmitter if it's not being already used.
        /// </summary>
        /// <param name="sourceName">The name of the target AudioEmitter.</param>
        /// <param name="clip">The AudioClip to be played.</param>
        /// <param name="volume">The AudioEmitter volume.</param>
        public void Play(string sourceName, AudioClip clip, float volume)
        {
            if (m_Sources.ContainsKey(sourceName))
            {
                AudioEmitter audioSource = m_Sources[sourceName];
                audioSource.Play(clip, volume);
            }
            else
            {
                Debug.LogError("AudioManager: AudioSource '" + sourceName + "' was not found.");
            }
        }

        /// <summary>
        /// Forces the AudioEmitter to stop playing the current AudioClip and play immediately the requested sound.
        /// </summary>
        /// <param name="sourceName">The name of the target AudioEmitter.</param>
        /// <param name="clip">The AudioClip to be played.</param>
        /// <param name="volume">The AudioEmitter volume.</param>
        public void ForcePlay(string sourceName, AudioClip clip, float volume)
        {
            if (m_Sources.ContainsKey(sourceName))
            {
                AudioEmitter audioSource = m_Sources[sourceName];
                audioSource.ForcePlay(clip, volume);
            }
            else
            {
                Debug.LogError("AudioManager: AudioSource '" + sourceName + "' was not found.");
            }
        }

        /// <summary>
        /// Calculate the AudioEmitter volume by using the formula: (volume = 1 - value / startValue).
        /// Useful when you need to start playing a sound at certain point. e.g when the character 
        /// vitality is below certain point, it will increase the volume as the vitality gets lower.
        /// </summary>
        /// <param name="sourceName">The name of the target AudioEmitter.</param>
        /// <param name="startValue">The volume will be calculated from this value.</param>
        /// <param name="value">The current value to be calculated the volume.</param>
        /// <param name="maxVolume">The AudioEmitter maximum volume.</param>
        public void CalculateVolumeByPercent(string sourceName, float startValue, float value, float maxVolume)
        {
            if (m_Sources.ContainsKey(sourceName))
            {
                AudioEmitter audioSource = m_Sources[sourceName];
                audioSource.CalculateVolumeByPercent(startValue, value, maxVolume);
            }
            else
            {
                Debug.LogError("AudioManager: AudioSource '" + sourceName + "' was not found.");
            }
        }

        /// <summary>
        /// Immediately stop playing the current AudioClip.
        /// </summary>
        /// <param name="sourceName">The name of the target AudioEmitter.</param>
        public void Stop(string sourceName)
        {
            if (m_Sources.ContainsKey(sourceName))
            {
                AudioEmitter audioSource = m_Sources[sourceName];
                audioSource.Stop();
            }
            else
            {
                Debug.LogError("AudioManager: AudioSource '" + sourceName + "' was not found.");
            }
        }

        /// <summary>
        /// Returns the first AudioSource available in the object pool.
        /// </summary>
        private AudioSource GetAvailableSource()
        {
            for (int i = 0, c = m_InstancedSources.Count; i < c; i++)
            {
                if (!m_InstancedSources[i].isPlaying)
                {
                    return m_InstancedSources[i];
                }
            }

            if (m_InstancedSources.Count < k_MaxInstancedSources)
            {
                // If can't find any audio source available, create a new one and return it.
                GameObject go = new GameObject("Generic Source");
                AudioSource source = go.AddComponent<AudioSource>();

                m_InstancedSources.Add(source);
                return source;
            }

            int index = m_LastPlayedIndex++ % m_InstancedSources.Count;
            m_InstancedSources[index].Stop();
            return m_InstancedSources[index];
        }

        /// <summary>
        /// Plays an AudioClip at a given position in world space.
        /// </summary>
        /// <param name="clip">The AudioClip to be played.</param>
        /// <param name="position">The AudioSource position in the world.</param>
        /// <param name="minDistance">Within the Min distance the AudioEmitter will cease to grow louder in volume.</param>
        /// <param name="maxDistance">MaxDistance is the distance a sound stops attenuating at.</param>
        /// <param name="volume">The AudioSource volume.</param>
        /// <param name="spatialBlend">Sets how much this AudioSource is affected by 3D spatialisation calculations (attenuation, doppler etc).</param>
        public void PlayClipAtPoint(AudioClip clip, Vector3 position, float minDistance, float maxDistance, float volume, float spatialBlend = 1)
        {
            if (!clip)
                return;

            AudioSource source = GetAvailableSource();
            source.gameObject.name = "Generic Source [Position " + position + "]";
            source.transform.position = position;
            source.playOnAwake = false;

            source.clip = clip;
            source.volume = volume * m_SfxVolume;

            source.rolloffMode = AudioRolloffMode.Linear;
            source.minDistance = minDistance;
            source.maxDistance = maxDistance;

            source.spatialBlend = spatialBlend;
            source.outputAudioMixerGroup = m_SfxMixer;

            source.Play();
        }
    }
}
