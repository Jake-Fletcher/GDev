//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using System.Collections.Generic;
using UnityEngine;

namespace FPSBuilder.Core.Input
{
    /// <summary>
    /// An Input Bindings Asset allows you to define input axes and buttons and their associated actions for your Project.
    /// </summary>
    [CreateAssetMenu(menuName = "Input Bindings", fileName = "Input Bindings", order = 201)]
    public sealed class InputBindings : ScriptableObject
    {
        /// <summary>
        /// The list of all buttons registered in this input binding.
        /// </summary>
        [SerializeField]
        private List<Button> m_Buttons = new List<Button>();

        /// <summary>
        /// The list of all axes registered in this input binding.
        /// </summary>
        [SerializeField]
        private List<Axis> m_Axes = new List<Axis>();

        #region PROPERTIES

        /// <summary>
        /// The list of all buttons registered in this input binding.
        /// </summary>
        public List<Button> Buttons => m_Buttons;

        /// <summary>
        /// The list of all axes registered in this input binding.
        /// </summary>
        public List<Axis> Axes => m_Axes;

        #endregion

        #region IMPUT MANAGER

        /// <summary>
        /// Check if the key has already been registered by any button.
        /// </summary>
        /// <param name="key">The key to be verified.</param>
        private bool Exists(string key)
        {
            for (int i = 0, c = m_Buttons.Count; i < c; i++)
            {
                if (m_Buttons[i].Key == key)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Register a new button.
        /// </summary>
        /// <param name="buttonName">The button name.</param>
        /// <param name="defaultKey">The key related to this button.</param>
        public void AddButton(string buttonName, string defaultKey)
        {
            if (!Exists(defaultKey))
            {
                m_Buttons.Add(new Button(buttonName, defaultKey));
            }
        }

        /// <summary>
        /// Remove the button specified by the index.
        /// </summary>
        /// <param name="index">The index to be removed of the button list.</param>
        public void RemoveButton(int index)
        {
            m_Buttons.RemoveAt(index);
        }

        /// <summary>
        /// Restores the default value of the button.
        /// </summary>
        /// <param name="index">The button index to be redefined.</param>
        public void ResetButton(int index)
        {
            m_Buttons[index] = new Button("Button", string.Empty);
        }

        /// <summary>
        /// Register a new axis.
        /// </summary>
        /// <param name="axisName">Name of the axis.</param>
        /// <param name="positiveKey">The key that sends a positive value to the axis.</param>
        /// <param name="negativeKey">The key that sends a negative value to the axis.</param>
        /// <param name="sensitivity">For keyboard input, a larger value results in faster response time. A lower value is smoother.</param>
        /// <param name="gravity">Set how fast the input re-centers.</param>
        /// <param name="deadZone">Any positive or negative values that are less than this number register as zero.</param>
        public void AddAxis(string axisName, string positiveKey, string negativeKey, float sensitivity, float gravity, float deadZone)
        {
            m_Axes.Add(new Axis(axisName, positiveKey, negativeKey, sensitivity, gravity, deadZone));
        }

        /// <summary>
        /// Remove the axis specified by the index.
        /// </summary>
        /// <param name="index">The index to be removed of the axes list.</param>
        public void RemoveAxis(int index)
        {
            m_Axes.RemoveAt(index);
        }

        /// <summary>
        /// Restores the default value of the axis.
        /// </summary>
        /// <param name="index">The axis index to be redefined.</param>
        public void ResetAxis(int index)
        {
            m_Axes[index] = new Axis("Axis", string.Empty, string.Empty, 1, 1, 0.01f);
        }

        #endregion
    }
}

