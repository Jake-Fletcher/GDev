﻿//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using System.Collections;
using FPSBuilder.Core.Player;
using UnityEngine;

#pragma warning disable CS0649

namespace FPSBuilder.Core.Animation
{
    /// <summary>
    /// Breath Animation Modes.
    /// </summary>
    public enum BreathAnimationMode
    {
        Unrestricted = 0,
        EnableWhenAim = 1,
        DisableWhenAim = 2
    }

    /// <summary>
    /// The Motion Animation class is responsible for all procedural animations performed on the character.
    /// </summary>
    [System.Serializable]
    public sealed class MotionAnimation
    {
        /// <summary>
        /// Determines the overall magnitude of the animation amplitude.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Determines the overall magnitude of the animation amplitude.")]
        private float m_ScaleFactor = 1;

        /// <summary>
        /// Defines the overall speed of this animator.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines the overall speed of this animator.")]
        private float m_OverallSpeed = 1;

        /// <summary>
        /// Provides a set of rules used to simulate the walking animation on the Target Transform.
        /// </summary>
        [SerializeField]
        [Tooltip("Provides a set of rules used to simulate the walking animation on the Target Transform.")]
        private MotionData m_WalkingMotionData;

        /// <summary>
        /// Provides a set of rules used to simulate the walking animation on the Target Transform (Used when the character has their legs injured).
        /// </summary>
        [SerializeField]
        [Tooltip("Provides a set of rules used to simulate the walking animation on the Target Transform (Used when the character has their legs injured).")]
        private MotionData m_BrokenLegsMotionData;

        /// <summary>
        /// Provides a set of rules used to simulate the running animation on the Target Transform.
        /// </summary>
        [SerializeField]
        [Tooltip("Provides a set of rules used to simulate the running animation on the Target Transform.")]
        private MotionData m_RunningMotionData;

        /// <summary>
        /// Defines the transform which will be animated.
        /// </summary>
        [SerializeField]
        [Required]
        [Tooltip("Defines the transform which will be animated.")]
        private Transform m_TargetTransform;

        #region JUMP & FALL ANIMATIONS

        /// <summary>
        /// Brace For Jump Animation is a routine that defines the transform’s target position and rotation,
        /// interpolating between them and returning to the original coordinates.
        /// This routine is used to simulate the animation of the character while it’s preparing to perform a jump, like channeling energy.
        /// </summary>
        [SerializeField]
        private LerpAnimation m_BraceForJumpAnimation = new LerpAnimation(new Vector3(0, -0.2f, 0), new Vector3(5, 0, 0), 0.25f, 0.2f);

        /// <summary>
        /// The Jump Animation is a routine that defines the transform’s target position and rotation,
        /// interpolating between them and returning to the original coordinates.
        /// This routine is used to simulate the character jumping animation.
        /// </summary>
        [SerializeField]
        private LerpAnimation m_JumpAnimation = new LerpAnimation(new Vector3(0, 0.05f, 0), new Vector3(10, 0, 0), 0.15f);

        /// <summary>
        /// The Landing Animation is a routine that defines the transform’s target position and rotation,
        /// interpolating between them and returning to the original coordinates.
        /// This routine is used to simulate the character landing animation, played after the character touches the ground.
        /// </summary>
        [SerializeField]
        private LerpAnimation m_LandingAnimation = new LerpAnimation(new Vector3(0, -0.075f, 0), Vector3.zero, 0.15f, 0.15f);

        #endregion

        #region DAMAGE

        /// <summary>
        /// Defines the point showing the smallest effect that will be applied when the character gets hit.
        /// </summary>
        [SerializeField]
        [Tooltip("Defines the point showing the smallest effect that will be applied when the character gets hit.")]
        private Vector3 m_MinHitRotation = new Vector3(5, -5, 0);

        /// <summary>
        /// Defines the point indicating the maximum effect that will be applied when the character gets hit.
        /// The animation will be generated by computing a random point inside the minimum and maximum bounds, rotating the camera towards the calculated target direction.
        /// </summary>
        [SerializeField]
        [Tooltip("Defines the point indicating the maximum effect that will be applied when the character gets hit."
            + " The animation will be generated by computing a random point inside the minimum and maximum bounds, rotating the camera towards the calculated target direction.")]
        private Vector3 m_MaxHitRotation = new Vector3(5, 5, 0);

