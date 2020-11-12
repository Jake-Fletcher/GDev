//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using System;
using FPSBuilder.Editor;
using UnityEditor;
using UnityEngine;

public class LitShaderGUI : ShaderGUI
{
    private static GUIContent staticLabel = new GUIContent();
    private MaterialEditor editor;
    private MaterialProperty[] properties;

    private bool surfaceInputs;
    private bool detailInputs;
    private bool emissionInputs;
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

    private void NormalProperties ()
    {
        MaterialProperty normalMap = FindProperty("_NormalMap");
        Texture tex = normalMap.textureValue;

        EditorGUI.BeginChangeCheck();
        editor.TexturePropertySingleLine(MakeLabel("Normal Map", "Specifies the Normal Map for this Material (BC7/BC5/DXT5(nm)) and control its strength."),
            normalMap, tex ? FindProperty("_NormalScale") : null);
        if (EditorGUI.EndChangeCheck() && tex != normalMap.textureValue)
        {
            SetKeyword("_NORMAL_MAP", normalMap.textureValue);
        }
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

    private void DetailProperties ()
    {
        MaterialProperty detailMap = FindProperty("_DetailMap");
        MaterialProperty detailNormalMap = FindProperty("_DetailNormalMap");
        Texture tex1 = detailMap.textureValue;
        Texture tex2 = detailNormalMap.textureValue;

        EditorGUI.BeginChangeCheck();
        editor.TexturePropertySingleLine(MakeLabel(detailMap), detailMap, FindProperty("_DetailAlbedoScale"));
        if (EditorGUI.EndChangeCheck() && tex1 != detailMap.textureValue)
        {
            SetKeyword("_DETAIL_ALBEDO_MAP", detailMap.textureValue);
        }

        EditorGUI.BeginChangeCheck();
        editor.TexturePropertySingleLine(MakeLabel(detailNormalMap), detailNormalMap, FindProperty("_DetailNormalScale"));
        if (EditorGUI.EndChangeCheck() && tex2 != detailNormalMap.textureValue)
        {
            SetKeyword("_DETAIL_NORMAL_MAP", detailNormalMap.textureValue);
        }
    }

    private void EmissionProperties ()
    {
        MaterialProperty emission = FindProperty("_EmissiveColorMap");
        Texture tex = emission.textureValue;

        EditorGUI.BeginChangeCheck();
        editor.TexturePropertySingleLine(MakeLabel("Emission Map", "Specifies the Emission Map (RGB) for this Material."), emission, FindProperty("_EmissiveColor"));
        if (EditorGUI.EndChangeCheck() && tex != emission.textureValue)
        {
            SetKeyword("_EMISSION_MAP", emission.textureValue);
        }
    }

    private void ShaderProperties ()
    {
        EditorUtilities.FoldoutHeader("Surface Inputs", ref surfaceInputs);

        if (surfaceInputs)
        {
            EditorGUI.indentLevel += 1;
            MaterialProperty mainTex = FindProperty("_BaseColorMap");
            editor.TexturePropertySingleLine(MakeLabel("Base Map", "Specify the base color(RGB) and opacity(A)."), mainTex, FindProperty("_BaseColor"));

            MaskProperties();
            NormalProperties();

            editor.TextureScaleOffsetProperty(mainTex);
            EditorGUI.indentLevel -= 1;
        }

        EditorUtilities.FoldoutHeader("Detail Inputs", ref detailInputs);

        if (detailInputs)
        {
            EditorGUI.indentLevel += 1;
            DetailProperties();
            EditorGUI.indentLevel -= 1;
        }

        EditorUtilities.FoldoutHeader("Emission Inputs", ref emissionInputs);

        if (emissionInputs)
        {
            EditorGUI.indentLevel += 1;
            EmissionProperties();
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
