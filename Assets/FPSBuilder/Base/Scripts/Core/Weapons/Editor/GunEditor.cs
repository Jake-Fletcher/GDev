//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using FPSBuilder.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace FPSBuilder.Core.Weapons.Editor
{
    [CustomEditor(typeof(Gun))]
    public sealed class GunEditor : UnityEditor.Editor
    {
        private Gun m_Target;
        private SerializedProperty m_GunData;
        private SerializedProperty m_CameraTransformReference;
        private SerializedProperty m_FPController;

        #region WEAPON SWING

        private SerializedProperty m_WeaponSwing;
        private SerializedProperty m_Swing;
        private SerializedProperty m_TiltAngle;
        private SerializedProperty m_SwingAngle;
        private SerializedProperty m_AnimateAllAxes;
        private SerializedProperty m_TiltBoost;
        private SerializedProperty m_Speed;
        private SerializedProperty m_SwingTarget;
        private SerializedProperty m_TremorAmount;

        #endregion

        #region MOTION ANIMATIONS

        private SerializedProperty m_MotionAnimation;
        private SerializedProperty m_TargetTransform;
        private SerializedProperty m_WalkingMotionData;
        private SerializedProperty m_BrokenLegsMotionData;
        private SerializedProperty m_RunningMotionData;
        private SerializedProperty m_OverallSpeed;
        private SerializedProperty m_ScaleFactor;

        private SerializedProperty m_JumpAnimation;
        private SerializedProperty m_LandingAnimation;

        private SerializedProperty m_WeaponRecoil;
        private SerializedProperty m_WeaponRecoilPosition;
        private SerializedProperty m_WeaponRecoilRotation;
        private SerializedProperty m_WeaponRecoilDuration;

        private SerializedProperty m_CameraRecoil;
        private SerializedProperty m_CameraAnimationsController;
        private SerializedProperty m_MinCameraRecoilRotation;
        private SerializedProperty m_MaxCameraRecoilRotation;
        private SerializedProperty m_CameraRecoilDuration;
        private SerializedProperty m_CameraReturnDuration;

        private SerializedProperty m_BreathAnimation;
        private SerializedProperty m_BreathingSpeed;
        private SerializedProperty m_BreathingAmplitude;

        private SerializedProperty m_Lean;
        private SerializedProperty m_LeanAngle;
        private SerializedProperty m_LeanSpeed;

        #endregion

        #region GUN ANIMATOR

        private Vector3 m_HIPPosition;
        private Vector3 m_HIPRotation;

        private SerializedProperty m_GunAnimator;
        private SerializedProperty m_Animator;
        private SerializedProperty m_SpeedParameter;

        private SerializedProperty m_Draw;
        private SerializedProperty m_DrawAnimation;
        private SerializedProperty m_DrawSpeed;
        private SerializedProperty m_DrawSound;
        private SerializedProperty m_DrawVolume;

        private SerializedProperty m_Hide;
        private SerializedProperty m_HideAnimation;
        private SerializedProperty m_HideSpeed;
        private SerializedProperty m_HideSound;
        private SerializedProperty m_HideVolume;

        private SerializedProperty m_Fire;
        private SerializedProperty m_FireAnimations;
        private ReorderableList m_FireAnimationList;

        private SerializedProperty m_AimedFireAnimations;
        private ReorderableList m_AimedFireAnimationList;

        private SerializedProperty m_OverrideLastFire;
        private SerializedProperty m_FireSpeed;
        private SerializedProperty m_AimedFireSpeed;
        private SerializedProperty m_LastFireAnimation;
        private SerializedProperty m_FireAnimationType;

        private SerializedProperty m_FireSounds;
        private ReorderableList m_FireSoundList;

        private SerializedProperty m_OutOfAmmoSound;
        private SerializedProperty m_FireVolume;
        private SerializedProperty m_AdditiveSound;

        private SerializedProperty m_Reload;
        private SerializedProperty m_ReloadAnimation;
        private SerializedProperty m_ReloadSpeed;
        private SerializedProperty m_ReloadSound;
        private SerializedProperty m_ReloadEmptyAnimation;
        private SerializedProperty m_ReloadEmptySpeed;
        private SerializedProperty m_ReloadEmptySound;
        private SerializedProperty m_ReloadVolume;

        private SerializedProperty m_StartReloadAnimation;
        private SerializedProperty m_StartReloadSpeed;
        private SerializedProperty m_StartReloadSound;
        private SerializedProperty m_StartReloadVolume;

        private SerializedProperty m_InsertInChamberAnimation;
        private SerializedProperty m_InsertInChamberSpeed;
        private SerializedProperty m_InsertInChamberSound;
        private SerializedProperty m_InsertInChamberVolume;

        private SerializedProperty m_InsertAnimation;
        private SerializedProperty m_InsertSpeed;
        private SerializedProperty m_InsertSound;
        private SerializedProperty m_InsertVolume;

        private SerializedProperty m_StopReloadAnimation;
        private SerializedProperty m_StopReloadSpeed;
        private SerializedProperty m_StopReloadSound;
        private SerializedProperty m_StopReloadVolume;

        private SerializedProperty m_Melee;
        private SerializedProperty m_MeleeAnimation;
        private SerializedProperty m_MeleeSpeed;
        private SerializedProperty m_MeleeDelay;
        private SerializedProperty m_MeleeSound;
        private SerializedProperty m_MeleeVolume;

        private SerializedProperty m_HitSounds;
        private ReorderableList m_HitSoundList;
        private SerializedProperty m_HitVolume;

        private SerializedProperty m_SwitchMode;
        private SerializedProperty m_SwitchModeAnimation;
        private SerializedProperty m_SwitchModeSpeed;
        private SerializedProperty m_SwitchModeSound;
        private SerializedProperty m_SwitchModeVolume;

        private SerializedProperty m_Interact;
        private SerializedProperty m_InteractAnimation;
        private SerializedProperty m_InteractDelay;
        private SerializedProperty m_InteractSpeed;
        private SerializedProperty m_InteractSound;
        private SerializedProperty m_InteractVolume;

        private SerializedProperty m_Vault;
        private SerializedProperty m_VaultAnimation;
        private SerializedProperty m_VaultSpeed;
        private SerializedProperty m_VaultSound;
        private SerializedProperty m_VaultVolume;

        private SerializedProperty m_RunAnimation;
        private SerializedProperty m_RunningPosition;
        private SerializedProperty m_RunningRotation;
        private SerializedProperty m_RunningSpeed;

        private SerializedProperty m_SlidingAnimation;
        private SerializedProperty m_SlidingPosition;
        private SerializedProperty m_SlidingRotation;
        private SerializedProperty m_SlidingSpeed;

        private SerializedProperty m_AimAnimation;
        private SerializedProperty m_AimingPosition;
        private SerializedProperty m_AimingRotation;
        private SerializedProperty m_AimInSound;
        private SerializedProperty m_AimOutSound;
        private SerializedProperty m_ZoomAnimation;
        private SerializedProperty m_AimFOV;
        private SerializedProperty m_AimingSpeed;
        private SerializedProperty m_HoldBreath;

        #endregion

        #region GUN EFFECTS

        private SerializedProperty m_GunEffects;

        private SerializedProperty m_MuzzleFlash;
        private SerializedProperty m_MuzzleParticle;

        private SerializedProperty m_Tracer;
        private SerializedProperty m_TracerPrefab;
        private SerializedProperty m_TracerDuration;
        private SerializedProperty m_TracerSpeed;
        private SerializedProperty m_TracerOrigin;

        private SerializedProperty m_Shell;
        private SerializedProperty m_ShellParticle;
        private SerializedProperty m_ShellSpeed;
        private SerializedProperty m_StartDelay;

        private SerializedProperty m_MagazineDrop;
        private SerializedProperty m_MagazinePrefab;
        private SerializedProperty m_MaxMagazinesPrefabs;
        
        private SerializedProperty m_TacticalReloadDrop;
        private SerializedProperty m_TacticalDropDelay;
        
        private SerializedProperty m_FullReloadDrop;
        private SerializedProperty m_FullDropDelay;
        
        private SerializedProperty m_DropOrigin;

        #endregion

        private void OnEnable()
        {
            m_Target = (target as Gun);

            if (m_Target != null)
            {
                m_Target.DisableShadowCasting();
                m_Target.name = m_Target.InspectorName;

                m_HIPPosition = m_Target.transform.localPosition;
                
                // ReSharper disable once Unity.InefficientPropertyAccess
                m_HIPRotation = m_Target.transform.localRotation.eulerAngles;
            }

            m_GunData = serializedObject.FindProperty("m_GunData");
            m_CameraTransformReference = serializedObject.FindProperty("m_CameraTransformReference");
            m_FPController = serializedObject.FindProperty("m_FPController");

            m_WeaponSwing = serializedObject.FindProperty("m_WeaponSwing");
            m_Swing = m_WeaponSwing.FindPropertyRelative("m_Swing");
            m_TiltAngle = m_WeaponSwing.FindPropertyRelative("m_TiltAngle");
            m_SwingAngle = m_WeaponSwing.FindPropertyRelative("m_SwingAngle");
            m_AnimateAllAxes = m_WeaponSwing.FindPropertyRelative("m_AnimateAllAxes");
            m_TiltBoost = m_WeaponSwing.FindPropertyRelative("m_TiltBoost");
            m_Speed = m_WeaponSwing.FindPropertyRelative("m_Speed");
            m_SwingTarget = m_WeaponSwing.FindPropertyRelative("m_SwingTarget");
            m_TremorAmount = m_WeaponSwing.FindPropertyRelative("m_TremorAmount");

            m_MotionAnimation = serializedObject.FindProperty("m_MotionAnimation");
            m_ScaleFactor = m_MotionAnimation.FindPropertyRelative("m_ScaleFactor");
            m_OverallSpeed = m_MotionAnimation.FindPropertyRelative("m_OverallSpeed");
            m_WalkingMotionData = m_MotionAnimation.FindPropertyRelative("m_WalkingMotionData");
            m_BrokenLegsMotionData = m_MotionAnimation.FindPropertyRelative("m_BrokenLegsMotionData");
            m_RunningMotionData = m_MotionAnimation.FindPropertyRelative("m_RunningMotionData");
            m_TargetTransform = m_MotionAnimation.FindPropertyRelative("m_TargetTransform");
            m_JumpAnimation = m_MotionAnimation.FindPropertyRelative("m_JumpAnimation");
            m_LandingAnimation = m_MotionAnimation.FindPropertyRelative("m_LandingAnimation");
            m_BreathAnimation = m_MotionAnimation.FindPropertyRelative("m_BreathAnimation");
            m_BreathingSpeed = m_MotionAnimation.FindPropertyRelative("m_BreathingSpeed");
            m_BreathingAmplitude = m_MotionAnimation.FindPropertyRelative("m_BreathingAmplitude");

            m_Lean = m_MotionAnimation.FindPropertyRelative("m_Lean");
            m_LeanAngle = m_MotionAnimation.FindPropertyRelative("m_LeanAngle");
            m_LeanSpeed = m_MotionAnimation.FindPropertyRelative("m_LeanSpeed");

            m_WeaponRecoil = m_MotionAnimation.FindPropertyRelative("m_WeaponRecoil");
            m_WeaponRecoilPosition = m_MotionAnimation.FindPropertyRelative("m_WeaponRecoilPosition");
            m_WeaponRecoilRotation = m_MotionAnimation.FindPropertyRelative("m_WeaponRecoilRotation");
            m_WeaponRecoilDuration = m_MotionAnimation.FindPropertyRelative("m_WeaponRecoilDuration");

            m_CameraRecoil = m_MotionAnimation.FindPropertyRelative("m_CameraRecoil");
            m_CameraAnimationsController = serializedObject.FindProperty("m_CameraAnimationsController");
            m_MinCameraRecoilRotation = m_MotionAnimation.FindPropertyRelative("m_MinCameraRecoilRotation");
            m_MaxCameraRecoilRotation = m_MotionAnimation.FindPropertyRelative("m_MaxCameraRecoilRotation");
            m_CameraRecoilDuration = m_MotionAnimation.FindPropertyRelative("m_CameraRecoilDuration");
            m_CameraReturnDuration = m_MotionAnimation.FindPropertyRelative("m_CameraReturnDuration");

            m_GunAnimator = serializedObject.FindProperty("m_GunAnimator");
            m_Animator = m_GunAnimator.FindPropertyRelative("m_Animator");
            m_SpeedParameter = m_GunAnimator.FindPropertyRelative("m_SpeedParameter");

            m_Draw = m_GunAnimator.FindPropertyRelative("m_Draw");
            m_DrawAnimation = m_GunAnimator.FindPropertyRelative("m_DrawAnimation");
            m_DrawSpeed = m_GunAnimator.FindPropertyRelative("m_DrawSpeed");
            m_DrawSound = m_GunAnimator.FindPropertyRelative("m_DrawSound");
            m_DrawVolume = m_GunAnimator.FindPropertyRelative("m_DrawVolume");

            m_Hide = m_GunAnimator.FindPropertyRelative("m_Hide");
            m_HideAnimation = m_GunAnimator.FindPropertyRelative("m_HideAnimation");
            m_HideSpeed = m_GunAnimator.FindPropertyRelative("m_HideSpeed");
            m_HideSound = m_GunAnimator.FindPropertyRelative("m_HideSound");
            m_HideVolume = m_GunAnimator.FindPropertyRelative("m_HideVolume");

            m_Fire = m_GunAnimator.FindPropertyRelative("m_Fire");
            m_FireAnimations = m_GunAnimator.FindPropertyRelative("m_FireAnimationList");
            m_AimedFireAnimations = m_GunAnimator.FindPropertyRelative("m_AimedFireAnimationList");
            m_LastFireAnimation = m_GunAnimator.FindPropertyRelative("m_LastFireAnimation");
            m_OverrideLastFire = m_GunAnimator.FindPropertyRelative("m_OverrideLastFire");
            m_FireAnimationType = m_GunAnimator.FindPropertyRelative("m_FireAnimationType");
            m_FireSounds = m_GunAnimator.FindPropertyRelative("m_FireSoundList");
            m_FireSpeed = m_GunAnimator.FindPropertyRelative("m_FireSpeed");
            m_AimedFireSpeed = m_GunAnimator.FindPropertyRelative("m_AimedFireSpeed");
            m_OutOfAmmoSound = m_GunAnimator.FindPropertyRelative("m_OutOfAmmoSound");
            m_FireVolume = m_GunAnimator.FindPropertyRelative("m_FireVolume");
            m_AdditiveSound = m_GunAnimator.FindPropertyRelative("m_AdditiveSound");

            m_Reload = m_GunAnimator.FindPropertyRelative("m_Reload");
            m_ReloadAnimation = m_GunAnimator.FindPropertyRelative("m_ReloadAnimation");
            m_ReloadSpeed = m_GunAnimator.FindPropertyRelative("m_ReloadSpeed");
            m_ReloadSound = m_GunAnimator.FindPropertyRelative("m_ReloadSound");
            m_ReloadEmptyAnimation = m_GunAnimator.FindPropertyRelative("m_ReloadEmptyAnimation");
            m_ReloadEmptySpeed = m_GunAnimator.FindPropertyRelative("m_ReloadEmptySpeed");
            m_ReloadEmptySound = m_GunAnimator.FindPropertyRelative("m_ReloadEmptySound");
            m_ReloadVolume = m_GunAnimator.FindPropertyRelative("m_ReloadVolume");
            m_StartReloadAnimation = m_GunAnimator.FindPropertyRelative("m_StartReloadAnimation");
            m_StartReloadSpeed = m_GunAnimator.FindPropertyRelative("m_StartReloadSpeed");
            m_StartReloadSound = m_GunAnimator.FindPropertyRelative("m_StartReloadSound");
            m_StartReloadVolume = m_GunAnimator.FindPropertyRelative("m_StartReloadVolume");
            m_InsertInChamberAnimation = m_GunAnimator.FindPropertyRelative("m_InsertInChamberAnimation");
            m_InsertInChamberSpeed = m_GunAnimator.FindPropertyRelative("m_InsertInChamberSpeed");
            m_InsertInChamberSound = m_GunAnimator.FindPropertyRelative("m_InsertInChamberSound");
            m_InsertInChamberVolume = m_GunAnimator.FindPropertyRelative("m_InsertInChamberVolume");
            m_InsertAnimation = m_GunAnimator.FindPropertyRelative("m_InsertAnimation");
            m_InsertSpeed = m_GunAnimator.FindPropertyRelative("m_InsertSpeed");
            m_InsertSound = m_GunAnimator.FindPropertyRelative("m_InsertSound");
            m_InsertVolume = m_GunAnimator.FindPropertyRelative("m_InsertVolume");
            m_StopReloadAnimation = m_GunAnimator.FindPropertyRelative("m_StopReloadAnimation");
            m_StopReloadSpeed = m_GunAnimator.FindPropertyRelative("m_StopReloadSpeed");
            m_StopReloadSound = m_GunAnimator.FindPropertyRelative("m_StopReloadSound");
            m_StopReloadVolume = m_GunAnimator.FindPropertyRelative("m_StopReloadVolume");

            m_Melee = m_GunAnimator.FindPropertyRelative("m_Melee");
            m_MeleeAnimation = m_GunAnimator.FindPropertyRelative("m_MeleeAnimation");
            m_MeleeSpeed = m_GunAnimator.FindPropertyRelative("m_MeleeSpeed");
            m_MeleeDelay = m_GunAnimator.FindPropertyRelative("m_MeleeDelay");
            m_MeleeSound = m_GunAnimator.FindPropertyRelative("m_MeleeSound");
            m_MeleeVolume = m_GunAnimator.FindPropertyRelative("m_MeleeVolume");
            m_HitSounds = m_GunAnimator.FindPropertyRelative("m_HitSoundList");
            m_HitVolume = m_GunAnimator.FindPropertyRelative("m_HitVolume");

            m_SwitchMode = m_GunAnimator.FindPropertyRelative("m_SwitchMode");
            m_SwitchModeAnimation = m_GunAnimator.FindPropertyRelative("m_SwitchModeAnimation");
            m_SwitchModeSpeed = m_GunAnimator.FindPropertyRelative("m_SwitchModeSpeed");
            m_SwitchModeSound = m_GunAnimator.FindPropertyRelative("m_SwitchModeSound");
            m_SwitchModeVolume = m_GunAnimator.FindPropertyRelative("m_SwitchModeVolume");

            m_Interact = m_GunAnimator.FindPropertyRelative("m_Interact");
            m_InteractAnimation = m_GunAnimator.FindPropertyRelative("m_InteractAnimation");
            m_InteractDelay = m_GunAnimator.FindPropertyRelative("m_InteractDelay");
            m_InteractSpeed = m_GunAnimator.FindPropertyRelative("m_InteractSpeed");
            m_InteractSound = m_GunAnimator.FindPropertyRelative("m_InteractSound");
            m_InteractVolume = m_GunAnimator.FindPropertyRelative("m_InteractVolume");

            m_Vault = m_GunAnimator.FindPropertyRelative("m_Vault");
            m_VaultAnimation = m_GunAnimator.FindPropertyRelative("m_VaultAnimation");
            m_VaultSpeed = m_GunAnimator.FindPropertyRelative("m_VaultSpeed");
            m_VaultSound = m_GunAnimator.FindPropertyRelative("m_VaultSound");
            m_VaultVolume = m_GunAnimator.FindPropertyRelative("m_VaultVolume");

            m_RunAnimation = m_GunAnimator.FindPropertyRelative("m_RunAnimation");
            m_RunningPosition = m_GunAnimator.FindPropertyRelative("m_RunningPosition");
            m_RunningRotation = m_GunAnimator.FindPropertyRelative("m_RunningRotation");
            m_RunningSpeed = m_GunAnimator.FindPropertyRelative("m_RunningSpeed");

            m_SlidingAnimation = m_GunAnimator.FindPropertyRelative("m_SlidingAnimation");
            m_SlidingPosition = m_GunAnimator.FindPropertyRelative("m_SlidingPosition");
            m_SlidingRotation = m_GunAnimator.FindPropertyRelative("m_SlidingRotation");
            m_SlidingSpeed = m_GunAnimator.FindPropertyRelative("m_SlidingSpeed");

            m_AimAnimation = m_GunAnimator.FindPropertyRelative("m_AimAnimation");
            m_AimingPosition = m_GunAnimator.FindPropertyRelative("m_AimingPosition");
            m_AimingRotation = m_GunAnimator.FindPropertyRelative("m_AimingRotation");
            m_AimInSound = m_GunAnimator.FindPropertyRelative("m_AimInSound");
            m_AimOutSound = m_GunAnimator.FindPropertyRelative("m_AimOutSound");
            m_ZoomAnimation = m_GunAnimator.FindPropertyRelative("m_ZoomAnimation");
            m_AimFOV = m_GunAnimator.FindPropertyRelative("m_AimFOV");
            m_AimingSpeed = m_GunAnimator.FindPropertyRelative("m_AimingSpeed");
            m_HoldBreath = m_GunAnimator.FindPropertyRelative("m_HoldBreath");

            m_GunEffects = serializedObject.FindProperty("m_GunEffects");
            m_MuzzleFlash = m_GunEffects.FindPropertyRelative("m_MuzzleFlash");
            m_MuzzleParticle = m_GunEffects.FindPropertyRelative("m_MuzzleParticle");

            m_Tracer = m_GunEffects.FindPropertyRelative("m_Tracer");
            m_TracerPrefab = m_GunEffects.FindPropertyRelative("m_TracerPrefab");
            m_TracerDuration = m_GunEffects.FindPropertyRelative("m_TracerDuration");
            m_TracerSpeed = m_GunEffects.FindPropertyRelative("m_TracerSpeed");
            m_TracerOrigin = m_GunEffects.FindPropertyRelative("m_TracerOrigin");

            m_Shell = m_GunEffects.FindPropertyRelative("m_Shell");
            m_ShellParticle = m_GunEffects.FindPropertyRelative("m_ShellParticle");
            m_ShellSpeed = m_GunEffects.FindPropertyRelative("m_ShellSpeed");
            m_StartDelay = m_GunEffects.FindPropertyRelative("m_StartDelay");

            m_MagazineDrop = m_GunEffects.FindPropertyRelative("m_MagazineDrop");
            m_MagazinePrefab = m_GunEffects.FindPropertyRelative("m_MagazinePrefab");
            m_MaxMagazinesPrefabs = m_GunEffects.FindPropertyRelative("m_MaxMagazinesPrefabs");
        
            m_TacticalReloadDrop = m_GunEffects.FindPropertyRelative("m_TacticalReloadDrop");
            m_TacticalDropDelay = m_GunEffects.FindPropertyRelative("m_TacticalDropDelay");
            m_FullReloadDrop = m_GunEffects.FindPropertyRelative("m_FullReloadDrop");
            m_FullDropDelay = m_GunEffects.FindPropertyRelative("m_FullDropDelay");
            
            m_DropOrigin = m_GunEffects.FindPropertyRelative("m_DropOrigin");
        
            m_FireAnimationList = EditorUtilities.CreateReorderableList(m_FireAnimations, "Animation");
            m_AimedFireAnimationList = EditorUtilities.CreateReorderableList(m_AimedFireAnimations, "Animation");
            
            m_FireSoundList = EditorUtilities.CreateReorderableList(m_FireSounds, "Sound");
            m_HitSoundList = EditorUtilities.CreateReorderableList(m_HitSounds, "Sound");
        }

        private void OnDisable()
        {
            if (!m_Target) 
                return;

            if (m_Target.transform.localPosition != m_HIPPosition)
            {
                m_Target.transform.localPosition = m_HIPPosition;
            }

            if (m_Target.transform.localRotation != Quaternion.Euler(m_HIPRotation))
            {
                // ReSharper disable once Unity.InefficientPropertyAccess
                m_Target.transform.localRotation = Quaternion.Euler(m_HIPRotation);
            }
        }

        public override void OnInspectorGUI()
        {
            //Update the serializedProperty - always do this in the beginning of OnInspectorGUI
            serializedObject.Update();
            
            EditorUtilities.DrawHeader("Controllers & Managers");
            EditorGUILayout.PropertyField(m_GunData);
            EditorGUILayout.PropertyField(m_CameraAnimationsController, new GUIContent("Camera Animator"));
            EditorGUILayout.PropertyField(m_FPController, new GUIContent("First Person Character"));
            EditorGUILayout.PropertyField(m_CameraTransformReference);

            EditorGUILayout.Space();
            EditorUtilities.DrawHeader("Procedural Animations");
            Animations();

            EditorGUILayout.Space();
            EditorUtilities.DrawHeader("Weapon Effects");
            Effects();

            EditorGUI.indentLevel = 0;
            //Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI
            serializedObject.ApplyModifiedProperties();
        }

        private void Animations()
        {
            #region MOTION ANIMATION

            EditorGUI.indentLevel = 0;
            EditorUtilities.FoldoutHeader("Motion", m_MotionAnimation);

            if (m_MotionAnimation.isExpanded)
            {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(m_ScaleFactor);
                EditorGUILayout.PropertyField(m_OverallSpeed);
                EditorGUILayout.PropertyField(m_TargetTransform);
                EditorGUILayout.PropertyField(m_WalkingMotionData);
                EditorGUILayout.PropertyField(m_BrokenLegsMotionData);
                EditorGUILayout.PropertyField(m_RunningMotionData);

                EditorGUI.indentLevel = 0;
                EditorGUILayout.Space();

                EditorUtilities.CustomLerpAnimationDrawer("Jump Animation", m_JumpAnimation, m_HIPPosition, m_HIPRotation, true, false, m_Target.transform);
                EditorUtilities.CustomLerpAnimationDrawer("Landing Animation", m_LandingAnimation, m_HIPPosition, m_HIPRotation, true, false, m_Target.transform);

                using (new EditorGUILayout.VerticalScope(Styling.background))
                {
                    EditorGUILayout.PropertyField(m_BreathAnimation);

                    if (m_BreathAnimation.boolValue)
                    {
                        EditorGUI.indentLevel = 1;
                        EditorGUILayout.PropertyField(m_BreathingSpeed);
                        EditorGUILayout.PropertyField(m_BreathingAmplitude);
                    }
                }

                EditorGUI.indentLevel = 0;
                using (new EditorGUILayout.VerticalScope(Styling.background))
                {
                    EditorGUILayout.PropertyField(m_Lean);
                    if (m_Lean.boolValue)
                    {
                        EditorGUI.indentLevel = 1;
                        EditorGUILayout.PropertyField(m_LeanAngle);
                        EditorGUILayout.PropertyField(m_LeanSpeed);
                    }
                }
                EditorGUI.indentLevel = 0;
            }

            #region WEAPON SWING
            EditorGUI.indentLevel = 0;
            EditorUtilities.ToggleHeader("Weapon Swing", m_Swing);

            if (m_Swing.isExpanded)
            {
                EditorGUI.indentLevel = 1;
                using (new EditorGUI.DisabledScope(!m_Swing.boolValue))
                {
                    EditorGUILayout.PropertyField(m_TiltAngle);
                    EditorGUILayout.PropertyField(m_SwingAngle);
                    
                    EditorGUI.indentLevel = 0;
                    using (new EditorGUILayout.VerticalScope(Styling.background))
                    {
                        EditorGUILayout.PropertyField(m_AnimateAllAxes);

                        if (m_AnimateAllAxes.boolValue)
                        {
                            EditorGUI.indentLevel = 1;
                            EditorGUILayout.PropertyField(m_TiltBoost);
                        }
                    }
                    
                    EditorGUI.indentLevel = 1;

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(m_TremorAmount);

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(m_Speed);
                    
                    EditorGUILayout.PropertyField(m_SwingTarget);
                }

            }
            #endregion

            EditorGUI.indentLevel = 0;
            EditorUtilities.ToggleHeader("Weapon Recoil", m_WeaponRecoil);

            if (m_WeaponRecoil.isExpanded)
            {
                EditorGUI.indentLevel = 1;
                using (new EditorGUI.DisabledScope(!m_WeaponRecoil.boolValue))
                {
                    EditorGUILayout.PropertyField(m_WeaponRecoilPosition);
                    EditorGUILayout.PropertyField(m_WeaponRecoilRotation);

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(m_WeaponRecoilDuration);

                    EditorGUILayout.Space();
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace();

                        if (GUILayout.Button("Get From Transform", Styling.leftButton))
                        {
                            m_WeaponRecoilPosition.vector3Value = m_Target.transform.localPosition;
                            
                            // ReSharper disable once Unity.InefficientPropertyAccess
                            m_WeaponRecoilRotation.vector3Value = m_Target.transform.localEulerAngles;
                        }

                        if (GUILayout.Button("Reset", Styling.rightButton))
                        {
                            m_WeaponRecoilPosition.vector3Value = new Vector3(0, 0, 0);
                            m_WeaponRecoilRotation.vector3Value = Vector3.zero;
                            m_WeaponRecoilDuration.floatValue = 0.1f;
                        }

                        GUILayout.FlexibleSpace();
                    }
                }
            }

            EditorGUI.indentLevel = 0;
            EditorUtilities.ToggleHeader("Camera Recoil", m_CameraRecoil);

            if (m_CameraRecoil.isExpanded)
            {
                EditorGUI.indentLevel = 1;
                using (new EditorGUI.DisabledScope(!m_CameraRecoil.boolValue))
                {
                    if (m_CameraAnimationsController.objectReferenceValue == null && m_CameraRecoil.boolValue)
                        EditorGUILayout.HelpBox("Camera Animations Controller must be assigned to apply the recoil on camera.", MessageType.Warning);

                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(m_MinCameraRecoilRotation);
                    EditorGUILayout.PropertyField(m_MaxCameraRecoilRotation);
                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(m_CameraRecoilDuration);
                    EditorGUILayout.PropertyField(m_CameraReturnDuration);
                }
            }
            #endregion

            #region GUN ANIMATOR

            #region SPRINT
            
            EditorGUI.indentLevel = 0;
            EditorUtilities.ToggleHeader("Sprint", m_RunAnimation);

            if (m_RunAnimation.isExpanded)
            {
                using (new EditorGUI.DisabledScope(!m_RunAnimation.boolValue))
                {
                    using (new EditorGUILayout.VerticalScope(Styling.background))
                    {
                        EditorGUILayout.PropertyField(m_RunningPosition, new GUIContent("Sprint Position"));
                        EditorGUILayout.PropertyField(m_RunningRotation, new GUIContent("Sprint Rotation"));

                        EditorGUILayout.Space();
                        EditorGUILayout.PropertyField(m_RunningSpeed, new GUIContent("Sprint Speed"));
                    }
                    
                    EditorGUILayout.Space();
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace();

                        if (GUILayout.Button("Get From Transform", Styling.leftButton))
                        {
                            m_RunningPosition.vector3Value = m_Target.transform.localPosition;
                            
                            // ReSharper disable once Unity.InefficientPropertyAccess
                            m_RunningRotation.vector3Value = m_Target.transform.localRotation.eulerAngles;
                        }

                        if (GUILayout.Button("Reset", Styling.rightButton))
                        {
                            m_RunningPosition.vector3Value = Vector3.zero;
                            m_RunningRotation.vector3Value = Vector3.zero;
                            m_RunningSpeed.floatValue = 10;
                        }

                        if (m_RunningPosition.vector3Value != Vector3.zero || m_RunningRotation.vector3Value != Vector3.zero)
                        {
                            GUILayout.FlexibleSpace();
                        
                            // ReSharper disable once Unity.InefficientPropertyAccess
                            bool isPreviewing = m_Target.transform.localPosition == m_RunningPosition.vector3Value &&
                                                m_Target.transform.localRotation.eulerAngles == m_RunningRotation.vector3Value;
                        
                            if (GUILayout.Button(isPreviewing ? "Exit Preview" : "Preview", Styling.button, GUILayout.Width(96)))
                            {
                                if (isPreviewing)
                                {
                                    m_Target.transform.localPosition = m_HIPPosition;
                                
                                    // ReSharper disable once Unity.InefficientPropertyAccess
                                    m_Target.transform.localRotation = Quaternion.Euler(m_HIPRotation);
                                }
                                else
                                {
                                    m_Target.transform.localPosition = m_RunningPosition.vector3Value;
                                
                                    // ReSharper disable once Unity.InefficientPropertyAccess
                                    m_Target.transform.localEulerAngles = m_RunningRotation.vector3Value;
                                }
                            }
                        }

                        GUILayout.FlexibleSpace();
                    }
                }
            }

            #endregion

            #region SLIDING

            EditorGUI.indentLevel = 0;
            EditorUtilities.ToggleHeader("Sliding", m_SlidingAnimation);

            if (m_SlidingAnimation.isExpanded)
            {
                using (new EditorGUI.DisabledScope(!m_SlidingAnimation.boolValue))
                {
                    using (new EditorGUILayout.VerticalScope(Styling.background))
                    {
                        EditorGUILayout.PropertyField(m_SlidingPosition, new GUIContent("Sliding Position"));
                        EditorGUILayout.PropertyField(m_SlidingRotation, new GUIContent("Sliding Rotation"));

                        EditorGUILayout.Space();
                        EditorGUILayout.PropertyField(m_SlidingSpeed, new GUIContent("Sliding Speed"));
                    }
                    
                    EditorGUILayout.Space();
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace();

                        if (GUILayout.Button("Get From Transform", Styling.leftButton))
                        {
                            m_SlidingPosition.vector3Value = m_Target.transform.localPosition;
                            
                            // ReSharper disable once Unity.InefficientPropertyAccess
                            m_SlidingRotation.vector3Value = m_Target.transform.localRotation.eulerAngles;
                        }

                        if (GUILayout.Button("Reset", Styling.rightButton))
                        {
                            m_SlidingPosition.vector3Value = Vector3.zero;
                            m_SlidingRotation.vector3Value = Vector3.zero;
                            m_SlidingSpeed.floatValue = 10;
                        }

                        if (m_SlidingPosition.vector3Value != Vector3.zero || m_SlidingRotation.vector3Value != Vector3.zero)
                        {
                            GUILayout.FlexibleSpace();
                        
                            // ReSharper disable once Unity.InefficientPropertyAccess
                            bool isPreviewing = m_Target.transform.localPosition == m_SlidingPosition.vector3Value &&
                                                m_Target.transform.localRotation.eulerAngles == m_SlidingRotation.vector3Value;
                        
                            if (GUILayout.Button(isPreviewing ? "Exit Preview" : "Preview", Styling.button, GUILayout.Width(96)))
                            {
                                if (isPreviewing)
                                {
                                    m_Target.transform.localPosition = m_HIPPosition;
                                
                                    // ReSharper disable once Unity.InefficientPropertyAccess
                                    m_Target.transform.localRotation = Quaternion.Euler(m_HIPRotation);
                                }
                                else
                                {
                                    m_Target.transform.localPosition = m_SlidingPosition.vector3Value;
                                
                                    // ReSharper disable once Unity.InefficientPropertyAccess
                                    m_Target.transform.localEulerAngles = m_SlidingRotation.vector3Value;
                                }
                            }
                        }

                        GUILayout.FlexibleSpace();
                    }
                }
            }

            #endregion

            #region AIM DOWN SIGHTS

            EditorGUI.indentLevel = 0;
            EditorUtilities.ToggleHeader("Aim Down Sights", m_AimAnimation);

            if (m_AimAnimation.isExpanded)
            {
                EditorGUI.indentLevel = 0;
                using (new EditorGUI.DisabledScope(!m_AimAnimation.boolValue))
                {
                    using (new EditorGUILayout.VerticalScope(Styling.background))
                    {
                        EditorGUILayout.PropertyField(m_AimingPosition);
                        EditorGUILayout.PropertyField(m_AimingRotation);

                        EditorGUILayout.Space();
                        EditorGUILayout.PropertyField(m_AimingSpeed);
                    }
                    
                    EditorGUILayout.Space();
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace();

                        if (GUILayout.Button("Get From Transform", Styling.leftButton))
                        {
                            m_AimingPosition.vector3Value = m_Target.transform.localPosition;
                            
                            // ReSharper disable once Unity.InefficientPropertyAccess
                            m_AimingRotation.vector3Value = m_Target.transform.localRotation.eulerAngles;
                        }

                        if (GUILayout.Button("Reset", Styling.rightButton))
                        {
                            m_AimingPosition.vector3Value = Vector3.zero;
                            m_AimingRotation.vector3Value = Vector3.zero;
                            m_AimingSpeed.floatValue = 10;
                        }

                        if (m_AimingPosition.vector3Value != Vector3.zero || m_AimingRotation.vector3Value != Vector3.zero)
                        {
                            GUILayout.FlexibleSpace();
                            
                            // ReSharper disable once Unity.InefficientPropertyAccess
                            bool isPreviewing = m_Target.transform.localPosition == m_AimingPosition.vector3Value &&
                                                (m_Target.transform.localRotation.eulerAngles - m_AimingRotation.vector3Value).magnitude < 0.01f;

                            if (GUILayout.Button(isPreviewing ? "Exit Preview" : "Preview", Styling.button, GUILayout.Width(96)))
                            {
                                if (isPreviewing)
                                {
                                    m_Target.transform.localPosition = m_HIPPosition;
                                
                                    // ReSharper disable once Unity.InefficientPropertyAccess
                                    m_Target.transform.localRotation = Quaternion.Euler(m_HIPRotation);
                                }
                                else
                                {
                                    m_Target.transform.localPosition = m_AimingPosition.vector3Value;
                                
                                    // ReSharper disable once Unity.InefficientPropertyAccess
                                    m_Target.transform.localRotation = Quaternion.Euler(m_AimingRotation.vector3Value);
                                }
                            }
                        }

                        GUILayout.FlexibleSpace();
                    }

                    EditorGUILayout.Space();
                    EditorGUI.indentLevel = 1;
                    EditorGUILayout.PropertyField(m_HoldBreath);

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(m_AimInSound);
                    EditorGUILayout.PropertyField(m_AimOutSound);

                    EditorGUILayout.Space();
                    EditorGUI.indentLevel = 0;
                    using (new EditorGUILayout.VerticalScope(Styling.background))
                    {
                        EditorGUILayout.PropertyField(m_ZoomAnimation);

                        if (m_ZoomAnimation.boolValue)
                        {
                            EditorGUI.indentLevel = 1;
                            EditorGUILayout.PropertyField(m_AimFOV);
                        }
                    }
                }
            }
            
            EditorUtilities.DrawSplitter();

            #endregion

            EditorGUILayout.Space();
            EditorGUI.indentLevel = 0;

            EditorUtilities.DrawHeader("Weapon Animations");
            EditorGUILayout.PropertyField(m_Animator);

            if (m_Animator.objectReferenceValue != null)
                EditorGUILayout.PropertyField(m_SpeedParameter);

            EditorUtilities.ToggleHeader("Draw", m_Draw);

            if (m_Draw.isExpanded)
            {
                EditorGUI.indentLevel = 1;
                using (new EditorGUI.DisabledScope(!m_Draw.boolValue))
                {
                    EditorGUILayout.PropertyField(m_DrawAnimation);
                    EditorGUILayout.PropertyField(m_DrawSpeed);
                    EditorGUILayout.PropertyField(m_DrawSound);
                    EditorGUILayout.PropertyField(m_DrawVolume);
                }
            }

            EditorGUI.indentLevel = 0;
            EditorUtilities.ToggleHeader("Hide", m_Hide);

            if (m_Hide.isExpanded)
            {
                EditorGUI.indentLevel = 1;
                using (new EditorGUI.DisabledScope(!m_Hide.boolValue))
                {
                    EditorGUILayout.PropertyField(m_HideAnimation);
                    EditorGUILayout.PropertyField(m_HideSpeed);
                    EditorGUILayout.PropertyField(m_HideSound);
                    EditorGUILayout.PropertyField(m_HideVolume);
                }
            }

            EditorGUI.indentLevel = 0;
            EditorUtilities.ToggleHeader("Fire", m_Fire);

            if (m_Fire.isExpanded)
            {
                EditorGUI.indentLevel = 1;
                using (new EditorGUI.DisabledScope(!m_Fire.boolValue))
                {
                    EditorGUI.indentLevel = 0;
                    EditorGUILayout.Space();
                    m_FireAnimationList.DoLayoutList();
                    
                    EditorGUILayout.Space();
                    
                    EditorGUI.indentLevel = 0;
                    m_AimedFireAnimationList.DoLayoutList();
                    
                    EditorGUI.indentLevel = 1;
                    EditorGUILayout.PropertyField(m_FireSpeed);
                    EditorGUILayout.PropertyField(m_AimedFireSpeed);
                    EditorGUILayout.PropertyField(m_FireAnimationType);
                    EditorGUILayout.Space();
                    
                    EditorGUI.indentLevel = 0;
                    using (new EditorGUILayout.VerticalScope(Styling.background))
                    {
                        EditorGUILayout.PropertyField(m_OverrideLastFire);
                    
                        if (m_OverrideLastFire.boolValue)
                        {
                            EditorGUI.indentLevel = 1;
                            EditorGUILayout.PropertyField(m_LastFireAnimation);
                        }
                    }
                    
                    
                    EditorGUILayout.Space();
                    EditorGUI.indentLevel = 0;
                    m_FireSoundList.DoLayoutList();
                    EditorGUI.indentLevel = 1;
                    EditorGUILayout.PropertyField(m_AdditiveSound);
                    
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(m_OutOfAmmoSound, new GUIContent("Dry Fire Sound"));
                    EditorGUILayout.PropertyField(m_FireVolume);
                    
                }
            }

            EditorGUI.indentLevel = 0;
            EditorUtilities.ToggleHeader("Reload", m_Reload);

            if (m_Reload.isExpanded)
            {
                EditorGUI.indentLevel = 1;
                using (new EditorGUI.DisabledScope(!m_Reload.boolValue))
                {
                    if (m_Target.ReloadType == GunData.ReloadMode.Magazines)
                    {
                        EditorGUILayout.PropertyField(m_ReloadAnimation, new GUIContent("Tactical Reload Animation"));
                        EditorGUILayout.PropertyField(m_ReloadSpeed, new GUIContent("Tactical Reload Speed"));
                        EditorGUILayout.PropertyField(m_ReloadSound, new GUIContent("Tactical Reload Sound"));
                        EditorGUILayout.Space();
                        EditorGUILayout.PropertyField(m_ReloadEmptyAnimation, new GUIContent("Full Reload Animation"));
                        EditorGUILayout.PropertyField(m_ReloadEmptySpeed, new GUIContent("Full Reload Speed"));
                        EditorGUILayout.PropertyField(m_ReloadEmptySound, new GUIContent("Full Reload Sound"));
                        EditorGUILayout.Space();
                        EditorGUILayout.PropertyField(m_ReloadVolume);

                    }
                    else if (m_Target.ReloadType == GunData.ReloadMode.BulletByBullet)
                    {
                        EditorGUILayout.PropertyField(m_StartReloadAnimation);
                        EditorGUILayout.PropertyField(m_StartReloadSpeed);
                        EditorGUILayout.PropertyField(m_StartReloadSound);
                        EditorGUILayout.PropertyField(m_StartReloadVolume);
                        EditorGUILayout.Space();

                        if (m_Target.HasChamber)
                        {
                            EditorGUILayout.PropertyField(m_InsertInChamberAnimation);
                            EditorGUILayout.PropertyField(m_InsertInChamberSpeed);
                            EditorGUILayout.PropertyField(m_InsertInChamberSound);
                            EditorGUILayout.PropertyField(m_InsertInChamberVolume);
                            EditorGUILayout.Space();
                        }

                        EditorGUILayout.PropertyField(m_InsertAnimation);
                        EditorGUILayout.PropertyField(m_InsertSpeed);
                        EditorGUILayout.PropertyField(m_InsertSound);
                        EditorGUILayout.PropertyField(m_InsertVolume);
                        EditorGUILayout.Space();
                        EditorGUILayout.PropertyField(m_StopReloadAnimation);
                        EditorGUILayout.PropertyField(m_StopReloadSpeed);
                        EditorGUILayout.PropertyField(m_StopReloadSound);
                        EditorGUILayout.PropertyField(m_StopReloadVolume);
                    }
                }
            }

            EditorGUI.indentLevel = 0;
            EditorUtilities.ToggleHeader("Melee Attack", m_Melee);

            if (m_Melee.isExpanded)
            {
                EditorGUI.indentLevel = 1;
                using (new EditorGUI.DisabledScope(!m_Melee.boolValue))
                {
                    EditorGUILayout.PropertyField(m_MeleeAnimation);
                    EditorGUILayout.PropertyField(m_MeleeSpeed);
                    EditorGUILayout.PropertyField(m_MeleeSound);
                    EditorGUILayout.PropertyField(m_MeleeVolume);

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(m_MeleeDelay);

                    EditorGUILayout.Space();
                    EditorGUI.indentLevel = 0;
                    m_HitSoundList.DoLayoutList();
                    EditorGUI.indentLevel = 1;
                    EditorGUILayout.PropertyField(m_HitVolume);
                }
            }

            if (m_Target.HasSecondaryMode)
            {
                EditorGUI.indentLevel = 0;
                EditorUtilities.ToggleHeader("Switch Mode", m_SwitchMode);

                if (m_SwitchMode.isExpanded)
                {
                    EditorGUI.indentLevel = 1;
                    using (new EditorGUI.DisabledScope(!m_SwitchMode.boolValue))
                    {
                        EditorGUILayout.PropertyField(m_SwitchModeAnimation);
                        EditorGUILayout.PropertyField(m_SwitchModeSpeed);
                        EditorGUILayout.PropertyField(m_SwitchModeSound);
                        EditorGUILayout.PropertyField(m_SwitchModeVolume);
                    }
                }
            }

            EditorGUI.indentLevel = 0;
            EditorUtilities.ToggleHeader("Interact", m_Interact);

            if (m_Interact.isExpanded)
            {
                EditorGUI.indentLevel = 1;
                using (new EditorGUI.DisabledScope(!m_Interact.boolValue))
                {
                    EditorGUILayout.PropertyField(m_InteractAnimation);
                    EditorGUILayout.PropertyField(m_InteractSpeed);
                    EditorGUILayout.PropertyField(m_InteractSound);
                    EditorGUILayout.PropertyField(m_InteractVolume);

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(m_InteractDelay);
                }
            }

            EditorGUI.indentLevel = 0;
            EditorUtilities.ToggleHeader("Vault", m_Vault);

            if (m_Vault.isExpanded)
            {
                EditorGUI.indentLevel = 1;
                using (new EditorGUI.DisabledScope(!m_Vault.boolValue))
                {
                    EditorGUILayout.PropertyField(m_VaultAnimation);
                    EditorGUILayout.PropertyField(m_VaultSpeed);
                    EditorGUILayout.PropertyField(m_VaultSound);
                    EditorGUILayout.PropertyField(m_VaultVolume);
                }
            }
            
            EditorUtilities.DrawSplitter();
            
            #endregion
        }

        private void Effects()
        {
            EditorGUI.indentLevel = 0;
            EditorUtilities.ToggleHeader("Muzzle Blast", m_MuzzleFlash);

            if (m_MuzzleFlash.isExpanded)
            {
                EditorGUI.indentLevel = 1;
                using (new EditorGUI.DisabledScope(!m_MuzzleFlash.boolValue))
                {
                    EditorGUILayout.PropertyField(m_MuzzleParticle);
                }
            }

            EditorGUI.indentLevel = 0;
            EditorUtilities.ToggleHeader("Tracer", m_Tracer);

            if (m_Tracer.isExpanded)
            {
                EditorGUI.indentLevel = 1;
                using (new EditorGUI.DisabledScope(!m_Tracer.boolValue))
                {
                    EditorGUILayout.PropertyField(m_TracerPrefab);
                    EditorGUILayout.PropertyField(m_TracerDuration);

                    EditorGUILayout.PropertyField(m_TracerSpeed);

                    if (m_TracerSpeed.floatValue < 0)
                        EditorGUILayout.HelpBox("Tracer Speed must be greater than 0.", MessageType.Warning);

                    EditorGUILayout.PropertyField(m_TracerOrigin);
                }
            }

            EditorGUI.indentLevel = 0;
            EditorUtilities.ToggleHeader("Shell Ejection", m_Shell);

            if (m_Shell.isExpanded)
            {
                EditorGUI.indentLevel = 1;
                using (new EditorGUI.DisabledScope(!m_Shell.boolValue))
                {
                    EditorGUILayout.PropertyField(m_ShellParticle);
                    EditorGUILayout.PropertyField(m_ShellSpeed);
                    
                    //EditorUtilities.MinMaxSlider("Speed", m_MinShellSpeed, m_MaxShellSpeed, 0, 5);
                    EditorGUILayout.PropertyField(m_StartDelay);
                }
            }

            if (m_Target.ReloadType == GunData.ReloadMode.Magazines)
            {
                EditorGUI.indentLevel = 0;
                EditorUtilities.ToggleHeader("Magazine Drop", m_MagazineDrop);

                if (m_MagazineDrop.isExpanded)
                {
                    EditorGUI.indentLevel = 1;
                    using (new EditorGUI.DisabledScope(!m_MagazineDrop.boolValue))
                    {
                        EditorGUILayout.PropertyField(m_MagazinePrefab);
                        EditorGUILayout.PropertyField(m_DropOrigin);
                        EditorGUILayout.PropertyField(m_MaxMagazinesPrefabs);

                        EditorGUI.indentLevel = 0;
                        using (new EditorGUILayout.VerticalScope(Styling.background))
                        {
                            EditorGUILayout.PropertyField(m_TacticalReloadDrop);

                            if (m_TacticalReloadDrop.boolValue)
                            {
                                EditorGUI.indentLevel = 1;
                                EditorGUILayout.PropertyField(m_TacticalDropDelay);
                            }
                        }
                        
                        EditorGUI.indentLevel = 0;
                        using (new EditorGUILayout.VerticalScope(Styling.background))
                        {
                            EditorGUILayout.PropertyField(m_FullReloadDrop);

                            if (m_FullReloadDrop.boolValue)
                            {
                                EditorGUI.indentLevel = 1;
                                EditorGUILayout.PropertyField(m_FullDropDelay);
                            }
                        }
                        EditorGUI.indentLevel = 1;
                    }
                }
            }
            EditorUtilities.DrawSplitter();
            EditorGUI.indentLevel = 0;
        }
    }
}