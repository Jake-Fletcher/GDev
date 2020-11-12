//=========== Copyright (c) GameBuilders, All rights reserved. ================
//
// Purpose: The GunData Asset is a robust and straightforward tool that is responsible for defining individual weapon characteristics. 
//          It handles weapon settings all in one place and effectively communicates with the Gun script creating seamless weapon tweaking.
//          Anything related with weapon performance such as: Shooting settings, Magazine Settings, Accuracy settings,
//          and even characteristics of a weapon such as weight and melee force is handled within a gun's respective GunData Asset.
//
//=============================================================================

using UnityEngine;

#pragma warning disable CS0649

namespace FPSBuilder.Core.Weapons
{
    /// <summary>
    /// GunData Asset is a container responsible for defining individual weapon characteristics.
    /// </summary>
    [CreateAssetMenu(menuName = "Gun Data", fileName = "Gun Data", order = 201)]
    public sealed class GunData : ScriptableObject
    {
        /// <summary>
        /// How a gun can fire projectiles.
        /// </summary>
        public enum FireMode
        {
            None,
            FullAuto,
            Single,
            Burst,
            ShotgunSingle,
            ShotgunAuto
        }

        /// <summary>
        /// How the gun can be loaded with ammo.
        /// </summary>
        public enum ReloadMode
        {
            Magazines,
            BulletByBullet
        }

        /// <summary>
        /// How the damage inflicted by the projectiles can be calculated.
        /// </summary>
        public enum DamageMode
        {
            DecreaseByDistance,
            Constant
        }

        #region GENERAL

        /// <summary>
        /// The gun name.
        /// </summary>
        [SerializeField]
        [Tooltip("The gun name.")]
        private string m_GunName = "Gun";

        /// <summary>
        /// The Prefab dropped when the character picks up a different gun.
        /// </summary>
        [SerializeField]
        [Required]
        [Tooltip("The Prefab dropped when the character picks up a different gun.")]
        private GameObject m_DroppablePrefab;

        /// <summary>
        /// The gun weight.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("The gun weight.")]
        private float m_Weight = 4.0f;

        /// <summary>
        /// The gun size. (Used to define how far the character can hit with a melee attack)
        /// </summary>
        [SerializeField]
        [MinMax(0.001f, Mathf.Infinity)]
        [Tooltip("The gun size. (Used to define how far the character can hit with a melee attack)")]
        private float m_Size = 1;

        /// <summary>
        /// Defines how much damage will be inflict by a melee attack.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines how much damage will be inflict by a melee attack.")]
        private float m_MeleeDamage = 50;

        /// <summary>
        /// Defines how much force will be applied when melee attack.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines how much force will be applied when melee attack.")]
        private float m_MeleeForce = 5;

        #endregion

        #region SHOOTING

        /// <summary>
        /// Defines the primary fire mode of this gun.
        /// </summary>
        [SerializeField]
        [Tooltip("Defines the primary fire mode of this gun.")]
        private FireMode m_PrimaryFireMode = FireMode.FullAuto;

        /// <summary>
        /// Defines the secondary fire mode of this gun.
        /// </summary>
        [SerializeField]
        [Tooltip("Defines the secondary fire mode of this gun.")]
        private FireMode m_SecondaryFireMode = FireMode.None;

        /// <summary>
        /// Rate of fire is the frequency at which a specific gun can fire or launch its projectiles. It is usually measured in rounds per minute (RPM or round/min).
        /// </summary>
        [SerializeField]
        [Tooltip("Rate of fire is the frequency at which a specific gun can fire or launch its projectiles. It is usually measured in rounds per minute (RPM or round/min).")]
        [Range(1, 1500)]
        private int m_PrimaryRateOfFire = 600;

        /// <summary>
        /// Rate of fire is the frequency at which a specific gun can fire or launch its projectiles. It is usually measured in rounds per minute (RPM or round/min).
        /// </summary>
        [SerializeField]
        [Tooltip("Rate of fire is the frequency at which a specific gun can fire or launch its projectiles. It is usually measured in rounds per minute (RPM or round/min).")]
        [Range(1, 1500)]
        private float m_SecondaryRateOfFire = 600;

