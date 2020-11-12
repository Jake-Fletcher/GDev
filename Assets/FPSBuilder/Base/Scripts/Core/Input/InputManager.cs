//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using FPSBuilder.Core.Managers;
using UnityEngine;

namespace FPSBuilder.Core.Input
{
    using Input = UnityEngine.Input;

    /// <summary>
    /// Interface for handling user Input.
    /// </summary>
    public static class InputManager
    {
        private static InputBindings m_InputBindings;

        /// <summary>
        /// Loads the first InputBindings found in the project.
        /// </summary>
        private static bool FindInputBindings()
        {
            // The quickest and most correct way to know which input binding to use is by using the GameplayManager reference.
            // Note that you must assign an input binding in GameplayManager or the file will be stripped out of your game when you compile it.
            if (GameplayManager.Instance.Bindings)
            {
                m_InputBindings = GameplayManager.Instance.Bindings;
                return true;
            }

            InputBindings[] foundItems = (InputBindings[])Resources.FindObjectsOfTypeAll(typeof(InputBindings));

            if (foundItems == null || foundItems.Length == 0)
            {
                Debug.LogError("No InputBindings found in your project.");
                return false;
            }

            m_InputBindings = foundItems[0];
            return true;
        }

        /// <summary>
        /// Returns true if the player presses the same button twice.
        /// </summary>
        public static bool DoubleTap(string buttonName, float interval)
        {
            if (Mathf.Abs(Time.timeScale) < float.Epsilon || !Input.anyKeyDown)
                return false;

            Button b = FindButton(buttonName);

            return b != null && DoubleTap(b, interval);
        }

        /// <summary>
        /// Returns true if the player presses the same button twice.
        /// </summary>
        public static bool DoubleTap(Button button, float interval)
        {
            if (Mathf.Abs(Time.timeScale) < float.Epsilon || !Input.anyKeyDown)
                return false;

            if (button == null)
                return false;

            if (Input.GetKeyDown(button.Key) && button.LastUseTime + interval > Time.time)
            {
                button.LastUseTime = 0;
                return true;
            }
            if (Input.GetKeyUp(button.Key))
            {
                button.LastUseTime = Time.time;
            }
            return false;
        }

        /// <summary>
        /// Returns true if the player keeps the button pressed in a time interval.
        /// </summary>
        public static bool HoldButton(string buttonName, float time)
        {
            if (Mathf.Abs(Time.timeScale) < float.Epsilon || !Input.anyKey)
                return false;

            Button b = FindButton(buttonName);

            return b != null && HoldButton(b, time);
        }

        /// <summary>
        /// Returns true if the player keeps the button pressed in a time interval.
        /// </summary>
        public static bool HoldButton(Button button, float time)
        {
            if (Mathf.Abs(Time.timeScale) < float.Epsilon || !Input.anyKey)
                return false;

            if (button == null)
                return false;

            if (Input.GetKeyDown(button.Key))
            {
                button.LastUseTime = Time.time;
            }

            if (!Input.GetKey(button.Key) || (Time.time - button.LastUseTime < time))
                return false;

            button.LastUseTime = Time.time;
            return true;
        }

        /// <summary>
        /// Returns true if the player is pressing the button.
        /// </summary>
        public static bool GetButton(string buttonName)
        {
            if (Mathf.Abs(Time.timeScale) < float.Epsilon || !Input.anyKey)
                return false;

            Button b = FindButton(buttonName);
            return GetButton(b);
        }

        /// <summary>
        /// Returns true if the player is pressing the button.
        /// </summary>
        public static bool GetButton(Button button)
        {
            if (Mathf.Abs(Time.timeScale) < float.Epsilon || !Input.anyKey)
                return false;

            return button != null && Input.GetKey(button.Key);
        }

        /// <summary>
        /// Return true if the player has pressed the button.
        /// </summary>
        public static bool GetButtonDown(string buttonName)
        {
            if (Mathf.Abs(Time.timeScale) < float.Epsilon || !Input.anyKeyDown)
                return false;

            Button b = FindButton(buttonName);
            return GetButtonDown(b);
        }

        /// <summary>
        /// Return true if the player has pressed the button.
        /// </summary>
        public static bool GetButtonDown(Button button)
        {
            if (Mathf.Abs(Time.timeScale) < float.Epsilon || !Input.anyKeyDown)
                return false;

            return button != null && Input.GetKeyDown(button.Key);
        }

        /// <summary>
        /// Return true if the player has released the button.
        /// </summary>
        public static bool GetButtonUp(string buttonName)
        {
            if (Mathf.Abs(Time.timeScale) < float.Epsilon)
                return false;

            Button b = FindButton(buttonName);
            return b != null && Input.GetKeyUp(b.Key);
        }

