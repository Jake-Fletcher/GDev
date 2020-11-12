//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using System.Collections;
using FPSBuilder.Core.Managers;
using FPSBuilder.Core.Weapons;
using FPSBuilder.Interfaces;
using UnityEngine;

namespace FPSBuilder.Core.Items
{
    [AddComponentMenu("FPS Builder/Items/Grenade"), DisallowMultipleComponent]
    public class Grenade : MonoBehaviour, IUsable
    {
        /// <summary>
        /// The grenade explosive prefab.
        /// </summary>
        [SerializeField]
        [Required]
        [Tooltip("The grenade explosive prefab.")]
        protected Rigidbody m_Grenade;

        /// <summary>
        /// The Transform reference used to know where the grenade will be instantiated from.
        /// </summary>
        [SerializeField]
        [Required]
        [Tooltip("The Transform reference used to know where the grenade will be instantiated from.")]
        protected Transform m_ThrowTransformReference;

        /// <summary>
        /// Defines the force that the character will throw the grenade.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines the force that the character will throw the grenade.")]
        protected float m_ThrowForce = 20;

        /// <summary>
        /// Defines the delay in seconds for the character throw the grenade.
        /// (For some grenades it is necessary to remove the protection pin before throwing, use this field to adjust the necessary time for such action.)
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines the delay in seconds for the character throw the grenade. " +
                 "(For some grenades it is necessary to remove the protection pin before throwing, use this field to adjust the necessary time for such action.)")]
        protected float m_DelayToInstantiate = 0.14f;

        /// <summary>
        /// Defines the amount of grenades the character will start.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines the amount of grenades the character will start.")]
        protected int m_Amount = 3;

        /// <summary>
        /// Allow the character to use unlimited grenades.
        /// </summary>
        [SerializeField]
        [Tooltip("Allow the character to use unlimited grenades.")]
        protected bool m_InfiniteGrenades;

        /// <summary>
        /// Defines the maximum number of grenades the character can carry.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines the maximum number of grenades the character can carry.")]
        protected int m_MaxAmount = 3;

        /// <summary>
        /// The animator reference.
        /// </summary>
        [SerializeField]
        [Required]
        [Tooltip("The animator reference.")]
        protected Animator m_Animator;

        /// <summary>
        /// Animation of pulling the grenade pin.
        /// </summary>
        [SerializeField]
        [Tooltip("Animation of pulling the grenade pin.")]
        protected string m_PullAnimation;

        /// <summary>
        /// Sound of pulling the grenade pin.
        /// </summary>
        [SerializeField]
        [Tooltip("Sound of pulling the grenade pin.")]
        protected AudioClip m_PullSound;

        /// <summary>
        /// Defines the volume of the grenade pin pulling sound.
        /// </summary>
        [SerializeField]
        [Range(0, 1)]
        [Tooltip("Defines the volume of the grenade pin pulling sound.")]
        protected float m_PullVolume = 0.2f;

        /// <summary>
        /// Animation of throwing the grenade.
        /// </summary>
        [SerializeField]
        [Tooltip("Animation of throwing the grenade.")]
        protected string m_ThrowAnimation;

        /// <summary>
        /// The sound of throwing the grenade.
        /// </summary>
        [SerializeField]
        [Tooltip("The sound of throwing the grenade.")]
        protected AudioClip m_ThrowSound;

        /// <summary>
        /// The volume of the grenade throwing sound.
        /// </summary>
        [SerializeField]
        [Range(0, 1)]
        [Tooltip("The volume of the grenade throwing sound.")]
        protected float m_ThrowVolume = 0.2f;

        private WaitForSeconds m_PullDuration;
        private WaitForSeconds m_InstantiateDelay;
        private AudioEmitter m_PlayerBodySource;

        #region PROPERTIES

        /// <summary>
        /// The current amount of grenades the character has.
        /// </summary>
        public int Amount => m_InfiniteGrenades ? 99 : m_Amount;

        /// <summary>
        /// The duration in seconds of the animations of pulling and throwing a grenade combined.
        /// </summary>
        public virtual float PullAndThrowLength
        {
            get
            {
                if (!m_Animator)
                    return 0;

                if (m_PullAnimation.Length == 0 && m_ThrowAnimation.Length == 0)
                    return 0;

                if (m_Animator.GetAnimationClip(m_ThrowAnimation).length < m_DelayToInstantiate)
                    return m_Animator.GetAnimationClip(m_PullAnimation).length + m_DelayToInstantiate;

                return m_Animator.GetAnimationClip(m_PullAnimation).length + m_Animator.GetAnimationClip(m_ThrowAnimation).length;
            }
        }

        /// <summary>
        /// Can the character carry more grenades?
        /// </summary>
        public bool CanRefill => m_Amount < m_MaxAmount;

        #endregion

        /// <summary>
        /// Initializes the object.
        /// </summary>
        protected virtual void Init()
        {
            m_PullDuration = new WaitForSeconds(m_Animator.GetAnimationClip(m_PullAnimation).length);
            m_InstantiateDelay = new WaitForSeconds(m_DelayToInstantiate);

            DisableShadowCasting();
        }

        /// <summary>
        /// Uses a unit of the item and instantiates a grenade.
        /// </summary>
        public virtual void Use()
        {
            // In case the object has not yet been initialized.
            if (m_PullDuration == null || m_InstantiateDelay == null)
            {
                Init();
            }

            if (m_Grenade && m_ThrowTransformReference)
                StartCoroutine(ThrowGrenade());
        }

        /// <summary>
        /// Play the animation and instantiates a grenade.
        /// </summary>
        protected virtual IEnumerator ThrowGrenade()
        {
            if (m_Animator)
                m_Animator.CrossFadeInFixedTime(m_PullAnimation, 0.1f);

            if (m_PlayerBodySource == null)
                m_PlayerBodySource = AudioManager.Instance.RegisterSource("[AudioEmitter] CharacterBody", transform.root, spatialBlend: 0);

            m_PlayerBodySource.ForcePlay(m_PullSound, m_PullVolume);

            yield return m_PullDuration;

            if (m_Animator)
                m_Animator.CrossFadeInFixedTime(m_ThrowAnimation, 0.1f);

            m_PlayerBodySource.ForcePlay(m_ThrowSound, m_ThrowVolume);

            yield return m_InstantiateDelay;

            InstantiateGrenade();
        }

        /// <summary>
        /// Throw a grenade using the parameters.
        /// </summary>
        protected virtual void InstantiateGrenade()
        {
            if (!m_Grenade)
                return;

            Rigidbody clone = Instantiate(m_Grenade, m_ThrowTransformReference.position, m_ThrowTransformReference.rotation);
            clone.velocity = clone.transform.TransformDirection(Vector3.forward) * m_ThrowForce;

            if (!m_InfiniteGrenades)
                m_Amount--;
        }

        /// <summary>
        /// Prevents the item from casting shadows.
        /// </summary>
        public void DisableShadowCasting()
        {
            // For each object that has a renderer inside the weapon gameObject
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
            {
                r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }
        }

        /// <summary>
        /// Refill the grenades.
        /// </summary>
        public virtual void Refill()
        {
            m_Amount = m_MaxAmount;
        }
    }
}
