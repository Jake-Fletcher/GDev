//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using System;
using UnityEditor;
using UnityEngine;

public class TelescopicSightShaderGUI : ShaderGUI
{
    private static GUIContent staticLabel = new GUIContent();
    private MaterialEditor editor;
    private MaterialProperty[] properties;

    private void SetKeyword (string keyword, bool state)
    {
        if (state)
        {
            foreach (UnityEngine.Material m in editor.targets)
            {
                m.EnableKeyword(keyword);
            }
        }
        else
        {
            foreach (UnityEngine.Material m in editor.targets)
            {
                m.DisableKeyword(keyword);
            }
        }
    }

    private MaterialProperty FindProperty (string name, bool propertyIsMandatory = true)
    {
        return FindProperty(name, properties, propertyIsMandatory);
    }

    private static GUIContent MakeLabel (string text, string tooltip = null)
    {
        staticLabel.text = text;
        staticLabel.tooltip = tooltip;
        return staticLabel;
    }

    private static GUIContent MakeLabel (MaterialProperty property, string tooltip = null)
    {
        staticLabel.text = property.displayName;
        staticLabel.tooltip = tooltip;
        return staticLabel;
    }

    private void ReticleProperties ()
    {
        MaterialProperty reticleMap = FindProperty("_ReticleMap");
        Texture tex = reticleMap.textureValue;

        EditorGUI.BeginChangeCheck();
        editor.TexturePropertySingleLine(MakeLabel(reticleMap), reticleMap, FindProperty("_ReticleColor"), FindProperty("_ReticleOpacity"));
        editor.ShaderProperty(FindProperty("_Aperture"), MakeLabel("Aperture"));
        if (EditorGUI.EndChangeCheck() && tex != reticleMap.textureValue)
        {
            SetKeyword("_RETICLE_MAP", reticleMap.textureValue);
        }
    }

    private void ShaderProperties ()
    {
        MaterialProperty mainTex = FindProperty("_MainTex");
        editor.TexturePropertySingleLine(MakeLabel("Base Map", "Specify the base color(RGB) and opacity(A)."), mainTex, FindProperty("_Color"));
        ReticleProperties();

        MaterialProperty isViewmodel = FindProperty("_Viewmodel");

        EditorGUI.BeginChangeCheck();
        editor.ShaderProperty(isViewmodel, MakeLabel(isViewmodel));

        if (Math.Abs(isViewmodel.floatValue - 1) < Mathf.Epsilon)
        {
            EditorGUI.indentLevel += 1;
            editor.ShaderProperty(FindProperty("_ViewmodelFOV"), MakeLabel("Field of View"));
            EditorGUI.indentLevel -= 1;
        }

        if (EditorGUI.EndChangeCheck())
        {
            SetKeyword("_VIEWMODEL", Math.Abs(isViewmodel.floatValue - 1) < Mathf.Epsilon);
        }

        editor.EnableInstancingField();
    }

    public override void OnGUI (MaterialEditor _editor, MaterialProperty[] _properties)
    {
        editor = _editor;
        properties = _properties;
        ShaderProperties();
    }
}
