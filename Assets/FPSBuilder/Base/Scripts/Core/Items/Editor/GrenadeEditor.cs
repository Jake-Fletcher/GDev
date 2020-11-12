//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using UnityEditor;

namespace FPSBuilder.Core.Items.Editor
{
    [CustomEditor(typeof(Grenade))]
    public sealed class GrenadeEditor : UnityEditor.Editor
    {
        private Grenade m_Target;
        private SerializedProperty m_Grenade;
        private SerializedProperty m_ThrowTransformReference;
        private SerializedProperty m_ThrowForce;
        private SerializedProperty m_DelayToInstantiate;
        private SerializedProperty m_Amount;
        private SerializedProperty m_MaxAmount;
        private SerializedProperty m_InfiniteGrenades;
        private SerializedProperty m_Animator;
        private SerializedProperty m_PullAnimation;
        private SerializedProperty m_PullSound;
        private SerializedProperty m_PullVolume;
        private SerializedProperty m_ThrowAnimation;
        private SerializedProperty m_ThrowSound;
        private SerializedProperty m_ThrowVolume;

        private void OnEnable ()
        {
            m_Target = (target as Grenade);
            
            if (m_Target != null) 
                m_Target.DisableShadowCasting();

            m_Grenade = serializedObject.FindProperty("m_Grenade");
            m_ThrowTransformReference = serializedObject.FindProperty("m_ThrowTransformReference");
            m_ThrowForce = serializedObject.FindProperty("m_ThrowForce");
            m_DelayToInstantiate = serializedObject.FindProperty("m_DelayToInstantiate");
            m_Amount = serializedObject.FindProperty("m_Amount");
            m_MaxAmount = serializedObject.FindProperty("m_MaxAmount");
            m_InfiniteGrenades = serializedObject.FindProperty("m_InfiniteGrenades");
            m_Animator = serializedObject.FindProperty("m_Animator");
            m_PullAnimation = serializedObject.FindProperty("m_PullAnimation");
            m_PullSound = serializedObject.FindProperty("m_PullSound");
            m_PullVolume = serializedObject.FindProperty("m_PullVolume");
            m_ThrowAnimation = serializedObject.FindProperty("m_ThrowAnimation");
            m_ThrowSound = serializedObject.FindProperty("m_ThrowSound");
            m_ThrowVolume = serializedObject.FindProperty("m_ThrowVolume");
        }

        public override void OnInspectorGUI ()
        {
            //Update the serializedProperty - always do this in the beginning of OnInspectorGUI
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(m_Grenade);
            EditorGUILayout.PropertyField(m_ThrowTransformReference);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_ThrowForce);
            EditorGUILayout.PropertyField(m_DelayToInstantiate);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_InfiniteGrenades);

            EditorGUI.indentLevel = 0;
            if (!m_InfiniteGrenades.boolValue)
            {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(m_Amount);
                EditorGUILayout.PropertyField(m_MaxAmount);
            }

            EditorGUI.indentLevel = 0;
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Grenade Animations", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_Animator);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_PullAnimation);
            EditorGUILayout.PropertyField(m_PullSound);
            EditorGUILayout.PropertyField(m_PullVolume);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_ThrowAnimation);
            EditorGUILayout.PropertyField(m_ThrowSound);
            EditorGUILayout.PropertyField(m_ThrowVolume);

            serializedObject.ApplyModifiedProperties();
        }
    }
}