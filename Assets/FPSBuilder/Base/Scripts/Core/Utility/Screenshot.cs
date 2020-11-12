//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using System.Collections.Generic;
using FPSBuilder.Core.Input;
using UnityEngine;

namespace FPSBuilder.Core
{
    public class Screenshot : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> minhaLista = new List<GameObject>();

        [SerializeField]
        [Range(1, 8)]
        private int m_SuperSampling = 1;

        private Button m_PrtSc;

        private void Start()
        {
            m_PrtSc = InputManager.FindButton("Prt Sc");
        }

        // Update is called once per frame
        private void Update()
        {
            if (InputManager.GetButtonDown(m_PrtSc))
            {
                string screenshotName = "Screenshots/" + "Screenshot_" + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".png";
                ScreenCapture.CaptureScreenshot(screenshotName, m_SuperSampling);
            }
        }

        [ContextMenu("Capture")]
        private void DoSomething()
        {
            string screenshotName = "Screenshots/" + "Screenshot_" + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".png";
            ScreenCapture.CaptureScreenshot(screenshotName, m_SuperSampling);
        }
    }
}

