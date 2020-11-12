//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using UnityEngine;

#pragma warning disable CS0649

namespace FPSBuilder.PostProcessing
{
    [AddComponentMenu("FPS Builder/Effects/Blood Splash Effect"), DisallowMultipleComponent]
    public class BloodSplashEffect : MonoBehaviour
    {
        [SerializeField]
        private Texture2D m_BloodTexture;

        [SerializeField]
        private Texture2D m_BloodNormalMap;

        [SerializeField]
        [Range(0, 1)]
        private float m_BloodAmount;

        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        private float m_Distortion = 1.0f;

        private Material m_Material;
        private static readonly int BloodTex = Shader.PropertyToID("_BloodTex");
        private static readonly int BloodBump = Shader.PropertyToID("_BloodBump");
        private static readonly int Distortion = Shader.PropertyToID("_Distortion");
        private static readonly int Amount = Shader.PropertyToID("_BloodAmount");

        //Properties
        public float BloodAmount
        {
            get => m_BloodAmount;
            set => m_BloodAmount = value;
        }

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            if (m_Material == null)
                m_Material = new Material(Shader.Find("Hidden/BloodSplashEffect"));

            if (m_Material == null)
                return;

            //Send data into Shader
            if (m_BloodTexture != null)
                m_Material.SetTexture(BloodTex, m_BloodTexture);

            if (m_BloodNormalMap != null)
                m_Material.SetTexture(BloodBump, m_BloodNormalMap);

            m_Material.SetFloat(Distortion, m_Distortion);
            m_Material.SetFloat(Amount, m_BloodAmount);

            Graphics.Blit(src, dest, m_Material);
        }
    }
}
