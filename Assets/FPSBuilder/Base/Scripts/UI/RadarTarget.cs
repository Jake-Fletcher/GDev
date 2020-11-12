//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS0649

namespace FPSBuilder.UI
{
    public class RadarTarget : MonoBehaviour
    {
        [SerializeField]
        private Image m_Image;

        private void Start ()
        {
            RadarUI.RegisterRadarObject(gameObject, m_Image);
        }

        private void OnDestroy ()
        {
            RadarUI.RemoveRadarObject(gameObject);
        }
    }
}
