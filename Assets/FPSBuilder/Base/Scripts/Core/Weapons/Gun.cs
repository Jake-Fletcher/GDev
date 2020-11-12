//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using System;
using System.Collections;
using FPSBuilder.Core.Animation;
using FPSBuilder.Core.Input;
using FPSBuilder.Core.Managers;
using FPSBuilder.Core.Player;
using FPSBuilder.Core.Surface;
using FPSBuilder.Interfaces;
using UnityEngine;

namespace FPSBuilder.Core.Weapons
{
    [AddComponentMenu("FPS Builder/Weapon/Gun"), DisallowMultipleComponent]
    public class Gun : MonoBehaviour, IWeapon
    {
        /// <summary>
        /// GunData Asset is a container responsible for defining individual weapon characteristics.
        /// </summary>
        [SerializeField]
        [Required]
        [Tooltip("GunData Asset is a container responsible for defining individual weapon characteristics.")]
        protected GunData m_GunData;

        /// <summary>
        /// Defines the reference to the character’s Main Camera transform.
        /// </summary>
        [SerializeField]
        [Required]
        [Tooltip("Defines the reference to the character’s Main Camera transform.")]
        protected Transform m_CameraTransformReference;

        /// <summary>
        /// Defines the reference to the First Person Character Controller.
        /// </summary>
        [SerializeField]
        [Required]
        [Tooltip("Defines the reference to the First Person Character Controller.")]
        protected FirstPersonCharacterController m_FPController;

        /// <summary>
        /// Defines the reference to the Camera Animator.
        /// </summary>
        [SerializeField]
        [Required]
        [Tooltip("Defines the reference to the Camera Animator.")]
        protected CameraAnimator m_CameraAnimationsController;

        /// <summary>
        /// The WeaponSwing component.
        /// </summary>
        [SerializeField]
        protected WeaponSwing m_WeaponSwing = new WeaponSwing();

        /// <summary>
        /// The MotionAnimation component.
        /// </summary>
        [SerializeField]
        protected MotionAnimation m_MotionAnimation = new MotionAnimation();

        /// <summary>
        /// The GunAnimator component.
        /// </summary>
        [SerializeField]
        protected GunAnimator m_GunAnimator = new GunAnimator();

        /// <summary>
        /// The GunEffects component.
        /// </summary>
        [SerializeField]
        protected GunEffects m_GunEffects = new GunEffects();

        protected bool m_GunActive;
        protected bool m_Aiming;
        protected bool m_IsReloading;
        protected bool m_Attacking;

        protected WaitForSeconds m_ReloadDuration;
        protected WaitForSeconds m_CompleteReloadDuration;

        protected WaitForSeconds m_StartReloadDuration;
        protected WaitForSeconds m_InsertInChamberDuration;
        protected WaitForSeconds m_InsertDuration;
        protected WaitForSeconds m_StopReloadDuration;

        protected float m_FireInterval;
        protected float m_NextFireTime;
        protected float m_NextReloadTime;
        protected float m_NextSwitchModeTime;
        protected float m_NextInteractTime;
        protected float m_Accuracy;

        protected Camera m_Camera;
        protected float m_IsShooting;
        protected Vector3 m_NextShootDirection;

        protected Button m_FireButton;
        protected Button m_AimButton;
        protected Button m_ReloadButton;
        protected Button m_Melee;
        protected Button m_FireMode;

        #region EDITOR

        /// <summary>
        /// Returns the ReloadMode used by this gun.
        /// </summary>
        public GunData.ReloadMode ReloadType => m_GunData != null ? m_GunData.ReloadType : GunData.ReloadMode.Magazines;

        /// <summary>
        /// Returns true if this gun has secondary firing mode, false otherwise.
        /// </summary>
        public bool HasSecondaryMode => m_GunData != null && m_GunData.SecondaryFireMode != GunData.FireMode.None;

        /// <summary>
        /// Returns true if this gun has a chamber, false otherwise.
        /// </summary>
        public bool HasChamber => m_GunData != null && m_GunData.HasChamber;

        /// <summary>
        /// Returns the name of the weapon displayed on the Inspector tab.
        /// </summary>
        public string InspectorName => m_GunData != null ? m_GunData.GunName : "No Name";

        #endregion

        #region GUN PROPERTIES

        /// <summary>
        /// Returns true if this weapon is ready to be replaced, false otherwise.
        /// </summary>
        public virtual bool CanSwitch
        {
            get
            {
                if (!m_FPController)
                    return false;

                return m_GunActive && m_NextSwitchModeTime < Time.time && !m_Attacking && m_NextInteractTime < Time.time && m_NextFireTime < Time.time;
            }
        }

