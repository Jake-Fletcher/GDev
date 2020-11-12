//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using FPSBuilder.Editor;
using UnityEditor;

namespace FPSBuilder.Core.Player.Editor
{
    [CustomEditor(typeof(FirstPersonCharacterController))]
    public sealed class FirstPersonCharacterControllerEditor : UnityEditor.Editor
    {
        private SerializedProperty m_CharacterHeight;
        private SerializedProperty m_CharacterShoulderWidth;
        private SerializedProperty m_CharacterWeight;
        private SerializedProperty m_AllowJump;
        private SerializedProperty m_AllowRun;
        private SerializedProperty m_AllowCrouch;
        private SerializedProperty m_CrouchingHeight;
        private SerializedProperty m_CrouchingSpeed;
        
        private SerializedProperty m_MaxWeight;
        private SerializedProperty m_MaxSpeedLoss;

        private SerializedProperty m_WeightAffectSpeed;
        private SerializedProperty m_WeightAffectJump;

        private SerializedProperty m_WalkingForce;
        private SerializedProperty m_CrouchForce;
        private SerializedProperty m_RunMultiplier;
        private SerializedProperty m_AirControlPercent;
        private SerializedProperty m_JumpForce;
        private SerializedProperty m_JumpDelay;

        private SerializedProperty m_SlopeLimit;
        private SerializedProperty m_StepOffset;

        private SerializedProperty m_Ladder;
        private SerializedProperty m_ClimbingSpeed;

        private SerializedProperty m_Sliding;
        private SerializedProperty m_SlidingThrust;
        private SerializedProperty m_SlidingDistance;
        private SerializedProperty m_SlidingSlopeLimit;
        private SerializedProperty m_DelayToGetUp;
        private SerializedProperty m_StandAfterSliding;
        private SerializedProperty m_OverrideCameraPitchLimit;
        private SerializedProperty m_SlidingCameraPitch;

        private SerializedProperty m_HeightThreshold;
        private SerializedProperty m_DamageMultiplier;

        private SerializedProperty m_MainCamera;
        private SerializedProperty m_CameraController;

        #region CAMERA CONTROLLER

        private SerializedProperty m_YawSensitivity;
        private SerializedProperty m_PitchSensitivity;
        private SerializedProperty m_AimingYawSensitivity;
        private SerializedProperty m_AimingPitchSensitivity;
        private SerializedProperty m_LimitPitchRotation;
        private SerializedProperty m_PitchLimit;
        private SerializedProperty m_Smoothness;

        #endregion

        private SerializedProperty m_Stamina;
        private SerializedProperty m_MaxStaminaAmount;
        private SerializedProperty m_IncrementRatio;
        private SerializedProperty m_DecrementRatio;
        private SerializedProperty m_Fatigue;
        private SerializedProperty m_StaminaThreshold;
        private SerializedProperty m_BreathSound;
        private SerializedProperty m_MaximumBreathVolume;

        private SerializedProperty m_Vault;
        private SerializedProperty m_AffectedLayers;
        private SerializedProperty m_AllowWallJumping;
        private SerializedProperty m_VaultAnimationCurve;
        private SerializedProperty m_VaultDuration;

        private SerializedProperty m_Footsteps;

        private SerializedProperty m_AutomaticallyCalculateIntervals;
        private SerializedProperty m_WalkingBaseInterval;
        private SerializedProperty m_RunningBaseInterval;
        private SerializedProperty m_CrouchBaseInterval;
        private SerializedProperty m_ClimbingInterval;
        
        private SerializedProperty m_WalkingVolume;
        private SerializedProperty m_CrouchVolume;
        private SerializedProperty m_RunningVolume;
        private SerializedProperty m_JumpVolume;
        private SerializedProperty m_LandingVolume;
        private SerializedProperty m_CrouchingDownSound;
        private SerializedProperty m_StandingUpSound;
        private SerializedProperty m_CrouchingVolume;
        private SerializedProperty m_SlidingVolume;