        /// <summary>
        /// Returns the value of the virtual axis with smoothing filtering.
        /// </summary>
        public static float GetAxis(string axisName)
        {
            if (Mathf.Abs(Time.timeScale) < float.Epsilon)
                return 0;

            Axis a = FindAxis(axisName);
            return GetAxis(a);
        }

        /// <summary>
        /// Returns the value of the virtual axis with smoothing filtering.
        /// </summary>
        public static float GetAxis(Axis axis)
        {
            if (axis == null)
                return 0;

            float target = GetAxisRaw(axis);
            axis.CurrentValue = Mathf.MoveTowards(axis.CurrentValue, target, ((Mathf.Abs(target) > Mathf.Epsilon) ? Time.deltaTime / axis.Sensitivity : Time.deltaTime * axis.Gravity));
            return (Mathf.Abs(axis.CurrentValue) < axis.DeadZone) ? 0 : axis.CurrentValue;
        }

        /// <summary>
        /// Returns the value of the virtual axis.
        /// </summary>
        private static float GetAxisRaw(Axis axis)
        {
            if (Input.GetKey(axis.PositiveKey))
            {
                return 1;
            }
            if (Input.GetKey(axis.NegativeKey))
            {
                return -1;
            }
            return 0;
        }

        /// <summary>
        /// Returns the value of the virtual axis without smoothing filtering.
        /// </summary>
        public static float GetAxisRaw(string axisName)
        {
            if (Mathf.Abs(Time.timeScale) < float.Epsilon)
                return 0;

            Axis a = FindAxis(axisName);
            if (a == null)
                return 0;

            return GetAxisRaw(a);
        }

        /// <summary>
        /// Returns the value alpha key (1,2,3,4,5,6,7,8,9,0) based on the given index.
        /// </summary>
        public static KeyCode GetAlphaKeyCode(int index)
        {
            switch (index)
            {
                case 0:
                return KeyCode.Alpha1;
                case 1:
                return KeyCode.Alpha2;
                case 2:
                return KeyCode.Alpha3;
                case 3:
                return KeyCode.Alpha4;
                case 4:
                return KeyCode.Alpha5;
                case 5:
                return KeyCode.Alpha6;
                case 6:
                return KeyCode.Alpha7;
                case 7:
                return KeyCode.Alpha8;
                case 8:
                return KeyCode.Alpha9;
                case 9:
                return KeyCode.Alpha0;
            }
            return KeyCode.None;
        }

        /// <summary>
        /// Returns the first found button with the given name.
        /// </summary>
        public static Button FindButton(string name)
        {
            if (m_InputBindings == null)
            {
                if (!FindInputBindings())
                    return null;
            }

            for (int i = 0, c = m_InputBindings.Buttons.Count; i < c; i++)
            {
                if (m_InputBindings.Buttons[i].Name.Equals(name))
                    return m_InputBindings.Buttons[i];
            }

            Debug.LogError("InputManager: Button '" + name + "' was not found.");
            return null;
        }

        /// <summary>
        /// Returns the first found axis with the given name.
        /// </summary>
        public static Axis FindAxis(string name)
        {
            if (m_InputBindings == null)
            {
                if (!FindInputBindings())
                    return null;
            }

            for (int i = 0, c = m_InputBindings.Axes.Count; i < c; i++)
            {
                if (m_InputBindings.Axes[i].Name.Equals(name))
                    return m_InputBindings.Axes[i];
            }

            Debug.LogError("InputManager: Axis '" + name + "' was not found.");
            return null;
        }

        /// <summary>
        /// Edit the key used by the button.
        /// </summary>
        /// <param name="button">The button to be edited.</param>
        /// <param name="newKey">The new key to be assigned to the button.</param>
        public static void EditButton(Button button, string newKey)
        {
            button?.EditKey(newKey);
        }

        /// <summary>
        /// Edit the keys used by the axis.
        /// </summary>
        /// <param name="axis">The axis to be edited.</param>
        /// <param name="positiveKey">The new positive key to be assigned to the axis.</param>
        /// <param name="negativeKey">The new negative key to be assigned to the axis.</param>
        public static void EditAxis(Axis axis, string positiveKey, string negativeKey)
        {
            axis?.EditAxis(positiveKey, negativeKey);
        }

        /// <summary>
        /// Returns all buttons registered in the InputBinding.
        /// </summary>
        public static Button[] GetButtonData()
        {
            if (m_InputBindings == null)
                return FindInputBindings() ? m_InputBindings.Buttons.ToArray() : null;

            return m_InputBindings.Buttons.ToArray();
        }

        /// <summary>
        /// Returns all axes registered in the InputBinding.
        /// </summary>
        public static Axis[] GetAxisData()
        {
            if (m_InputBindings == null)
                return FindInputBindings() ? m_InputBindings.Axes.ToArray() : null;

            return m_InputBindings.Axes.ToArray();
        }
    }
}