        /// <summary>
        /// Defines how long the hit animation will last.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines how long the hit animation will last.")]
        private float m_HitDuration = 0.1f;

        #endregion

        #region BREATH

        /// <summary>
        /// Enables Breath Animation simulation.
        /// </summary>
        [SerializeField]
        [Tooltip("Enables Breath Animation simulation.")]
        private bool m_BreathAnimation = true;

        /// <summary>
        /// Defines how fast the breathing animation will be.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines how fast the breathing animation will be.")]
        private float m_BreathingSpeed = 2;

        /// <summary>
        /// Defines the how much the Target Transform will be affected by this animation by increasing the animation amplitude.
        /// </summary>
        [SerializeField]
        [Tooltip("Defines the how much the Target Transform will be affected by this animation by increasing the animation amplitude.")]
        private float m_BreathingAmplitude = 1;

        #endregion

        #region RECOIL

        /// <summary>
        /// Enables Weapon Recoil simulation.
        /// </summary>
        [SerializeField]
        [Tooltip("Enables Weapon Recoil simulation.")]
        private bool m_WeaponRecoil;

        /// <summary>
        /// Adjusts the position of the weapon during shooting to adjust the shooting animation or to increase its effect visually.
        /// </summary>
        [SerializeField]
        [Tooltip("Adjusts the position of the weapon during shooting to adjust the shooting animation or to increase its effect visually.")]
        private Vector3 m_WeaponRecoilPosition = Vector3.zero;

        /// <summary>
        /// Adjusts the rotation of the weapon during shooting to adjust the shooting animation or to increase its effect visually.
        /// </summary>
        [SerializeField]
        [Tooltip("Adjusts the rotation of the weapon during shooting to adjust the shooting animation or to increase its effect visually.")]
        private Vector3 m_WeaponRecoilRotation = Vector3.zero;

        /// <summary>
        /// Duration of animation. (time taken to move the weapon to the target position or move it back to the origin)
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Duration of animation. (time taken to move the weapon to the target position or move it back to the origin)")]
        private float m_WeaponRecoilDuration = 0.1f;

        /// <summary>
        /// Enables Camera Recoil simulation.
        /// </summary>
        [SerializeField]
        [Tooltip("Enables Camera Recoil simulation.")]
        private bool m_CameraRecoil;

        /// <summary>
        /// The smallest possible amount that the camera can rotate while shooting to simulate the effect.
        /// </summary>
        [SerializeField]
        [Tooltip("The smallest possible amount that the camera can rotate while shooting to simulate the effect.")]
        private Vector3 m_MinCameraRecoilRotation = Vector3.zero;

        /// <summary>
        /// The highest amount that the camera can rotate while shooting.
        /// </summary>
        [SerializeField]
        [Tooltip("The highest amount that the camera can rotate while shooting.")]
        private Vector3 m_MaxCameraRecoilRotation = Vector3.zero;

        /// <summary>
        /// Time required for the camera to move to the next calculated rotation.
        /// </summary>
        [SerializeField]
        [Range(0.01f, 0.25f)]
        [Tooltip("Time required for the camera to move to the next calculated rotation.")]
        private float m_CameraRecoilDuration = 0.05f;

        /// <summary>
        /// The time required for the camera to retreat to its original position after firing.
        /// </summary>
        [SerializeField]
        [Range(0.01f, 0.25f)]
        [Tooltip("The time required for the camera to retreat to its original position after firing.")]
        private float m_CameraReturnDuration = 0.1f;

        #endregion

        #region EXPLOSION

        /// <summary>
        /// Struct that define how the tremor caused by an explosion close to the character will be simulated.
        /// </summary>
        [SerializeField]
        private ExplosionShakeProperties m_ExplosionShake;

        private const float k_MaxAngle = 90.0f;
        private Vector3 m_ExplosionPos;
        private Vector3 m_ExplosionRot;

