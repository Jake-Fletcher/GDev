//=========== Copyright (c) GameBuilders, All rights reserved. ================//

Shader "FPS Builder/Sights/Telescopic Sight" 
{
	Properties 
	{
		_Color("Color", Color) = (1,1,1,1)
		[NoScaleOffset] _MainTex("Base Map", 2D) = "white" {}

		_ReticleColor("Reticle Color", Color) = (1,1,1,1)
		[NoScaleOffset]_ReticleMap ("Reticle (RGBA)", 2D) = "white" {}
		_ReticleOpacity("Reticle Opacity", Range(0.0, 1.0)) = 1
		_Aperture("Aperture", Range(0.0, 1.0)) = 0.5

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

		#pragma shader_feature	_RETICLE_MAP

		#pragma surface surf Standard fullforwardshadows vertex:vert
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _ReticleMap;

		fixed4 	_Color;
		fixed4	_ReticleColor;
		half	_Aperture;
		half 	_ReticleOpacity;
		fixed	_ViewmodelFOV;

		struct Input 
		{
			float2 uv_MainTex;
			float3 viewDir;
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

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			fixed4 tex = tex2D (_MainTex, IN.uv_MainTex) * _Color;

			#if defined (_RETICLE_MAP)
				half dotV = saturate(dot(IN.viewDir, o.Normal));
				fixed4 reticle = tex2D(_ReticleMap, IN.uv_MainTex) * _ReticleColor * (2 * _ReticleOpacity);
				tex.rgb = lerp(fixed4(0,0,0,1), lerp(tex.rgb, reticle.rgb, reticle.a), pow(dotV, (1 - _Aperture) * 300));
			#endif

			o.Emission = tex.rgb;
			o.Metallic = 0.5;
			o.Smoothness = 0;
		}
		ENDCG
	}
	FallBack "Diffuse"
	CustomEditor "TelescopicSightShaderGUI"
}