        /// <summary>
        /// Defines how much force will be applied by the projectile.
        /// </summary>
        [SerializeField]
        [Tooltip("Defines how much force will be applied by the projectile.")]
        [MinMax(0, Mathf.Infinity)]
        private float m_Force = 5;

        /// <summary>
        /// Defines how far this gun can accurately hit a target.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines how far this gun can accurately hit a target.")]
        private float m_Range = 50;

        /// <summary>
        /// Defines how many bullets the shotgun will fire at once.
        /// </summary>
        [SerializeField]
        [Tooltip("Defines how many bullets the shotgun will fire at once.")]
        [MinMax(1, Mathf.Infinity)]
        private int m_BulletsPerShoot = 5;

        /// <summary>
        /// Defines how many bullets will be fired sequentially with a single pull of the trigger.
        /// </summary>
        [SerializeField]
        [Tooltip("Defines how many bullets will be fired sequentially with a single pull of the trigger.")]
        [MinMax(1, Mathf.Infinity)]
        private int m_BulletsPerBurst = 3;

        /// <summary>
        /// The Layers affected by this gun.
        /// </summary>
        [SerializeField]
        [Tooltip("The Layers affected by this gun.")]
        private LayerMask m_AffectedLayers = 1;

        /// <summary>
        /// Defines how the damage inflicted by the projectiles will be calculated.
        /// </summary>
        [SerializeField]
        [Tooltip("Defines how the damage inflicted by the projectiles will be calculated.")]
        private DamageMode m_DamageMode = DamageMode.Constant;

        /// <summary>
        /// Defines the minimum and maximum damage value inflicted by this weapon.
        /// </summary>
        [SerializeField]
        [MinMaxSlider(0, 100, "Defines the minimum and maximum damage value inflicted by this weapon.")]
        private Vector2 m_Damage = new Vector2(15, 30);

        /// <summary>
        /// Defines how the damage will be calculated based on the distance. 
        /// (The X axis is the target distance, in which 0 means 0 units and 1 means the full effective range and the Y axis is the damage percent.)
        /// </summary>
        [SerializeField]
        [Tooltip("Defines how the damage will be calculated based on the distance. " +
            "(The X axis is the target distance, in which 0 means 0 units and 1 means the full effective range and the Y axis is the damage percent)")]
        private AnimationCurve m_DamageFalloffCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(0.4f, 1), new Keyframe(0.6f, 0.5f), new Keyframe(1, 0.5f));

        /// <summary>
        /// Defines whether the projectiles fired by this gun can penetrate objects.
        /// </summary>
        [SerializeField]
        [Tooltip("Defines whether the projectiles fired by this gun can penetrate objects.")]
        private bool m_PenetrateObjects = true;

        /// <summary>
        /// The maximum distance that a projectile can travel penetrating an object.
        /// </summary>
        [SerializeField]
        [Tooltip("The maximum distance that a projectile can travel penetrating an object.")]
        [Range(0, 10)]
        private float m_PenetrationPower = 1;

        #endregion

        #region MAGAZINE

        /// <summary>
        /// Defines how the gun is loaded with ammo.
        /// </summary>
        [SerializeField]
        [Tooltip("Defines how the gun is loaded with ammo.")]
        private ReloadMode m_ReloadMode = ReloadMode.Magazines;

        /// <summary>
        /// Defines how many bullets has in the magazine.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines how many bullets has in the magazine.")]
        private int m_RoundsPerMagazine = 30;

        /// <summary>
        /// Enabling the chamber will add an additional bullet to your gun.
        /// </summary>
        [SerializeField]
        [Tooltip("Enabling the chamber will add an additional bullet to your gun.")]
        private bool m_HasChamber;

