//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using FPSBuilder.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace FPSBuilder.Core.Surface.Editor
{
    [CustomEditor(typeof(SurfaceType))]
    public sealed class SurfaceTypeEditor : UnityEditor.Editor
    {
        private SerializedProperty m_FootstepsSounds;
        private ReorderableList m_FootstepsSoundsList;

        private SerializedProperty m_SprintingFootstepsSounds;
        private ReorderableList m_SprintingFootstepsSoundsList;
        
        private SerializedProperty m_JumpSounds;
        private ReorderableList m_JumpSoundsList;

        private SerializedProperty m_LandingSounds;
        private ReorderableList m_LandingSoundsList;
        
        private SerializedProperty m_SlidingSounds;
        private ReorderableList m_SlidingSoundsList;

        private SerializedProperty m_BulletImpactMaterial;
        private ReorderableList m_BulletImpactMaterialList;

        private SerializedProperty m_DecalSize;

        private SerializedProperty m_BulletImpactParticle;
        private ReorderableList m_BulletImpactParticleList;

        private SerializedProperty m_BulletImpactSound;
        private ReorderableList m_BulletImpactSoundList;
        private SerializedProperty m_BulletImpactVolume;

        private void OnEnable ()
        {
            m_FootstepsSounds = serializedObject.FindProperty("m_FootstepsSounds");
            m_SprintingFootstepsSounds = serializedObject.FindProperty("m_SprintingFootstepsSounds");
            m_JumpSounds = serializedObject.FindProperty("m_JumpSounds");
            m_LandingSounds = serializedObject.FindProperty("m_LandingSounds");
            m_SlidingSounds = serializedObject.FindProperty("m_SlidingSounds");

            m_BulletImpactMaterial = serializedObject.FindProperty("m_BulletImpactMaterial");
            m_DecalSize = serializedObject.FindProperty("m_DecalSize");

            m_BulletImpactParticle = serializedObject.FindProperty("m_BulletImpactParticle");
            m_BulletImpactSound = serializedObject.FindProperty("m_BulletImpactSound");
            m_BulletImpactVolume = serializedObject.FindProperty("m_BulletImpactVolume");
            
            m_FootstepsSoundsList = EditorUtilities.CreateReorderableList(m_FootstepsSounds, "Sound", "Walking Sounds");
            m_SprintingFootstepsSoundsList = EditorUtilities.CreateReorderableList(m_SprintingFootstepsSounds, "Sound", "Sprinting Sounds");
            m_LandingSoundsList = EditorUtilities.CreateReorderableList(m_LandingSounds, "Sound");
            m_JumpSoundsList = EditorUtilities.CreateReorderableList(m_JumpSounds, "Sound");
            m_SlidingSoundsList = EditorUtilities.CreateReorderableList(m_SlidingSounds, "Sound");
            m_BulletImpactMaterialList = EditorUtilities.CreateReorderableList(m_BulletImpactMaterial, "Material");
            m_BulletImpactParticleList = EditorUtilities.CreateReorderableList(m_BulletImpactParticle, "Particle");
            m_BulletImpactSoundList = EditorUtilities.CreateReorderableList(m_BulletImpactSound, "Sound");
        }

        public override void OnInspectorGUI ()
        {
            serializedObject.Update();

            EditorGUI.LabelField(EditorUtilities.GetRect(36), "Footsteps Settings", Styling.headerLabel);

            EditorGUILayout.Space();
            m_FootstepsSoundsList.DoLayoutList();

            EditorGUILayout.Space();
            m_SprintingFootstepsSoundsList.DoLayoutList();
            
            EditorGUILayout.Space();
            m_JumpSoundsList.DoLayoutList();

            EditorGUILayout.Space();
            m_LandingSoundsList.DoLayoutList();
            
            EditorGUILayout.Space();
            m_SlidingSoundsList.DoLayoutList();

            EditorGUILayout.Space();
            EditorGUI.LabelField(EditorUtilities.GetRect(36), "Projectile Impact Settings", Styling.headerLabel);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_DecalSize, new GUIContent("Impact Decal Size"));

            EditorGUILayout.Space();
            m_BulletImpactMaterialList.DoLayoutList();

            EditorGUILayout.Space();
            m_BulletImpactParticleList.DoLayoutList();

            EditorGUILayout.Space();
            m_BulletImpactSoundList.DoLayoutList();
            EditorGUILayout.PropertyField(m_BulletImpactVolume);

            serializedObject.ApplyModifiedProperties();
        }
    }
}