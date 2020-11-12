//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using System;
using UnityEngine;

namespace FPSBuilder.Core.Input
{
    /// <summary>
    /// Virtual keyboard button.
    /// </summary>
    [Serializable]
    public sealed class Button
    {
        /// <summary>
        /// The button name.
        /// </summary>
        [SerializeField]
        [Tooltip("The button name.")]
        private string m_Name;

        /// <summary>
        /// The key used to perform the activities related to this button.
        /// </summary>
        [SerializeField]
        [Tooltip("The key used to perform the activities related to this button.")]
        private string m_Key;

        #region PROPERTIES

        /// <summary>
        /// The string that refers to the button.
        /// </summary>
        public string Name => m_Name;

        /// <summary>
        /// The key used to perform the activities related to this button.
        /// </summary>
        public string Key => m_Key;

        /// <summary>
        /// Return the last time that this button was used. (Measured concerning the time since startup)
        /// </summary>
        public float LastUseTime
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public Button(string name, string defaultKey)
        {
            m_Name = name;
            m_Key = defaultKey;

            LastUseTime = 0;
        }

        /// <summary>
        /// Alter the button key in realtime.
        /// </summary>
        public void EditKey(string newKey)
        {
            m_Key = newKey;
        }
    }
}