        /// <summary>
        /// Returns true if the character is not performing an action that prevents him from using items, false otherwise.
        /// </summary>
        public virtual bool CanUseItems
        {
            get
            {
                if (!m_FPController)
                    return false;

                return m_GunActive && !IsAiming && !m_IsReloading && m_NextReloadTime < Time.time && m_FPController.State != MotionState.Running
                && m_NextFireTime < Time.time && m_NextSwitchModeTime < Time.time && !m_Attacking && m_NextInteractTime < Time.time;
            }
        }

        /// <summary>
        /// Returns true if the character is not performing an action that prevents him from vaulting, false otherwise.
        /// </summary>
        public virtual bool CanVault
        {
            get
            {
                if (!m_FPController)
                    return false;

                return m_GunActive && !IsAiming && !m_IsReloading && m_NextReloadTime < Time.time && m_NextFireTime < Time.time
                && m_NextSwitchModeTime < Time.time && !m_Attacking && m_NextInteractTime < Time.time;
            }
        }

        /// <summary>
        /// Returns true if the character is aiming with this weapon, false otherwise.
        /// </summary>
        protected virtual bool IsAiming => m_GunAnimator.IsAiming;

        /// <summary>
        /// Returns the maximum number of rounds a magazine can hold.
        /// </summary>
        protected int RoundsPerMagazine
        {
            get;
            set;
        }

        /// <summary>
        /// Returns the current number of rounds that are in the magazine coupled to the gun.
        /// </summary>
        public int CurrentRounds
        {
            get;
            protected set;
        }

        /// <summary>
        /// Returns the number of magazines available to reload this gun.
        /// </summary>
        public int Magazines
        {
            get;
            protected set;
        }

        /// <summary>
        /// Return the weapon identifier.
        /// </summary>
        public int Identifier => m_GunData != null ? m_GunData.GetInstanceID() : -1;

        /// <summary>
        /// Return the name of the gun.
        /// </summary>
        public string GunName => m_GunData != null ? m_GunData.GunName : "No Name";

        /// <summary>
        /// Returns the current accuracy of the gun.
        /// </summary>
        public float Accuracy => m_Accuracy;

        /// <summary>
        /// Returns the selected fire mode on the gun.
        /// </summary>
        public GunData.FireMode FireMode
        {
            get;
            protected set;
        }

        /// <summary>
        /// Returns the viewmodel (GameObject) of this gun.
        /// </summary>
        public GameObject Viewmodel => gameObject;

        /// <summary>
        /// Returns the dropped object when swapping the gun.
        /// </summary>
        public GameObject DroppablePrefab => m_GunData != null ? m_GunData.DroppablePrefab : null;

        /// <summary>
        /// Returns the weight of the gun.
        /// </summary>
        public float Weight => m_GunData != null ? m_GunData.Weight : 0;

        /// <summary>
        /// Returns the length of the gun.
        /// </summary>
        public float Size => m_GunData != null ? m_GunData.Size : 0;

        /// <summary>
        /// 
        /// </summary>
        public float HideAnimationLength => m_GunAnimator.HideAnimationLength;
        public float InteractAnimationLength => m_GunAnimator.InteractAnimationLength;
        public float InteractDelay => m_GunAnimator.InteractDelay;

        public bool Reloading => m_IsReloading || m_NextReloadTime > Time.time;
        public bool Firing => m_NextFireTime > Time.time;
        public bool MeleeAttacking => m_Attacking;
        public bool Interacting => m_NextInteractTime > Time.time;
        public bool Idle => !Reloading && !Firing && !MeleeAttacking && !Interacting;

        public bool CanRefill =>
            CurrentRounds < (m_GunData.HasChamber ? RoundsPerMagazine + 1 : RoundsPerMagazine) ||
            Magazines < m_GunData.MaxMagazines *
            (m_GunData.HasChamber ? RoundsPerMagazine + 1 : RoundsPerMagazine);

        public bool OutOfAmmo => CurrentRounds + Magazines == 0;

        #endregion

        protected virtual void Awake()
        {
            if (!m_GunData)
            {
                throw new Exception("Gun Controller was not assigned");
            }

            if (!m_CameraTransformReference)
            {
                throw new Exception("Camera Transform Reference was not assigned");
            }

            if (!m_CameraAnimationsController)
            {
                throw new Exception("Camera Animations Controller was not assigned");
            }

            if (!m_FPController)
            {
                throw new Exception("FPController was not assigned");
            }
        }

