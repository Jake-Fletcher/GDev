//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using System;
using FPSBuilder.Core.Managers;
using FPSBuilder.Core.Player;
using UnityEngine;

#pragma warning disable CS0649

namespace FPSBuilder.Core.Weapons
{
    using Input = UnityEngine.Input;

    [Serializable]
    public sealed class WeaponSwing
    {
        /// <summary>
        /// Defines the style of animation depending on the pivot.
        /// </summary>
        public enum SwingTarget
        {
            Fist,
            Weapon
        }

        /// <summary>
        /// Enables the swinging feature in the weapon.
        /// </summary>
        [SerializeField]
        [Tooltip("Enables the swinging feature in the weapon.")]
        private bool m_Swing;

        /// <summary>
        /// Defines the tilt angle of the weapon when moving sideways.
        /// </summary>
        [SerializeField]
        [MinMaxSlider(-20, 20, "Defines the tilt angle of the weapon when moving sideways.")]
        private Vector2 m_TiltAngle = new Vector2(5, 10);

        /// <summary>
        /// Defines the swing angle when moving the camera.
        /// </summary>
        [SerializeField]
        [MinMaxSlider(-20, 20, "Defines the swing angle when moving the camera.")]
        private Vector2 m_SwingAngle = new Vector2(4, 8);

        /// <summary>
        /// Determines how fast the animation will be played.
        /// </summary>
        [SerializeField]
        [Range(0, 1)]
        [Tooltip("Determines how fast the animation will be played.")]
        private float m_Speed = 0.5f;

        /// <summary>
        /// Boosts the animation effect by allowing rotation on all axes.
        /// </summary>
        [SerializeField]
        [Tooltip("Boosts the animation effect by allowing rotation on all axes.")]
        private bool m_AnimateAllAxes;

        /// <summary>
        /// Defines the tilt boost of the weapon when moving the camera.
        /// </summary>
        [SerializeField]
        [MinMaxSlider(-20, 20, "Defines the tilt boost of the weapon when moving the camera.")]
        private Vector2 m_TiltBoost = new Vector2(2.5f, 5);

        /// <summary>
        /// Defines the magnitude of the weapon shaking animation when the tremor effect is active.
        /// </summary>
        [SerializeField]
        [Range(0, 2)]
        [Tooltip("Defines the magnitude of the weapon shaking animation when the tremor effect is active.")]
        private float m_TremorAmount = 1;

        /// <summary>
        /// Defines the style of animation depending on the pivot.
        /// </summary>
        [SerializeField]
        [Tooltip("Defines the style of animation depending on the pivot.")]
        private SwingTarget m_SwingTarget = SwingTarget.Weapon;

        private float m_ScaleFactor = 1;
        private Vector3 m_TargetPos;
        private Quaternion m_TargetRot;

        /// <summary>
        /// Initializes the component providing the necessary references to simulate the effect.
        /// </summary>
        /// <param name="weaponSwing">The target Transform reference.</param>
        /// <param name="scaleFactor">Scales the animation magnitude.</param>
        internal void Init(Transform weaponSwing, float scaleFactor)
        {
            m_ScaleFactor = scaleFactor;
            m_TargetPos = weaponSwing.localPosition;
            m_TargetRot = weaponSwing.localRotation;
        }

        /// <summary>
        /// Simulates the swinging effect animation on the weapon relative to the character movement.
        /// </summary>
        /// <param name="weaponSwing">The target Transform reference.</param>
        /// <param name="FPController">The CharacterController reference.</param>
        internal void Swing(Transform weaponSwing, FirstPersonCharacterController FPController)
        {
            if (Mathf.Abs(Time.timeScale) < float.Epsilon)
                return;

            if (m_Swing)
            {
                // Calculates the swing angle by the mouse movement
                float yRot = Mathf.Clamp(Input.GetAxis("Mouse X") * -m_SwingAngle.x * GameplayManager.Instance.OverallMouseSensitivity, -m_SwingAngle.y, m_SwingAngle.y);
                float xRot = Mathf.Clamp(Input.GetAxis("Mouse Y") * -m_SwingAngle.x * GameplayManager.Instance.OverallMouseSensitivity, -m_SwingAngle.y, m_SwingAngle.y);

                // Calculates the tilt angle by sideways movement
                float zRot = FPController.Velocity.sqrMagnitude > 1 && !FPController.IsSliding
                    ? Mathf.Clamp(FPController.GetInput().x  * -m_TiltAngle.x, -m_TiltAngle.y, m_TiltAngle.y) : 0;

                float zRotBoost = Mathf.Clamp(m_AnimateAllAxes ? Input.GetAxis("Mouse X") * GameplayManager.Instance.OverallMouseSensitivity * -m_TiltBoost.x : 0, -m_TiltBoost.y, m_TiltBoost.y);

                // Simulates the tremor effect (shaking effect)
                if (FPController.TremorTrauma)
                {
                    yRot += UnityEngine.Random.Range(-1.0f, 1.0f) * m_TremorAmount;
                    xRot += UnityEngine.Random.Range(-1.0f, 1.0f) * m_TremorAmount;
                }

                if (m_SwingTarget == SwingTarget.Fist)
                {
                    m_TargetRot = Quaternion.Euler(xRot, yRot, zRot + zRotBoost);
                    m_TargetPos = new Vector3(-yRot / 100 + (zRot + zRotBoost) / 500, xRot / 100, 0);

                    if (FPController.IsAiming)
                        m_TargetPos /= 2;
                }
                else
                {
                    m_TargetRot = Quaternion.Euler(-xRot, yRot, zRot + zRotBoost);
                    m_TargetPos = new Vector3((zRot + zRotBoost) / 500, 0, 0);
                }
            }
            else
            {
                m_TargetRot = Quaternion.identity;
                m_TargetPos = Vector3.zero;
            }

            weaponSwing.localPosition = Vector3.Lerp(weaponSwing.localPosition, m_TargetPos * m_ScaleFactor, Time.deltaTime * m_Speed * 10);
            weaponSwing.localRotation = Quaternion.Slerp(weaponSwing.localRotation, m_TargetRot, Time.deltaTime * m_Speed * 10);
        }
    }
}
