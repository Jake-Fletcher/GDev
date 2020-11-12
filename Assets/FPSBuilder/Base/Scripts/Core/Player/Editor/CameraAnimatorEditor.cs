//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using FPSBuilder.Editor;
using UnityEditor;

namespace FPSBuilder.Core.Player.Editor
{
    [CustomEditor(typeof(CameraAnimator))]
    public sealed class CameraAnimatorEditor : UnityEditor.Editor
    {
        private CameraAnimator m_Target;

        private SerializedProperty m_FPController;
        private SerializedProperty m_HealthController;

        private SerializedProperty m_MotionAnimation;
        private SerializedProperty m_OverallSpeed;
        private SerializedProperty m_WalkingMotionData;
        private SerializedProperty m_BrokenLegsMotionData;
        private SerializedProperty m_RunningMotionData;
        private SerializedProperty m_TargetTransform;

        private SerializedProperty m_BraceForJumpAnimation;
        private SerializedProperty m_JumpAnimation;
        private SerializedProperty m_LandingAnimation;

        private SerializedProperty m_BreathAnimation;
        private SerializedProperty m_BreathingSpeed;
        private SerializedProperty m_BreathingAmplitude;

        private SerializedProperty m_MinHitRotation;
        private SerializedProperty m_MaxHitRotation;
        private SerializedProperty m_HitDuration;

        private SerializedProperty m_VaultAnimation;

        private SerializedProperty m_ExplosionShake;

        private SerializedProperty m_BreathAnimationMode;

        private SerializedProperty m_Lean;
        private SerializedProperty m_LeanAmount;
        private SerializedProperty m_LeanAngle;
        private SerializedProperty m_LeanSpeed;

        private SerializedProperty m_HoldBreath;
        private SerializedProperty m_Exhale;
        private SerializedProperty m_HoldBreathVolume;

        private void OnEnable()
        {
            m_Target = (target as CameraAnimator);

            m_FPController = serializedObject.FindProperty("m_FPController");
            m_HealthController = serializedObject.FindProperty("m_HealthController");

            m_MotionAnimation = serializedObject.FindProperty("m_MotionAnimation");
            m_OverallSpeed = m_MotionAnimation.FindPropertyRelative("m_OverallSpeed");
            m_WalkingMotionData = m_MotionAnimation.FindPropertyRelative("m_WalkingMotionData");
            m_BrokenLegsMotionData = m_MotionAnimation.FindPropertyRelative("m_BrokenLegsMotionData");
            m_RunningMotionData = m_MotionAnimation.FindPropertyRelative("m_RunningMotionData");
            m_TargetTransform = m_MotionAnimation.FindPropertyRelative("m_TargetTransform");
            m_BraceForJumpAnimation = m_MotionAnimation.FindPropertyRelative("m_BraceForJumpAnimation");
            m_JumpAnimation = m_MotionAnimation.FindPropertyRelative("m_JumpAnimation");
            m_LandingAnimation = m_MotionAnimation.FindPropertyRelative("m_LandingAnimation");
            m_BreathAnimation = m_MotionAnimation.FindPropertyRelative("m_BreathAnimation");
            m_BreathingSpeed = m_MotionAnimation.FindPropertyRelative("m_BreathingSpeed");
            m_BreathingAmplitude = m_MotionAnimation.FindPropertyRelative("m_BreathingAmplitude");
            m_ExplosionShake = m_MotionAnimation.FindPropertyRelative("m_ExplosionShake");

            m_MinHitRotation = m_MotionAnimation.FindPropertyRelative("m_MinHitRotation");
            m_MaxHitRotation = m_MotionAnimation.FindPropertyRelative("m_MaxHitRotation");
            m_HitDuration = m_MotionAnimation.FindPropertyRelative("m_HitDuration");

            m_VaultAnimation = m_MotionAnimation.FindPropertyRelative("m_VaultAnimation");

            m_Lean = m_MotionAnimation.FindPropertyRelative("m_Lean");
            m_LeanAmount = m_MotionAnimation.FindPropertyRelative("m_LeanAmount");
            m_LeanAngle = m_MotionAnimation.FindPropertyRelative("m_LeanAngle");
            m_LeanSpeed = m_MotionAnimation.FindPropertyRelative("m_LeanSpeed");

            m_BreathAnimationMode = serializedObject.FindProperty("m_BreathAnimationMode");

            m_HoldBreath = serializedObject.FindProperty("m_HoldBreath");
            m_Exhale = serializedObject.FindProperty("m_Exhale");
            m_HoldBreathVolume = serializedObject.FindProperty("m_HoldBreathVolume");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(m_FPController, new UnityEngine.GUIContent("First Person Character"));
            EditorGUILayout.PropertyField(m_HealthController);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_TargetTransform);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_OverallSpeed);
            EditorGUILayout.PropertyField(m_WalkingMotionData);
            EditorGUILayout.PropertyField(m_BrokenLegsMotionData);
            EditorGUILayout.PropertyField(m_RunningMotionData);

