//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using FPSBuilder.Editor;
using UnityEditor;

namespace FPSBuilder.Core.Managers.Editor
{
    [CustomEditor(typeof(GameplaySettings))]
    public class GameplaySettingsEditor : UnityEditor.Editor
    {
        private SerializedProperty m_AimStyle;
        private SerializedProperty m_CrouchStyle;
        private SerializedProperty m_SprintStyle;
        private SerializedProperty m_LeanStyle;
        private SerializedProperty m_FieldOfView;

        private SerializedProperty m_OverallMouseSensitivity;
        private SerializedProperty m_InvertHorizontalAxis;
        private SerializedProperty m_InvertVerticalAxis;

        private void OnEnable ()
        {
            m_AimStyle = serializedObject.FindProperty("m_AimStyle");
            m_CrouchStyle = serializedObject.FindProperty("m_CrouchStyle");
            m_SprintStyle = serializedObject.FindProperty("m_SprintStyle");
            m_LeanStyle = serializedObject.FindProperty("m_LeanStyle");
            m_FieldOfView = serializedObject.FindProperty("m_FieldOfView");

            m_OverallMouseSensitivity = serializedObject.FindProperty("m_OverallMouseSensitivity");
            m_InvertHorizontalAxis = serializedObject.FindProperty("m_InvertHorizontalAxis");
            m_InvertVerticalAxis = serializedObject.FindProperty("m_InvertVerticalAxis");
        }

        public override void OnInspectorGUI ()
        {
            serializedObject.Update();
            
            EditorGUI.LabelField(EditorUtilities.GetRect(36), "Gameplay Settings", Styling.headerLabel);

            EditorGUILayout.Space();
            using (new EditorGUILayout.VerticalScope())
            {
                EditorGUILayout.PropertyField(m_FieldOfView);

                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(m_AimStyle);
                EditorGUILayout.PropertyField(m_CrouchStyle);
                EditorGUILayout.PropertyField(m_SprintStyle);
                EditorGUILayout.PropertyField(m_LeanStyle);

                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(m_OverallMouseSensitivity, new UnityEngine.GUIContent("Mouse Sensitivity"));
                EditorGUILayout.PropertyField(m_InvertHorizontalAxis);
                EditorGUILayout.PropertyField(m_InvertVerticalAxis);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}