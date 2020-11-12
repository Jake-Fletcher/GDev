//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using FPSBuilder.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace FPSBuilder.Core.Weapons.Editor
{
    [CustomEditor(typeof(Arms))]
    public sealed class ArmsEditor : UnityEditor.Editor
    {
        private Arms m_Target;
        private SerializedProperty m_CameraTransformReference;
        private SerializedProperty m_FPController;

        private SerializedProperty m_Range;
        private SerializedProperty m_AttackRate;
        private SerializedProperty m_Force;
        private SerializedProperty m_Damage;
        private SerializedProperty m_AffectedLayers;

        #region WEAPON SWING

        private SerializedProperty m_WeaponSwing;
        private SerializedProperty m_Swing;
        private SerializedProperty m_TiltAngle;
        private SerializedProperty m_SwingAngle;
        private SerializedProperty m_Speed;
        private SerializedProperty m_SwingTarget;
        private SerializedProperty m_TremorAmount;

        #endregion

        #region MOTION ANIMATIONS
        
        private Vector3 m_HIPPosition;
        private Vector3 m_HIPRotation;
        
        private SerializedProperty m_MotionAnimation;
        private SerializedProperty m_WalkingMotionData;
        private SerializedProperty m_BrokenLegsMotionData;
        private SerializedProperty m_RunningMotionData;
        private SerializedProperty m_TargetTransform;
        private SerializedProperty m_ScaleFactor;

        private SerializedProperty m_JumpAnimation;
        private SerializedProperty m_LandingAnimation;

        private SerializedProperty m_BreathAnimation;
        private SerializedProperty m_BreathingSpeed;
        private SerializedProperty m_BreathingAmplitude;

        #endregion

        private SerializedProperty m_ArmsAnimator;
        private SerializedProperty m_Animator;
        private SerializedProperty m_VelocityParameter;
        private SerializedProperty m_Draw;
        private SerializedProperty m_DrawAnimation;
        private SerializedProperty m_DrawSound;
        private SerializedProperty m_DrawVolume;

        private SerializedProperty m_Hide;
        private SerializedProperty m_HideAnimation;
        private SerializedProperty m_HideSound;
        private SerializedProperty m_HideVolume;

        private SerializedProperty m_Attack;

        private SerializedProperty m_RightAttackAnimations;
        private ReorderableList m_RightAttackAnimationList;

        private SerializedProperty m_LeftAttackAnimations;
        private ReorderableList m_LeftAttackAnimationList;

        private SerializedProperty m_AttackAnimationType;

        private SerializedProperty m_AttackSounds;
        private ReorderableList m_AttackSoundList;
        private SerializedProperty m_AttackVolume;

        private SerializedProperty m_HitSounds;
        private ReorderableList m_HitSoundList;
        private SerializedProperty m_HitVolume;

        private SerializedProperty m_Interact;
        private SerializedProperty m_InteractAnimation;
        private SerializedProperty m_InteractDelay;
        private SerializedProperty m_InteractSound;
        private SerializedProperty m_InteractVolume;

        private SerializedProperty m_Vault;
        private SerializedProperty m_VaultAnimation;
        private SerializedProperty m_VaultSound;
        private SerializedProperty m_VaultVolume;

        private void OnEnable ()
        {
            m_Target = (target as Arms);

            if (m_Target != null)
            {
                m_Target.DisableShadowCasting();
            
                m_HIPPosition = m_Target.transform.localPosition;
                
                // ReSharper disable once Unity.InefficientPropertyAccess
                m_HIPRotation = m_Target.transform.localRotation.eulerAngles;
            }

            m_CameraTransformReference = serializedObject.FindProperty("m_CameraTransformReference");
            m_FPController = serializedObject.FindProperty("m_FPController");
            m_Range = serializedObject.FindProperty("m_Range");
            m_AttackRate = serializedObject.FindProperty("m_AttackRate");
            m_Force = serializedObject.FindProperty("m_Force");
            m_Damage = serializedObject.FindProperty("m_Damage");
            m_AffectedLayers = serializedObject.FindProperty("m_AffectedLayers");

            m_WeaponSwing = serializedObject.FindProperty("m_WeaponSwing");
            m_Swing = m_WeaponSwing.FindPropertyRelative("m_Swing");
            m_TiltAngle = m_WeaponSwing.FindPropertyRelative("m_TiltAngle");
            m_SwingAngle = m_WeaponSwing.FindPropertyRelative("m_SwingAngle");
            m_Speed = m_WeaponSwing.FindPropertyRelative("m_Speed");
            m_SwingTarget = m_WeaponSwing.FindPropertyRelative("m_SwingTarget");
            m_TremorAmount = m_WeaponSwing.FindPropertyRelative("m_TremorAmount");

            m_MotionAnimation = serializedObject.FindProperty("m_MotionAnimation");
            m_ScaleFactor = m_MotionAnimation.FindPropertyRelative("m_ScaleFactor");
            m_WalkingMotionData = m_MotionAnimation.FindPropertyRelative("m_WalkingMotionData");
            m_BrokenLegsMotionData = m_MotionAnimation.FindPropertyRelative("m_BrokenLegsMotionData");
            m_RunningMotionData = m_MotionAnimation.FindPropertyRelative("m_RunningMotionData");
            m_TargetTransform = m_MotionAnimation.FindPropertyRelative("m_TargetTransform");
            m_JumpAnimation = m_MotionAnimation.FindPropertyRelative("m_JumpAnimation");
            m_LandingAnimation = m_MotionAnimation.FindPropertyRelative("m_LandingAnimation");
            m_BreathAnimation = m_MotionAnimation.FindPropertyRelative("m_BreathAnimation");
            m_BreathingSpeed = m_MotionAnimation.FindPropertyRelative("m_BreathingSpeed");
            m_BreathingAmplitude = m_MotionAnimation.FindPropertyRelative("m_BreathingAmplitude");

            m_ArmsAnimator = serializedObject.FindProperty("m_ArmsAnimator");
            m_Animator = m_ArmsAnimator.FindPropertyRelative("m_Animator");
            m_VelocityParameter = m_ArmsAnimator.FindPropertyRelative("m_VelocityParameter");

            m_Draw = m_ArmsAnimator.FindPropertyRelative("m_Draw");
            m_DrawAnimation = m_ArmsAnimator.FindPropertyRelative("m_DrawAnimation");
            m_DrawSound = m_ArmsAnimator.FindPropertyRelative("m_DrawSound");
            m_DrawVolume = m_ArmsAnimator.FindPropertyRelative("m_DrawVolume");

            m_Hide = m_ArmsAnimator.FindPropertyRelative("m_Hide");
            m_HideAnimation = m_ArmsAnimator.FindPropertyRelative("m_HideAnimation");
            m_HideSound = m_ArmsAnimator.FindPropertyRelative("m_HideSound");
            m_HideVolume = m_ArmsAnimator.FindPropertyRelative("m_HideVolume");

            m_Attack = m_ArmsAnimator.FindPropertyRelative("m_Attack");
            m_RightAttackAnimations = m_ArmsAnimator.FindPropertyRelative("m_RightAttackAnimationList");
            m_LeftAttackAnimations = m_ArmsAnimator.FindPropertyRelative("m_LeftAttackAnimationList");
            m_AttackAnimationType = m_ArmsAnimator.FindPropertyRelative("m_AttackAnimationType");
            m_AttackSounds = m_ArmsAnimator.FindPropertyRelative("m_AttackSoundList");
            m_AttackVolume = m_ArmsAnimator.FindPropertyRelative("m_AttackVolume");
            m_HitSounds = m_ArmsAnimator.FindPropertyRelative("m_HitSoundList");
            m_HitVolume = m_ArmsAnimator.FindPropertyRelative("m_HitVolume");

            m_Interact = m_ArmsAnimator.FindPropertyRelative("m_Interact");
            m_InteractAnimation = m_ArmsAnimator.FindPropertyRelative("m_InteractAnimation");
            m_InteractDelay = m_ArmsAnimator.FindPropertyRelative("m_InteractDelay");
            m_InteractSound = m_ArmsAnimator.FindPropertyRelative("m_InteractSound");
            m_InteractVolume = m_ArmsAnimator.FindPropertyRelative("m_InteractVolume");

            m_Vault = m_ArmsAnimator.FindPropertyRelative("m_Vault");
            m_VaultAnimation = m_ArmsAnimator.FindPropertyRelative("m_VaultAnimation");
            m_VaultSound = m_ArmsAnimator.FindPropertyRelative("m_VaultSound");
            m_VaultVolume = m_ArmsAnimator.FindPropertyRelative("m_VaultVolume");

            m_RightAttackAnimationList = EditorUtilities.CreateReorderableList(m_RightAttackAnimations, "Animation");
            m_LeftAttackAnimationList = EditorUtilities.CreateReorderableList(m_LeftAttackAnimations, "Animation");
            
            m_AttackSoundList = EditorUtilities.CreateReorderableList(m_AttackSounds, "Sound");
            m_HitSoundList = EditorUtilities.CreateReorderableList(m_HitSounds, "Sound");
        }

        private void OnDisable()
        {
            if (!m_Target)
                return;
            
            m_Target.transform.localPosition = m_HIPPosition;
                                
            // ReSharper disable once Unity.InefficientPropertyAccess
            m_Target.transform.localRotation = Quaternion.Euler(m_HIPRotation);
        }

        public override void OnInspectorGUI ()
        {
            serializedObject.Update();

            EditorGUILayout.Space();
            EditorUtilities.DrawHeader("Controllers & Managers");
            EditorGUILayout.PropertyField(m_FPController, new GUIContent("First Person Character"));
            EditorGUILayout.PropertyField(m_CameraTransformReference);

            EditorGUILayout.Space();

            using (new EditorGUILayout.VerticalScope(Styling.background))
            {
                EditorUtilities.SimpleFoldoutHeader("Attack Settings", m_Range);
                if (m_Range.isExpanded)
                {
                    EditorGUI.indentLevel = 1;
                    EditorGUILayout.PropertyField(m_Range);
                    EditorGUILayout.PropertyField(m_AttackRate);
                    EditorGUILayout.PropertyField(m_Force);
                    EditorGUILayout.PropertyField(m_Damage);
                    EditorGUILayout.PropertyField(m_AffectedLayers);
                    EditorGUI.indentLevel = 0;
                }
            }

            EditorGUILayout.Space();
            EditorUtilities.DrawHeader("Procedural Animations");
            Animations();

            serializedObject.ApplyModifiedProperties();
        }

        private void Animations ()
        {
            #region MOTION ANIMATION

            EditorGUI.indentLevel = 0;
            EditorUtilities.FoldoutHeader("Motion", m_MotionAnimation);

            if (m_MotionAnimation.isExpanded)
            {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(m_ScaleFactor);
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
            }

            #endregion

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

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(m_TremorAmount);

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(m_Speed);
                    EditorGUILayout.PropertyField(m_SwingTarget);
                }

            }
            #endregion
            
            EditorUtilities.DrawSplitter();

            EditorGUI.indentLevel = 0;
            EditorGUILayout.Space();
            EditorUtilities.DrawHeader("Arms Animations");
            EditorGUILayout.PropertyField(m_Animator);
            EditorGUILayout.PropertyField(m_VelocityParameter);

            EditorUtilities.ToggleHeader("Draw", m_Draw);

            if (m_Draw.isExpanded)
            {
                EditorGUI.indentLevel = 1;
                using (new EditorGUI.DisabledScope(!m_Draw.boolValue))
                {
                    EditorGUILayout.PropertyField(m_DrawAnimation);
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
                    EditorGUILayout.PropertyField(m_HideSound);
                    EditorGUILayout.PropertyField(m_HideVolume);
                }
            }

            EditorGUI.indentLevel = 0;
            EditorUtilities.ToggleHeader("Attack", m_Attack);

            if (m_Attack.isExpanded)
            {
                //EditorGUI.indentLevel = 1;
                using (new EditorGUI.DisabledScope(!m_Attack.boolValue))
                {
                    EditorGUILayout.Space();
                    m_RightAttackAnimationList.DoLayoutList();

                    EditorGUILayout.Space();
                    m_LeftAttackAnimationList.DoLayoutList();

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(m_AttackAnimationType);

                    EditorGUILayout.Space();
                    m_AttackSoundList.DoLayoutList();
                    EditorGUILayout.PropertyField(m_AttackVolume);

                    EditorGUILayout.Space();
                    m_HitSoundList.DoLayoutList();
                    EditorGUILayout.PropertyField(m_HitVolume);
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
                    EditorGUILayout.PropertyField(m_VaultSound);
                    EditorGUILayout.PropertyField(m_VaultVolume);
                }
            }
            
            EditorUtilities.DrawSplitter();
        }
    }
}