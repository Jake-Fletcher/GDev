//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using FPSBuilder.Editor;
using UnityEditor;
using UnityEngine;

namespace FPSBuilder.Core.Input.Editor
{
    [CustomEditor(typeof(InputBindings))]
    public sealed class InputBindingsEditor : UnityEditor.Editor
    {
        private InputBindings m_Target;

        private SerializedProperty m_Buttons;
        private SerializedProperty m_Axes;

        private static int m_ToolbarIndex;

        private void OnEnable ()
        {
            m_Target = (InputBindings)target;

            m_Buttons = serializedObject.FindProperty("m_Buttons");
            m_Axes = serializedObject.FindProperty("m_Axes");
        }

        public override void OnInspectorGUI ()
        {
            serializedObject.Update();
            
            EditorGUI.LabelField(EditorUtilities.GetRect(36), "Input Bindings", Styling.headerLabel);

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                m_ToolbarIndex = GUILayout.Toolbar(m_ToolbarIndex, new[] { "Axes", "Buttons" }, GUILayout.Height(24), GUILayout.Width(256));
                GUILayout.FlexibleSpace();
            }

            EditorGUILayout.Space();

            switch (m_ToolbarIndex)
            {
                case 0:
                {
                    for (int i = 0; i < m_Axes.arraySize; i++)
                    {
                        AxisDrawer(m_Axes.GetArrayElementAtIndex(i), i);
                    }

                    EditorUtilities.DrawSplitter();
                    EditorGUILayout.Space();

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace();

                        EditorGUI.BeginChangeCheck();
                        if (GUILayout.Button("New Axis", Styling.button, GUILayout.Height(28), GUILayout.Width(256)))
                        {
                            if (EditorGUI.EndChangeCheck())
                            {
                                Undo.RecordObject(m_Target, "Undo Inspector");
                                m_Target.AddAxis("Axis", string.Empty, string.Empty, 1, 1, 0.01f);

                                EditorUtility.SetDirty(m_Target);
                                return;
                            }
                        }

                        GUILayout.FlexibleSpace();
                    }

                    break;
                }
                case 1:
                {
                    for (int i = 0; i < m_Buttons.arraySize; i++)
                    {
                        ButtonDrawer(m_Buttons.GetArrayElementAtIndex(i), i);
                    }

                    EditorUtilities.DrawSplitter();
                    EditorGUILayout.Space();

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace();

                        EditorGUI.BeginChangeCheck();
                        if (GUILayout.Button("New Button", Styling.button, GUILayout.Height(28), GUILayout.Width(256)))
                        {
                            if (EditorGUI.EndChangeCheck())
                            {
                                Undo.RecordObject(m_Target, "Undo Inspector");
                                m_Target.AddButton("Button", string.Empty);
                                EditorUtility.SetDirty(m_Target);
                                return;
                            }
                        }

                        GUILayout.FlexibleSpace();
                    }

                    break;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void AxisDrawer (SerializedProperty property, int index)
        {
            SerializedProperty axisName = property.FindPropertyRelative("m_Name");

            EditorUtilities.DrawSplitter();
            var backgroundRect = GUILayoutUtility.GetRect(1f, 17f);

            var labelRect = backgroundRect;
            labelRect.xMin += 16f;
            labelRect.xMax -= 20f;

            var foldoutRect = backgroundRect;
            foldoutRect.y += 1f;
            foldoutRect.width = 13f;
            foldoutRect.height = 13f;

            var menuIcon = Styling.paneOptionsIcon;
            var menuRect = new Rect(labelRect.xMax + 4f, labelRect.y, menuIcon.width, menuIcon.height);

            // Background rect should be full-width
            backgroundRect.xMin = 0f;
            backgroundRect.width += 4f;

            // Background
            EditorGUI.DrawRect(backgroundRect, Styling.headerBackground);

            // Title
            EditorGUI.LabelField(labelRect, axisName.stringValue, EditorStyles.boldLabel);

            // Dropdown menu icon
            GUI.DrawTexture(menuRect, menuIcon);

            Event e = Event.current;

            switch (e.type)
            {
                case EventType.Repaint:
                    Styling.headerFoldout.Draw(foldoutRect, false, false, property.isExpanded, false);
                    break;
                case EventType.MouseDown:
                {
                    if (menuRect.Contains(e.mousePosition))
                    {
                        ShowAxisHeaderContextMenu(new Vector2(menuRect.x, menuRect.yMax), axisName.stringValue, index);
                        e.Use();
                    }

                    if (backgroundRect.Contains(e.mousePosition))
                    {
                        property.isExpanded = !property.isExpanded;
                        e.Use();
                    }
                    break;
                }
            }

            if (!property.isExpanded) 
                return;
            
            SerializedProperty axisPositiveKey = property.FindPropertyRelative("m_PositiveKey");
            SerializedProperty axisNegativeKey = property.FindPropertyRelative("m_NegativeKey");
            SerializedProperty axisSensitivity = property.FindPropertyRelative("m_Sensitivity");
            SerializedProperty axisGravity = property.FindPropertyRelative("m_Gravity");
            SerializedProperty m_DeadZone = property.FindPropertyRelative("m_DeadZone");

            EditorGUI.indentLevel = 1;
            EditorGUILayout.PropertyField(axisName);
            EditorGUILayout.PropertyField(axisPositiveKey);
            EditorGUILayout.PropertyField(axisNegativeKey);
            EditorGUILayout.PropertyField(axisSensitivity);
            EditorGUILayout.PropertyField(axisGravity);
            EditorGUILayout.PropertyField(m_DeadZone);
            EditorGUI.indentLevel = 0;
        }

