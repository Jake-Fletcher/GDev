//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using FPSBuilder.Core.Weapons;
using FPSBuilder.Interfaces;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS0649

namespace FPSBuilder.UI
{
    public class WeaponUI : MonoBehaviour
    {
        [System.Serializable]
        public struct WeaponIcon
        {
            [Required]
            public GunData gunData;

            [Required]
            public Sprite icon;
        }

        [SerializeField]
        private MenuController m_MenuController;

        [Header("Weapon")]

        [SerializeField]
        private Text m_WeaponName;
    
        [SerializeField] 
        private Text m_CurrentAmmo;

        [SerializeField]
        private Text m_Magazines;

        [SerializeField]
        private Image m_WeaponIcon;

        [SerializeField]
        private Text m_Pickup;

        [SerializeField]
        private WeaponIcon[] m_WeaponIcons;

        [SerializeField]
        private Sprite m_ArmsIcon;
    
        [SerializeField]
        private GameObject m_KeyButton;
    
        [SerializeField] 
        private Text m_UseKey;

        [Header("Crosshair")]
    
        [SerializeField]
        private float m_CrosshairAperture = 192;

        [SerializeField]
        private RectTransform m_Up;

        [SerializeField]
        private RectTransform m_Down;

        [SerializeField]
        private RectTransform m_Right;

        [SerializeField]
        private RectTransform m_Left;

        [Header("Crosshair Shotgun")]

        [SerializeField]
        private RectTransform m_ShotgunUp;

        [SerializeField]
        private RectTransform m_ShotgunDown;

        [SerializeField]
        private RectTransform m_ShotgunRight;

        [SerializeField]
        private RectTransform m_ShotgunLeft;

        [Header("Health")]

        [SerializeField]
        private Image m_Health;

        [SerializeField]
        private Image m_HealthIcon;

        [SerializeField]
        private Color m_NormalColor = Color.white;

        [SerializeField]
        private Color m_BloodLossColor = Color.red;

        [SerializeField]
        private Image m_Stamina;

        [Header("Grenades")]

        [SerializeField]
        private GameObject m_GrenadeUI;

        [SerializeField]
        private Text m_GrenadesAmount;

        [Header("Adrenaline")]

        [SerializeField]
        private GameObject m_AdrenalineUI;

        [SerializeField]
        private Text m_AdrenalineAmount;

        private void Start ()
        {
            InvokeRepeating(nameof(UpdatePickupMessage), 0, 0.1f);
        }

        private void Update ()
        {
            m_UseKey.text = m_MenuController.UseKey;
            m_Stamina.fillAmount = m_MenuController.FirstPersonCharacter.StaminaPercent;
            m_Health.fillAmount = m_MenuController.HealthController.HealthPercent;

            float amount = Mathf.PingPong(Time.time / (Mathf.Clamp(m_MenuController.HealthController.HealthPercent, 0.35f, 0.8f)), 1);
            float old_amount = m_HealthIcon.fillAmount;
            m_HealthIcon.fillOrigin = amount > old_amount ? (int)Image.OriginHorizontal.Left : (int)Image.OriginHorizontal.Right;
            m_HealthIcon.fillAmount = amount;

            m_Health.color = Color.Lerp(m_Health.color, m_MenuController.HealthController.Bleeding ? m_BloodLossColor : m_NormalColor, Time.deltaTime * 3);
            m_WeaponName.text = m_MenuController.WeaponManager.GunName + "\n" + m_MenuController.WeaponManager.FireMode;
            m_CurrentAmmo.text = m_MenuController.WeaponManager.CurrentAmmo != -1 ? m_MenuController.WeaponManager.CurrentAmmo.ToString() : "";
            m_Magazines.text = m_MenuController.WeaponManager.Magazines != -1 ? m_MenuController.WeaponManager.Magazines.ToString() : "";

            m_WeaponIcon.sprite = GetWeaponIcon(m_MenuController.WeaponManager.GunID);

            m_Up.gameObject.SetActive(!m_MenuController.FirstPersonCharacter.IsAiming && m_MenuController.WeaponManager.Accuracy != -1 && !m_MenuController.WeaponManager.IsShotgun);
            m_Down.gameObject.SetActive(!m_MenuController.FirstPersonCharacter.IsAiming && m_MenuController.WeaponManager.Accuracy != -1 && !m_MenuController.WeaponManager.IsShotgun);
            m_Right.gameObject.SetActive(!m_MenuController.FirstPersonCharacter.IsAiming && m_MenuController.WeaponManager.Accuracy != -1 && !m_MenuController.WeaponManager.IsShotgun);
            m_Left.gameObject.SetActive(!m_MenuController.FirstPersonCharacter.IsAiming && m_MenuController.WeaponManager.Accuracy != -1 && !m_MenuController.WeaponManager.IsShotgun);

            m_ShotgunUp.gameObject.SetActive(!m_MenuController.FirstPersonCharacter.IsAiming && m_MenuController.WeaponManager.Accuracy != -1 && m_MenuController.WeaponManager.IsShotgun);
            m_ShotgunDown.gameObject.SetActive(!m_MenuController.FirstPersonCharacter.IsAiming && m_MenuController.WeaponManager.Accuracy != -1 && m_MenuController.WeaponManager.IsShotgun);
            m_ShotgunRight.gameObject.SetActive(!m_MenuController.FirstPersonCharacter.IsAiming && m_MenuController.WeaponManager.Accuracy != -1 && m_MenuController.WeaponManager.IsShotgun);
            m_ShotgunLeft.gameObject.SetActive(!m_MenuController.FirstPersonCharacter.IsAiming && m_MenuController.WeaponManager.Accuracy != -1 && m_MenuController.WeaponManager.IsShotgun);

            if (m_MenuController.WeaponManager.Accuracy != -1)
            {
                if (m_MenuController.WeaponManager.IsShotgun)
                {
                    m_ShotgunUp.localPosition = new Vector3(0, m_CrosshairAperture * (1 - m_MenuController.WeaponManager.Accuracy));
                    m_ShotgunDown.localPosition = new Vector3(0, -m_CrosshairAperture * (1 - m_MenuController.WeaponManager.Accuracy));
                    m_ShotgunRight.localPosition = new Vector3(m_CrosshairAperture * (1 - m_MenuController.WeaponManager.Accuracy), 0);
                    m_ShotgunLeft.localPosition = new Vector3(-m_CrosshairAperture * (1 - m_MenuController.WeaponManager.Accuracy), 0);
                }
                else
                {
                    m_Up.localPosition = new Vector3(0, m_CrosshairAperture * (1 - m_MenuController.WeaponManager.Accuracy));
                    m_Down.localPosition = new Vector3(0, -m_CrosshairAperture * (1 - m_MenuController.WeaponManager.Accuracy));
                    m_Right.localPosition = new Vector3(m_CrosshairAperture * (1 - m_MenuController.WeaponManager.Accuracy), 0);
                    m_Left.localPosition = new Vector3(-m_CrosshairAperture * (1 - m_MenuController.WeaponManager.Accuracy), 0);
                }
            }

            if(m_MenuController.WeaponManager.Target == null)
                HidePickupMessage();

            if (m_MenuController.Grenade != null && m_MenuController.Grenade.Amount > 0)
            {
                m_GrenadeUI.SetActive(true);
                m_GrenadesAmount.text = m_MenuController.Grenade.Amount.ToString();
            }
            else
            {
                m_GrenadeUI.SetActive(false);
            }

            if (m_MenuController.Adrenaline != null && m_MenuController.Adrenaline.Amount > 0)
            {
                m_AdrenalineUI.SetActive(true);
                m_AdrenalineAmount.text = m_MenuController.Adrenaline.Amount.ToString();
            }
            else
            {
                m_AdrenalineUI.SetActive(false);
            }
        }

