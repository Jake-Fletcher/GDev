//=========== Copyright (c) GameBuilders, All rights reserved. ================
//
// Purpose: The Gameplay Settings Asset is a data container for the behaviour settings of some player actions,
//          like holding a button or simply tapping it to do an action, camera Field Of View (FOV) and mouse axes behaviour.
//
//=============================================================================

using UnityEngine;

namespace FPSBuilder.Core.Managers
{
    /// <summary>
    /// The input behaviour. (Hold or Press a button to execute an action)
    /// </summary>
    public enum ActionMode
    {
        Toggle,
        Hold
    }

    /// <summary>
    /// Gameplay Settings Asset is a data container for the behaviour settings of some player actions.
    /// </summary>
    [CreateAssetMenu(menuName = "Gameplay Settings", fileName = "Gameplay Settings", order = 201)]
    public sealed class GameplaySettings : ScriptableObject
    {
        /// <summary>
        /// Defines the Aim Down Sights behavior according to input mode.
        /// </summary>
        [SerializeField]
        [Tooltip("Defines the Aim Down Sights behavior according to input mode.")]
        private ActionMode m_AimStyle = ActionMode.Toggle;

        /// <summary>
        /// Defines the Crouch behavior according to input mode.
        /// </summary>
        [SerializeField]
        [Tooltip("Defines the Crouch behavior according to input mode.")]
        private ActionMode m_CrouchStyle = ActionMode.Toggle;

        /// <summary>
        /// Defines the Sprint behavior according to input mode.
        /// </summary>
        [SerializeField]
        [Tooltip("Defines the Sprint behavior according to input mode.")]
        private ActionMode m_SprintStyle = ActionMode.Toggle;

        /// <summary>
        /// Defines the Lean behavior according to input mode.
        /// </summary>
        [SerializeField]
        [Tooltip("Defines the Lean behavior according to input mode.")]
        private ActionMode m_LeanStyle = ActionMode.Toggle;

        /// <summary>
        /// Defines the overall mouse sensitivity.
        /// </summary>
        [SerializeField]
        [Range(0.1f, 10)]
        [Tooltip("Defines the overall mouse sensitivity.")]
        private float m_OverallMouseSensitivity = 1;

        /// <summary>
        /// Defines whether the camera’s horizontal movement must be opposite to the mouse movement.
        /// </summary>
        [SerializeField]
        [Tooltip("Defines whether the camera’s horizontal movement must be opposite to the mouse movement.")]
        private bool m_InvertHorizontalAxis;

        /// <summary>
        /// Defines whether the camera’s vertical movement must be opposite to the mouse movement.
        /// </summary>
        [SerializeField]
        [Tooltip("Defines whether the camera’s vertical movement must be opposite to the mouse movement.")]
        private bool m_InvertVerticalAxis;

        /// <summary>
        /// Defines the camera FOV in hip-fire mode.
        /// </summary>
        [SerializeField]
        [Range(50, 90)]
        [Tooltip("Defines the camera FOV in hip-fire mode.")]
        private float m_FieldOfView = 75;

        #region PROPERTIES

        /// <summary>
        /// Defines the crouch behaviour according to the player's input.
        /// </summary>
        public ActionMode CrouchStyle
        {
            get => m_CrouchStyle;
            set => m_CrouchStyle = value;
        }

        /// <summary>
        /// Defines the aiming behaviour according to the player's input.
        /// </summary>
        public ActionMode AimStyle
        {
            get => m_AimStyle;
            set => m_AimStyle = value;
        }

        /// <summary>
        /// Defines the running behaviour according to the player's input.
        /// </summary>
        public ActionMode SprintStyle
        {
            get => m_SprintStyle;
            set => m_SprintStyle = value;
        }

        /// <summary>
        /// Defines the leaning behaviour according to the player's input.
        /// </summary>
        public ActionMode LeanStyle
        {
            get => m_LeanStyle;
            set => m_LeanStyle = value;
        }

        /// <summary>
        /// Defines the overall mouse sensitivity.
        /// </summary>
        public float OverallMouseSensitivity
        {
            get => m_OverallMouseSensitivity;
            set => m_OverallMouseSensitivity = Mathf.Clamp(value, 0.1f, 10);
        }

        /// <summary>
        /// Returns true if the mouse input is reversed, false otherwise.
        /// </summary>
        public bool InvertHorizontalAxis
        {
            get => m_InvertHorizontalAxis;
            set => m_InvertHorizontalAxis = value;
        }

        /// <summary>
        /// Returns true if the mouse input is reversed, false otherwise.
        /// </summary>
        public bool InvertVerticalAxis
        {
            get => m_InvertVerticalAxis;
            set => m_InvertVerticalAxis = value;
        }

        /// <summary>
        /// Returns the target field of view used by the character main camera.
        /// </summary>
        public float FieldOfView
        {
            get => m_FieldOfView;
            set => m_FieldOfView = Mathf.Clamp(value, 50, 90);
        }

        #endregion
    }
}