        private void ShowAxisHeaderContextMenu (Vector2 position, string axisName, int index)
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Reset"), false, () => ResetAxis(index));
            menu.AddItem(new GUIContent("Remove"), false, () => RemoveAxis(axisName, index));

            menu.DropDown(new Rect(position, Vector2.zero));
        }

        private void ResetAxis (int index)
        {
            Undo.RecordObject(m_Target, "Undo Inspector");
            m_Target.ResetAxis(index);
            EditorUtility.SetDirty(target);
        }

        private void RemoveAxis (string axisName, int index)
        {
            if (EditorUtility.DisplayDialog("Are you sure?", "Are you sure you want to remove " + axisName + " ?", "Yes", "No"))
            {
                Undo.RecordObject(m_Target, "Undo Inspector");
                m_Target.RemoveAxis(index);
                EditorUtility.SetDirty(target);
            }
        }

        private void ButtonDrawer (SerializedProperty property, int index)
        {
            SerializedProperty buttonName = property.FindPropertyRelative("m_Name");

            EditorUtilities.DrawSplitter();
            var backgroundRect = GUILayoutUtility.GetRect(1f, 17f);

            var labelRect = backgroundRect;
            labelRect.xMin += 16f;
            labelRect.xMax -= 20f;

            var foldoutRect = backgroundRect;
            foldoutRect.y += 1f;
            foldoutRect.width = 13f;
            foldoutRect.height = 13f;

            var menuIcon = Styling.paneOptionsIcon;
            var menuRect = new Rect(labelRect.xMax + 4f, labelRect.y, menuIcon.width, menuIcon.height);

            // Background rect should be full-width
            backgroundRect.xMin = 0f;
            backgroundRect.width += 4f;

            // Background
            EditorGUI.DrawRect(backgroundRect, Styling.headerBackground);

            // Title
            EditorGUI.LabelField(labelRect, buttonName.stringValue, EditorStyles.boldLabel);

            // Dropdown menu icon
            GUI.DrawTexture(menuRect, menuIcon);

            Event e = Event.current;

            switch (e.type)
            {
                case EventType.Repaint:
                    Styling.headerFoldout.Draw(foldoutRect, false, false, property.isExpanded, false);
                    break;
                case EventType.MouseDown:
                {
                    if (menuRect.Contains(e.mousePosition))
                    {
                        ShowButtonHeaderContextMenu(new Vector2(menuRect.x, menuRect.yMax), buttonName.stringValue, index);
                        e.Use();
                    }

                    if (backgroundRect.Contains(e.mousePosition))
                    {
                        property.isExpanded = !property.isExpanded;
                        e.Use();
                    }
                    break;
                }
            }

            if (!property.isExpanded) 
                return;
            
            SerializedProperty buttonKey = property.FindPropertyRelative("m_Key");

            EditorGUI.indentLevel = 1;
            EditorGUILayout.PropertyField(buttonName);
            EditorGUILayout.PropertyField(buttonKey);
            EditorGUI.indentLevel = 0;
        }

        private void ShowButtonHeaderContextMenu (Vector2 position, string buttonName, int index)
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Reset"), false, () => ResetButton(index));
            menu.AddItem(new GUIContent("Remove"), false, () => RemoveButton(buttonName, index));

            menu.DropDown(new Rect(position, Vector2.zero));
        }

        private void ResetButton (int index)
        {
            Undo.RecordObject(m_Target, "Undo Inspector");
            m_Target.ResetButton(index);
            EditorUtility.SetDirty(target);
        }

        private void RemoveButton (string buttonName, int index)
        {
            if (EditorUtility.DisplayDialog("Are you sure?", "Are you sure you want to remove " + buttonName + " ?", "Yes", "No"))
            {
                Undo.RecordObject(m_Target, "Undo Inspector");
                m_Target.RemoveButton(index);
                EditorUtility.SetDirty(target);
            }
        }
    }
}