//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using System;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace FPSBuilder.Editor
{
    using Object = UnityEngine.Object;
    
    public static class EditorUtilities
    {
        private static MethodInfo m_BoldFontMethodInfo;

        #region Search and Load Assets

        private static string m_EditorResourcesPath = string.Empty;
        private static string path = string.Empty;

        internal static string EditorResourcesPath
        {
            get
            {
                if (string.IsNullOrEmpty(m_EditorResourcesPath))
                {
                    if (SearchForEditorResourcesPath(out string _path))
                        m_EditorResourcesPath = _path;
                    else
                        Debug.LogError("Unable to locate editor resources.");
                }

                return m_EditorResourcesPath;
            }
        }

        private static bool SearchForEditorResourcesPath (out string _path)
        {
            _path = string.Empty;

            string searchStr = path;
            string str = null;

            foreach (string assetPath in AssetDatabase.GetAllAssetPaths())
            {
                if (!assetPath.Contains(searchStr)) 
                    continue;
                
                str = assetPath;
                break;
            }

            if (str == null)
                return false;

            _path = str.Substring(0, str.LastIndexOf(searchStr, StringComparison.Ordinal) + searchStr.Length);
            return true;
        }

        #endregion

        // Load assets to use in editor.
        internal static T Load<T> (string _path, string name) where T : Object
        {
            path = _path;
            return AssetDatabase.LoadAssetAtPath<T>(EditorResourcesPath + name);
        }

        // Unity has a private method called EditorGUIUtility.SetBoldDefaultFont, which they use internally to
        // show modified prefab values in bold in the inspector. We are using reflection to access this method
        // so we can mirror this behaviour for our custom inspectors.
        // Warning: Using reflection like this is sneaky and can break in future versions if Unity decides to
        // move or rename this method
        public static void SetBoldDefaultFont (bool value)
        {
            if (m_BoldFontMethodInfo == null)
            {
                m_BoldFontMethodInfo = typeof(EditorGUIUtility).GetMethod(nameof(SetBoldDefaultFont), BindingFlags.Static | BindingFlags.NonPublic);
            }

            if (m_BoldFontMethodInfo != null) 
                m_BoldFontMethodInfo.Invoke(null, new[] {value as object});
        }
        
        public static Rect GetRect (float height = 18)
        {
            return GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(height));
        }

        public static Rect GetRect (float width, float height)
        {
            return GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Width(width), GUILayout.Height(height));
        }

        /// <summary>
        /// Draws a horizontal split line.
        /// </summary>
        public static void DrawSplitter (bool fullWidth = true)
        {
            var rect = GUILayoutUtility.GetRect(1f, 1f);

            // Splitter rect should be full-width
            if (fullWidth)
            {
                rect.xMin = 0f;
                rect.width += 4f;
            }

            if (Event.current.type != EventType.Repaint)
                return;

            EditorGUI.DrawRect(rect, Styling.splitter);
        }

        /// <summary>
        /// Draws a header label.
        /// </summary>
        /// <param name="title">The label to display as a header</param>
        public static void DrawHeader (string title)
        {
            var labelRect = GUILayoutUtility.GetRect(1f, 17f);

            // Title
            EditorGUI.LabelField(labelRect, title, EditorStyles.boldLabel);
        }

        public static void SimpleFoldoutHeader (string title, SerializedProperty property)
        {
            var backgroundRect = GUILayoutUtility.GetRect(1f, 17f);

            var labelRect = backgroundRect;
            labelRect.xMin += 16f;
            labelRect.xMax -= 20f;

            var foldoutRect = backgroundRect;
            foldoutRect.y += 1f;
            foldoutRect.width = 13f;
            foldoutRect.height = 13f;

            // Title
            EditorGUI.LabelField(labelRect, title, EditorStyles.boldLabel);

            Event e = Event.current;

            switch (e.type)
            {
                case EventType.Repaint:
                    Styling.headerFoldout.Draw(foldoutRect, false, false, property.isExpanded, false);
                    break;
                case EventType.MouseDown:
                {
                    if (backgroundRect.Contains(e.mousePosition))
                    {
                        property.isExpanded = !property.isExpanded;
                        e.Use();
                    }
                    break;
                }
            }
        }

        public static void FoldoutHeader (string title, ref bool state)
        {
            DrawSplitter();
            var backgroundRect = GUILayoutUtility.GetRect(1f, 17f);

            var labelRect = backgroundRect;
            labelRect.xMin += 16f;
            labelRect.xMax -= 20f;

            var foldoutRect = backgroundRect;
            foldoutRect.y += 1f;
            foldoutRect.width = 13f;
            foldoutRect.height = 13f;

            // Background rect should be full-width
            backgroundRect.xMin = 0f;
            backgroundRect.width += 4f;

            // Background
            EditorGUI.DrawRect(backgroundRect, Styling.headerBackground);

            // Title
            EditorGUI.LabelField(labelRect, title, EditorStyles.boldLabel);

            Event e = Event.current;

            switch (e.type)
            {
                case EventType.Repaint:
                    Styling.headerFoldout.Draw(foldoutRect, false, false, state, false);
                    break;
                case EventType.MouseDown:
                {
                    if (backgroundRect.Contains(e.mousePosition))
                    {
                        state = !state;
                        e.Use();
                    }
                    break;
                }
            }
        }

        public static void FoldoutHeader (string title, SerializedProperty property)
        {
            DrawSplitter();
            var backgroundRect = GUILayoutUtility.GetRect(1f, 17f);

            var labelRect = backgroundRect;
            labelRect.xMin += 16f;
            labelRect.xMax -= 20f;

            var foldoutRect = backgroundRect;
            foldoutRect.y += 1f;
            foldoutRect.width = 13f;
            foldoutRect.height = 13f;

            // Background rect should be full-width
            backgroundRect.xMin = 0f;
            backgroundRect.width += 4f;

            // Background
            EditorGUI.DrawRect(backgroundRect, Styling.headerBackground);

            // Title
            EditorGUI.LabelField(labelRect, title, EditorStyles.boldLabel);

            Event e = Event.current;

            switch (e.type)
            {
                case EventType.Repaint:
                    Styling.headerFoldout.Draw(foldoutRect, false, false, property.isExpanded, false);
                    break;
                case EventType.MouseDown:
                {
                    if (backgroundRect.Contains(e.mousePosition))
                    {
                        property.isExpanded = !property.isExpanded;
                        e.Use();
                    }
                    break;
                }
            }
        }

        public static void ToggleHeader (string title, SerializedProperty property)
        {
            DrawSplitter();
            var backgroundRect = GUILayoutUtility.GetRect(1f, 17f);

            var labelRect = backgroundRect;
            labelRect.xMin += 32f;
            labelRect.xMax -= 20f;

            var foldoutRect = backgroundRect;
            foldoutRect.y += 1f;
            foldoutRect.width = 13f;
            foldoutRect.height = 13f;

            var toggleRect = backgroundRect;
            toggleRect.x += 16f;
            toggleRect.y += 2f;
            toggleRect.width = 13f;
            toggleRect.height = 13f;

            // Background rect should be full-width
            backgroundRect.xMin = 0f;
            backgroundRect.width += 4f;

            // Background
            EditorGUI.DrawRect(backgroundRect, Styling.headerBackground);

            // Title
            using (new EditorGUI.DisabledScope(!property.boolValue))
                EditorGUI.LabelField(labelRect, title, EditorStyles.boldLabel);

            // foldout
            property.isExpanded = GUI.Toggle(foldoutRect, property.isExpanded, GUIContent.none, EditorStyles.foldout);

            // Active checkbox
            property.boolValue = GUI.Toggle(toggleRect, property.boolValue, GUIContent.none, Styling.smallTickbox);

            Event e = Event.current;

            if (e.type == EventType.MouseDown)
            {
                if (backgroundRect.Contains(e.mousePosition))
                {
                    property.isExpanded = !property.isExpanded;
                    e.Use();
                }
            }
        }

        public static void LerpAnimationDrawer (string title, SerializedProperty property, bool enableSetAndReset = false, Transform transform = null)
        {
            FoldoutHeader(title, property);

            EditorGUI.indentLevel++;

            if (property.isExpanded)
            {
                SerializedProperty m_TargetPosition = property.FindPropertyRelative("m_TargetPosition");
                SerializedProperty m_TargetRotation = property.FindPropertyRelative("m_TargetRotation");
                SerializedProperty m_Duration = property.FindPropertyRelative("m_Duration");
                SerializedProperty m_ReturnDuration = property.FindPropertyRelative("m_ReturnDuration");

                EditorGUILayout.PropertyField(m_TargetPosition);
                EditorGUILayout.PropertyField(m_TargetRotation);

                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(m_Duration);
                EditorGUILayout.PropertyField(m_ReturnDuration);

                if (enableSetAndReset && transform != null)
                {
                    EditorGUILayout.Space();
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace();

                        if (GUILayout.Button("Get From Transform", Styling.leftButton))
                        {
                            m_TargetPosition.vector3Value = transform.localPosition;
                            m_TargetRotation.vector3Value = transform.localEulerAngles;
                        }

                        if (GUILayout.Button("Reset", Styling.rightButton))
                        {
                            m_TargetPosition.vector3Value = Vector3.zero;
                            m_TargetRotation.vector3Value = Vector3.zero;
                            m_Duration.floatValue = 0;
                            m_ReturnDuration.floatValue = 0;
                        }

                        GUILayout.FlexibleSpace();
                    }
                }
            }

            EditorGUI.indentLevel--;
        }

        public static void CustomLerpAnimationDrawer (string title, SerializedProperty property, Vector3 hipPosition, Vector3 hipRotation, bool enableSetAndReset = false, bool enablePreviewing = false, Transform transform = null)
        {
            using (new EditorGUILayout.VerticalScope(Styling.background))
            {
                SimpleFoldoutHeader(title, property);

                EditorGUI.indentLevel++;

                if (property.isExpanded)
                {
                    SerializedProperty m_TargetPosition = property.FindPropertyRelative("m_TargetPosition");
                    SerializedProperty m_TargetRotation = property.FindPropertyRelative("m_TargetRotation");
                    SerializedProperty m_Duration = property.FindPropertyRelative("m_Duration");
                    SerializedProperty m_ReturnDuration = property.FindPropertyRelative("m_ReturnDuration");

                    EditorGUILayout.PropertyField(m_TargetPosition);
                    EditorGUILayout.PropertyField(m_TargetRotation);

                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(m_Duration);
                    EditorGUILayout.PropertyField(m_ReturnDuration);

                    if (enableSetAndReset)
                    {
                        EditorGUILayout.Space();
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();

                            if (GUILayout.Button("Get From Transform", Styling.leftButton))
                            {
                                if (transform != null)
                                {
                                    m_TargetPosition.vector3Value = transform.localPosition;
                                    m_TargetRotation.vector3Value = transform.localEulerAngles;
                                }
                            }

                            if (GUILayout.Button("Reset", Styling.rightButton))
                            {
                                m_TargetPosition.vector3Value = Vector3.zero;
                                m_TargetRotation.vector3Value = Vector3.zero;
                                m_Duration.floatValue = 0;
                                m_ReturnDuration.floatValue = 0;
                            }
                            
                            if (enablePreviewing && (m_TargetPosition.vector3Value != Vector3.zero || m_TargetRotation.vector3Value != Vector3.zero) && transform != null)
                            {
                                GUILayout.FlexibleSpace();
                                
                                bool isPreviewing = transform.localPosition == m_TargetPosition.vector3Value &&
                                                    transform.localRotation.eulerAngles == m_TargetRotation.vector3Value;
                        
                                if (GUILayout.Button(isPreviewing ? "Exit Preview" : "Preview", Styling.button, GUILayout.Width(96)))
                                {
                                    if (isPreviewing)
                                    {
                                        transform.localPosition = hipPosition;
                                        transform.localRotation = Quaternion.Euler(hipRotation);
                                    }
                                    else
                                    {
                                        transform.localPosition = m_TargetPosition.vector3Value;
                                        transform.localEulerAngles = m_TargetRotation.vector3Value;
                                    }
                                }
                            }

                            GUILayout.FlexibleSpace();
                        }
                    }
                }
                EditorGUI.indentLevel--;
            }
        }

        public static ReorderableList CreateReorderableList(SerializedProperty property, string elementName = "Element", string headerName = "")
        {
            ReorderableList reorderableList = null;
            
            reorderableList = new ReorderableList(property.serializedObject, property, true, true, true, true)
            {
                drawHeaderCallback = rect =>
                {
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width - 128, EditorGUIUtility.singleLineHeight), headerName == "" ? property.displayName : headerName, EditorStyles.boldLabel);
                    EditorGUI.LabelField(new Rect(rect.x + rect.width - 68, rect.y, rect.width - 128, EditorGUIUtility.singleLineHeight), $"Length: {property.arraySize}");
                },

                drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    var element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
                    rect.y += 2;

                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 96, EditorGUIUtility.singleLineHeight), elementName + " " + (index + 1));
                    EditorGUI.PropertyField(new Rect(rect.x + 96, rect.y, rect.width - 96, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
                },

                onSelectCallback = list =>
                {
                    if (list.serializedProperty.propertyType != SerializedPropertyType.ObjectReference) 
                        return;
                    
                    var prefab = list.serializedProperty.GetArrayElementAtIndex(list.index).objectReferenceValue as GameObject;
                    if (prefab != null)
                        EditorGUIUtility.PingObject(prefab.gameObject);
                },

                onCanRemoveCallback = list => list.count > 0
            };

            return reorderableList;
        }
        
        public static ReorderableList CreateCustomReorderableList(SerializedProperty property, SerializedProperty header, string elementName = "Element", string headerName = "")
        {
            ReorderableList reorderableList = null;
            
            reorderableList = new ReorderableList(property.serializedObject, property, true, true, true, true)
            {
                drawHeaderCallback = rect =>
                {
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 96, EditorGUIUtility.singleLineHeight), headerName == "" ? property.displayName : headerName, EditorStyles.boldLabel);
                    EditorGUI.PropertyField(new Rect(rect.x + 112, rect.y, rect.width - 112, EditorGUIUtility.singleLineHeight), header);
                },

                drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    var element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
                    rect.y += 2;

                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 96, EditorGUIUtility.singleLineHeight), elementName + " " + (index + 1));
                    EditorGUI.PropertyField(new Rect(rect.x + 96, rect.y, rect.width - 96, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
                },

                onSelectCallback = list =>
                {
                    var prefab = list.serializedProperty.GetArrayElementAtIndex(list.index).objectReferenceValue as GameObject;
                    if (prefab != null)
                        EditorGUIUtility.PingObject(prefab.gameObject);
                },

                onCanRemoveCallback = list => list.count > 0
            };

            return reorderableList;
        }
    }
}