        protected virtual void Start()
        {
            FireMode = m_GunData.PrimaryFireMode;
            m_FireInterval = m_GunData.PrimaryRateOfFire;
            RoundsPerMagazine = m_GunData.RoundsPerMagazine;

            if (CurrentRounds == 0 && Magazines == 0)
                SetAmmo(m_GunData.HasChamber ? RoundsPerMagazine + 1 : RoundsPerMagazine, (m_GunData.HasChamber ? RoundsPerMagazine + 1 : RoundsPerMagazine) * m_GunData.InitialMagazines);

            if (m_MotionAnimation.Lean)
                m_MotionAnimation.LeanAmount = 0;

            switch (m_GunData.ReloadType)
            {
                case GunData.ReloadMode.Magazines:
                m_ReloadDuration = new WaitForSeconds(m_GunAnimator.ReloadAnimationLength);
                m_CompleteReloadDuration = new WaitForSeconds(m_GunAnimator.FullReloadAnimationLength);
                break;
                case GunData.ReloadMode.BulletByBullet:
                m_StartReloadDuration = new WaitForSeconds(m_GunAnimator.StartReloadAnimationLength);
                m_InsertInChamberDuration = new WaitForSeconds(m_GunAnimator.InsertInChamberAnimationLength / 2);
                m_InsertDuration = new WaitForSeconds(m_GunAnimator.InsertAnimationLength / 2);
                m_StopReloadDuration = new WaitForSeconds(m_GunAnimator.StopReloadAnimationLength);
                break;
                default:
                throw new ArgumentOutOfRangeException();
            }

            InitSwing(transform);
            DisableShadowCasting();

            m_FireButton = InputManager.FindButton("Fire");
            m_AimButton = InputManager.FindButton("Aim");
            m_ReloadButton = InputManager.FindButton("Reload");
            m_Melee = InputManager.FindButton("Melee");
            m_FireMode = InputManager.FindButton("Fire Mode");

            m_FPController.PreJumpEvent += WeaponJump;
            m_FPController.LandingEvent += WeaponLanding;
            m_FPController.VaultEvent += Vault;
            m_FPController.GettingUpEvent += GettingUp;
        }

        protected virtual void Update()
        {
            if (m_GunActive)
            {
                m_CameraAnimationsController.HoldBreath = m_GunAnimator.CanHoldBreath;
                m_FPController.IsAiming = m_GunAnimator.IsAiming;
                m_FPController.ReadyToVault = CanVault;
                m_IsShooting = Mathf.MoveTowards(m_IsShooting, 0, Time.deltaTime);

                if (m_FPController.Controllable)
                    HandleInput();
            }

            if (m_Aiming)
            {
                m_GunAnimator.Aim(true);
            }
            else
            {
                bool canSprint = m_FPController.State == MotionState.Running && !m_IsReloading && m_NextReloadTime < Time.time
                                 && m_NextSwitchModeTime < Time.time && m_NextFireTime < Time.time && m_GunActive && !m_Attacking
                                 && m_NextInteractTime < Time.time && !m_FPController.IsSliding;

                m_GunAnimator.Sprint(canSprint, m_FPController.IsSliding);
            }

            m_Accuracy = Mathf.Clamp(Mathf.MoveTowards(m_Accuracy, GetCurrentAccuracy(), Time.deltaTime *
                                    (m_IsShooting > 0 ? m_GunData.DecreaseRateByShooting : m_GunData.DecreaseRateByWalking)),
                                    m_GunData.BaseAccuracy, m_GunData.AIMAccuracy);

            bool canLean = !m_Attacking && m_FPController.State != MotionState.Running && m_NextInteractTime < Time.time
                                        && m_CameraAnimationsController != null && m_MotionAnimation.Lean;

            m_MotionAnimation.LeanAnimation(canLean ? m_CameraAnimationsController.LeanDirection : 0);

            m_WeaponSwing.Swing(transform.parent, m_FPController);
            m_MotionAnimation.MovementAnimation(m_FPController);
            m_MotionAnimation.BreathingAnimation(m_FPController.IsAiming ? 0 : 1);
        }

        protected virtual float GetCurrentAccuracy()
        {
            if (m_GunAnimator.IsAiming)
            {
                if (m_IsShooting > 0)
                    return m_GunData.HIPAccuracy;

                return m_FPController.State != MotionState.Idle ? m_GunData.HIPAccuracy : m_GunData.AIMAccuracy;
            }
            if (m_IsShooting > 0)
                return m_GunData.BaseAccuracy;

            return m_FPController.State != MotionState.Idle ? m_GunData.BaseAccuracy : m_GunData.HIPAccuracy;
        }

