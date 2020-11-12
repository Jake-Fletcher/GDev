//=========== Copyright (c) GameBuilders, All rights reserved. ================
//
// Purpose: The Camera Animator Script is the component used to animate the camera transform. Since all camera animations are procedural,
//          this script is responsible to coordinate the playing order and fading.
//
//=============================================================================

using System;
using System.Collections;
using FPSBuilder.Core.Animation;
using FPSBuilder.Core.Input;
using FPSBuilder.Core.Managers;
using UnityEngine;

namespace FPSBuilder.Core.Player
{
    /// <summary>
    /// The Camera Animator Script is the component used to animate the camera transform.
    /// </summary>
    [AddComponentMenu("FPS Builder/Controllers/Camera Animator"), DisallowMultipleComponent]
    public class CameraAnimator : MonoBehaviour
    {
        /// <summary>
        /// Defines the reference to the First Person Character Controller script.
        /// </summary>
        [SerializeField]
        [Required]
        [Tooltip("Defines the reference to the First Person Character Controller script.")]
        protected FirstPersonCharacterController m_FPController;

        /// <summary>
        /// Defines the reference to the Health Controller script.
        /// </summary>
        [SerializeField]
        [Required]
        [Tooltip("Defines the reference to the Health Controller script.")]
        protected HealthController m_HealthController;

        /// <summary>
        /// Defines the how the camera breath animation should be played.
        /// </summary>
        [SerializeField]
        [Tooltip("Defines the how the camera breath animation should be played.")]
        protected BreathAnimationMode m_BreathAnimationMode = BreathAnimationMode.EnableWhenAim;

        /// <summary>
        /// The MotionAnimation component.
        /// </summary>
        [SerializeField]
        protected MotionAnimation m_MotionAnimation = new MotionAnimation();

        /// <summary>
        /// Sound played when the character hold his breath to be more steady while aiming.
        /// </summary>
        [SerializeField]
        [Tooltip("Sound played when the character hold his breath to be more steady while aiming.")]
        protected AudioClip m_HoldBreath;

        /// <summary>
        /// Sound played when the character exhale after a long period holding his breath.
        /// </summary>
        [SerializeField]
        [Tooltip("Sound played when the character exhale after a long period holding his breath.")]
        protected AudioClip m_Exhale;

        /// <summary>
        /// Defines the volume of Hold Breath Sound and Exhale Sound.
        /// </summary>
        [SerializeField]
        [Range(0, 1)]
        [Tooltip("Defines the volume of Hold Breath Sound and Exhale Sound.")]
        protected float m_HoldBreathVolume = 0.3f;

        #region PROPERTIES

        /// <summary>
        /// Defines if the camera can hold your breath to stabilize your vision while aiming.
        /// </summary>
        public bool HoldBreath
        {
            protected get;
            set;
        }

        /// <summary>
        /// Returns the current character leaning direction (left or right). Interval [-1, 1]
        /// </summary>
        public int LeanDirection
        {
            get;
            private set;
        }

        #endregion

        private Button m_RunButton;
        private Button m_LeanRight;
        private Button m_LeanLeft;

        private float m_NextHoldBreathTime;
        private float m_HoldBreathDuration;

        private IEnumerator m_CurrentShakeCoroutine;
        private AudioEmitter m_PlayerGenericSource;

        protected virtual void Start()
        {
            // Events callback
            m_FPController.PreJumpEvent += CameraBraceForJump;
            m_FPController.JumpEvent += CameraJump;
            m_FPController.LandingEvent += CameraLanding;
            m_FPController.VaultEvent += Vault;
            m_FPController.GettingUpEvent += Vault;

            m_HealthController.ExplosionEvent += GrenadeExplosion;
            m_HealthController.HitEvent += Hit;

            // Input buttons
            m_LeanRight = InputManager.FindButton("Lean Right");
            m_LeanLeft = InputManager.FindButton("Lean Left");
            m_RunButton = InputManager.FindButton("Run");

            // AudioSources
            m_PlayerGenericSource = AudioManager.Instance.RegisterSource("[AudioEmitter] Generic", transform.root);
        }

