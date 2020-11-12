//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using System;
using FPSBuilder.Editor;
using UnityEditor;
using UnityEngine;

public class ReflectorSightShaderGUI : ShaderGUI
{
    private static GUIContent staticLabel = new GUIContent();
    private MaterialEditor editor;
    private MaterialProperty[] properties;

    private bool surfaceInputs;
    private bool reticleInputs;
    private bool advancedOptions;

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

    private void MaskProperties ()
    {
        MaterialProperty maskMap = FindProperty("_MaskMap");
        Texture tex = maskMap.textureValue;

        EditorGUI.BeginChangeCheck();
        editor.TexturePropertySingleLine(MakeLabel("Mask Map", "Specifies the Mask Map for this Material - Metallic (R), Ambient occlusion (G), Detail mask (B), Smoothness (A)."),
            maskMap, FindProperty("_Metallic"));

        EditorGUI.indentLevel += 2;
        editor.ShaderProperty(FindProperty("_Glossiness"), MakeLabel("Smoothness", "Controls the scale factor for the Material's Smoothness."));

        if (maskMap.textureValue)
        {
            editor.ShaderProperty(FindProperty("_OcclusionStrength"), MakeLabel("Ambient Occlusion", "Controls the scale factor for the Material's Ambient Occlusion."));
        }

        EditorGUI.indentLevel -= 2;

        if (EditorGUI.EndChangeCheck() && tex != maskMap.textureValue)
        {
            SetKeyword("_MASK_MAP", maskMap.textureValue);
        }
    }

    private void ReticleProperties ()
    {
        MaterialProperty reticleMap = FindProperty("_ReticleMap");
        Texture tex = reticleMap.textureValue;

        EditorGUI.BeginChangeCheck();
        editor.TexturePropertySingleLine(MakeLabel(reticleMap), reticleMap, FindProperty("_ReticleColor"));
        editor.ShaderProperty(FindProperty("_ReticleScale"), MakeLabel("Reticle Scale"));
        editor.ShaderProperty(FindProperty("_VerticalOffset"), MakeLabel("Vertical Offset"));

        if (EditorGUI.EndChangeCheck() && tex != reticleMap.textureValue)
        {
            SetKeyword("_RETICLE_MAP", reticleMap.textureValue);
        }
    }

    private void ShaderProperties ()
    {
        EditorUtilities.FoldoutHeader("Surface Inputs", ref surfaceInputs);

        if (surfaceInputs)
        {
            EditorGUI.indentLevel += 1;
            MaterialProperty mainTex = FindProperty("_MainTex");
            editor.TexturePropertySingleLine(MakeLabel("Base Map", "Specify the base color(RGB) and opacity(A)."), mainTex, FindProperty("_Color"));

            MaskProperties();

            editor.TextureScaleOffsetProperty(mainTex);
            EditorGUI.indentLevel -= 1;
        }


        EditorUtilities.FoldoutHeader("Reticle Inputs", ref reticleInputs);

        if (reticleInputs)
        {
            EditorGUI.indentLevel += 1;
            ReticleProperties();
            EditorGUI.indentLevel -= 1;
        }

        EditorUtilities.FoldoutHeader("Advanced Options", ref advancedOptions);

        if (advancedOptions)
        {
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
    }

    public override void OnGUI (MaterialEditor _editor, MaterialProperty[] _properties)
    {
        editor = _editor;
        properties = _properties;
        ShaderProperties();
    }
}
