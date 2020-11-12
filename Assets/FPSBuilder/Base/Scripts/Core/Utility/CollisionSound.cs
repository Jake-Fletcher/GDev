//=========== Copyright (c) GameBuilders, All rights reserved. ================
//
// Purpose: The collision sound component is utilized to play a sound whenever the object hits
//          another with a relative velocity greater than the minimum impact force.
//
//=============================================================================

using FPSBuilder.Core.Managers;
using UnityEngine;

#pragma warning disable CS0649

namespace FPSBuilder.Core
{
    [RequireComponent(typeof(AudioSource))]
    public class CollisionSound : MonoBehaviour
    {
        /// <summary>
        /// Should ignore collision with the character?
        /// </summary>
        [SerializeField]
        [Tooltip("Should ignore collision with the character?")]
        private bool m_IgnoreCharacter = true;

        /// <summary>
        /// The minimum force required when the collider hits another one to play the impact sound.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("The minimum force required when the collider hits another one to play the impact sound.")]
        private float m_MinimumImpactForce = 0.25f;

        /// <summary>
        /// The sound played when the collider hits another one.
        /// </summary>
        [SerializeField]
        [Tooltip("The sound played when the collider hits another one.")]
        private AudioClip m_CollisionSound;

        /// <summary>
        /// The collision sound volume.
        /// </summary>
        [SerializeField]
        [Range(0, 1)]
        private float m_CollisionVolume = 0.3f;

        private AudioSource m_ColliderSource;

        private void Start()
        {
            m_ColliderSource = GetComponent<AudioSource>();
            m_ColliderSource.playOnAwake = false;
            m_ColliderSource.clip = m_CollisionSound;
            m_ColliderSource.volume = m_CollisionVolume * AudioManager.Instance.SFxVolume;
        }

        /// <summary>
        /// Check if the collider collided with something and the contact force.
        /// </summary>
        /// <param name="col">Collider hit by the explosive.</param>
        private void OnCollisionEnter(Collision col)
        {
            if (m_IgnoreCharacter && col.gameObject.CompareTag("Player"))
                return;

            if (col.relativeVelocity.magnitude > m_MinimumImpactForce)
            {
                m_ColliderSource.Play();
            }
        }
    }
}