            EditorGUILayout.Space();

            EditorUtilities.LerpAnimationDrawer("Brace For Jump Animation", m_BraceForJumpAnimation, true, m_Target.transform);

            EditorUtilities.LerpAnimationDrawer("Jump Animation", m_JumpAnimation, true, m_Target.transform);

            EditorUtilities.LerpAnimationDrawer("Landing Animation", m_LandingAnimation, true, m_Target.transform);

            EditorGUI.indentLevel = 0;
            EditorUtilities.ToggleHeader("Breath Animation", m_BreathAnimation);

            if (m_BreathAnimation.isExpanded)
            {
                EditorGUI.indentLevel = 1;
                using (new EditorGUI.DisabledScope(!m_BreathAnimation.boolValue))
                {
                    EditorGUILayout.PropertyField(m_BreathAnimationMode);
                    EditorGUILayout.PropertyField(m_BreathingSpeed);
                    EditorGUILayout.PropertyField(m_BreathingAmplitude);

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(m_HoldBreath, new UnityEngine.GUIContent("Hold Breath Sound"));
                    EditorGUILayout.PropertyField(m_Exhale, new UnityEngine.GUIContent("Exhale Sound"));
                    EditorGUILayout.PropertyField(m_HoldBreathVolume);
                }
            }

            EditorGUI.indentLevel = 0;
            EditorUtilities.ToggleHeader("Lean Animation", m_Lean);

            if (m_Lean.isExpanded)
            {
                EditorGUI.indentLevel = 1;
                using (new EditorGUI.DisabledScope(!m_Lean.boolValue))
                {
                    EditorGUILayout.PropertyField(m_LeanAmount);
                    EditorGUILayout.PropertyField(m_LeanAngle);
                    EditorGUILayout.PropertyField(m_LeanSpeed);
                }
            }

            EditorGUI.indentLevel = 0;
            EditorUtilities.FoldoutHeader("Hit Animation", m_MinHitRotation);

            if (m_MinHitRotation.isExpanded)
            {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(m_MinHitRotation);
                EditorGUILayout.PropertyField(m_MaxHitRotation);

                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(m_HitDuration);
            }

            EditorGUI.indentLevel = 0;
            EditorUtilities.LerpAnimationDrawer("Vault Animation", m_VaultAnimation);

            EditorGUI.indentLevel = 0;
            EditorUtilities.FoldoutHeader("Explosion Animation", m_ExplosionShake);

            if (m_ExplosionShake.isExpanded)
            {
                EditorGUI.indentLevel = 1;
                ShakePropertiesDrawer(m_ExplosionShake);
            }
            
            EditorUtilities.DrawSplitter();
            
            EditorGUI.indentLevel = 0;
            serializedObject.ApplyModifiedProperties();
        }

        private void ShakePropertiesDrawer(SerializedProperty property)
        {
            SerializedProperty m_Angle = property.FindPropertyRelative("m_Angle");
            SerializedProperty m_Strength = property.FindPropertyRelative("m_Strength");
            SerializedProperty m_MinSpeed = property.FindPropertyRelative("m_MinSpeed");
            SerializedProperty m_MaxSpeed = property.FindPropertyRelative("m_MaxSpeed");
            SerializedProperty m_Duration = property.FindPropertyRelative("m_Duration");
            SerializedProperty m_NoisePercent = property.FindPropertyRelative("m_NoisePercent");
            SerializedProperty m_DampingPercent = property.FindPropertyRelative("m_DampingPercent");
            SerializedProperty m_RotationPercent = property.FindPropertyRelative("m_RotationPercent");

            EditorGUILayout.PropertyField(m_Angle);
            EditorGUILayout.PropertyField(m_Strength);
            EditorGUILayout.PropertyField(m_MinSpeed);
            EditorGUILayout.PropertyField(m_MaxSpeed);
            EditorGUILayout.PropertyField(m_Duration);
            EditorGUILayout.PropertyField(m_NoisePercent);
            EditorGUILayout.PropertyField(m_DampingPercent);
            EditorGUILayout.PropertyField(m_RotationPercent);
        }
    }
}