        private Sprite GetWeaponIcon (int id)
        {
            for (int i = 0; i < m_WeaponIcons.Length; i++)
            {
                if (m_WeaponIcons[i].gunData == null)
                    continue;

                if (m_WeaponIcons[i].gunData.GetInstanceID() == id)
                    return m_WeaponIcons[i].icon;
            }
            return m_ArmsIcon;
        }

        private void UpdatePickupMessage ()
        {
            if (m_MenuController.WeaponManager.Target != null)
            {
                if (m_MenuController.WeaponManager.CanSwitch)
                {
                    GunPickup gunPickup = m_MenuController.WeaponManager.Target.GetComponent<GunPickup>();

                    if (gunPickup != null)
                    {
                        IWeapon weapon = m_MenuController.WeaponManager.GetWeaponByID(gunPickup.ID);
                        if (weapon != null)
                        {
                            if (!m_MenuController.WeaponManager.IsEquipped(weapon))
                            {
                                if (m_MenuController.WeaponManager.HasFreeSlot)
                                {
                                    m_KeyButton.SetActive(true);
                                    ShowPickupMessage("TO PICK UP <color=#FCB628>" + ((Gun)weapon ).InspectorName + "</color>");
                                }
                                else
                                {
                                    m_KeyButton.SetActive(true);
                                    ShowPickupMessage("TO SWAP <color=#FCB628>" + m_MenuController.WeaponManager.GunName + "</color> FOR THE <color=#FCB628>" + ((Gun) weapon).InspectorName + "</color>");
                                }
                            }
                            else
                            {
                                m_KeyButton.SetActive(false);
                                ShowPickupMessage("<color=#FCB628>" + ((Gun) weapon).InspectorName + "</color> ALREADY EQUIPPED");
                            }
                        }
                    }
                    else
                    {
                        HidePickupMessage();
                    }
                
                    IActionable target = m_MenuController.WeaponManager.Target.GetComponent<IActionable>();
                
                    if (target != null)
                    {
                        m_KeyButton.SetActive(true);
                        ShowPickupMessage("TO " + target.Message());
                    }
                
                    if (m_MenuController.WeaponManager.GunID != -1)
                    {
                        if (m_MenuController.WeaponManager.Target.CompareTag(m_MenuController.WeaponManager.AmmoTag))
                        {
                            m_KeyButton.SetActive(true);
                            ShowPickupMessage("TO REFILL <color=#FCB628>AMMO</color>");
                        }

                        if (m_MenuController.WeaponManager.Target.CompareTag(m_MenuController.WeaponManager
                            .AdrenalinePackTag) && m_MenuController.Adrenaline.CanRefill)
                        {
                            m_KeyButton.SetActive(true);
                            ShowPickupMessage("TO PICK UP <color=#FCB628>ADRENALINE SHOTS</color>");
                        }
                    }

                    if (!m_MenuController.WeaponManager.Target.CompareTag(m_MenuController.WeaponManager.AmmoTag)
                        && !m_MenuController.WeaponManager.Target.CompareTag(m_MenuController.WeaponManager
                            .AdrenalinePackTag)
                        && target == null && gunPickup == null)
                    {
                        HidePickupMessage();
                    }
                }
            }
            else
            {
                HidePickupMessage();
            }
        }

        public void ShowPickupMessage (string message)
        {
            m_Pickup.gameObject.SetActive(true);
            m_Pickup.text = message;
        }

        public void HidePickupMessage ()
        {
            m_KeyButton.SetActive(false);
            m_Pickup.gameObject.SetActive(false);
        }
    }
}