        #endregion

        #region VAULT

        /// <summary>
        /// The Vault Animation is a routine that defines the transform’s target position and rotation,
        /// interpolating between them and returning to the original coordinates.
        /// This routine is used to simulate the character vaulting animation.
        /// </summary>
        [SerializeField]
        private LerpAnimation m_VaultAnimation = new LerpAnimation(Vector3.zero, new Vector3(-10, 0, 10), 0.3f, 0.3f);

        #endregion

        #region LEAN

        /// <summary>
        /// Enables peek and leaning functions on the character.
        /// </summary>
        [SerializeField]
        [Tooltip("Enables peek and leaning functions on the character.")]
        private bool m_Lean = true;

        /// <summary>
        /// Defines the horizontal offset while leaning.
        /// </summary>
        [SerializeField]
        [Range(0, 0.2f)]
        [Tooltip("Defines the horizontal offset while leaning.")]
        private float m_LeanAmount = 0.2f;

        /// <summary>
        /// Defines the angle (in degrees) that the target will be rotated.
        /// </summary>
        [SerializeField]
        [Tooltip("Defines the angle (in degrees) that the target will be rotated.")]
        private float m_LeanAngle = 10;

        /// <summary>
        /// Defines how fast the animation will be played.
        /// </summary>
        [SerializeField]
        [MinMax(1, 20)]
        [Tooltip("Defines how fast the animation will be played.")]
        private float m_LeanSpeed = 20;

        #endregion

        // Variables used to simulate the animation.
        private Vector3 m_TargetPos;
        private Quaternion m_TargetRot;

        private float m_Angle;
        private float m_VerticalInfluence;

        private Vector3 m_CurrentPos;
        private Vector3 m_CurrentRot;

        private Vector3 m_DamageRot;

        private Vector3 m_RecoilPos;
        private Vector3 m_RecoilRot;

        private Vector3 m_LeanPos;
        private Vector3 m_LeanRot;

        private Vector3 m_BreathingRot;
        private float m_BreathingProgress;

        #region PROPERTIES

        /// <summary>
        /// The overall speed of this animator.
        /// </summary>
        public float OverallSpeed
        {
            get => m_OverallSpeed;
            set => m_OverallSpeed = Mathf.Clamp(value, 0, Mathf.Infinity);
        }

        /// <summary>
        /// The overall magnitude of the animation amplitude.
        /// </summary>
        public float ScaleFactor => m_ScaleFactor;

        /// <summary>
        /// Is Camera Recoil simulation enabled on this object?
        /// </summary>
        public bool CameraRecoil
        {
            get => m_CameraRecoil;
            set => m_CameraRecoil = value;
        }

        /// <summary>
        /// The smallest possible amount that the camera can rotate while shooting to simulate the effect.
        /// </summary>
        public Vector3 MinCameraRecoilRotation
        {
            get => m_MinCameraRecoilRotation;
            set => m_MinCameraRecoilRotation = value;
        }

        /// <summary>
        /// The highest amount that the camera can rotate while shooting.
        /// </summary>
        public Vector3 MaxCameraRecoilRotation
        {
            get => m_MaxCameraRecoilRotation;
            set => m_MaxCameraRecoilRotation = value;
        }

        /// <summary>
        /// The time required for the camera to move to the next calculated rotation.
        /// </summary>
        public float CameraRecoilDuration
        {
            get => m_CameraRecoilDuration;
            set => m_CameraRecoilDuration = value;
        }

        /// <summary>
        /// The time required for the camera to retreat to its original position after firing.
        /// </summary>
        public float CameraReturnDuration
        {
            get => m_CameraReturnDuration;
            set => m_CameraReturnDuration = value;
        }

        /// <summary>
        /// Struct that define how the tremor caused by an explosion close to the character will be simulated.
        /// </summary>
        public ExplosionShakeProperties ExplosionShake => m_ExplosionShake;

