//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using FPSBuilder.Editor;
using FPSBuilder.Interfaces;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace FPSBuilder.Core.Weapons.Editor
{
    [CustomEditor(typeof(WeaponManager))]
    public sealed class WeaponManagerEditor : UnityEditor.Editor
    {
        private WeaponManager m_Target;

        private SerializedProperty m_FPController;
        private SerializedProperty m_CameraTransformReference;
        private SerializedProperty m_InteractionRadius;
        private SerializedProperty m_WeaponSwitchMode;
        private SerializedProperty m_AmmoTag;
        private SerializedProperty m_AdrenalinePackTag;
        
        private SerializedProperty m_ItemPickupSound;
        private SerializedProperty m_ItemPickupVolume;
        private SerializedProperty m_FastChangeWeapons;
        
        private SerializedProperty m_EquippedWeaponsList;
        
        private SerializedProperty m_WeaponList;

        private SerializedProperty m_DefaultWeapon;
        private SerializedProperty m_FragGrenade;
        private SerializedProperty m_Adrenaline;

        private void OnEnable ()
        {
            m_Target = (target as WeaponManager);
            m_FPController = serializedObject.FindProperty("m_FPController");
            m_CameraTransformReference = serializedObject.FindProperty("m_CameraTransformReference");
            m_InteractionRadius = serializedObject.FindProperty("m_InteractionRadius");
            m_WeaponSwitchMode = serializedObject.FindProperty("m_WeaponSwitchMode");
            m_AmmoTag = serializedObject.FindProperty("m_AmmoTag");
            m_AdrenalinePackTag = serializedObject.FindProperty("m_AdrenalinePackTag");
            m_EquippedWeaponsList = serializedObject.FindProperty("m_EquippedWeaponsList");
            m_ItemPickupSound = serializedObject.FindProperty("m_ItemPickupSound");
            m_ItemPickupVolume = serializedObject.FindProperty("m_ItemPickupVolume");
            m_FastChangeWeapons = serializedObject.FindProperty("m_FastChangeWeapons");
            m_WeaponList = serializedObject.FindProperty("m_WeaponList");
            m_DefaultWeapon = serializedObject.FindProperty("m_DefaultWeapon");
            m_FragGrenade = serializedObject.FindProperty("m_FragGrenade");
            m_Adrenaline = serializedObject.FindProperty("m_Adrenaline");
        }

        public override void OnInspectorGUI ()
        {
            //Update the serializedProperty - always do this in the beginning of OnInspectorGUI
            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_FPController, new GUIContent("First Person Character"));
            EditorGUILayout.PropertyField(m_CameraTransformReference);
            EditorGUILayout.PropertyField(m_InteractionRadius);
            EditorGUILayout.PropertyField(m_WeaponSwitchMode);
            EditorGUILayout.PropertyField(m_FastChangeWeapons);
            
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_AmmoTag);
            EditorGUILayout.PropertyField(m_AdrenalinePackTag);
            EditorGUILayout.PropertyField(m_ItemPickupSound);
            EditorGUILayout.PropertyField(m_ItemPickupVolume);

            EditorGUILayout.Space();
            EditorGUI.indentLevel = 0;
            EditorGUILayout.LabelField("Equipped Weapons" + " (" + m_EquippedWeaponsList.arraySize + (m_EquippedWeaponsList.arraySize > 1 ? " Slots)" : " Slot)"), EditorStyles.boldLabel);

            using (new EditorGUILayout.VerticalScope(Styling.background))
            {
                if (m_EquippedWeaponsList.arraySize == 0)
                {
                    EditorGUILayout.HelpBox("No weapon slot", MessageType.Info);

                    EditorGUI.BeginChangeCheck();
                    if (GUILayout.Button("Add new slot"))
                    {
                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(m_Target, "Undo Inspector");
                            m_Target.AddWeaponSlot();

                            EditorUtility.SetDirty(m_Target);
                            return;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < m_EquippedWeaponsList.arraySize; i++)
                    {
                        ShowEquippedWeapons(i);
                    }
                }
            }

            EditorGUILayout.Space();
            EditorGUI.indentLevel = 0;
            EditorGUILayout.LabelField("Weapon List", EditorStyles.boldLabel);

            using (new EditorGUILayout.VerticalScope(Styling.background))
            {
                if (m_WeaponList.arraySize == 0)
                {
                    EditorGUILayout.HelpBox("No weapon available", MessageType.Info);

                    EditorGUI.BeginChangeCheck();
                    if (GUILayout.Button("Add new weapon"))
                    {
                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(m_Target, "Undo Inspector");
                            m_Target.AddWeapon();

                            EditorUtility.SetDirty(m_Target);
                            return;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < m_WeaponList.arraySize; i++)
                    {
                        ShowWeaponList(i);
                    }
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_DefaultWeapon);

            EditorGUILayout.Space();
            EditorGUI.indentLevel = 0;
            EditorGUILayout.LabelField("Items", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(m_FragGrenade);
            EditorGUILayout.PropertyField(m_Adrenaline);

            //Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI
            serializedObject.ApplyModifiedProperties();
        }

        private void ShowEquippedWeapons (int index)
        {
            SerializedProperty weapon = m_EquippedWeaponsList.GetArrayElementAtIndex(index);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (weapon.objectReferenceValue != null)
                {
                    EditorGUILayout.LabelField("Slot " + (index + 1) + "  →\t" + weapon.objectReferenceValue.name);
                }
                else
                {
                    EditorGUILayout.LabelField("Slot " + (index + 1) + "  →\tEmpty");
                }

                if (weapon.objectReferenceValue != null)
                {
                    EditorGUI.BeginChangeCheck();
                    if (GUILayout.Button("Deselect", Styling.button))
                    {
                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(m_Target, "Undo Inspector");
                            m_Target.UnequipWeapon(index);

                            EditorUtility.SetDirty(m_Target);
                            return;
                        }
                    }
                }

                EditorGUI.BeginChangeCheck();
                if (GUILayout.Button("+", Styling.leftButton, GUILayout.Width(24)))
                {
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(m_Target, "Undo Inspector");
                        m_Target.AddWeaponSlot();

                        EditorUtility.SetDirty(m_Target);
                        return;
                    }
                }

                EditorGUI.BeginChangeCheck();
                if (GUILayout.Button("-", Styling.rightButton, GUILayout.Width(24)))
                {
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(m_Target, "Undo Inspector");
                        m_Target.RemoveWeaponSlot(index);

                        EditorUtility.SetDirty(m_Target);
                    }
                }
            }
        }

        private void ShowWeaponList (int index)
        {
            SerializedProperty weapon = m_WeaponList.GetArrayElementAtIndex(index);

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.PropertyField(weapon, new GUIContent("Weapon " + (index + 1)));

                if (weapon.objectReferenceValue != null)
                {
                    if (!m_Target.IsEquipped((IWeapon)weapon.objectReferenceValue) && m_Target.HasFreeSlot)
                    {
                        EditorGUI.BeginChangeCheck();
                        if (GUILayout.Button("Equip", Styling.button))
                        {
                            if (EditorGUI.EndChangeCheck())
                            {
                                Undo.RecordObject(m_Target, "Undo Inspector");
                                m_Target.EquipWeapon(index);

                                EditorUtility.SetDirty(m_Target);
                                return;
                            }
                        }
                    }
                }

                EditorGUI.BeginChangeCheck();
                if (GUILayout.Button("+", Styling.leftButton, GUILayout.Width(24)))
                {
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(m_Target, "Undo Inspector");
                        m_Target.AddWeapon();

                        EditorUtility.SetDirty(m_Target);
                        return;
                    }
                }

                EditorGUI.BeginChangeCheck();
                if (GUILayout.Button("-", Styling.rightButton, GUILayout.Width(24)))
                {
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(m_Target, "Undo Inspector");
                        m_Target.RemoveWeapon(index);

                        EditorUtility.SetDirty(m_Target);
                    }
                }
            }
        }
    }
}