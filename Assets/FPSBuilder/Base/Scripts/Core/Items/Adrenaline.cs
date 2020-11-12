//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using System.Collections;
using FPSBuilder.Core.Managers;
using FPSBuilder.Core.Player;
using FPSBuilder.Core.Weapons;
using FPSBuilder.Interfaces;
using UnityEngine;

namespace FPSBuilder.Core.Items
{
    /// <summary>
    /// Adrenaline is an item that can restore the character’s vitality and apply a speed bonus temporarily.
    /// </summary>
    [AddComponentMenu("FPS Builder/Items/Adrenaline"), DisallowMultipleComponent]
    public class Adrenaline : MonoBehaviour, IUsable
    {
        /// <summary>
        /// The character HealthController reference.
        /// </summary>
        [SerializeField]
        [Required]
        [Tooltip("The character HealthController reference.")]
        protected HealthController m_HealthController;

        /// <summary>
        /// Defines how much vitality will be restored per use.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines how much vitality will be restored per use.")]
        protected float m_HealAmount = 100;

        /// <summary>
        /// Defines the delay in seconds to apply the effect.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines the delay in seconds to apply the effect.")]
        protected float m_DelayToInject = 1.3f;

        /// <summary>
        /// Defines how long the effect will last.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines how long the effect will last.")]
        protected float m_StaminaBonusDuration = 10;

        /// <summary>
        /// Defines the amount of syringes the character will start.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines the amount of syringes the character will start.")]
        protected int m_Amount = 3;

        /// <summary>
        /// Allow the character to use unlimited shots.
        /// </summary>
        [SerializeField]
        [Tooltip("Allow the character to use unlimited shots.")]
        protected bool m_InfiniteShots;

        /// <summary>
        /// Defines the maximum number of syringes the character can carry.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines the maximum number of syringes the character can carry.")]
        protected int m_MaxAmount = 3;

        /// <summary>
        /// The animator reference.
        /// </summary>
        [SerializeField]
        [Tooltip("The animator reference.")]
        protected Animator m_Animator;

        /// <summary>
        /// The name of the animation that will be played when applying an injection of adrenaline.
        /// </summary>
        [SerializeField]
        [Tooltip("The name of the animation that will be played when applying an injection of adrenaline.")]
        protected string m_ShotAnimation = "Inject";

        /// <summary>
        /// The audio that will be played when applying an injection of adrenaline.
        /// </summary>
        [SerializeField]
        [Tooltip("The audio that will be played when applying an injection of adrenaline.")]
        protected AudioClip m_ShotSound;

        /// <summary>
        /// Defines the sound volume when applying an adrenaline shot.
        /// </summary>
        [SerializeField]
        [Range(0, 1)]
        protected float m_ShotVolume = 0.3f;

        private WaitForSeconds m_ShotDuration;
        private AudioEmitter m_PlayerBodySource;

        #region PROPERTIES

        /// <summary>
        /// The current amount of syringes the character has.
        /// </summary>
        public int Amount => m_InfiniteShots ? 99 : m_Amount;

        /// <summary>
        /// The duration in seconds of the animation of applying an injection of adrenaline.
        /// </summary>
        public float ShotLength
        {
            get
            {
                if (!m_Animator)
                    return 0;

                if (m_ShotAnimation.Length == 0)
                    return 0;
                return m_Animator.GetAnimationClip(m_ShotAnimation).length > m_DelayToInject ? m_Animator.GetAnimationClip(m_ShotAnimation).length : m_DelayToInject;
            }
        }

        /// <summary>
        /// Can the character carry more adrenaline?
        /// </summary>
        public bool CanRefill => m_Amount < m_MaxAmount;

        #endregion

        /// <summary>
        /// Initializes the object.
        /// </summary>
        protected virtual void Init()
        {
            m_ShotDuration = new WaitForSeconds(m_Animator.GetAnimationClip(m_ShotAnimation).length > m_DelayToInject
                ? m_Animator.GetAnimationClip(m_ShotAnimation).length - m_DelayToInject : m_DelayToInject);

            DisableShadowCasting();
        }

        /// <summary>
        /// Uses a unit of the item and apply the effects to the character.
        /// </summary>
        public virtual void Use()
        {
            // In case the object has not yet been initialized.
            if (m_ShotDuration == null)
            {
                Init();
            }
            StartCoroutine(AdrenalineShot());
        }

        /// <summary>
        /// Play the animation, regenerate the character's vitality and apply a temporary speed bonus.
        /// </summary>
        protected virtual IEnumerator AdrenalineShot()
        {
            if (m_Animator)
                m_Animator.CrossFadeInFixedTime(m_ShotAnimation, 0.1f);

            if (m_PlayerBodySource == null)
                m_PlayerBodySource = AudioManager.Instance.RegisterSource("[AudioEmitter] CharacterBody", transform.root, spatialBlend: 0);

            m_PlayerBodySource.ForcePlay(m_ShotSound, m_ShotVolume);

            yield return new WaitForSeconds(m_DelayToInject);
            m_HealthController.Heal(m_HealAmount, m_StaminaBonusDuration > 0, m_StaminaBonusDuration);

            if (!m_InfiniteShots)
                m_Amount--;

            yield return m_ShotDuration;
        }

        /// <summary>
        /// Prevents the item from casting shadows.
        /// </summary>
        public void DisableShadowCasting()
        {
            // For each object that has a renderer inside the weapon gameObject
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
            {
                // Prevents the weapon from casting shadows
                r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }
        }

        /// <summary>
        /// Refill the adrenaline's syringes.
        /// </summary>
        public virtual void Refill()
        {
            m_Amount = m_MaxAmount;
        }
    }
}
