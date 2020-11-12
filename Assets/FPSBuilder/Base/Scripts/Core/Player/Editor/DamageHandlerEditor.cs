//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using FPSBuilder.Editor;
using UnityEditor;

namespace FPSBuilder.Core.Player.Editor
{
    [CustomEditor(typeof(DamageHandler))]
    public sealed class DamageHandlerEditor : UnityEditor.Editor
    {
        private SerializedProperty m_BodyPart;
        private SerializedProperty m_Vital;
        private SerializedProperty m_Vitality;

        private SerializedProperty m_AllowInjury;
        private SerializedProperty m_BleedChance;
        private SerializedProperty m_BleedThreshold;
        private SerializedProperty m_DamageThreshold;
        private SerializedProperty m_TraumaType;
        private SerializedProperty m_BleedRate;

        private SerializedProperty m_AllowRegeneration;
        private SerializedProperty m_StartDelay;
        private SerializedProperty m_RegenerationRate;

        private void OnEnable ()
        {
            m_BodyPart = serializedObject.FindProperty("m_BodyPart");
            m_Vital = serializedObject.FindProperty("m_Vital");
            m_Vitality = serializedObject.FindProperty("m_Vitality");

            m_AllowInjury = serializedObject.FindProperty("m_AllowInjury");
            m_BleedChance = serializedObject.FindProperty("m_BleedChance");
            m_BleedThreshold = serializedObject.FindProperty("m_BleedThreshold");
            m_DamageThreshold = serializedObject.FindProperty("m_DamageThreshold");
            m_TraumaType = serializedObject.FindProperty("m_TraumaType");
            m_BleedRate = serializedObject.FindProperty("m_BleedRate");

            m_AllowRegeneration = serializedObject.FindProperty("m_AllowRegeneration");
            m_StartDelay = serializedObject.FindProperty("m_StartDelay");
            m_RegenerationRate = serializedObject.FindProperty("m_RegenerationRate");
        }

        public override void OnInspectorGUI ()
        {
            //Update the serializedProperty - always do this in the beginning of OnInspectorGUI
            serializedObject.Update();

            EditorGUI.indentLevel = 0;
            EditorGUILayout.PropertyField(m_BodyPart);
            EditorGUILayout.PropertyField(m_Vital);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_Vitality, new UnityEngine.GUIContent("Vitality (Hit Points)"));

            using (new EditorGUILayout.VerticalScope(Styling.background))
            {
                EditorGUILayout.PropertyField(m_AllowRegeneration);

                if (m_AllowRegeneration.boolValue)
                {
                    EditorGUI.indentLevel = 1;
                    EditorGUILayout.PropertyField(m_StartDelay);
                    EditorGUILayout.PropertyField(m_RegenerationRate);
                }

                EditorGUI.indentLevel = 0;
            }

            using (new EditorGUILayout.VerticalScope(Styling.background))
            {
                EditorGUILayout.PropertyField(m_AllowInjury);

                if (m_AllowInjury.boolValue)
                {
                    EditorGUI.indentLevel = 1;
                    EditorGUILayout.PropertyField(m_TraumaType);

                    EditorGUILayout.PropertyField(m_DamageThreshold);

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(m_BleedThreshold);
                    EditorGUILayout.PropertyField(m_BleedChance);

                    if (m_BleedChance.floatValue > 0)
                    {
                        EditorGUILayout.PropertyField(m_BleedRate);
                    }
                }
                EditorGUI.indentLevel = 0;
            }

            //Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI
            serializedObject.ApplyModifiedProperties();
        }
    }
}