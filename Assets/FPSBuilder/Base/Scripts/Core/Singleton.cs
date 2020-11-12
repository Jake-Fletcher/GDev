//=========== Copyright (c) GameBuilders, All rights reserved. ================
//
// Purpose: The singleton pattern is essentially a class which only allows a single instance of itself
//          to be created and usually gives simple access to that instance. Behaving much like a regular
//          static class but with some advantages. This is very useful for making global manager type classes
//          that hold global variables and functions that many other classes need to access.
//
//=============================================================================

using UnityEngine;

namespace FPSBuilder.Core
{
    /// <summary>
    /// Inherit from this base class to create a singleton.
    /// </summary>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// Access singleton instance through this propriety.
        /// </summary>
        public static T Instance
        {
            get;
            private set;
        }

        public virtual void Awake()
        {
            if (Instance != this)
                Instance = this as T;
        }

        public virtual void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        private void OnApplicationQuit()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}