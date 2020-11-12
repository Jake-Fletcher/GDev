//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using System;
using UnityEngine;

namespace FPSBuilder.Core.Input
{
    /// <summary>
    /// Virtual keyboard axis.
    /// </summary>
    [Serializable]
    public sealed class Axis
    {
        /// <summary>
        /// The axis name.
        /// </summary>
        [SerializeField]
        [Tooltip("The axis name.")]
        private string m_Name;

        /// <summary>
        /// The key that sends a positive value to the axis.
        /// </summary>
        [SerializeField]
        [Tooltip("The key that sends a positive value to the axis.")]
        private string m_PositiveKey;

        /// <summary>
        /// The key that sends a negative value to the axis.
        /// </summary>
        [SerializeField]
        [Tooltip("The key that sends a negative value to the axis.")]
        private string m_NegativeKey;

        /// <summary>
        /// For keyboard input, a larger value results in faster response time. A lower value is smoother.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("For keyboard input, a larger value results in faster response time. A lower value is smoother.")]
        private float m_Sensitivity;

        /// <summary>
        /// Set how fast the input re-centers.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Set how fast the input re-centers.")]
        private float m_Gravity;

        /// <summary>
        /// Any positive or negative values that are less than this number register as zero.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Any positive or negative values that are less than this number register as zero.")]
        private float m_DeadZone;

        #region PROPERTIES

        /// <summary>
        /// The string that refers to the axis.
        /// </summary>
        public string Name => m_Name;

        /// <summary>
        /// The name of the button that sends a positive value to the axis.
        /// </summary>
        public string PositiveKey => m_PositiveKey;

        /// <summary>
        /// The name of the button that sends a negative value to the axis.
        /// </summary>
        public string NegativeKey => m_NegativeKey;

        /// <summary>
        /// A larger value results in faster response time. A lower value is smoother.
        /// </summary>
        public float Sensitivity => m_Sensitivity;

        /// <summary>
        /// Set how fast the input re-centers.
        /// </summary>
        public float Gravity => m_Gravity;

        /// <summary>
        /// Any positive or negative values that are less than this number register as zero.
        /// </summary>
        public float DeadZone => m_DeadZone;

        /// <summary>
        /// The current value stored in this axis.
        /// </summary>
        public float CurrentValue
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public Axis(string name, string positiveKey, string negativeKey, float sensitivity, float gravity, float deadZone)
        {
            m_Name = name;
            m_PositiveKey = positiveKey;
            m_NegativeKey = negativeKey;
            m_Sensitivity = sensitivity;
            m_Gravity = gravity;
            m_DeadZone = deadZone;
            CurrentValue = 0;
        }

        /// <summary>
        /// Alter the axis keys in realtime.
        /// </summary>
        public void EditAxis(string positiveKey, string negativeKey)
        {
            m_PositiveKey = positiveKey;
            m_NegativeKey = negativeKey;
        }
    }
}
