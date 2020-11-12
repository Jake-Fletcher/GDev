//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using UnityEditor;

namespace FPSBuilder.Core.Items.Editor
{
    [CustomEditor(typeof(Explosive))]
    public sealed class ExplosiveEditor : UnityEditor.Editor
    {
        private SerializedProperty m_ExplosionRadius;
        private SerializedProperty m_ExplosionForce;
        private SerializedProperty m_IgnoreCover;
        private SerializedProperty m_Damage;
        private SerializedProperty m_ExplodeWhenCollide;
        private SerializedProperty m_TimeToExplode;
        private SerializedProperty m_ExplosionParticle;
        private SerializedProperty m_ExplosionSound;
        private SerializedProperty m_ExplosionVolume;

        private void OnEnable ()
        {
            m_ExplosionRadius = serializedObject.FindProperty("m_ExplosionRadius");
            m_ExplosionForce = serializedObject.FindProperty("m_ExplosionForce");
            m_IgnoreCover = serializedObject.FindProperty("m_IgnoreCover");
            m_Damage = serializedObject.FindProperty("m_Damage");
            m_ExplodeWhenCollide = serializedObject.FindProperty("m_ExplodeWhenCollide");
            m_TimeToExplode = serializedObject.FindProperty("m_TimeToExplode");
            m_ExplosionParticle = serializedObject.FindProperty("m_ExplosionParticle");

            m_ExplosionSound = serializedObject.FindProperty("m_ExplosionSound");
            m_ExplosionVolume = serializedObject.FindProperty("m_ExplosionVolume");
        }

        public override void OnInspectorGUI ()
        {
            //Update the serializedProperty - always do this in the beginning of OnInspectorGUI
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(m_ExplosionParticle);
            EditorGUILayout.PropertyField(m_ExplosionRadius);
            EditorGUILayout.PropertyField(m_ExplosionForce);
            EditorGUILayout.PropertyField(m_Damage);
            EditorGUILayout.PropertyField(m_IgnoreCover);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_ExplosionSound);
            if (m_ExplosionSound.objectReferenceValue != null)
                EditorGUILayout.PropertyField(m_ExplosionVolume);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_ExplodeWhenCollide);

            if (!m_ExplodeWhenCollide.boolValue)
                EditorGUILayout.PropertyField(m_TimeToExplode);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
