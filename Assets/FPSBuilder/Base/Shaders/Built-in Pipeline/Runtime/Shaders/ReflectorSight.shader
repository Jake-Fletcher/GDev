//=========== Copyright (c) GameBuilders, All rights reserved. ================//

Shader "FPS Builder/Sights/Reflector Sight" 
{
	Properties 
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Base Map", 2D) = "white" {}

		_Metallic ("Metallic", Range(0.0, 1.0)) = 0.0
		_Glossiness ("Smoothness", Range(0.0, 1.0)) = 0.5
		_OcclusionStrength("Occlusion Strength", Range(0.0, 1.0)) = 1.0

		[NoScaleOffset] _MaskMap("Mask Map", 2D) = "white" {}

		[HDR]_ReticleColor("Reticle Color", Color) = (1,1,1,1)
		_ReticleMap("Reticle", 2D) = "white" {}
		_ReticleScale ("Reticle Scale", Float) = 0.05
		_VerticalOffset("Vertical Offset", Float) = 0

		[Toggle(VIEWMODEL)] _Viewmodel ("Viewmodel", Float) = 0
		_ViewmodelFOV("Field of View", Range(1.0, 179.0)) = 60
	}

	SubShader 
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite false
		LOD 300

		CGPROGRAM
		#pragma multi_compile __ _VIEWMODEL

		#pragma shader_feature	_NORMAL_MAP
		#pragma shader_feature	_MASK_MAP
		#pragma shader_feature	_RETICLE_MAP

		#pragma surface surf Standard fullforwardshadows vertex:vert alpha
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _MaskMap;
		sampler2D _ReticleMap;

		fixed4 	_Color;
		half 	_Metallic;
		half 	_Glossiness;
		half 	_OcclusionStrength;

		fixed4 	_ReticleColor;
		half 	_ReticleScale;
		half 	_VerticalOffset;
		fixed	_ViewmodelFOV;

		struct Input 
		{
			float2 uv_MainTex;
			float3 worldPos;
			float3 worldNormal;
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
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex) * _Color;

			#if defined (_RETICLE_MAP)
				float dst = dot(_WorldSpaceCameraPos - IN.worldPos, IN.worldNormal);
				float3 cp = _WorldSpaceCameraPos - (dst * IN.worldNormal);
				float2 uv_Reticle = (mul((float3x3)unity_WorldToObject, IN.worldPos).xy - mul((float3x3)unity_WorldToObject, cp)).xy * (1 / _ReticleScale);

				fixed4 reticleTex = tex2D(_ReticleMap, float2(0.5, 0.5 + _VerticalOffset * 2) + uv_Reticle / dst);
				o.Emission = (reticleTex.a * _ReticleColor.rgb * _ReticleColor.a);
				o.Albedo = max(tex.rgb, reticleTex.a * _ReticleColor.rgb);
				o.Alpha = max(tex.a, reticleTex.a);
			#else
				o.Albedo = tex.rgb;
				o.Alpha = tex.a;
			#endif

			fixed4 mask;
			#if defined (_MASK_MAP)
				mask = tex2D(_MaskMap, IN.uv_MainTex);
			#else
				mask = 0;
			#endif

			o.Metallic = GetMetallic(mask);
			o.Smoothness = GetSmoothness(mask);
			o.Occlusion = GetOcclusion(mask);
		}
		ENDCG
	}
	FallBack "Diffuse"
	CustomEditor "ReflectorSightShaderGUI"
}