        protected virtual void Update()
        {
            // Movement animation
            m_MotionAnimation.MovementAnimation(m_FPController);

            // Breath animation
            m_MotionAnimation.BreathingAnimation(((m_BreathAnimationMode == BreathAnimationMode.EnableWhenAim && m_FPController.IsAiming)
                || m_BreathAnimationMode == BreathAnimationMode.Unrestricted || m_BreathAnimationMode == BreathAnimationMode.DisableWhenAim && !m_FPController.IsAiming)
                && Math.Abs(m_HoldBreathDuration) < Mathf.Epsilon ? 1 : 0);

            if (HoldBreath)
            {
                //Hold breath
                if (InputManager.GetButton(m_RunButton) && m_NextHoldBreathTime < Time.time && m_FPController.IsAiming && !m_FPController.TremorTrauma)
                {
                    if (Math.Abs(m_HoldBreathDuration) < Mathf.Epsilon)
                        m_PlayerGenericSource.Play(m_HoldBreath, m_HoldBreathVolume);

                    m_HoldBreathDuration += Time.deltaTime;
                    if (m_HoldBreathDuration > m_HoldBreath.length)
                    {
                        m_NextHoldBreathTime = Time.time + 3 + m_HoldBreathDuration;
                        m_HoldBreathDuration = 0;
                        m_PlayerGenericSource.Play(m_Exhale, m_HoldBreathVolume);
                    }
                }
                //Release the breath
                else
                {
                    if (m_HoldBreathDuration > 0)
                        m_PlayerGenericSource.Stop();

                    if (m_HoldBreathDuration > m_HoldBreath.length * 0.7f)
                    {
                        m_NextHoldBreathTime = Time.time + 3 + m_HoldBreathDuration;
                        m_PlayerGenericSource.Play(m_Exhale, m_HoldBreathVolume);
                    }

                    m_HoldBreathDuration = 0;
                }
            }

            // Leaning animations
            if (!m_MotionAnimation.Lean)
                return;

            if (m_FPController.State != MotionState.Flying && m_FPController.State != MotionState.Running && m_FPController.State != MotionState.Climbing)
            {
                // Lean by holding the button
                if (GameplayManager.Instance.LeanStyle == ActionMode.Hold)
                {
                    if (InputManager.GetButton(m_LeanLeft) && !InputManager.GetButton(m_LeanRight))
                    {
                        LeanDirection = CanLean(Vector3.left) ? -1 : 0;
                    }

                    if (InputManager.GetButton(m_LeanRight) && !InputManager.GetButton(m_LeanLeft))
                    {
                        LeanDirection = CanLean(Vector3.right) ? 1 : 0;
                    }

                    if (!InputManager.GetButton(m_LeanLeft) && !InputManager.GetButton(m_LeanRight))
                    {
                        LeanDirection = 0;
                    }

                    m_MotionAnimation.LeanAnimation(LeanDirection);
                }
                else
                {
                    // Lean by tapping the button
                    if (InputManager.GetButtonDown(m_LeanLeft) && LeanDirection != -1)
                    {
                        LeanDirection = LeanDirection == 1 ? 0 : -1;
                    }
                    else if (InputManager.GetButtonDown(m_LeanLeft) && LeanDirection == -1)
                    {
                        LeanDirection = 0;
                    }

                    if (InputManager.GetButtonDown(m_LeanRight) && LeanDirection != 1)
                    {
                        LeanDirection = LeanDirection == -1 ? 0 : 1;
                    }
                    else if (InputManager.GetButtonDown(m_LeanRight) && LeanDirection == 1)
                    {
                        LeanDirection = 0;
                    }

                    if ((LeanDirection == -1 && !CanLean(Vector3.left)) || (LeanDirection == 1 && !CanLean(Vector3.right)))
                        LeanDirection = 0;

                    m_MotionAnimation.LeanAnimation(LeanDirection);
                }
            }
            else
            {
                LeanDirection = 0;
                m_MotionAnimation.LeanAnimation(0);
            }
        }