        public virtual void Select()
        {
            if (!m_GunAnimator.Initialized)
            {
                m_Camera = m_CameraTransformReference.GetComponent<Camera>();
                m_GunAnimator.Init(transform, m_Camera);
            }

            m_GunAnimator.Draw();
            StartCoroutine(Draw());
        }

        protected virtual IEnumerator Draw()
        {
            yield return new WaitForSeconds(m_GunAnimator.DrawAnimationLength);
            m_GunActive = true;
        }

        public virtual void Deselect()
        {
            m_GunActive = false;
            m_Aiming = false;
            m_IsReloading = false;
            m_NextReloadTime = 0;
            m_FPController.IsAiming = false;
            m_FPController.ReadyToVault = false;
            m_IsShooting = 0;
            m_GunAnimator.Hide();
        }

        protected virtual void HandleInput()
        {
            // Restrictions:
            // Is firing = m_NextFireTime > Time.time
            // Is reloading = m_IsReloading || m_NextReloadTime > Time.time
            // Is empty = CurrentRounds == 0
            // Is running = m_FPController.State == MotionState.Running
            // Is attacking = m_Attacking
            // Is switching mode = m_NextSwitchModeTime > Time.time
            // Is interacting = m_NextInteractTime > Time.time
            // Can reload = Magazines > 0

            bool canShoot = !m_IsReloading && m_NextReloadTime < Time.time && m_NextFireTime < Time.time && CurrentRounds >= 0
                            && (m_FPController.State != MotionState.Running || m_FPController.IsSliding) && !m_Attacking
                            && m_NextSwitchModeTime < Time.time && m_NextInteractTime < Time.time;

            if (canShoot)
            {
                if (FireMode == GunData.FireMode.FullAuto || FireMode == GunData.FireMode.ShotgunAuto)
                {
                    if (InputManager.GetButton(m_FireButton))
                    {
                        if (CurrentRounds == 0 && Magazines > 0)
                        {
                            Reload();
                        }
                        else
                        {
                            PullTheTrigger();
                        }
                    }
                }
                else if (FireMode == GunData.FireMode.Single || FireMode == GunData.FireMode.ShotgunSingle || FireMode == GunData.FireMode.Burst)
                {
                    if (InputManager.GetButtonDown(m_FireButton))
                    {
                        if (CurrentRounds == 0 && Magazines > 0)
                        {
                            Reload();
                        }
                        else
                        {
                            PullTheTrigger();
                        }
                    }
                }
            }

            if (m_GunData.ReloadType == GunData.ReloadMode.BulletByBullet && m_IsReloading && m_NextReloadTime < Time.time && CurrentRounds > (m_GunData.HasChamber ? 1 : 0))
            {
                if (InputManager.GetButtonDown(m_FireButton))
                {
                    m_IsReloading = false;
                    StartCoroutine(StopReload());
                }
            }

            bool canAim = !m_IsReloading && m_NextReloadTime < Time.time && m_FPController.State != MotionState.Running && !m_Attacking && m_NextInteractTime < Time.time;

            if (canAim)
            {
                if (GameplayManager.Instance.AimStyle == ActionMode.Toggle)
                {
                    if (InputManager.GetButtonDown(m_AimButton) && !m_Aiming)
                    {
                        m_Aiming = !m_Aiming;
                    }
                    else if (InputManager.GetButtonDown(m_AimButton) && m_Aiming && m_GunAnimator.IsAiming)
                    {
                        m_Aiming = !m_Aiming;
                    }
                }
                else
                {
                    m_Aiming = InputManager.GetButton(m_AimButton);
                }
            }
            else
            {
                m_Aiming = false;
            }

            bool canReload = !m_IsReloading && m_NextReloadTime < Time.time && CurrentRounds < (m_GunData.HasChamber ? RoundsPerMagazine + 1 : RoundsPerMagazine) && Magazines > 0
                             && !m_Attacking && m_NextSwitchModeTime < Time.time && m_NextInteractTime < Time.time && m_NextFireTime < Time.time;

            if (canReload)
            {
                if (InputManager.GetButtonDown(m_ReloadButton))
                {
                    Reload();
                }
            }

            bool canAttack = !m_Attacking && !m_IsReloading && m_NextReloadTime < Time.time && m_FPController.State != MotionState.Running && !IsAiming
                             && m_NextFireTime < Time.time && m_GunAnimator.CanMeleeAttack && m_NextSwitchModeTime < Time.time && m_NextInteractTime < Time.time;

            if (canAttack)
            {
                if (InputManager.GetButtonDown(m_Melee))
                {
                    StartCoroutine(MeleeAttack());
                }
            }

            bool canChangeFireMode = HasSecondaryMode && !m_Attacking && !m_IsReloading
                                     && m_NextReloadTime < Time.time && m_FPController.State != MotionState.Running && m_NextSwitchModeTime < Time.time
                                     && m_NextInteractTime < Time.time;

            if (canChangeFireMode)
            {
                if (InputManager.GetButtonDown(m_FireMode))
                {
                    if (FireMode == m_GunData.PrimaryFireMode)
                    {
                        m_NextSwitchModeTime = Time.time + m_GunAnimator.SwitchModeAnimationLength;
                        m_GunAnimator.SwitchMode();

                        FireMode = m_GunData.SecondaryFireMode;
                        m_FireInterval = m_GunData.SecondaryRateOfFire;
                    }
                    else
                    {
                        m_NextSwitchModeTime = Time.time + m_GunAnimator.SwitchModeAnimationLength;
                        m_GunAnimator.SwitchMode();

                        FireMode = m_GunData.PrimaryFireMode;
                        m_FireInterval = m_GunData.PrimaryRateOfFire;
                    }
                }
            }
        }