        private void OnEnable ()
        {
            //Setup the SerializedProperties
            m_CharacterHeight = serializedObject.FindProperty("m_CharacterHeight");
            m_CharacterShoulderWidth = serializedObject.FindProperty("m_CharacterShoulderWidth");
            m_CharacterWeight = serializedObject.FindProperty("m_CharacterWeight");
            m_AllowJump = serializedObject.FindProperty("m_AllowJump");
            m_AllowRun = serializedObject.FindProperty("m_AllowRun");
            m_AllowCrouch = serializedObject.FindProperty("m_AllowCrouch");
            m_CrouchingHeight = serializedObject.FindProperty("m_CrouchingHeight");
            m_CrouchingSpeed = serializedObject.FindProperty("m_CrouchingSpeed");
            
            m_MaxWeight = serializedObject.FindProperty("m_MaxWeight");
            m_MaxSpeedLoss = serializedObject.FindProperty("m_MaxSpeedLoss");

            m_WeightAffectSpeed = serializedObject.FindProperty("m_WeightAffectSpeed");
            m_WeightAffectJump = serializedObject.FindProperty("m_WeightAffectJump");

            m_WalkingForce = serializedObject.FindProperty("m_WalkingForce");
            m_CrouchForce = serializedObject.FindProperty("m_CrouchForce");
            m_RunMultiplier = serializedObject.FindProperty("m_RunMultiplier");
            m_AirControlPercent = serializedObject.FindProperty("m_AirControlPercent");
            m_JumpForce = serializedObject.FindProperty("m_JumpForce");
            m_JumpDelay = serializedObject.FindProperty("m_JumpDelay");
            m_SlopeLimit = serializedObject.FindProperty("m_SlopeLimit");
            m_StepOffset = serializedObject.FindProperty("m_StepOffset");

            m_Ladder = serializedObject.FindProperty("m_Ladder");
            m_ClimbingSpeed = serializedObject.FindProperty("m_ClimbingSpeed");

            m_Sliding = serializedObject.FindProperty("m_Sliding");
            m_SlidingThrust = serializedObject.FindProperty("m_SlidingThrust");
            m_SlidingDistance = serializedObject.FindProperty("m_SlidingDistance");
            m_SlidingSlopeLimit = serializedObject.FindProperty("m_SlidingSlopeLimit");
            m_DelayToGetUp = serializedObject.FindProperty("m_DelayToGetUp");
            m_StandAfterSliding = serializedObject.FindProperty("m_StandAfterSliding");
            m_OverrideCameraPitchLimit = serializedObject.FindProperty("m_OverrideCameraPitchLimit");
            m_SlidingCameraPitch = serializedObject.FindProperty("m_SlidingCameraPitch");

            m_HeightThreshold = serializedObject.FindProperty("m_HeightThreshold");
            m_DamageMultiplier = serializedObject.FindProperty("m_DamageMultiplier");

            m_MainCamera = serializedObject.FindProperty("m_MainCamera");

            m_CameraController = serializedObject.FindProperty("m_CameraController");

            m_YawSensitivity = m_CameraController.FindPropertyRelative("m_YawSensitivity");
            m_PitchSensitivity = m_CameraController.FindPropertyRelative("m_PitchSensitivity");

            m_AimingYawSensitivity = m_CameraController.FindPropertyRelative("m_AimingYawSensitivity");
            m_AimingPitchSensitivity = m_CameraController.FindPropertyRelative("m_AimingPitchSensitivity");

            m_LimitPitchRotation = m_CameraController.FindPropertyRelative("m_LimitPitchRotation");
            m_PitchLimit = m_CameraController.FindPropertyRelative("m_PitchLimit");
            m_Smoothness = m_CameraController.FindPropertyRelative("m_Smoothness");

            m_Stamina = serializedObject.FindProperty("m_Stamina");
            m_MaxStaminaAmount = serializedObject.FindProperty("m_MaxStaminaAmount");
            m_IncrementRatio = serializedObject.FindProperty("m_IncrementRatio");
            m_DecrementRatio = serializedObject.FindProperty("m_DecrementRatio");
            m_Fatigue = serializedObject.FindProperty("m_Fatigue");
            m_StaminaThreshold = serializedObject.FindProperty("m_StaminaThreshold");
            m_BreathSound = serializedObject.FindProperty("m_BreathSound");
            m_MaximumBreathVolume = serializedObject.FindProperty("m_MaximumBreathVolume");

            m_Vault = serializedObject.FindProperty("m_Vault");
            m_AffectedLayers = serializedObject.FindProperty("m_AffectedLayers");
            m_AllowWallJumping = serializedObject.FindProperty("m_AllowWallJumping");
            m_VaultAnimationCurve = serializedObject.FindProperty("m_VaultAnimationCurve");
            m_VaultDuration = serializedObject.FindProperty("m_VaultDuration");

            m_Footsteps = serializedObject.FindProperty("m_Footsteps");

            m_AutomaticallyCalculateIntervals = serializedObject.FindProperty("m_AutomaticallyCalculateIntervals");
            m_WalkingBaseInterval = serializedObject.FindProperty("m_WalkingBaseInterval");
            m_RunningBaseInterval = serializedObject.FindProperty("m_RunningBaseInterval");
            m_CrouchBaseInterval = serializedObject.FindProperty("m_CrouchBaseInterval");
            m_ClimbingInterval = serializedObject.FindProperty("m_ClimbingInterval");
            
            m_WalkingVolume = serializedObject.FindProperty("m_WalkingVolume");
            m_CrouchVolume = serializedObject.FindProperty("m_CrouchVolume");
            m_RunningVolume = serializedObject.FindProperty("m_RunningVolume");
            m_JumpVolume = serializedObject.FindProperty("m_JumpVolume");
            m_LandingVolume = serializedObject.FindProperty("m_LandingVolume");

            m_CrouchingDownSound = serializedObject.FindProperty("m_CrouchingDownSound");
            m_StandingUpSound = serializedObject.FindProperty("m_StandingUpSound");
            m_CrouchingVolume = serializedObject.FindProperty("m_CrouchingVolume");
            
            m_SlidingVolume = serializedObject.FindProperty("m_SlidingVolume");
        }

