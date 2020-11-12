//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using UnityEditor;

namespace FPSBuilder.PostProcessing.Editor
{
    [CustomEditor(typeof(BloodSplashEffect))]
    public class BloodSplashEffectEditor : UnityEditor.Editor
    {
        private static readonly string[] m_ScriptField = { "m_Script" };

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawPropertiesExcluding(serializedObject, m_ScriptField);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