        protected virtual void PullTheTrigger()
        {
            if (CurrentRounds > 0 && Magazines >= 0)
            {
                if (FireMode == GunData.FireMode.FullAuto || FireMode == GunData.FireMode.Single)
                {
                    m_NextFireTime = Time.time + m_FireInterval;
                    CurrentRounds--;

                    m_NextShootDirection = GetBulletSpread();
                    Shot();

                    m_IsShooting = 0.1f;

                    m_GunAnimator.Shot(CurrentRounds == 0);
                    m_GunEffects.Play();

                    StartCoroutine(m_MotionAnimation.RecoilAnimation(false));

                    if (m_CameraAnimationsController)
                    {
                        m_CameraAnimationsController.ApplyRecoil(m_MotionAnimation.CameraRecoil, m_MotionAnimation.MinCameraRecoilRotation, m_MotionAnimation.MaxCameraRecoilRotation,
                                                                 m_MotionAnimation.CameraRecoilDuration, m_MotionAnimation.CameraReturnDuration);
                    }
                }
                else if (FireMode == GunData.FireMode.ShotgunAuto || FireMode == GunData.FireMode.ShotgunSingle)
                {
                    m_NextFireTime = Time.time + m_FireInterval;
                    CurrentRounds--;

                    for (int i = 0; i < m_GunData.BulletsPerShoot; i++)
                    {
                        m_NextShootDirection = GetBulletSpread();
                        Shot();
                    }

                    m_IsShooting = 0.1f;

                    m_GunAnimator.Shot(CurrentRounds == 0);
                    m_GunEffects.Play();

                    StartCoroutine(m_MotionAnimation.RecoilAnimation(false));

                    if (m_CameraAnimationsController != null)
                    {
                        m_CameraAnimationsController.ApplyRecoil(m_MotionAnimation.CameraRecoil, m_MotionAnimation.MinCameraRecoilRotation, m_MotionAnimation.MaxCameraRecoilRotation,
                                                                 m_MotionAnimation.CameraRecoilDuration, m_MotionAnimation.CameraReturnDuration);
                    }

                }
                else if (FireMode == GunData.FireMode.Burst)
                {
                    m_NextFireTime = Time.time + m_FireInterval * (m_GunData.BulletsPerBurst + 1);
                    StartCoroutine(Burst());
                }
            }
            else
            {
                m_NextFireTime = Time.time + 0.25f;
                m_GunAnimator.OutOfAmmo();
            }
        }

        protected virtual IEnumerator Burst()
        {
            for (int i = 0; i < m_GunData.BulletsPerBurst; i++)
            {
                if (CurrentRounds == 0)
                    break;

                m_NextShootDirection = GetBulletSpread();
                CurrentRounds--;
                Shot();

                m_IsShooting = 0.1f;

                m_GunAnimator.Shot(CurrentRounds == 0);
                m_GunEffects.Play();

                StartCoroutine(m_MotionAnimation.RecoilAnimation(false));

                if (m_CameraAnimationsController)
                {
                    m_CameraAnimationsController.ApplyRecoil(m_MotionAnimation.CameraRecoil, m_MotionAnimation.MinCameraRecoilRotation, m_MotionAnimation.MaxCameraRecoilRotation,
                                                             m_MotionAnimation.CameraRecoilDuration, m_MotionAnimation.CameraReturnDuration);
                }
                yield return new WaitForSeconds(m_FireInterval);
            }
        }

