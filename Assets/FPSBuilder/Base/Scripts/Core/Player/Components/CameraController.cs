//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using System;
using FPSBuilder.Core.Managers;
using UnityEngine;

namespace FPSBuilder.Core.Player
{
    using Input = UnityEngine.Input;

    /// <summary>
    /// CameraController handles the camera movement and related functions.
    /// </summary>
    [Serializable]
    public sealed class CameraController
    {
        /// <summary>
        /// Defines how horizontally sensitive the camera will be to mouse movement.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines how horizontally sensitive the camera will be to mouse movement.")]
        private float m_YawSensitivity = 3f;

        /// <summary>
        /// Defines how vertically sensitive the camera will be to mouse movement.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines how vertically sensitive the camera will be to mouse movement.")]
        private float m_PitchSensitivity = 3f;

        /// <summary>
        /// Defines how horizontally sensitive the camera will be to mouse movement while aiming.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines how horizontally sensitive the camera will be to mouse movement while aiming.")]
        private float m_AimingYawSensitivity = 1f;

        /// <summary>
        /// Defines how vertically sensitive the camera will be to mouse movement while aiming.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines how vertically sensitive the camera will be to mouse movement while aiming.")]
        private float m_AimingPitchSensitivity = 1f;

        /// <summary>
        /// Limits the camera’s vertical rotation (pitch).
        /// </summary>
        [SerializeField]
        [Tooltip("Limits the camera’s vertical rotation (pitch).")]
        private bool m_LimitPitchRotation = true;

        /// <summary>
        /// Defines the minimum and maximum vertical angle.
        /// </summary>
        [SerializeField]
        [MinMaxSlider(-90f, 90f, "Defines the minimum and maximum vertical angle.", "F0")]
        private Vector2 m_PitchLimit = new Vector2(-75f, 80);

        /// <summary>
        /// Defines how fast the camera will decelerate.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines how fast the camera will decelerate.")]
        private float m_Smoothness = 0.3f;

        private float m_MinPitch;
        private float m_MaxPitch;
        private Quaternion m_CharacterTargetRot;
        private Quaternion m_CameraTargetRot;

        private Transform m_CharacterReference;
        private Transform m_CameraReference;

        #region PROPERTIES

        /// <summary>
        /// Defines whether the camera controller can receive mouse input or not.
        /// </summary>
        public bool Controllable
        {
            get;
            set;
        }

        /// <summary>
        /// The current camera pitch in the interval : [-1, 1]. (Read Only)
        /// </summary>
        public float CurrentPitch => m_CameraTargetRot.x;

        /// <summary>
        /// The current yaw input. Can be used to verify which direction is the character rotating to. (Read Only)
        /// </summary>
        public float CurrentYaw
        {
            get;
            private set;
        }

        /// <summary>
        /// The current sensitivity of the camera's control axes. (Read Only)
        /// </summary>
        public Vector2 CurrentSensitivity
        {
            private set;
            get;
        }

        #endregion

        /// <summary>
        /// Initialize the camera controller with the character initial rotation.
        /// </summary>
        /// <param name="character">The character transform.</param>
        /// <param name="camera">The character's camera.</param>
        public void Init(Transform character, Transform camera)
        {
            m_CharacterReference = character;
            m_CameraReference = camera;

            m_CharacterTargetRot = character.localRotation;
            m_CameraTargetRot = camera.localRotation;

            m_MinPitch = m_PitchLimit.x;
            m_MaxPitch = m_PitchLimit.y;
        }

        /// <summary>
        /// Forces the character to look at a position in the world.
        /// </summary>
        /// <param name="position">The target position.</param>
        public void LookAt(Vector3 position)
        {
            Vector3 characterDirection = position - m_CharacterReference.position;
            characterDirection.y = 0;

            // Forces the character to look at the target position.
            m_CharacterTargetRot = Quaternion.Slerp(m_CharacterTargetRot, Quaternion.LookRotation(characterDirection), 10 * Time.deltaTime);
            m_CharacterReference.localRotation = Quaternion.Slerp(m_CharacterReference.localRotation, m_CharacterTargetRot, 10 * Time.deltaTime);
        }

        /// <summary>
        /// Defines whether the character camera should use the default pitch settings or override it.
        /// </summary>
        /// <param name="overridePitchLimit">Should the character uses custom pitch values?</param>
        /// <param name="minPitch">Camera minimum pitch.</param>
        /// <param name="maxPitch">Camera maximum pitch.</param>
        public void OverrideCameraPitchLimit(bool overridePitchLimit, float minPitch, float maxPitch)
        {
            m_MinPitch = overridePitchLimit ? minPitch : m_PitchLimit.x;
            m_MaxPitch = overridePitchLimit ? maxPitch : m_PitchLimit.y;
        }

        /// <summary>
        /// Updates the character and camera rotation based on the player input.
        /// </summary>
        /// <param name="isAiming">Is the character aiming?</param>
        public void UpdateRotation(bool isAiming)
        {
            if (!Controllable)
                return;

            // Avoids the mouse looking if the game is effectively paused.
            if (Mathf.Abs(Time.timeScale) < float.Epsilon)
                return;

            CurrentSensitivity = new Vector2(isAiming ? m_AimingYawSensitivity : m_YawSensitivity, isAiming ? m_AimingPitchSensitivity : m_PitchSensitivity);

            float xRot = (GameplayManager.Instance.InvertVerticalAxis ? -Input.GetAxis("Mouse Y") : Input.GetAxis("Mouse Y"))
                * CurrentSensitivity.y * GameplayManager.Instance.OverallMouseSensitivity;

            CurrentYaw = (GameplayManager.Instance.InvertHorizontalAxis ? -Input.GetAxis("Mouse X") : Input.GetAxis("Mouse X"))
                * CurrentSensitivity.x * GameplayManager.Instance.OverallMouseSensitivity;

            m_CharacterTargetRot *= Quaternion.Euler(0f, CurrentYaw, 0f);
            m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            if (m_LimitPitchRotation)
            {
                m_CameraTargetRot = MathfUtilities.ClampRotationAroundXAxis(m_CameraTargetRot, -m_MaxPitch, -m_MinPitch);
            }

            if (m_Smoothness > 0)
            {
                m_CharacterReference.localRotation = Quaternion.Slerp(m_CharacterReference.localRotation, m_CharacterTargetRot, 10 / m_Smoothness * Time.deltaTime);
                m_CameraReference.localRotation = Quaternion.Slerp(m_CameraReference.localRotation, m_CameraTargetRot, 10 / m_Smoothness * Time.deltaTime);
            }
            else
            {
                m_CharacterReference.localRotation = m_CharacterTargetRot;
                m_CameraReference.localRotation = m_CameraTargetRot;
            }
        }
    }
}