        /// <summary>
        /// Casts a ray to evaluate if the character can lean to a determinate direction.
        /// </summary>
        /// <param name="direction">The desired direction.</param>
        private bool CanLean(Vector3 direction)
        {
            Ray ray = new Ray(m_FPController.transform.position, m_FPController.transform.TransformDirection(direction));
            return !Physics.SphereCast(ray, m_MotionAnimation.LeanAmount, out _, m_MotionAnimation.LeanAmount * 2, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        }

        /// <summary>
        /// Event method that simulates the effect of preparing to jump on the motion animation component.
        /// </summary>
        protected virtual void CameraBraceForJump()
        {
            StartCoroutine(m_MotionAnimation.BraceForJumpAnimation.Play());
        }

        /// <summary>
        /// Event method that simulates the effect of jump on the motion animation component.
        /// </summary>
        protected virtual void CameraJump()
        {
            StartCoroutine(m_MotionAnimation.JumpAnimation.Play());
        }

        /// <summary>
        /// Event method that simulates the effect of landing on the motion animation component.
        /// </summary>
        protected virtual void CameraLanding(float fallDamage)
        {
            StartCoroutine(m_MotionAnimation.LandingAnimation.Play());
        }

        /// <summary>
        /// Updates the camera recoil settings and play a new recoil animation.
        /// </summary>
        /// <param name="cameraRecoil">Is camera recoil available for this weapon?</param>
        /// <param name="minCameraRecoilRotation">The minimum rotation applied by this recoil.</param>
        /// <param name="maxCameraRecoilRotation">The maximum rotation applied by this recoil.</param>
        /// <param name="cameraRecoilDuration">The duration of the recoil animation.</param>
        /// <param name="cameraReturnDuration">The duration of the stabilization animation.</param>
        public virtual void ApplyRecoil(bool cameraRecoil, Vector3 minCameraRecoilRotation, Vector3 maxCameraRecoilRotation, float cameraRecoilDuration, float cameraReturnDuration)
        {
            // Update camera recoil properties
            m_MotionAnimation.CameraRecoil = cameraRecoil;
            m_MotionAnimation.MinCameraRecoilRotation = minCameraRecoilRotation;
            m_MotionAnimation.MaxCameraRecoilRotation = maxCameraRecoilRotation;
            m_MotionAnimation.CameraRecoilDuration = cameraRecoilDuration;
            m_MotionAnimation.CameraReturnDuration = cameraReturnDuration;

            StopCoroutine(m_MotionAnimation.RecoilAnimation(true));
            StartCoroutine(m_MotionAnimation.RecoilAnimation(true));
        }

        /// <summary>
        /// Event method that simulates the effect of explosion on the motion animation component.
        /// </summary>
        protected virtual void GrenadeExplosion()
        {
            if (m_CurrentShakeCoroutine != null)
            {
                StopCoroutine(m_CurrentShakeCoroutine);
            }

            m_CurrentShakeCoroutine = m_MotionAnimation.Shake(m_MotionAnimation.ExplosionShake);
            StartCoroutine(m_CurrentShakeCoroutine);
        }

        /// <summary>
        /// Event method that simulates the effect of character hit by a projectile on the motion animation component.
        /// </summary>
        protected virtual void Hit()
        {
            StartCoroutine(m_MotionAnimation.HitAnimation());
        }

        /// <summary>
        /// Event method that simulates the effect of vaulting on the motion animation component.
        /// </summary>
        protected virtual void Vault()
        {
            StartCoroutine(m_MotionAnimation.VaultAnimation.Play());
        }
    }
}