        /// <summary>
        /// Brace For Jump Animation is a routine that defines the transform’s target position and rotation,
        /// interpolating between them and returning to the original coordinates.
        /// routine is used to simulate the animation of the character while it’s preparing to perform a jump, like channeling energy.
        /// </summary>
        public LerpAnimation BraceForJumpAnimation => m_BraceForJumpAnimation;

        /// <summary>
        /// The Jump Animation is a routine that defines the transform’s target position and rotation,
        /// interpolating between them and returning to the original coordinates.
        /// This routine is used to simulate the character jumping animation.
        /// </summary>
        public LerpAnimation JumpAnimation => m_JumpAnimation;

        /// <summary>
        /// The Landing Animation is a routine that defines the transform’s target position and rotation,
        /// interpolating between them and returning to the original coordinates.
        /// This routine is used to simulate the character landing animation, played after the character touches the ground.
        /// </summary>
        public LerpAnimation LandingAnimation => m_LandingAnimation;

        /// <summary>
        /// The Vault Animation is a routine that defines the transform’s target position and rotation,
        /// interpolating between them and returning to the original coordinates.
        /// This routine is used to simulate the character vaulting animation.
        /// </summary>
        public LerpAnimation VaultAnimation => m_VaultAnimation;

        /// <summary>
        /// Enables peek and leaning functions on the character.
        /// </summary>
        public bool Lean => m_Lean;

        /// <summary>
        /// Defines the horizontal offset while leaning.
        /// </summary>
        public float LeanAmount
        {
            get => m_LeanAmount;
            set => m_LeanAmount = Mathf.Clamp(value, 0, 0.2f);
        }

        #endregion

        /// <summary>
        /// Breath animation simulates the natural movement of the arms while the character holds a weapon.
        /// </summary>
        /// <param name="speed">The animation speed.</param>
        public void BreathingAnimation(float speed = 1)
        {
            if (m_BreathAnimation)
            {
                // The animation progress
                CalculateAngle(ref m_BreathingProgress, m_BreathingSpeed, speed);

                if (speed > 0)
                {
                    float sin = Mathf.Sin(m_BreathingProgress);
                    float cos = Mathf.Cos(m_BreathingProgress);

                    // Calculates the target rotation using the values of sine and cosine multiplied by the animation magnitude.
                    Vector3 breathingRot = new Vector3(sin * cos * m_BreathingAmplitude, sin * m_BreathingAmplitude);

                    m_BreathingRot = Vector3.Lerp(m_BreathingRot, breathingRot, Time.deltaTime * 5 * m_BreathingSpeed * speed);
                }
                else
                {
                    m_BreathingRot = Vector3.Lerp(m_BreathingRot, Vector3.zero, Time.deltaTime * 5);
                }
            }
            else
            {
                m_BreathingRot = Vector3.Lerp(m_BreathingRot, Vector3.zero, Time.deltaTime * 5);
            }
        }

        /// <summary>
        /// The movement animation simulates the wave-like motion animation while the character is walking or running.
        /// </summary>
        public void MovementAnimation(FirstPersonCharacterController FPController)
        {
            if (!m_TargetTransform)
                return;

            // Stops the movement animation if the character is not walking or running.
            if (FPController.State == MotionState.Flying || FPController.State == MotionState.Idle || FPController.State == MotionState.Climbing || FPController.IsSliding)
            {
                PerformMovementAnimation(null, Vector3.zero, 0);
            }
            else
            {
                // Calculates the animation speed
                float speed = Mathf.Max(FPController.State == MotionState.Running ? 6.5f : 2.75f, FPController.CurrentTargetForce);

                // Lowerbody damaged (broken legs)
                if (FPController.LowerBodyDamaged && !FPController.IsAiming)
                {
                    PerformMovementAnimation(m_BrokenLegsMotionData, FPController.Grounded ? FPController.Velocity : Vector3.zero, speed);
                }
                else
                {
                    PerformMovementAnimation(FPController.State == MotionState.Running ? m_RunningMotionData : m_WalkingMotionData, FPController.Grounded ? FPController.Velocity : Vector3.zero, speed);
                }
            }

            m_TargetTransform.localPosition = m_TargetPos;
            m_TargetTransform.localRotation = m_TargetRot;
        }

