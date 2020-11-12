//=========== Copyright (c) GameBuilders, All rights reserved. ================
//
// Purpose: The Motion Data Asset contains all data used to perform procedural motion for the respective player state,
// affecting both camera and player weapons. These procedural animation controllers are focused on the walking
// and running motion within these different states having unique values in mind to stress a player state.
// (i.e. whilst the player is running, naturally, amplitude curves and general values of intensity are increased
// as motion while running logically would become more erratic. The opposite for the more subtle state of walking).
//
//=============================================================================

using UnityEngine;

#pragma warning disable CS0649

namespace FPSBuilder.Core.Animation
{
    /// <summary>
    /// The Motion Data Asset is a data container with all rules used to perform a procedural movement animation.
    /// </summary>
    [CreateAssetMenu(menuName = "Motion Data", fileName = "Motion Data", order = 201)]
    public sealed class MotionData : ScriptableObject
    {
        /// <summary>
        /// Determines how fast the animation will be played.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Determines how fast the animation will be played.")]
        private float m_Speed = 1;

        /// <summary>
        /// Determines how fast the animation can be modified or how smooth the animation will be blended.
        /// </summary>
        [SerializeField]
        [Range(0.1f, 3)]
        [Tooltip("Determines how fast the animation can be modified or how smooth the animation will be blended.")]
        private float m_Smoothness = 1;

        /// <summary>
        /// Determines how far the transform can move horizontally.
        /// </summary>
        [SerializeField]
        [Tooltip("Determines how far the transform can move horizontally.")]
        private float m_HorizontalAmplitude = 1;

        /// <summary>
        /// Determines how far the transform can move vertically.
        /// </summary>
        [SerializeField]
        [Tooltip("Determines how far the transform can move vertically.")]
        private float m_VerticalAmplitude = 1;

        /// <summary>
        /// Defines how the vertical movement will behave.
        /// </summary>
        [SerializeField]
        [Tooltip("Defines how the vertical movement will behave.")]
        private AnimationCurve m_VerticalAnimationCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));

        /// <summary>
        /// Determines how notably the transform can be rotated on Z-axis.
        /// </summary>
        [SerializeField]
        [Tooltip("Determines how notably the transform can be rotated on Z-axis.")]
        private float m_RotationAmplitude;

        /// <summary>
        /// Determines how distinctly the animation will be vertically affected by the direction of the character's movement.
        /// </summary>
        [SerializeField]
        [Range(-2, 2)]
        [Tooltip("Determines how distinctly the animation will be vertically affected by the direction of the character's movement.")]
        private float m_VelocityInfluence = 1;

        /// <summary>
        /// Determines a position offset for the animation, moving it to a more appropriate position.
        /// </summary>
        [SerializeField]
        [Tooltip("Determines a position offset for the animation, moving it to a more appropriate position.")]
        private Vector3 m_PositionOffset;

        /// <summary>
        /// Determines a rotation offset for the animation, rotating it to a more appropriate angle.
        /// </summary>
        [SerializeField]
        [Tooltip("Determines a rotation offset for the animation, rotating it to a more appropriate angle.")]
        private Vector3 m_RotationOffset;

        #region PROPERTIES

        /// <summary>
        /// Determines how fast the animation will be played. (Read Only)
        /// </summary>
        public float Speed => m_Speed;

        /// <summary>
        /// Determines how fast the animation can be changed or how smooth the animation will be blended. (Read Only)
        /// </summary>
        public float Smoothness => m_Smoothness;

        /// <summary>
        /// Determines how much the animation will be vertically affected by the direction of the character's movement. (Read Only)
        /// </summary>
        public float VelocityInfluence => m_VelocityInfluence;

        /// <summary>
        /// Determines how far the transform can be moved horizontally. (Read Only)
        /// </summary>
        public float HorizontalAmplitude => m_HorizontalAmplitude;

        /// <summary>
        /// Determines how far the transform can be moved vertically. (Read Only)
        /// </summary>
        public float VerticalAmplitude => m_VerticalAmplitude;

        /// <summary>
        /// Defines how the vertical movement will behave. (Read Only)
        /// </summary>
        public AnimationCurve VerticalAnimationCurve => m_VerticalAnimationCurve;

        /// <summary>
        /// Determines how far the transform can be rotated on Z-axis. (Read Only)
        /// </summary>
        public float RotationAmplitude => m_RotationAmplitude;

        /// <summary>
        /// Determines a position offset for the animation, moving it to a more appropriate position. (Read Only)
        /// </summary>
        public Vector3 PositionOffset => m_PositionOffset;

        /// <summary>
        /// Determines a rotation offset for the animation, rotating it to a more appropriate angle. (Read Only)
        /// </summary>
        public Vector3 RotationOffset => m_RotationOffset;

        #endregion
    }
}
