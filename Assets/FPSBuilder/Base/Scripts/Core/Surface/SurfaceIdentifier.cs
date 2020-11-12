//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using UnityEngine;

namespace FPSBuilder.Core.Surface
{
    [AddComponentMenu("FPS Builder/Surface/Surface Identifier"), DisallowMultipleComponent]
    public sealed class SurfaceIdentifier : MonoBehaviour
    {
        /// <summary>
        /// List of surfaces of the GameObject.
        /// </summary>
        [SerializeField]
        [Tooltip("List of surfaces of the GameObject.")]
        private SurfaceData[] m_SurfaceList = new SurfaceData[1];

        #region PROPERTIES

        /// <summary>
        /// Returns the current active terrain.
        /// </summary>
        private Terrain ActiveTerrain => GetComponent<Terrain>();

        /// <summary>
        /// Returns true if the surface is on terrain, false otherwise.
        /// </summary>
        public bool IsTerrain => GetComponent<Terrain>() != null;

        /// <summary>
        /// Returns the array of Materials found in the object.
        /// </summary>
        public Material[] Materials
        {
            get
            {
                Renderer r = gameObject.GetComponent<Renderer>();
                return r != null ? r.sharedMaterials : null;
            }
        }

        /// <summary>
        /// Returns the array of Textures found in the object.
        /// </summary>
        public Texture2D[] Textures
        {
            get
            {
                if (!IsTerrain || ActiveTerrain == null)
                    return null;

                TerrainLayer[] terrainLayers = ActiveTerrain.terrainData.terrainLayers;
                Texture2D[] tex = new Texture2D[terrainLayers.Length];

                for (int i = 0; i < terrainLayers.Length; i++)
                    tex[i] = terrainLayers[i].diffuseTexture;

                return tex;
            }
        }

        #region EDITOR

        /// <summary>
        /// Returns the number of surfaces on the object.
        /// </summary>
        public int SurfacesCount => m_SurfaceList.Length;

        #endregion

        #endregion

        /// <summary>
        /// Returns true if the surface allows decals to be drawn on it, false otherwise.
        /// </summary>
        /// <param name="triangleIndex">The hit triangle index.</param>
        public bool AllowDecals(int triangleIndex = -1)
        {
            if (m_SurfaceList == null)
                return false;

            return !IsTerrain && m_SurfaceList[SurfaceUtility.GetMaterialIndex(triangleIndex, gameObject)].AllowDecals;
        }

        /// <summary>
        /// Returns true if projectiles can penetrate this surface, false otherwise.
        /// </summary>
        /// <param name="triangleIndex">The hit triangle index.</param>
        public bool CanPenetrate(int triangleIndex = -1)
        {
            if (m_SurfaceList == null)
                return false;

            return !IsTerrain && m_SurfaceList[SurfaceUtility.GetMaterialIndex(triangleIndex, gameObject)].Penetration;
        }

        /// <summary>
        /// Returns the density of this surface.
        /// </summary>
        /// <param name="triangleIndex">The hit triangle index.</param>
        public float Density(int triangleIndex = -1)
        {
            if (m_SurfaceList == null)
                return 1;

            if (!IsTerrain)
            {
                return m_SurfaceList[SurfaceUtility.GetMaterialIndex(triangleIndex, gameObject)].Density;
            }
            return 1;
        }

        /// <summary>
        /// Returns the SurfaceType at given position.
        /// </summary>
        /// <param name="position">The contact position. (if the GameObject is a Terrain)</param>
        /// <param name="triangleIndex">The hit triangle index.</param>
        /// <returns></returns>
        public SurfaceType GetSurfaceType(Vector3 position, int triangleIndex = -1)
        {
            if (m_SurfaceList == null || m_SurfaceList.Length <= 0)
                return null;

            if (IsTerrain)
            {
                int index = SurfaceUtility.GetMainTexture(position, ActiveTerrain.transform.position, ActiveTerrain.terrainData);
                return index < m_SurfaceList.Length ? m_SurfaceList[index].SurfaceType : null;

            }
            return m_SurfaceList[SurfaceUtility.GetMaterialIndex(triangleIndex, gameObject)].SurfaceType;
        }

        /// <summary>
        /// Resets the Component.
        /// </summary>
        public void Reset()
        {
            m_SurfaceList = GetSurfaceList();
        }

        /// <summary>
        /// Caches all surfaces found on the GameObject.
        /// </summary>
        private SurfaceData[] GetSurfaceList()
        {
            SurfaceData[] surfaces;

            // Is this component attached to a terrain?
            if (IsTerrain)
            {
                TerrainLayer[] terrainLayers = ActiveTerrain.terrainData.terrainLayers;
                surfaces = new SurfaceData[terrainLayers.Length];

                for (int i = 0; i < terrainLayers.Length; i++)
                    surfaces[i] = new SurfaceData();
            }
            else
            {
                Renderer r = gameObject.GetComponent<Renderer>();

                if (r && r.sharedMaterials.Length > 0)
                {
                    surfaces = new SurfaceData[r.sharedMaterials.Length];

                    for (int i = 0; i < r.sharedMaterials.Length; i++)
                        surfaces[i] = new SurfaceData();
                }
                else
                {
                    surfaces = new[] { new SurfaceData() };
                }
            }
            return surfaces;
        }
    }
}