        protected virtual void Shot()
        {
            Vector3 direction = m_CameraTransformReference.TransformDirection(m_NextShootDirection);
            Vector3 origin = m_CameraTransformReference.transform.position;

            Ray ray = new Ray(origin, direction);

            float tracerDuration = m_GunEffects.TracerDuration;

            if (Physics.Raycast(ray, out RaycastHit hitInfo, m_GunData.Range, m_GunData.AffectedLayers, QueryTriggerInteraction.Collide))
            {
                SurfaceIdentifier surf = hitInfo.collider.GetSurface();
                float damage = m_GunData.DamageType == GunData.DamageMode.Constant ? m_GunData.Damage
                    : m_GunData.Damage * m_GunData.DamageFalloffCurve.Evaluate(hitInfo.distance / m_GunData.Range);

                if (surf)
                {
                    BulletDecalsManager.Instance.CreateBulletDecal(surf, hitInfo);

                    if (m_GunData.PenetrateObjects && surf.CanPenetrate(hitInfo.triangleIndex))
                    {
                        Penetrate(hitInfo, direction, surf, m_GunData.Range - hitInfo.distance, damage);
                    }
                }

                // If hit a rigid body applies force to push.
                Rigidbody rigidBody = hitInfo.collider.GetComponent<Rigidbody>();
                if (rigidBody)
                {
                    rigidBody.AddForce(direction * m_GunData.Force, ForceMode.Impulse);
                }

                if (hitInfo.transform.root != transform.root)
                {
                    IProjectileDamageable damageableTarget = hitInfo.collider.GetComponent<IProjectileDamageable>();
                    damageableTarget?.ProjectileDamage(damage, transform.root.position, hitInfo.point, m_GunData.PenetrationPower);
                }

                tracerDuration = hitInfo.distance / m_GunEffects.TracerSpeed;
            }

            if (tracerDuration > 0.05f)
                m_GunEffects.CreateTracer(transform, direction, tracerDuration);
        }

        protected virtual void Penetrate(RaycastHit lastHitInfo, Vector3 direction, SurfaceIdentifier surf, float range, float damage)
        {
            Ray ray = new Ray(lastHitInfo.point + direction * 0.1f, direction);

            int affectedObjectID = lastHitInfo.collider.GetInstanceID();

            if (Physics.Raycast(ray, out RaycastHit hitInfo, range, m_GunData.AffectedLayers, QueryTriggerInteraction.Collide))
            {
                // Get the surface type of the object.
                SurfaceIdentifier newSurf = hitInfo.collider.GetSurface();

                // Exit hole
                Ray exitRay = new Ray(hitInfo.point, direction * -1);

                if (Physics.Raycast(exitRay, out RaycastHit exitInfo, range, m_GunData.AffectedLayers, QueryTriggerInteraction.Collide))
                {
                    float distanceTraveled = Vector3.Distance(lastHitInfo.point, exitInfo.point) * surf.Density(lastHitInfo.triangleIndex);

                    // Does the bullet gets through?
                    if (m_GunData.PenetrationPower > distanceTraveled)
                    {
                        if (newSurf)
                            BulletDecalsManager.Instance.CreateBulletDecal(newSurf, hitInfo);

                        if (affectedObjectID == exitInfo.collider.GetInstanceID())
                            BulletDecalsManager.Instance.CreateBulletDecal(surf, exitInfo);

                        // If hit a rigid body applies force to push.
                        Rigidbody rigidBody = hitInfo.collider.GetComponent<Rigidbody>();
                        if (rigidBody)
                        {
                            rigidBody.AddForce(direction * m_GunData.Force, ForceMode.Impulse);
                        }

                        if (hitInfo.transform.root != transform.root)
                        {
                            IProjectileDamageable damageableTarget = hitInfo.collider.GetComponent<IProjectileDamageable>();
                            damageableTarget?.ProjectileDamage(damage * (distanceTraveled / m_GunData.PenetrationPower), transform.root.position, exitInfo.point, m_GunData.PenetrationPower - distanceTraveled);
                        }
                    }
                }
            }
            else
            {
                // Exit hole
                Ray exitRay = new Ray(lastHitInfo.point + direction * m_GunData.PenetrationPower, direction * -1);

                if (Physics.Raycast(exitRay, out RaycastHit exitInfo, m_GunData.PenetrationPower, m_GunData.AffectedLayers, QueryTriggerInteraction.Collide))
                {
                    if (affectedObjectID == exitInfo.collider.GetInstanceID())
                        BulletDecalsManager.Instance.CreateBulletDecal(surf, exitInfo);
                }
            }
        }

