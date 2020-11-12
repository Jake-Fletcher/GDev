//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using UnityEngine;

namespace FPSBuilder.Core.Weapons
{
    /// <summary>
    /// Contains all information used by the weapon manager to validate a object as a gun pickup prefab.
    /// </summary>
    [AddComponentMenu("FPS Builder/Weapon/Gun Pickup"), DisallowMultipleComponent]
    public class GunPickup : MonoBehaviour
    {
        /// <summary>
        /// The Gun Data Asset that this gun pickup represents.
        /// </summary>
        [SerializeField]
        [Required]
        [Tooltip("The Gun Data Asset that this gun pickup represents.")]
        protected GunData m_GunData;

        /// <summary>
        /// Returns the ID represented by this gun pickup.
        /// </summary>
        public int ID => m_GunData != null ? m_GunData.GetInstanceID() : -1;

        protected Rigidbody m_RigidBody;
        protected Collider m_Collider;

        protected void Start()
        {
            m_RigidBody = GetComponent<Rigidbody>();
            m_Collider = GetComponent<Collider>();
        }

        protected void Update()
        {
            if (!m_RigidBody || !m_Collider)
                return;

            if (!m_RigidBody.IsSleeping())
                return;

            m_RigidBody.isKinematic = true;
            m_Collider.isTrigger = true;
        }
    }
}