        /// <summary>
        /// Calculates the next angle based on the time elapsed since the last frame.
        /// </summary>
        /// <param name="angle">The reference angle to be updated.</param>
        /// <param name="animationSpeed">The current animation speed.</param>
        /// <param name="overallSpeed">The overall animator speed.</param>
        private void CalculateAngle(ref float angle, float animationSpeed, float overallSpeed)
        {
            if (angle >= Mathf.PI * 2)
            {
                m_Angle -= Mathf.PI * 2;
            }

            // Sum the time elapsed since the last frame multiplied by the animation speed.
            angle += Time.deltaTime * animationSpeed * overallSpeed;
        }

        /// <summary>
        /// Movement animation is simulated by simple waves, such as sine and cosine. Using the information contained in MotionData, 
        /// we simulate the animation by interpolating the target transforms following the parameters defined in the asset.
        /// </summary>
        /// <param name="motionData">The MotionData Asset used to define the animation behaviour.</param>
        /// <param name="velocityInfluence">The current velocity of the character.</param>
        /// <param name="speed">The animation speed.</param>
        private void PerformMovementAnimation(MotionData motionData, Vector3 velocityInfluence, float speed)
        {
            // Brief explanation on how the procedural animation system works:
            // Basically we have two main variables "m_TargetPos" and "m_TargetRot",
            // they are the values used to interpolate the current position of the target transform.

            // What we need to do is to simulate each animation separately and then sum all its respective
            // values in the variable "m_CurrentPos" and "m_CurrentRot" and then Interpolate(Target, Current, Speed).
            // As we use LerpAnimation all animations will be simulated in parallel using coroutines.
            // This way we can simulate all animations in a single frame using only one transform.

            if (speed > 0 && motionData)
            {
                CalculateAngle(ref m_Angle, motionData.Speed, speed);

                float sin = Mathf.Sin(m_Angle);
                float cos = Mathf.Cos(m_Angle);

                // Calculates the velocity influence
                m_VerticalInfluence = Mathf.Lerp(m_VerticalInfluence, velocityInfluence.y * motionData.VerticalAmplitude * motionData.VelocityInfluence, Time.deltaTime * speed);

                // Calculates the current position. (The sum of all the positions of the simulations)
                m_CurrentPos = motionData.PositionOffset + m_JumpAnimation.Position + m_LandingAnimation.Position + m_BraceForJumpAnimation.Position
                    + m_VaultAnimation.Position + m_RecoilPos + m_ExplosionPos + m_LeanPos;

                // Calculates the current rotation. (The multiplication of all the rotations of the simulations)
                m_CurrentRot = new Vector3((-Mathf.Abs(sin) * motionData.VerticalAmplitude + motionData.VerticalAmplitude)
                    * motionData.VerticalAnimationCurve.Evaluate(Mathf.Abs(cos)), cos * motionData.HorizontalAmplitude, cos * -motionData.RotationAmplitude)
                + new Vector3(m_VerticalInfluence, 0, 0) + motionData.RotationOffset + m_BraceForJumpAnimation.Rotation + m_JumpAnimation.Rotation + m_LandingAnimation.Rotation
                    + m_RecoilRot + m_ExplosionRot + m_DamageRot + m_VaultAnimation.Rotation + m_LeanRot;

                // Interpolate from the last position/rotation to the new current position/rotation.
                m_TargetPos = Vector3.Lerp(m_TargetPos, m_CurrentPos * m_ScaleFactor, Time.deltaTime * m_OverallSpeed * motionData.Speed * speed / motionData.Smoothness);
                m_TargetRot = Quaternion.Slerp(m_TargetRot, Quaternion.Euler(m_CurrentRot), Time.deltaTime * m_OverallSpeed * motionData.Speed * speed / motionData.Smoothness);
            }
            else
            {
                m_TargetPos = Vector3.Lerp(m_TargetPos, (Vector3.zero + m_BraceForJumpAnimation.Position + m_JumpAnimation.Position + m_LandingAnimation.Position
                    + m_VaultAnimation.Position + m_RecoilPos + m_LeanPos) * m_ScaleFactor, Time.deltaTime * m_OverallSpeed * 5);

                m_TargetRot = Quaternion.Slerp(m_TargetRot, Quaternion.identity * Quaternion.Euler(m_BraceForJumpAnimation.Rotation) * Quaternion.Euler(m_JumpAnimation.Rotation)
                    * Quaternion.Euler(m_LandingAnimation.Rotation) * Quaternion.Euler(m_BreathingRot) * Quaternion.Euler(m_RecoilRot) * Quaternion.Euler(m_ExplosionRot)
                    * Quaternion.Euler(m_DamageRot) * Quaternion.Euler(m_VaultAnimation.Rotation) * Quaternion.Euler(m_LeanRot), Time.deltaTime * m_OverallSpeed * 5);

                m_VerticalInfluence = 0;

                m_Angle = Random.Range(0, 10) % 2 == 0 ? 0 : Mathf.PI;
            }
        }