        public override void OnInspectorGUI ()
        {
            // Update the serializedProperty - always do this in the beginning of OnInspectorGUI
            serializedObject.Update();

            EditorGUI.indentLevel = 0;
            EditorUtilities.FoldoutHeader("Character Settings", m_CharacterHeight);

            if (m_CharacterHeight.isExpanded)
            {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(m_CharacterHeight, new UnityEngine.GUIContent("Height (m)"));
                EditorGUILayout.PropertyField(m_CharacterShoulderWidth, new UnityEngine.GUIContent("Shoulder Width (m)"));
                EditorGUILayout.PropertyField(m_CharacterWeight, new UnityEngine.GUIContent("Base Weight (kg)"));

                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(m_AllowJump);
                EditorGUILayout.PropertyField(m_AllowRun);

                using (new EditorGUILayout.VerticalScope(Styling.background))
                {
                    EditorGUI.indentLevel = 0;
                    EditorGUILayout.PropertyField(m_AllowCrouch);

                    if (m_AllowCrouch.boolValue)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(m_CrouchingHeight, new UnityEngine.GUIContent("Crouching Height (m)"));
                        EditorGUILayout.PropertyField(m_CrouchingSpeed);
                        EditorGUI.indentLevel--;
                    }
                }
                
                EditorGUI.indentLevel = 1;
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(m_MaxWeight, new UnityEngine.GUIContent("Max Weight (kg)"));
                EditorGUILayout.PropertyField(m_MaxSpeedLoss);

                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(m_SlopeLimit);
                EditorGUILayout.PropertyField(m_StepOffset);
            }

            EditorGUI.indentLevel = 0;
            EditorUtilities.FoldoutHeader("Movement Settings", m_WalkingForce);

