//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using FPSBuilder.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace FPSBuilder.Core.Player.Editor
{
    [CustomEditor(typeof(HealthController))]
    public sealed class HealthControllerEditor : UnityEditor.Editor
    {
        private HealthController m_Target;
        private SerializedProperty m_BodyParts;
        private ReorderableList m_BodyPartsList;

        private SerializedProperty m_HitSounds;
        private ReorderableList m_HitSoundsList;
        
        private SerializedProperty m_DamageSounds;
        private ReorderableList m_DamageSoundsList;

        private SerializedProperty m_HitVolume;
        private SerializedProperty m_BreakLegsSound;
        private SerializedProperty m_BreakLegsVolume;
        private SerializedProperty m_ExplosionNoiseVolume;
        private SerializedProperty m_HealSound;
        private SerializedProperty m_HealVolume;
        private SerializedProperty m_CoughSound;
        private SerializedProperty m_CoughVolume;
        private SerializedProperty m_ExplosionNoise;
        private SerializedProperty m_DeafnessDuration;
        private SerializedProperty m_NormalSnapshot;
        private SerializedProperty m_StunnedSnapshot;

        private SerializedProperty m_DeadCharacter;

        private static int m_ToolbarIndex;

        private void OnEnable ()
        {
            m_Target = (target as HealthController);
            m_BodyParts = serializedObject.FindProperty("m_BodyParts");

            m_HitSounds = serializedObject.FindProperty("m_HitSounds");
            m_DamageSounds = serializedObject.FindProperty("m_DamageSounds");
            m_HitVolume = serializedObject.FindProperty("m_HitVolume");
            m_BreakLegsSound = serializedObject.FindProperty("m_BreakLegsSound");
            m_BreakLegsVolume = serializedObject.FindProperty("m_BreakLegsVolume");
            m_HealSound = serializedObject.FindProperty("m_HealSound");
            m_HealVolume = serializedObject.FindProperty("m_HealVolume");

            m_CoughSound = serializedObject.FindProperty("m_CoughSound");
            m_CoughVolume = serializedObject.FindProperty("m_CoughVolume");

            m_ExplosionNoise = serializedObject.FindProperty("m_ExplosionNoise");
            m_ExplosionNoiseVolume = serializedObject.FindProperty("m_ExplosionNoiseVolume");
            m_DeafnessDuration = serializedObject.FindProperty("m_DeafnessDuration");
            m_NormalSnapshot = serializedObject.FindProperty("m_NormalSnapshot");
            m_StunnedSnapshot = serializedObject.FindProperty("m_StunnedSnapshot");

            m_DeadCharacter = serializedObject.FindProperty("m_DeadCharacter");
            
            // Reorderable List
            m_BodyPartsList = new ReorderableList(serializedObject, m_BodyParts, false, true, true, true)
            {
                drawHeaderCallback = rect =>
                {
                    EditorGUI.LabelField(rect, "Body Part List", EditorStyles.boldLabel);
                },

                drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    var element = m_BodyPartsList.serializedProperty.GetArrayElementAtIndex(index);
                    var displayName = element.objectReferenceValue ? ((DamageHandler)element.objectReferenceValue).BodyPart.ToString() : element.displayName;
                    rect.y += 2;

                    if (element.objectReferenceValue != null)
                    {
                        EditorGUI.LabelField(new Rect(rect.x, rect.y, 96, EditorGUIUtility.singleLineHeight), displayName == "FullBody" ? "Full Body" : displayName);
                    }
                    else
                    {
                        EditorGUI.LabelField(new Rect(rect.x, rect.y, 96, EditorGUIUtility.singleLineHeight), "Body Part");
                    }

                    EditorGUI.PropertyField(new Rect(rect.x + 96, rect.y, rect.width - 96, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
                },

                onSelectCallback = list =>
                {
                    var prefab = list.serializedProperty.GetArrayElementAtIndex(list.index).objectReferenceValue as GameObject;
                    if (prefab != null)
                        EditorGUIUtility.PingObject(prefab.gameObject);
                },

                onCanRemoveCallback = list => list.count > 0,
            };

            m_HitSoundsList = EditorUtilities.CreateReorderableList(m_HitSounds, "Sound");
            m_DamageSoundsList = EditorUtilities.CreateReorderableList(m_DamageSounds, "Sound");
        }

        public override void OnInspectorGUI ()
        {
            // Update the serializedProperty - always do this in the beginning of OnInspectorGUI
            serializedObject.Update();
            
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                m_ToolbarIndex = GUILayout.Toolbar(m_ToolbarIndex, new[] { "Body", "Sounds" }, GUILayout.Height(22), GUILayout.Width(256));
                GUILayout.FlexibleSpace();
            }

            EditorGUILayout.Space();
            switch (m_ToolbarIndex)
            {
                case 0:
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace();
                        GUILayout.Label(EditorUtilities.Load<Texture2D>("/Scripts/Editor Resources/", "human.png"));
                        GUILayout.FlexibleSpace();
                    }

                    EditorGUILayout.Space();
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace();
                        EditorGUI.ProgressBar(EditorUtilities.GetRect(200, 18), m_Target.EditorHealthPercent, "HP (" + (m_Target.EditorHealthPercent * 100).ToString("F0") + " / 100)");
                        GUILayout.FlexibleSpace();
                    }

                    GUILayout.FlexibleSpace();
                    EditorGUILayout.LabelField("Traumas (Debug Only)", EditorStyles.boldLabel);
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.Space();
                
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace();
                        GUILayout.Space(40);
                        EditorGUI.ToggleLeft(EditorUtilities.GetRect(100, 18), "Bleeding", m_Target.Bleeding);
                        EditorGUI.ToggleLeft(EditorUtilities.GetRect(100, 18), "Limping", m_Target.Limping);
                        EditorGUI.ToggleLeft(EditorUtilities.GetRect(100, 18), "Trembling", m_Target.Trembling);
                        GUILayout.FlexibleSpace();
                    }

                    EditorGUILayout.Space();
                    m_BodyPartsList.DoLayoutList();

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(m_DeadCharacter);
                    break;
                }
                case 1:
                    EditorGUILayout.PropertyField(m_NormalSnapshot);
                    EditorGUILayout.PropertyField(m_StunnedSnapshot);

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(m_HealSound);
                    EditorGUILayout.PropertyField(m_HealVolume);

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(m_CoughSound);
                    EditorGUILayout.PropertyField(m_CoughVolume);

                    EditorGUILayout.Space();
                    m_HitSoundsList.DoLayoutList();

                    EditorGUILayout.Space();
                    m_DamageSoundsList.DoLayoutList();
                    EditorGUILayout.PropertyField(m_HitVolume);

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(m_BreakLegsSound);
                    EditorGUILayout.PropertyField(m_BreakLegsVolume);

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(m_ExplosionNoise);
                    EditorGUILayout.PropertyField(m_ExplosionNoiseVolume);
                    EditorGUILayout.PropertyField(m_DeafnessDuration);
                    break;
            }

            //Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI
            serializedObject.ApplyModifiedProperties();
        }
    }
}