//=========== Copyright (c) GameBuilders, All rights reserved. ================
//
// Purpose: The Gameplay Manager Script is a reference for the player settings and input bindings.
//          It is mainly used to set the behaviour of the input keys, like holding a button or simply tapping it to do an action.
//
//=============================================================================

using FPSBuilder.Core.Input;
using UnityEngine;

#pragma warning disable CS0649

namespace FPSBuilder.Core.Managers
{
    /// <summary>
    /// Reference for the player settings and input behaviours.
    /// </summary>
    [AddComponentMenu("FPS Builder/Managers/Game Manager"), DisallowMultipleComponent]
    public sealed class GameplayManager : Singleton<GameplayManager>
    {
        /// <summary>
        /// Provides the behaviour settings of several actions, such Running, Aiming and mouse axes.
        /// </summary>
        [SerializeField]
        [Required()]
        [Tooltip("Provides the behaviour settings of several actions, such Running, Aiming and mouse axes.")]
        private GameplaySettings m_GameplaySettings;

        /// <summary>
        /// Provides all buttons and axes used by the character.
        /// </summary>
        [SerializeField]
        [Required()]
        [Tooltip("Provides all buttons and axes used by the character.")]
        private InputBindings m_InputBindings;

        #region PROPERTIES

        /// <summary>
        /// Is the character dead?
        /// </summary>
        public bool IsDead
        {
            get;
            set;
        }

        /// <summary>
        /// Returns the input behaviour of the crouching action.
        /// </summary>
        public ActionMode CrouchStyle => m_GameplaySettings.CrouchStyle;

        /// <summary>
        /// Returns the input behaviour of the aiming action.
        /// </summary>
        public ActionMode AimStyle => m_GameplaySettings.AimStyle;

        /// <summary>
        /// Returns the input behaviour of the running action.
        /// </summary>
        public ActionMode SprintStyle => m_GameplaySettings.SprintStyle;

        /// <summary>
        /// Returns the input behaviour of the leaning action.
        /// </summary>
        public ActionMode LeanStyle => m_GameplaySettings.LeanStyle;

        /// <summary>
        /// Returns the overall mouse sensitivity.
        /// </summary>
        public float OverallMouseSensitivity => m_GameplaySettings.OverallMouseSensitivity;

        /// <summary>
        /// Is the horizontal mouse input reversed?
        /// </summary>
        public bool InvertHorizontalAxis => m_GameplaySettings.InvertHorizontalAxis;

        /// <summary>
        /// Is the vertical mouse input reversed?
        /// </summary>
        public bool InvertVerticalAxis => m_GameplaySettings.InvertVerticalAxis;

        /// <summary>
        /// Returns the main camera field of view used by this character.
        /// </summary>
        public float FieldOfView => m_GameplaySettings.FieldOfView;

        /// <summary>
        /// Returns the Input Bindings used by this player.
        /// </summary>
        public InputBindings Bindings => m_InputBindings;

        #endregion
    }
}