        /// <summary>
        /// Defines how many magazines the character will start with.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines how many magazines the character will start with.")]
        private int m_InitialMagazines = 3;

        /// <summary>
        /// Defines how many magazines the character can carry for this gun.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines how many magazines the character can carry for this gun.")]
        private int m_MaxMagazines = 6;

        #endregion

        #region ACCURACY

        /// <summary>
        /// Sets the radius of the conical fustrum. Used to calculate the bullet spread angle.
        /// </summary>
        [SerializeField]
        [MinMax(0.001f, Mathf.Infinity)]
        [Tooltip("Sets the radius of the conical fustrum. Used to calculate the bullet spread angle.")]
        private float m_MaximumSpread = 1.75f;

        /// <summary>
        /// Defines the minimum accuracy while using this gun. (If the character is moving or holding the trigger it will lose accuracy until it reaches the Base Accuracy)
        /// </summary>
        [SerializeField]
        [Range(0.01f, 1)]
        [Tooltip("Defines the minimum accuracy while using this gun. (If the character is moving or holding the trigger it will lose accuracy until it reaches the Base Accuracy)")]
        private float m_BaseAccuracy = 0.75f;

        /// <summary>
        /// Defines the accuracy percentage when the character is shooting from the hip. (0 is totally inaccurate and 1 is totally accurate)
        /// </summary>
        [SerializeField]
        [Range(0.01f, 1)]
        [Tooltip("Defines the accuracy percentage when the character is shooting from the hip. (0 is totally inaccurate and 1 is totally accurate)")]
        private float m_HIPAccuracy = 0.6f;

        /// <summary>
        /// Defines the accuracy percentage when the character is aiming. (0 is totally inaccurate and 1 is totally accurate)
        /// </summary>
        [SerializeField]
        [Range(0.01f, 1)]
        [Tooltip("Defines the accuracy percentage when the character is aiming. (0 is totally inaccurate and 1 is totally accurate)")]
        private float m_AIMAccuracy = 0.9f;

        /// <summary>
        /// Defines how fast this gun will be inaccurate due the character movement.
        /// </summary>
        [SerializeField]
        [Range(0, 3)]
        [Tooltip("Defines how fast this gun will be inaccurate due the character movement.")]
        private float m_DecreaseRateByWalking = 1;

        /// <summary>
        /// Defines how fast this gun will be inaccurate due constant shooting.
        /// </summary>
        [SerializeField]
        [Range(0, 3)]
        [Tooltip("Defines how fast this gun will be inaccurate due constant shooting.")]
        private float m_DecreaseRateByShooting = 1;

        #endregion

        #region GUN PROPERTIES

        /// <summary>
        /// The gun name. (Read Only)
        /// </summary>
        public string GunName => m_GunName;

        /// <summary>
        /// The Prefab dropped when the character picks up a different gun.
        /// </summary>
        public GameObject DroppablePrefab => m_DroppablePrefab;

        /// <summary>
        /// The gun weight. (Read Only)
        /// </summary>
        public float Weight => m_Weight;

        /// <summary>
        /// The gun size.(Read Only)
        /// </summary>
        public float Size => m_Size;

        /// <summary>
        /// Defines how much force will be applied when melee attack. (Read Only)
        /// </summary>
        public float MeleeForce => m_MeleeForce;

        /// <summary>
        /// Defines how much damage will be inflict by a melee attack. (Read Only)
        /// </summary>
        public float MeleeDamage => m_MeleeDamage;

        /// <summary>
        /// Defines the primary fire mode of this gun. (Read Only)
        /// </summary>
        public FireMode PrimaryFireMode => m_PrimaryFireMode;

        /// <summary>
        /// Defines the secondary fire mode of this gun. (Read Only)
        /// </summary>
        public FireMode SecondaryFireMode => m_SecondaryFireMode;

        /// <summary>
        /// Defines the time interval between each shot while the primary fire mode is selected. (Read Only)
        /// </summary>
        public float PrimaryRateOfFire => 60.0f / m_PrimaryRateOfFire;

