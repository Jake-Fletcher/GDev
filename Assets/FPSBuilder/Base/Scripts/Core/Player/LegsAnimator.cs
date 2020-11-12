//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using System;
using UnityEngine;

#pragma warning disable CS0649

namespace FPSBuilder.Core.Player
{
    public sealed class LegsAnimator : MonoBehaviour
    {
        /// <summary>
        /// CharacterController reference.
        /// </summary>
        [SerializeField]
        [Required]
        [Tooltip("CharacterController reference.")]
        private FirstPersonCharacterController m_FPController;

        /// <summary>
        /// Leg animator.
        /// </summary>
        [SerializeField]
        [Required]
        [Tooltip("Leg animator.")]
        private Animator m_Animator;

        private float m_Yaw;
        private float m_Crouch;
        private float m_HorizontalVelocity;
        private float m_VerticalVelocity;
        private float m_Running;

        private bool m_IsFlying;

        #region Animator State Hashes

        private static readonly int Turn = Animator.StringToHash("Turn");
        private static readonly int Running = Animator.StringToHash("Running");
        private static readonly int Crouch = Animator.StringToHash("Crouch");
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Flying = Animator.StringToHash("Flying");
        private static readonly int Landing = Animator.StringToHash("Landing");
        private static readonly int Jumping = Animator.StringToHash("Jumping");
        private static readonly int Sliding = Animator.StringToHash("Sliding");
        private static readonly int Vaulting = Animator.StringToHash("Vaulting");

        #endregion

        private void Start()
        {
            //Enable the legs mesh
            m_Animator.gameObject.SetActive(true);

            m_FPController.PreJumpEvent += JumpEvent;
            m_FPController.LandingEvent += LandingEvent;
            m_FPController.VaultEvent += VaultingEvent;
            m_FPController.StartSlidingEvent += SlidingEvent;
        }

        private void Update()
        {
            m_Animator.gameObject.SetActive(m_FPController.State != MotionState.Climbing);

            m_Crouch = Mathf.MoveTowards(m_Crouch, m_FPController.State == MotionState.Crouched || m_FPController.IsCrouched ? 1 : 0, Time.deltaTime * 5);
            m_Running = Mathf.MoveTowards(m_Running, m_FPController.State == MotionState.Running ? 1 : 0, Time.deltaTime * 5);

            m_Yaw = Mathf.MoveTowards(m_Yaw, Mathf.Abs(m_FPController.CurrentYawTarget) > 0.75f && m_FPController.State == MotionState.Idle ? Mathf.Clamp(m_FPController.CurrentYawTarget, -1, 1) : 0,
                Time.deltaTime * (Math.Abs(m_FPController.CurrentYawTarget) > Mathf.Epsilon ? 1.5f : 3));

            if (m_FPController.State == MotionState.Flying && !m_IsFlying)
            {
                m_Animator.speed = 1f;
                m_Animator.CrossFadeInFixedTime(Flying, 0.1f);
                m_IsFlying = true;
            }

            if (m_FPController.State != MotionState.Idle && m_FPController.State != MotionState.Flying)
            {
                if (m_IsFlying)
                {
                    if (m_FPController.State != MotionState.Climbing)
                        m_Animator.CrossFadeInFixedTime(Landing, 0.1f);
                    m_IsFlying = false;
                }

                m_Animator.speed = Mathf.Max(m_FPController.CurrentTargetForce / (m_FPController.State == MotionState.Running ? 10 : 4), 0.7f);
                m_HorizontalVelocity = Mathf.MoveTowards(m_HorizontalVelocity, m_FPController.GetInput().x, Time.deltaTime * 5);
                m_VerticalVelocity = Mathf.MoveTowards(m_VerticalVelocity, m_FPController.GetInput().y, Time.deltaTime * 5);
            }
            else
            {
                m_Animator.speed = 1f;
                m_HorizontalVelocity = Mathf.MoveTowards(m_HorizontalVelocity, 0, Time.deltaTime * 5);
                m_VerticalVelocity = Mathf.MoveTowards(m_VerticalVelocity, 0, Time.deltaTime * 5);
            }

            if (m_FPController.State != MotionState.Climbing)
            {
                m_Animator.SetFloat(Turn, m_Yaw);
                m_Animator.SetFloat(Running, m_Running);
                m_Animator.SetFloat(Crouch, m_Crouch);
                m_Animator.SetFloat(Horizontal, m_HorizontalVelocity);
                m_Animator.SetFloat(Vertical, m_VerticalVelocity);
            }
        }

        private void JumpEvent()
        {
            if (m_FPController.State != MotionState.Climbing && m_Animator.gameObject.activeInHierarchy)
                m_Animator.CrossFadeInFixedTime(Jumping, 0.1f);
        }

        private void LandingEvent(float fallDamage)
        {
            if (m_FPController.State != MotionState.Climbing && m_Animator.gameObject.activeInHierarchy)
                m_Animator.CrossFadeInFixedTime(Landing, 0.1f);
        }

        private void SlidingEvent()
        {
            if (m_FPController.State != MotionState.Climbing && m_Animator.gameObject.activeInHierarchy)
                m_Animator.CrossFadeInFixedTime(Sliding, 0.1f);
        }

        private void VaultingEvent()
        {
            if (m_FPController.State != MotionState.Climbing && m_Animator.gameObject.activeInHierarchy)
                m_Animator.CrossFadeInFixedTime(Vaulting, 0.3f);
        }
    }
}