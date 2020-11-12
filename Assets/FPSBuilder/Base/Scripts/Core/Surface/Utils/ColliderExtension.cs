//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using System.Collections.Generic;
using UnityEngine;

namespace FPSBuilder.Core.Surface
{
    public static class ColliderExtension
    {
        /// <summary>
        /// Cached surfaces.
        /// </summary>
        private static Dictionary<int, SurfaceIdentifier> m_SurfaceIdentifiers = new Dictionary<int, SurfaceIdentifier>();

        /// <summary>
        /// Instead of trying to get the Surface Identifier using GetComponent() every time we need to check a object,
        /// a dictionary is much more performance efficient because we have to check a object only once.
        /// However it's not possible to detect changes in the object (add a SurfaceIdentifier at runtime) once it has been registered.
        /// </summary>
        public static SurfaceIdentifier GetSurface(this Collider col)
        {
            if (m_SurfaceIdentifiers.ContainsKey(col.GetInstanceID()))
            {
                return m_SurfaceIdentifiers[col.GetInstanceID()];
            }

            m_SurfaceIdentifiers.Add(col.GetInstanceID(), col.GetComponent<SurfaceIdentifier>());
            return m_SurfaceIdentifiers[col.GetInstanceID()];
        }
    }
}
