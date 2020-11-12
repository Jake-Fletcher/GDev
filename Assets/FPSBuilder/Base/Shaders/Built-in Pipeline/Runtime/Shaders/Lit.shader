//=========== Copyright (c) GameBuilders, All rights reserved. ================//

Shader "FPS Builder/Lit" 
{
	Properties 
	{
		_BaseColor("Color", Color) = (1,1,1,1)
		_BaseColorMap("Base Map", 2D) = "white" {}

		_Metallic ("Metallic", Range(0.0, 1.0)) = 0.0
		_Glossiness ("Smoothness", Range(0.0, 1.0)) = 0.5
		_OcclusionStrength("Occlusion Strength", Range(0.0, 1.0)) = 1.0

		[NoScaleOffset] _MaskMap("Mask Map", 2D) = "white" {}

		[NoScaleOffset] _NormalMap("NormalMap", 2D) = "bump" {}
		_NormalScale("NormalScale", Range(0.0, 8.0)) = 1

		[HDR] _EmissiveColor("EmissiveColor", Color) = (0, 0, 0)
		[NoScaleOffset] _EmissiveColorMap("EmissiveColorMap", 2D) = "white" {}

		[NoScaleOffset] _DetailMap("Detail Map", 2D) = "black" {}
		_DetailAlbedoScale("_DetailAlbedoScale", Range(0.0, 2.0)) = 1
		[NoScaleOffset] _DetailNormalMap("Detail Normal Map", 2D) = "bump" {}
        _DetailNormalScale("_DetailNormalScale", Range(0.0, 2.0)) = 1

		[Toggle(VIEWMODEL)] _Viewmodel ("Viewmodel", Float) = 0
		_ViewmodelFOV("Field of View", Range(1.0, 179.0)) = 60
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" "Queue" = "Geometry" }
		Blend Off
		ZWrite On
		LOD 300

		CGPROGRAM
		#pragma multi_compile __ _VIEWMODEL
		#pragma shader_feature	_NORMAL_MAP
		#pragma shader_feature	_MASK_MAP
		#pragma shader_feature	_EMISSION_MAP
		#pragma shader_feature	_DETAIL_ALBEDO_MAP
		#pragma shader_feature	_DETAIL_NORMAL_MAP

		#pragma surface surf Standard fullforwardshadows vertex:vert
		#pragma target 3.0
		
		sampler2D _BaseColorMap;
		sampler2D _MaskMap;
		sampler2D _NormalMap;
		sampler2D _EmissiveColorMap;
		sampler2D _DetailMap;
		sampler2D _DetailNormalMap;

		fixed4	_BaseColor;
		half	_Metallic;
		half	_Glossiness;
		half	_OcclusionStrength;
		half	_DetailAlbedoScale;
		half	_DetailNormalScale;
		fixed	_NormalScale;
		fixed4	_EmissiveColor;
		fixed	_ViewmodelFOV;

		struct Input 
		{
			float2 uv_MainTex;
			float2 uv_Detail;
		};

		void vert (inout appdata_full v) 
		{
			DefineFOV(_ViewmodelFOV);
      	}

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		float GetDetailMask (fixed4 mask)
		{
			#if defined (_MASK_MAP)
				return mask.b;
			#endif
			return 1;
		}

		float3 GetAlbedo (Input IN, fixed4 mask)
		{
			float3 albedo = tex2D(_BaseColorMap, IN.uv_MainTex).rgb * _BaseColor.rgb;
			#if defined (_DETAIL_ALBEDO_MAP)
				float3 detail = tex2D(_DetailMap, IN.uv_MainTex) * _DetailAlbedoScale;
				albedo = lerp(albedo, albedo * detail, GetDetailMask(mask));
			#endif
			return albedo;
		}

		float3 GetNormals (Input IN, fixed4 mask)
		{
			float3 normal = float3(0, 0, 1);
			#if defined (_NORMAL_MAP)
				normal = UnpackScaleNormal(tex2D(_NormalMap, IN.uv_MainTex), _NormalScale);
				#if defined (_DETAIL_NORMAL_MAP)
					float3 detailNormal = UnpackScaleNormal(tex2D(_DetailNormalMap, IN.uv_Detail), _DetailNormalScale);
					detailNormal = lerp(float3(0, 0, 1), detailNormal, GetDetailMask(mask));
					return BlendNormals(normal, detailNormal);
				#endif
			#endif
			return normal;
		}

		float3 GetEmission (Input IN)
		{
			#if defined(_EMISSION_MAP)
				return tex2D(_EmissiveColorMap, IN.uv_MainTex) * _EmissiveColor;
			#else
				return 0;
			#endif
		}

		half GetMetallic (fixed4 mask)
		{
			#if defined (_MASK_MAP)
				return lerp(1, mask.r, _Metallic);
			#else
				return _Metallic;
			#endif
		}

		half GetSmoothness (fixed4 mask)
		{
			#if defined (_MASK_MAP)
				return mask.a * _Glossiness;
			#else
				return _Glossiness;
			#endif
		}

		half GetOcclusion (fixed4 mask)
		{
			#if defined (_MASK_MAP)
				return lerp(1, mask.g, _OcclusionStrength);
			#else
				return _OcclusionStrength;
			#endif
		}

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			fixed4 mask;
			#if defined (_MASK_MAP)
				mask = tex2D(_MaskMap, IN.uv_MainTex);
			#else
				mask = 0;
			#endif

			o.Albedo = GetAlbedo(IN, mask);
			o.Normal = GetNormals(IN, mask);
			o.Emission = GetEmission(IN);
			o.Metallic = GetMetallic(mask);
			o.Smoothness = GetSmoothness(mask);
			o.Occlusion = GetOcclusion(mask);
		}
		ENDCG
	}
	
	FallBack "Diffuse"
	CustomEditor "LitShaderGUI"
}
