//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using FPSBuilder.Core;
using UnityEditor;

namespace FPSBuilder.PostProcessing.Editor
{
    [CustomEditor(typeof(CollisionSound))]
    public class CollisionSoundEditor : UnityEditor.Editor
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
