//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using UnityEngine;

namespace FPSBuilder.PostProcessing
{
    [AddComponentMenu("FPS Builder/Effects/Lens Distortion Effect"), DisallowMultipleComponent]
    public class LensDistortionEffect : MonoBehaviour
    {
        [SerializeField]
        private float m_Distortion = -0.5f;

        [SerializeField]
        private float m_CubicDistortion= 1.5f;

        private Material m_Material;
        private static readonly int Distortion = Shader.PropertyToID("_Distortion");
        private static readonly int CubicDistortion = Shader.PropertyToID("_CubicDistortion");

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            if (m_Material == null)
                m_Material = new Material(Shader.Find("Hidden/LensDistortion"));

            if (m_Material == null)
                return;

            m_Material.SetFloat(Distortion, m_Distortion);
            m_Material.SetFloat(CubicDistortion, m_CubicDistortion);

            Graphics.Blit(src, dest, m_Material);
        }
    }
}
