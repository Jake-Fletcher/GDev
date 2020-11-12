//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using FPSBuilder.Editor;
using UnityEditor;
using UnityEngine;

namespace FPSBuilder.Core.Surface.Editor
{
    [CustomEditor(typeof(SurfaceIdentifier)), CanEditMultipleObjects]
    public sealed class SurfaceIdentifierEditor : UnityEditor.Editor
    {
        private SurfaceIdentifier m_Target;
        private SerializedProperty m_SurfaceList;

        private void OnEnable ()
        {
            m_Target = (target as SurfaceIdentifier);
            m_SurfaceList = serializedObject.FindProperty("m_SurfaceList");
        }

        public override void OnInspectorGUI ()
        {
            serializedObject.Update();

            if (serializedObject.targetObjects.Length > 1)
            {
                int count = ((SurfaceIdentifier)serializedObject.targetObjects[0]).SurfacesCount;

                for (int i = 0; i < serializedObject.targetObjects.Length; i++)
                {
                    if (((SurfaceIdentifier) serializedObject.targetObjects[i]).SurfacesCount == count) 
                        continue;
                    
                    EditorGUILayout.HelpBox("Multi-object editing with different properties is not supported.", MessageType.Info);
                    return;
                }
            }
            
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label(m_Target.IsTerrain ? "Textures" : "Materials", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                EditorGUI.BeginChangeCheck();
                if (GUILayout.Button("Reset " + (m_Target.IsTerrain ? "Textures" : "Materials"), Styling.miniButton))
                {
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(m_Target, "Undo Inspector");
                        for (int i = 0; i < serializedObject.targetObjects.Length; i++)
                        {
                            ((SurfaceIdentifier)serializedObject.targetObjects[i]).Reset();
                        }

                        EditorUtility.SetDirty(m_Target);
                        return;
                    }
                }
            }

            EditorGUILayout.Space();
            for (int i = 0; i < m_SurfaceList.arraySize; i++)
            {
                DrawSurfaceData(i);
            }

            EditorUtilities.DrawSplitter();

            if (m_Target.IsTerrain)
            {
                EditorGUILayout.HelpBox("Can not generate decals on terrain!", MessageType.Info);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawSurfaceData (int index)
        {
            SerializedProperty surface = m_SurfaceList.GetArrayElementAtIndex(index);

            SerializedProperty surfaceType = surface.FindPropertyRelative("m_SurfaceType");
            SerializedProperty allowDecals = surface.FindPropertyRelative("m_AllowDecals");
            SerializedProperty penetration = surface.FindPropertyRelative("m_Penetration");
            SerializedProperty density = surface.FindPropertyRelative("m_Density");

            EditorUtilities.FoldoutHeader(MaterialName(index), surfaceType);

            if (!surfaceType.isExpanded) 
                return;
            
            using (new EditorGUILayout.HorizontalScope(Styling.background))
            {
                Texture2D texture = PreviewTexture(index);

                GUILayout.Label(texture, GUILayout.Height(72), GUILayout.Width(72));
                using (new EditorGUILayout.VerticalScope())
                {
                    if (!penetration.boolValue && surfaceType.objectReferenceValue != null)
                    {
                        GUILayout.Space(8);
                    }

                    EditorGUILayout.PropertyField(surfaceType);

                    if (surfaceType.objectReferenceValue == null) 
                        return;

                    if (m_Target.IsTerrain) 
                        return;
                    
                    EditorGUILayout.PropertyField(allowDecals);
                    EditorGUILayout.PropertyField(penetration);

                    if (penetration.boolValue)
                    {
                        EditorGUILayout.PropertyField(density);
                    }
                }
            }
        }

        private string MaterialName (int index)
        {
            if (m_Target.Materials != null)
            {
                if (m_Target.Materials[index] == null) 
                    return "Texture " + index;
                
                if (m_Target.Materials != null)
                {
                    return m_Target.Materials[index].name;
                }
            }
            else
            {
                if (m_Target.Textures != null)
                {
                    return m_Target.Textures[index].name;
                }
            }

            return "Default Material";
        }

        private Texture2D PreviewTexture (int index)
        {
            if (m_Target.Materials != null)
            {
                if (m_Target.Materials[index] == null)
                    return AssetPreview.GetAssetPreview(EditorUtilities.Load<Texture2D>("/Editor Resources/", "NoTexture.png"));
                
                
                return AssetPreview.GetAssetPreview(m_Target.Materials[index]);
            }

            return AssetPreview.GetAssetPreview(m_Target.Textures != null ? m_Target.Textures[index] : EditorUtilities.Load<Texture2D>("/Editor Resources/", "NoTexture.png"));
        }
    }
}