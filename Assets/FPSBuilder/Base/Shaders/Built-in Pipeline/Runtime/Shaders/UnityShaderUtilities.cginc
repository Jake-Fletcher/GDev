#ifndef UNITY_SHADER_UTILITIES_INCLUDED
#define UNITY_SHADER_UTILITIES_INCLUDED

#define PI 3.1415926

#include "UnityShaderVariables.cginc"

float _fov = 60;

void DefineFOV (float fov)
{
    _fov = fov;
}

float reciprocal (float IN)
{
    #if SHADER_TARGET >= 50
        return rcp(IN);
    #else
        return 1.0 / IN;
    #endif
}

inline float4 UnityObjectToClipPos(in float3 pos)
{
    #if defined(_VIEWMODEL)
        float4x4 proj = UNITY_MATRIX_P;
        proj[0][0] = reciprocal((_ScreenParams.x / _ScreenParams.y) * tan(PI * ((_fov / 180.0) * 0.5)));
        proj[1][1] = reciprocal(tan(PI * ((_fov / 180.0) * 0.5)));

        float4 mvp = mul(mul(proj, UNITY_MATRIX_V), mul(unity_ObjectToWorld, float4(pos, 1.0)));

        #if SHADER_API_D3D11
            mvp.y = -mvp.y;
        #endif

        return mvp;
    #else
        return mul(UNITY_MATRIX_VP, mul(unity_ObjectToWorld, float4(pos, 1.0)));
    #endif
}

inline float4 UnityObjectToClipPos(in float4 pos)
{
    return UnityObjectToClipPos(pos.xyz);
}

#endif