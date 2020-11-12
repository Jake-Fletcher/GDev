//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using UnityEngine.UI;
using UnityEngine;

#pragma warning disable CS0649

namespace FPSBuilder.UI
{
    public class CompassUI : MonoBehaviour
    {
        [SerializeField]
        private MenuController m_MenuController;
    
        [SerializeField]
        private RawImage m_Compass;
    
        [SerializeField]
        private Text m_DirectionText;
    
        public void Update()
        {
            m_Compass.uvRect = new Rect(m_MenuController.FirstPersonCharacter.transform.localEulerAngles.y / 360, 0, 1, 1);
            Vector3 forward = m_MenuController.FirstPersonCharacter.transform.forward;
            forward.y = 0;
    
            float headingAngle = Quaternion.LookRotation(forward).eulerAngles.y;
            headingAngle = 5 * (Mathf.RoundToInt(headingAngle / 5.0f));
    
            switch ((int)headingAngle)
            {
                case 0:
                    m_DirectionText.text = "<color=#FCB628>N</color>";
                    break;
                case 360:
                    m_DirectionText.text = "<color=#FCB628>N</color>";
                    break;
                case 45:
                    m_DirectionText.text = "<color=#FCB628>NE</color>";
                    break;
                case 90:
                    m_DirectionText.text = "<color=#FCB628>E</color>";
                    break;
                case 130:
                    m_DirectionText.text = "<color=#FCB628>SE</color>";
                    break;
                case 180:
                    m_DirectionText.text = "<color=#FCB628>S</color>";
                    break;
                case 225:
                    m_DirectionText.text = "<color=#FCB628>SW</color>";
                    break;
                case 270:
                    m_DirectionText.text = "<color=#FCB628>W</color>";
                    break;
                case 315:
                    m_DirectionText.text = "<color=#FCB628>NW</color>";
                    break;
                default:
                    m_DirectionText.text =  headingAngle.ToString("F0");
                    break;
            }
        }
    }
}