            if (m_WalkingForce.isExpanded)
            {
                EditorGUI.indentLevel = 1;

                EditorGUILayout.PropertyField(m_WalkingForce);
                if (m_AllowCrouch.boolValue)
                    EditorGUILayout.PropertyField(m_CrouchForce);
                if (m_AllowRun.boolValue)
                    EditorGUILayout.PropertyField(m_RunMultiplier);

                EditorGUILayout.Space();
                if (m_AllowJump.boolValue)
                {
                    EditorGUILayout.PropertyField(m_JumpForce);
                    EditorGUILayout.PropertyField(m_JumpDelay);
                }

                EditorGUILayout.PropertyField(m_AirControlPercent);

                EditorGUI.indentLevel = 0;
                using (new EditorGUILayout.VerticalScope(Styling.background))
                {
                    EditorUtilities.SimpleFoldoutHeader("Fall Settings", m_HeightThreshold);

                    if (m_HeightThreshold.isExpanded)
                    {
                        EditorGUI.indentLevel = 2;
                        EditorGUILayout.PropertyField(m_HeightThreshold);
                        EditorGUILayout.PropertyField(m_DamageMultiplier);
                    }
                    EditorGUI.indentLevel = 1;
                }
            }

            EditorGUI.indentLevel = 0;
            EditorUtilities.FoldoutHeader("Camera Settings", m_CameraController);

