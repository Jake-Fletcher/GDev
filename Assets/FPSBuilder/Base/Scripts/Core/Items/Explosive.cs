//=========== Copyright (c) GameBuilders, All rights reserved. ================
//
// Purpose: The Explosive Class can simulate various types of explosives, for example, grenades and impact explosives.
//          The properties define the behaviour of the explosive after being instantiated. No further configuration needed.
//
//=============================================================================

using FPSBuilder.Core.Managers;
using UnityEngine;

#pragma warning disable CS0649

namespace FPSBuilder.Core.Items
{
    [AddComponentMenu("FPS Builder/Items/Explosive"), DisallowMultipleComponent]
    public sealed class Explosive : MonoBehaviour
    {
        /// <summary>
        /// Defines the explosion radius of effect.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines the explosion radius of effect.")]
        private float m_ExplosionRadius = 15;

        /// <summary>
        /// Defines how strong the objects will be pushed if within the blast radius.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines how strong the objects will be pushed if within the blast radius.")]
        private float m_ExplosionForce = 35;

        /// <summary>
        /// Defines whether the explosion should ignore objects within the radius that may block its effects.
        /// </summary>
        [SerializeField]
        [Tooltip("Defines whether the explosion should ignore objects within the radius that may block its effects.")]
        private bool m_IgnoreCover;

        /// <summary>
        /// Defines the maximum damage caused by the explosion.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines the maximum damage caused by the explosion.")]
        private float m_Damage = 120;

        /// <summary>
        /// Defines whether the explosive should explode when it touches any surface.
        /// </summary>
        [SerializeField]
        [Tooltip("Defines whether the explosive should explode when it touches any surface.")]
        private bool m_ExplodeWhenCollide;

        /// <summary>
        /// Defines the time required in seconds for the object to explode.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines the time required in seconds for the object to explode.")]
        private float m_TimeToExplode = 3;

        /// <summary>
        /// The particle instantiated by the explosion.
        /// </summary>
        [SerializeField]
        [Required]
        [Tooltip("The particle instantiated by the explosion.")]
        private GameObject m_ExplosionParticle;

        /// <summary>
        /// The explosion sound.
        /// </summary>
        [SerializeField]
        [Tooltip("The explosion sound.")]
        private AudioClip m_ExplosionSound;

        /// <summary>
        /// Defines the explosion sound volume.
        /// </summary>
        [SerializeField]
        [Range(0, 1)]
        [Tooltip("Defines the explosion sound volume.")]
        private float m_ExplosionVolume = 0.5f;

        private void Start()
        {
            if (!m_ExplodeWhenCollide)
                Invoke(nameof(Explosion), m_TimeToExplode);
        }

        /// <summary>
        /// Simulates the explosion effect and instantiate the particles.
        /// </summary>
        private void Explosion()
        {
            if (m_ExplosionParticle != null)
                Instantiate(m_ExplosionParticle, transform.position, Quaternion.identity);

            Vector3 position = transform.position;

            // Calculate damage.
            Weapons.Explosion.CalculateExplosionDamage(m_ExplosionRadius, m_ExplosionForce, m_Damage, new Vector3(position.x, position.y, position.z), m_IgnoreCover);

            AudioManager.Instance.PlayClipAtPoint(m_ExplosionSound, position, 20, 100, m_ExplosionVolume);
            Destroy(gameObject);
        }

        /// <summary>
        /// Check if the explosive collided with something and the contact force.
        /// </summary>
        private void OnCollisionEnter()
        {
            if (m_ExplodeWhenCollide)
                Explosion();
        }

        /// <summary>
        /// Draw a sphere to visualize the radius of the blast effect.
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, m_ExplosionRadius);
        }
    }
}
