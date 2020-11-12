//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using UnityEditor;
using UnityEngine;

namespace FPSBuilder.Editor
{
    [CustomPropertyDrawer(typeof(RequiredAttribute))]
    internal sealed class RequiredPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var notnull = attribute as RequiredAttribute;

            if (property.objectReferenceValue == null && property.propertyType == SerializedPropertyType.ObjectReference)
            {
                Texture2D warnIcon = EditorUtilities.Load<Texture2D>("/Scripts/Editor Resources/", "warnIcon.png");
                GUIContent content;

                if (warnIcon != null)
                {
                    content = new GUIContent(label.text, warnIcon, notnull != null && notnull.overrideMessage ? notnull.message : "The field " + property.displayName + " can not be null");
                }
                else
                {
                    content = new GUIContent("*" + label.text, notnull != null && notnull.overrideMessage ? notnull.message : "The field " + property.displayName + " can not be null");
                }

                EditorGUI.PropertyField(position, property, content);
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}
