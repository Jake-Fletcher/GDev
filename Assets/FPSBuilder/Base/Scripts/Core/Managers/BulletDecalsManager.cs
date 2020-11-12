//=========== Copyright (c) GameBuilders, All rights reserved. ================
//
// Purpose: The Bullet Decals Manager Script is used by the weapons to register bullet impact decals
//          and prevent that the number of decals doesnt exceed the maximum allowed. It also separates them
//          to avoid overlapping (Z-fighting).
//
//=============================================================================

using System.Collections.Generic;
using FPSBuilder.Core.Surface;
using UnityEngine;

namespace FPSBuilder.Core.Managers
{
    /// <summary>
    /// The Bullet Decals Manager Script is mainly used by the weapons to register bullet impact decals 
    /// and prevent that the number of decals don’t exceed the maximum allowed.
    /// </summary>
    [AddComponentMenu("FPS Builder/Managers/Bullet Decals Manager"), DisallowMultipleComponent]
    public class BulletDecalsManager : Singleton<BulletDecalsManager>
    {
        /// <summary>
        /// Defines the maximum number of decals that can be generated on the scene.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines the maximum number of decals that can be generated on the scene.")]
        protected int m_MaxDecals = 250;

        /// <summary>
        /// Defines the minimum distance between two decals. (If the distance between two decals is less than the indicated value they will be merged into a single one)
        /// </summary>
        [SerializeField]
        [Range(0, 1)]
        [Tooltip("Defines the minimum distance between two decals. (If the distance between two decals is less than the indicated value they will be merged into a single one)")]
        protected float m_DecalSeparator = 0.166f;

        private readonly List<GameObject> m_DecalList = new List<GameObject>();
        private readonly List<float> m_PushDistances = new List<float>();

        /// <summary>
        /// Generates a bullet hole decal.
        /// </summary>
        /// <param name="surface">The surface properties of the hit object.</param>
        /// <param name="hitInfo">The information about the projectile impact such as position and rotation.</param>
        public virtual void CreateBulletDecal(SurfaceIdentifier surface, RaycastHit hitInfo)
        {
            SurfaceType surfaceType = surface.GetSurfaceType(hitInfo.point, hitInfo.triangleIndex);

            if (surface && surfaceType)
            {
                if (surface.AllowDecals(hitInfo.triangleIndex) && m_MaxDecals > 0)
                {
                    Material material = surfaceType.GetRandomDecalMaterial();
                    if (material)
                    {
                        GameObject decal = new GameObject("BulletMark_Decal");
                        decal.transform.position = hitInfo.point;
                        decal.transform.rotation = Quaternion.FromToRotation(Vector3.back, hitInfo.normal);

                        decal.transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)));

                        float scale = surfaceType.GetRandomDecalSize() / 5;
                        decal.transform.localScale = new Vector3(scale, scale, scale);

                        decal.transform.parent = hitInfo.transform;

                        DecalPresets decalPresets = new DecalPresets()
                        {
                            maxAngle = 60,
                            pushDistance = 0.009f + RegisterDecal(decal, scale),
                            material = material
                        };

                        Decal d = decal.AddComponent<Decal>();
                        d.Calculate(decalPresets, hitInfo.collider.gameObject);
                    }
                }

                GameObject particle = surfaceType.GetRandomImpactParticle();
                if (particle)
                {
                    Instantiate(particle, hitInfo.point, Quaternion.FromToRotation(Vector3.up, hitInfo.normal));
                }

                AudioClip clip = surfaceType.GetRandomImpactSound();
                if (clip)
                {
                    AudioManager.Instance.PlayClipAtPoint(clip, hitInfo.point, 1, 25, surfaceType.BulletImpactVolume);
                }
            }
        }

        /// <summary>
        /// Register a new decal and validate if it can be generated.
        /// </summary>
        /// <param name="decal">The decal object.</param>
        /// <param name="scale">The decal size.</param>
        protected virtual float RegisterDecal(GameObject decal, float scale)
        {
            GameObject auxGO;
            Transform currentT = decal.transform;
            Vector3 currentPos = currentT.position;

            float radius = Mathf.Sqrt((scale * scale * 0.25f) * 3);

            float realRadius = radius * 2;
            radius *= m_DecalSeparator;

            if (m_DecalList.Count == m_MaxDecals)
            {
                auxGO = m_DecalList[0];
                Destroy(auxGO);
                m_DecalList.RemoveAt(0);
                m_PushDistances.RemoveAt(0);
            }

            float pushDistance = 0;

            for (int i = 0; i < m_DecalList.Count; i++)
            {
                auxGO = m_DecalList[i];

                if (auxGO)
                {
                    Transform auxT = auxGO.transform;
                    float distance = (auxT.position - currentPos).magnitude;

                    if (distance < radius)
                    {
                        Destroy(auxGO);
                        m_DecalList.RemoveAt(i);
                        m_PushDistances.RemoveAt(i);
                        i--;
                    }
                    else if (distance < realRadius)
                    {
                        float cDist = m_PushDistances[i];
                        pushDistance = Mathf.Max(pushDistance, cDist);
                    }
                }
                else
                {
                    m_DecalList.RemoveAt(i);
                    m_PushDistances.RemoveAt(i);
                    i--;
                }
            }

            pushDistance += 0.001f;

            m_DecalList.Add(decal);
            m_PushDistances.Add(pushDistance);

            return pushDistance;
        }

        /// <summary>
        /// Clear all decals generated in the scene.
        /// </summary>
        public virtual void RemoveDecals()
        {
            if (m_DecalList.Count <= 0)
                return;

            for (int i = 0, c = m_DecalList.Count; i < c; i++)
            {
                GameObject go = m_DecalList[i];
                Destroy(go);
            }
            m_DecalList.Clear();
            m_PushDistances.Clear();
        }
    }
}