        /// <summary>
        /// Defines the time interval between each shot while the secondary fire mode is selected. (Read Only)
        /// </summary>
        public float SecondaryRateOfFire => 60.0f / m_SecondaryRateOfFire;

        /// <summary>
        /// Defines how much force will be applied by the projectile. (Read Only)
        /// </summary>
        public float Force => m_Force;

        /// <summary>
        /// Defines how far this gun can accurately hit a target. (Read Only)
        /// </summary>
        public float Range => m_Range;

        /// <summary>
        /// Defines how many bullets the shotgun will fire at once. (Read Only)
        /// </summary>
        public int BulletsPerShoot => m_BulletsPerShoot;

        /// <summary>
        /// Defines how many bullets will be fired sequentially with a single pull of the trigger. (Read Only)
        /// </summary>
        public int BulletsPerBurst => m_BulletsPerBurst;

        /// <summary>
        /// The Layers affected by this gun. (Read Only)
        /// </summary>
        public LayerMask AffectedLayers => m_AffectedLayers;

        /// <summary>
        /// Defines how the damage inflicted by the projectiles will be calculated. (Read Only)
        /// </summary>
        public DamageMode DamageType => m_DamageMode;

        /// <summary>
        /// Returns the damage inflicted by the projectile.
        /// </summary>
        public float Damage => Random.Range(m_Damage.x, m_Damage.y);

        /// <summary>
        /// Defines how the damage will be calculated based on the distance. (Read Only)
        /// </summary>
        public AnimationCurve DamageFalloffCurve => m_DamageFalloffCurve;

        /// <summary>
        /// Returns true if the projectiles shot by this gun can penetrate objects, false otherwise.
        /// </summary>
        public bool PenetrateObjects => m_PenetrateObjects;

        /// <summary>
        /// Defines the maximum distance that a projectile can travel penetrating an object. (Read Only)
        /// </summary>
        public float PenetrationPower => m_PenetrationPower;

        /// <summary>
        /// Defines how the gun is loaded with ammo. (Read Only)
        /// </summary>
        public ReloadMode ReloadType => m_ReloadMode;

        /// <summary>
        /// Defines how many bullets has in the magazine. (Read Only)
        /// </summary>
        public int RoundsPerMagazine => m_RoundsPerMagazine;

        /// <summary>
        /// Returns true if this gun has a chamber, false otherwise. (Read Only)
        /// </summary>
        public bool HasChamber => m_HasChamber;

        /// <summary>
        /// Defines how many magazines the character will start with. (Read Only)
        /// </summary>
        public int InitialMagazines => m_InitialMagazines;

        /// <summary>
        /// Defines how many magazines the character can carry for this gun. (Read Only)
        /// </summary>
        public int MaxMagazines => m_MaxMagazines;

        /// <summary>
        /// Defines the radius of the conical fustrum. Used to calculate the bullet spread angle. (Read Only)
        /// </summary>
        public float MaximumSpread => m_MaximumSpread;

        /// <summary>
        /// Defines the minimum accuracy while using this gun. (Read Only)
        /// </summary>
        public float BaseAccuracy => m_BaseAccuracy;

        /// <summary>
        /// Defines the accuracy percentage when the character is shooting from the hip. (Read Only)
        /// </summary>
        public float HIPAccuracy => m_HIPAccuracy;

        /// <summary>
        /// Defines the accuracy percentage when the character is aiming. (Read Only)
        /// </summary>
        public float AIMAccuracy => m_AIMAccuracy;

        /// <summary>
        /// Defines how fast this gun will be inaccurate due the character movement. (Read Only)
        /// </summary>
        public float DecreaseRateByWalking => m_DecreaseRateByWalking;

        /// <summary>
        /// Defines how fast this gun will be inaccurate due constant shooting. (Read Only)
        /// </summary>
        public float DecreaseRateByShooting => m_DecreaseRateByShooting;

        #endregion
    }
}