        protected virtual Vector3 GetBulletSpread()
        {
            if (Mathf.Abs(m_Accuracy - 1) < Mathf.Epsilon)
            {
                return new Vector3(0, 0, 1);
            }
            else
            {
                Vector2 randomPointInScreen = new Vector2(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f)) * ((1 - m_Accuracy) * (m_GunData.MaximumSpread / 10));
                return new Vector3(randomPointInScreen.x, randomPointInScreen.y, 1);
            }
        }

        protected virtual IEnumerator MeleeAttack()
        {
            m_Attacking = true;
            m_GunAnimator.Melee();
            yield return new WaitForSeconds(m_GunAnimator.MeleeDelay);

            Vector3 direction = m_CameraTransformReference.TransformDirection(Vector3.forward);
            Vector3 origin = m_CameraTransformReference.transform.position;
            float range = m_GunData.Size * 0.5f + m_FPController.Radius;

            Ray ray = new Ray(origin, direction);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, range, m_GunData.AffectedLayers, QueryTriggerInteraction.Collide))
            {
                m_GunAnimator.Hit(hitInfo.point);

                // If hit a rigidbody applies force to push.
                Rigidbody rigidBody = hitInfo.collider.GetComponent<Rigidbody>();
                if (rigidBody != null)
                {
                    rigidBody.AddForce(direction * m_GunData.MeleeForce, ForceMode.Impulse);
                }

                if (hitInfo.transform.root != transform.root)
                {
                    IProjectileDamageable damageableTarget = hitInfo.collider.GetComponent<IProjectileDamageable>();
                    if (damageableTarget != null)
                    {
                        damageableTarget.ProjectileDamage(m_GunData.MeleeDamage, transform.root.position, hitInfo.point, 0);
                    }
                }
            }

            yield return new WaitForSeconds(m_GunAnimator.MeleeAnimationLength - m_GunAnimator.MeleeDelay);
            m_Attacking = false;
        }

        #region RELOAD METHODS

        protected virtual void Reload()
        {
            if (m_GunData.ReloadType == GunData.ReloadMode.Magazines)
            {
                StartCoroutine(ReloadMagazines());

                if (CurrentRounds == 0 && m_GunEffects.FullReloadDrop)
                {
                    Invoke(nameof(DropMagazinePrefab), m_GunEffects.FullDropDelay);
                }
                else if (CurrentRounds > 0 && m_GunEffects.TacticalReloadDrop)
                {
                    Invoke(nameof(DropMagazinePrefab), m_GunEffects.TacticalDropDelay);
                }
            }
            else if (m_GunData.ReloadType == GunData.ReloadMode.BulletByBullet)
            {
                StartCoroutine(ReloadBulletByBullet());
            }
        }

        protected virtual IEnumerator ReloadMagazines()
        {
            m_IsReloading = true;

            m_GunAnimator.Reload(CurrentRounds > 0);

            yield return CurrentRounds == 0 ? m_CompleteReloadDuration : m_ReloadDuration;

            if (m_GunActive && m_IsReloading)
            {
                if (CurrentRounds > 0)
                {
                    if (CurrentRounds + Magazines > (m_GunData.HasChamber ? RoundsPerMagazine + 1 : RoundsPerMagazine))
                    {
                        Magazines -= (m_GunData.HasChamber ? RoundsPerMagazine + 1 : RoundsPerMagazine) - CurrentRounds;
                        CurrentRounds = (m_GunData.HasChamber ? RoundsPerMagazine + 1 : RoundsPerMagazine);
                    }
                    else
                    {
                        CurrentRounds += Magazines;
                        Magazines = 0;
                    }
                }
                else
                {
                    if (CurrentRounds + Magazines > RoundsPerMagazine)
                    {
                        Magazines -= RoundsPerMagazine - CurrentRounds;
                        CurrentRounds = RoundsPerMagazine;
                    }
                    else
                    {
                        CurrentRounds += Magazines;
                        Magazines = 0;
                    }
                }
            }

            m_IsReloading = false;
        }

        protected virtual IEnumerator ReloadBulletByBullet()
        {
            m_IsReloading = true;

            m_GunAnimator.StartReload(CurrentRounds > 0);

            if (CurrentRounds == 0)
            {
                yield return m_InsertInChamberDuration;
                CurrentRounds++;
                Magazines--;
                yield return m_InsertInChamberDuration;
            }
            else
            {
                yield return m_StartReloadDuration;
            }

            while (m_GunActive && (CurrentRounds < (m_GunData.HasChamber ? RoundsPerMagazine + 1 : RoundsPerMagazine) && Magazines > 0) && m_IsReloading)
            {
                m_GunAnimator.Insert();
                yield return m_InsertDuration;

                if (m_GunActive && m_IsReloading)
                {
                    CurrentRounds++;
                    Magazines--;
                }
                yield return m_InsertDuration;
            }

            if (m_GunActive && m_IsReloading)
            {
                StartCoroutine(StopReload());
            }
        }

        protected virtual IEnumerator StopReload()
        {
            m_GunAnimator.StopReload();
            m_IsReloading = false;
            m_NextReloadTime = m_GunAnimator.StopReloadAnimationLength + Time.time;
            yield return m_StopReloadDuration;
        }

        protected virtual void DropMagazinePrefab()
        {
            m_GunEffects.DropMagazine(m_FPController.GetComponent<Collider>());
        }

        #endregion

        #region ANIMATIONS

        protected void InitSwing(Transform weaponSwing)
        {
            if (!weaponSwing.parent.name.Equals("WeaponSwing"))
            {
                Transform parent = weaponSwing.parent.Find("WeaponSwing");

                if (parent != null)
                {
                    weaponSwing.parent = parent;
                }
                else
                {
                    GameObject weaponController = new GameObject("WeaponSwing");
                    weaponController.transform.SetParent(weaponSwing.parent, false);
                    weaponSwing.parent = weaponController.transform;
                }
            }
            m_WeaponSwing.Init(weaponSwing.parent, m_MotionAnimation.ScaleFactor);
        }

        protected void WeaponJump()
        {
            if (m_GunActive)
                StartCoroutine(m_MotionAnimation.JumpAnimation.Play());
        }

        protected void WeaponLanding(float fallDamage)
        {
            if (m_GunActive)
                StartCoroutine(m_MotionAnimation.LandingAnimation.Play());
        }

        #endregion

        internal virtual void Refill()
        {
            if (RoundsPerMagazine == 0)
                RoundsPerMagazine = m_GunData.RoundsPerMagazine;

            SetAmmo(m_GunData.HasChamber ? RoundsPerMagazine + 1 : RoundsPerMagazine, m_GunData.MaxMagazines * (m_GunData.HasChamber ? RoundsPerMagazine + 1 : RoundsPerMagazine));
        }

        protected virtual void SetAmmo(int currentRounds, int magazines)
        {
            CurrentRounds = Mathf.Max(0, Mathf.Min(currentRounds, m_GunData.HasChamber ? RoundsPerMagazine + 1 : RoundsPerMagazine));
            Magazines = Mathf.Max(0, Mathf.Min(magazines, m_GunData.MaxMagazines * (m_GunData.HasChamber ? RoundsPerMagazine + 1 : RoundsPerMagazine)));
        }

        public void DisableShadowCasting()
        {
            // For each object that has a renderer inside the weapon gameObject
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
            {
                r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }
        }

        public virtual void Interact()
        {
            m_NextInteractTime = Time.time + Mathf.Max(InteractAnimationLength, InteractDelay);
            m_GunAnimator.Interact();
        }

        protected virtual void Vault()
        {
            if (m_GunActive)
                m_GunAnimator.Vault();
        }

        protected virtual void GettingUp()
        {
            if (m_GunActive && CanVault)
                m_GunAnimator.Vault();
        }

        #region WEAPON CUSTOMIZATION

        internal virtual void UpdateAiming(Vector3 aimingPosition, Vector3 aimingRotation, bool zoomAnimation = false, float aimFOV = 50)
        {
            m_GunAnimator.UpdateAiming(aimingPosition, aimingRotation, zoomAnimation, aimFOV);
        }

        internal virtual void UpdateFireSound(AudioClip[] fireSoundList)
        {
            m_GunAnimator.UpdateFireSound(fireSoundList);
        }

        internal virtual void UpdateMuzzleFlash(ParticleSystem muzzleParticle)
        {
            m_GunEffects.UpdateMuzzleBlastParticle(muzzleParticle);
        }

        internal virtual void UpdateRecoil(Vector3 minCameraRecoilRotation, Vector3 maxCameraRecoilRotation)
        {
            m_MotionAnimation.MinCameraRecoilRotation = minCameraRecoilRotation;
            m_MotionAnimation.MaxCameraRecoilRotation = maxCameraRecoilRotation;
        }

        internal virtual void UpdateRoundsPerMagazine(int roundsPerMagazine)
        {
            RoundsPerMagazine = roundsPerMagazine;
        }

        #endregion
    }
}
