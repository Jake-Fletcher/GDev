//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using System;
using FPSBuilder.Core.Input;
using FPSBuilder.Core.Items;
using FPSBuilder.Core.Managers;
using FPSBuilder.Core.Player;
using FPSBuilder.Core.Weapons;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = FPSBuilder.Core.Input.Button;

#pragma warning disable CS0649

namespace FPSBuilder.UI
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_HUDCanvas;

        [SerializeField]
        private GameObject m_PauseCanvas;

        [SerializeField]
        private GameObject m_DeathScreenCanvas;

        [SerializeField]
        private GameObject m_EventSystem;

        [SerializeField]
        private BlackScreen m_PauseBlackScreen;

        [SerializeField]
        private BlackScreen m_DeathBlackScreen;
    
        [SerializeField]
        private Text m_DeathQuote;
    
        [SerializeField]
        private string[] m_Quoutes =
        {
            "There's no honorable way to kill, no gentle way to destroy. There is nothing good in war. Except its ending.\n— Abraham Lincoln",
            "The death of one man is a tragedy. The death of millions is a statistic.\n— Joseph Stalin",
            "Every man's life ends the same way. It is only the details of how he lived and how he died that distinguish one man from another.\n— Ernest Hemingway",
            "Better to fight for something than live for nothing.\n— General George S. Patton",
            "Mankind must put an end to war, or war will put an end to mankind.\n— John F. Kennedy"
        };

        [Header("References")]
        [SerializeField]
        [Required]
        private FirstPersonCharacterController m_FirstPersonCharacter;

        [SerializeField]
        [Required]
        private WeaponManager m_WeaponManager;

        [SerializeField]
        [Required]
        private HealthController m_HealthController;

        [SerializeField]
        private Grenade m_Grenade;

        [SerializeField]
        private Adrenaline m_Adrenaline;

        private Button m_PauseButton;
        private bool m_Restarting;

        #region PROPERTIES

        public FirstPersonCharacterController FirstPersonCharacter
        {
            get
            {
                return m_FirstPersonCharacter;
            }
        }

        public WeaponManager WeaponManager
        {
            get
            {
                return m_WeaponManager;
            }
        }

        public HealthController HealthController
        {
            get
            {
                return m_HealthController;
            }
        }

        public Grenade Grenade
        {
            get
            {
                return m_Grenade;
            }
        }

        public Adrenaline Adrenaline
        {
            get
            {
                return m_Adrenaline;
            }
        }

        public string UseKey { get; private set; } = string.Empty;

        #endregion

        private void Start ()
        {
            System.Random random = new System.Random();
            m_DeathQuote.text = m_Quoutes[random.Next(0, m_Quoutes.Length - 1)];

            m_PauseButton = InputManager.FindButton("Pause");
            UseKey = InputManager.FindButton("Use").Key;

            Resume();
        }

        private void Update ()
        {
            if (GameplayManager.Instance.IsDead && !m_Restarting)
            {
                DeathScreen();
            }
            else
            {
                if (InputManager.GetButtonDown(m_PauseButton))
                {
                    Pause();
                }

                if (Input.GetKeyDown(KeyCode.H) && m_HUDCanvas.activeSelf)
                {
                    m_HUDCanvas.SetActive(false);
                }
                else if (Input.GetKeyDown(KeyCode.H) && !m_HUDCanvas.activeSelf)
                {
                    m_HUDCanvas.SetActive(true);
                }
                
                // Slow Motion
                if (Input.GetKeyDown(KeyCode.J) && Math.Abs(Time.timeScale - 1.0f) < 0.01f)
                {
                    Time.timeScale = 0.5f;
                }
                else if (Input.GetKeyDown(KeyCode.J) && Math.Abs(Time.timeScale - 0.5f) < 0.01f)
                {
                    Time.timeScale = 1;
                }
            }
        }

        public void Resume ()
        {
            Time.timeScale = 1;
            m_HUDCanvas.SetActive(true);
            m_PauseCanvas.SetActive(false);
            m_DeathScreenCanvas.SetActive(false);
            m_EventSystem.SetActive(false);
            AudioListener.pause = false;
            HideCursor(true);
        }

        public void Pause ()
        {
            Time.timeScale = 0;
            m_HUDCanvas.SetActive(false);
            m_PauseCanvas.SetActive(true);
            m_DeathScreenCanvas.SetActive(false);
            m_EventSystem.SetActive(true);
            AudioListener.pause = true;
            HideCursor(false);
        }

        private void DeathScreen ()
        {
            Time.timeScale = 1;
            m_HUDCanvas.SetActive(false);
            m_PauseCanvas.SetActive(false);
            m_DeathScreenCanvas.SetActive(true);
            m_EventSystem.SetActive(true);

            AudioListener.pause = false;
            HideCursor(false);
        }

        public void Restart ()
        {
            m_Restarting = true;
            if (GameplayManager.Instance.IsDead)
            {
                m_DeathBlackScreen.Show = true;
                Invoke(nameof(LoadLastLevel), 1f);
            }
            else
            {
                Time.timeScale = 1;
                m_PauseBlackScreen.Show = true;
                Invoke(nameof(LoadLastLevel), 1f);
            }
        }

        private void LoadLastLevel ()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Quit ()
        {
            AudioListener.pause = false;
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        private void OnApplicationQuit ()
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
        }

        private void HideCursor (bool hide)
        {
            if (hide)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