        /// <summary>
        /// Tilts the target slightly in a certain direction.
        /// </summary>
        /// <param name="direction">The desired direction.</param>
        public void LeanAnimation(int direction)
        {
            if (m_Lean)
            {
                switch (direction)
                {
                    // Right
                    case 1:
                    m_LeanPos = Vector3.Lerp(m_LeanPos, new Vector3(m_LeanAmount, 0, 0), Time.deltaTime * m_LeanSpeed);
                    m_LeanRot = Vector3.Lerp(m_LeanRot, new Vector3(0, 0, -m_LeanAngle), Time.deltaTime * m_LeanSpeed);
                    break;
                    // Left
                    case -1:
                    m_LeanPos = Vector3.Lerp(m_LeanPos, new Vector3(-m_LeanAmount, 0, 0), Time.deltaTime * m_LeanSpeed);
                    m_LeanRot = Vector3.Lerp(m_LeanRot, new Vector3(0, 0, m_LeanAngle), Time.deltaTime * m_LeanSpeed);
                    break;
                    default:
                    m_LeanPos = Vector3.Lerp(m_LeanPos, Vector3.zero, Time.deltaTime * m_LeanSpeed);
                    m_LeanRot = Vector3.Lerp(m_LeanRot, Vector3.zero, Time.deltaTime * m_LeanSpeed);
                    break;
                }
            }
            else
            {
                m_LeanPos = Vector3.zero;
                m_LeanRot = Vector3.zero;
            }
        }

        /// <summary>
        /// Method that simulates the effect of being hit by a bullet projectile.
        /// </summary>
        public IEnumerator HitAnimation()
        {
            Vector3 initialRot = MathfUtilities.RandomInsideBounds(m_MinHitRotation, m_MaxHitRotation);

            // Make the GameObject move to target slightly
            for (float t = 0f; t <= m_HitDuration; t += Time.deltaTime)
            {
                m_DamageRot = Vector3.Lerp(initialRot, initialRot, t / m_HitDuration);
                yield return new WaitForFixedUpdate();
            }

            // Make it move back to neutral
            for (float t = 0f; t <= m_HitDuration; t += Time.deltaTime)
            {
                m_DamageRot = Vector3.Lerp(initialRot, Vector3.zero, t / m_HitDuration);
                yield return new WaitForFixedUpdate();
            }

            m_DamageRot = Vector3.zero;
        }