            if (m_CameraController.isExpanded)
            {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(m_MainCamera);
                EditorGUILayout.PropertyField(m_Smoothness);

                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(m_YawSensitivity, new UnityEngine.GUIContent("Yaw Sensitivity (X-Pivot)"));
                EditorGUILayout.PropertyField(m_PitchSensitivity, new UnityEngine.GUIContent("Pitch Sensitivity (Y-Pivot)"));

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(m_AimingYawSensitivity, new UnityEngine.GUIContent("Aiming Yaw Sensitivity (X-Pivot)"));
                EditorGUILayout.PropertyField(m_AimingPitchSensitivity, new UnityEngine.GUIContent("Aiming Pitch Sensitivity (Y-Pivot)"));

                EditorGUI.indentLevel = 0;
                using (new EditorGUILayout.VerticalScope(Styling.background))
                {
                    EditorGUILayout.PropertyField(m_LimitPitchRotation);

                    if (m_LimitPitchRotation.boolValue)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(m_PitchLimit);
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel = 1;
            }

            EditorGUI.indentLevel = 0;
            EditorUtilities.ToggleHeader("Climbing", m_Ladder);

            if (m_Ladder.isExpanded)
            {
                EditorGUI.indentLevel = 1;
                using (new EditorGUI.DisabledScope(!m_Ladder.boolValue))
                {
                    EditorGUILayout.PropertyField(m_ClimbingSpeed);
                }
            }

            EditorGUI.indentLevel = 0;
            EditorUtilities.ToggleHeader("Sliding", m_Sliding);

            if (m_Sliding.isExpanded)
            {
                EditorGUI.indentLevel = 1;
                using (new EditorGUI.DisabledScope(!m_Sliding.boolValue))
                {
                    EditorGUILayout.PropertyField(m_SlidingThrust);
                    if (m_SlidingThrust.floatValue <= m_CrouchForce.floatValue)
                        EditorGUILayout.HelpBox("Sliding Thrust should be greater than Crouch Force", MessageType.Warning);
                    
                    EditorGUILayout.PropertyField(m_SlidingDistance);
                    EditorGUILayout.PropertyField(m_DelayToGetUp);
                    EditorGUILayout.PropertyField(m_StandAfterSliding);
                    EditorGUILayout.PropertyField(m_SlidingSlopeLimit);

                    EditorGUI.indentLevel = 0;
                    using (new EditorGUILayout.VerticalScope(Styling.background))
                    {
                        EditorGUILayout.PropertyField(m_OverrideCameraPitchLimit, new UnityEngine.GUIContent("Override Pitch Limit"));
                        if (m_OverrideCameraPitchLimit.boolValue)
                        {
                            EditorGUI.indentLevel++;
                            EditorGUILayout.PropertyField(m_SlidingCameraPitch);
                            EditorGUI.indentLevel--;
                        }
                    }
                    EditorGUI.indentLevel = 1;
                }
            }

            EditorGUI.indentLevel = 0;
            EditorUtilities.ToggleHeader("Stamina", m_Stamina);

            if (m_Stamina.isExpanded)
            {
                EditorGUI.indentLevel = 1;
                using (new EditorGUI.DisabledScope(!m_Stamina.boolValue))
                {
                    EditorGUILayout.PropertyField(m_MaxStaminaAmount, new UnityEngine.GUIContent("Stamina Amount"));
                    EditorGUILayout.PropertyField(m_IncrementRatio, new UnityEngine.GUIContent("Increment Rate"));
                    EditorGUILayout.PropertyField(m_DecrementRatio, new UnityEngine.GUIContent("Decrement Rate"));
                    
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(m_WeightAffectSpeed, new UnityEngine.GUIContent("Extra Weight Affect Speed"));
                    EditorGUILayout.PropertyField(m_WeightAffectJump, new UnityEngine.GUIContent("Extra Weight Affect Jump"));
                    
                    EditorGUILayout.Space();
                    using (new EditorGUILayout.VerticalScope(Styling.background))
                    {
                        EditorGUI.indentLevel = 0;
                        EditorGUILayout.PropertyField(m_Fatigue);

                        if (m_Fatigue.boolValue)
                        {
                            EditorGUI.indentLevel++;
                            EditorGUILayout.PropertyField(m_StaminaThreshold);
                            EditorGUILayout.PropertyField(m_BreathSound);
                            EditorGUILayout.PropertyField(m_MaximumBreathVolume);
                            EditorGUI.indentLevel--;
                        }
                    }
                }
            }

            EditorGUI.indentLevel = 0;
            EditorUtilities.ToggleHeader("Parkour", m_Vault);

            if (m_Vault.isExpanded)
            {
                EditorGUI.indentLevel = 1;
                using (new EditorGUI.DisabledScope(!m_Vault.boolValue))
                {
                    EditorGUILayout.PropertyField(m_AffectedLayers);
                    EditorGUILayout.PropertyField(m_AllowWallJumping);
                    EditorGUILayout.PropertyField(m_VaultAnimationCurve);
                    EditorGUILayout.PropertyField(m_VaultDuration);
                }
            }

            EditorGUI.indentLevel = 0;
            EditorUtilities.ToggleHeader("Footsteps", m_Footsteps);

            if (m_Footsteps.isExpanded)
            {
                EditorGUI.indentLevel = 1;
                using (new EditorGUI.DisabledScope(!m_Footsteps.boolValue))
                {
                    EditorGUILayout.PropertyField(m_WalkingVolume);
                    EditorGUILayout.PropertyField(m_RunningVolume);
                    EditorGUILayout.PropertyField(m_CrouchVolume);
                    
                    EditorGUILayout.Space();
                    EditorGUI.indentLevel = 0;
                    using (new EditorGUILayout.VerticalScope(Styling.background))
                    {
                        EditorGUILayout.PropertyField(m_AutomaticallyCalculateIntervals);

                        if (!m_AutomaticallyCalculateIntervals.boolValue)
                        {
                            EditorGUI.indentLevel++;
                            EditorGUILayout.PropertyField(m_WalkingBaseInterval);
                            EditorGUILayout.PropertyField(m_RunningBaseInterval);
                            EditorGUILayout.PropertyField(m_CrouchBaseInterval);
                            EditorGUILayout.PropertyField(m_ClimbingInterval);
                            EditorGUI.indentLevel--;
                        }
                    }

                    EditorGUI.indentLevel = 1;
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(m_JumpVolume);
                    EditorGUILayout.PropertyField(m_LandingVolume);
                    EditorGUILayout.PropertyField(m_SlidingVolume);

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(m_StandingUpSound);
                    EditorGUILayout.PropertyField(m_CrouchingDownSound);
                    EditorGUILayout.PropertyField(m_CrouchingVolume);
                }
            }
            
            EditorUtilities.DrawSplitter();

            //Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI
            serializedObject.ApplyModifiedProperties();
        }
    }
}