        /// <summary>
        /// Method that simulates the recoil effect of a weapon while shooting.
        /// </summary>
        public IEnumerator RecoilAnimation(bool camera)
        {
            if (camera)
            {
                if (m_CameraRecoil)
                {
                    Vector3 initialPos = m_RecoilPos;
                    Vector3 initialRot = MathfUtilities.RandomInsideBounds(m_MinCameraRecoilRotation, m_MaxCameraRecoilRotation);

                    // Make the GameObject move to target slightly
                    for (float t = 0f; t <= m_CameraRecoilDuration; t += Time.deltaTime)
                    {
                        m_RecoilRot = Vector3.Lerp(initialRot, initialRot, t / m_CameraRecoilDuration);
                        yield return new WaitForFixedUpdate();
                    }

                    // Make it move back to neutral
                    for (float t = 0f; t <= m_CameraReturnDuration; t += Time.deltaTime)
                    {
                        m_RecoilPos = Vector3.Lerp(initialPos, Vector3.zero, t / m_CameraReturnDuration);
                        m_RecoilRot = Vector3.Lerp(initialRot, Vector3.zero, t / m_CameraReturnDuration);
                        yield return new WaitForFixedUpdate();
                    }
                }
            }
            else
            {
                if (m_WeaponRecoil)
                {
                    Vector3 initialPos = m_WeaponRecoilPosition;
                    Vector3 initialRot = m_WeaponRecoilRotation;

                    // Make the GameObject move to target slightly
                    for (float t = 0f; t <= m_WeaponRecoilDuration; t += Time.deltaTime)
                    {
                        m_RecoilPos = Vector3.Lerp(initialPos, m_WeaponRecoilPosition, t / m_WeaponRecoilDuration);
                        m_RecoilRot = Vector3.Lerp(initialRot, m_WeaponRecoilRotation, t / m_WeaponRecoilDuration);
                        yield return new WaitForFixedUpdate();
                    }

                    // Make it move back to neutral
                    for (float t = 0f; t <= m_WeaponRecoilDuration; t += Time.deltaTime)
                    {
                        m_RecoilPos = Vector3.Lerp(m_WeaponRecoilPosition, Vector3.zero, t / m_WeaponRecoilDuration);
                        m_RecoilRot = Vector3.Lerp(m_WeaponRecoilRotation, Vector3.zero, t / m_WeaponRecoilDuration);
                        yield return new WaitForFixedUpdate();
                    }
                }
            }

            m_RecoilPos = Vector3.zero;
            m_RecoilPos = Vector3.zero;
        }

        /// <summary>
        /// Method that mimics a shake animation, simulating the effect of an explosion near the character.
        /// </summary>
        public IEnumerator Shake(ExplosionShakeProperties prop)
        {
            // Original code written by Sebastian Lague.
            // https://github.com/SebLague/Camera-Shake

            float completionPercent = 0;
            float movePercent = 0;

            float radians = prop.Angle * Mathf.Deg2Rad - Mathf.PI;
            Vector3 previousWaypoint = Vector3.zero;
            Vector3 currentWaypoint = Vector3.zero;
            float moveDistance = 0;
            float speed = 0;

            Vector3 targetRotation = Vector3.zero;
            Vector3 previousRotation = Vector3.zero;

            do
            {
                if (movePercent >= 1 || Mathf.Abs(completionPercent) < Mathf.Epsilon)
                {
                    float dampingFactor = DampingCurve(completionPercent, prop.DampingPercent);
                    float noiseAngle = (Random.value - 0.5f) * Mathf.PI;
                    radians += Mathf.PI + noiseAngle * prop.NoisePercent;
                    currentWaypoint = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians)) * (prop.Strength * dampingFactor);
                    previousWaypoint = m_ExplosionPos;
                    moveDistance = Vector3.Distance(currentWaypoint, previousWaypoint);

                    targetRotation = new Vector3(currentWaypoint.y, currentWaypoint.x).normalized * (prop.RotationPercent * dampingFactor * k_MaxAngle);
                    previousRotation = m_ExplosionRot;

                    speed = Mathf.Lerp(prop.MinSpeed, prop.MaxSpeed, dampingFactor);

                    movePercent = 0;
                }

                completionPercent += Time.deltaTime / prop.Duration;
                movePercent += Time.deltaTime / moveDistance * speed;
                m_ExplosionPos = Vector3.Lerp(previousWaypoint, currentWaypoint, movePercent);
                m_ExplosionRot = Vector3.Lerp(previousRotation, targetRotation, movePercent);

                yield return null;

            } while (moveDistance > 0);
        }

        private static float DampingCurve(float x, float dampingPercent)
        {
            float a = Mathf.Lerp(2, .25f, dampingPercent);
            float b = 1 - Mathf.Pow(Mathf.Clamp01(x), a);
            return Mathf.Pow(b, 3);
        }
    }
}

