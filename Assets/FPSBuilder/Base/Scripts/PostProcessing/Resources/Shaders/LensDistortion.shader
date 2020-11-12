Shader "Hidden/LensDistortion" 
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Distortion ("Distortion", Float) = -0.15  
        _CubicDistortion ("Cubic Distortion", Float) = 0.5 
    }

    SubShader
    {
        Pass
        {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct v2f 
            {
                float4 pos : POSITION;
                half2 uv : TEXCOORD0;
            };

            v2f vert(appdata_img v) 
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
                return o;
            }

            sampler2D _MainTex;
            fixed _Distortion;  
            fixed _CubicDistortion;

            float4 frag(v2f i) : COLOR
            {
                fixed2 r = i.uv - 0.5;
                fixed r2 = r.x * r.x + r.y * r.y;
                fixed f = 0;
                
                if(_CubicDistortion == 0)
                {
                    f = 1 + r2 * _Distortion;
                }
                else
                {
                    f = 1 + r2 * (_Distortion + _CubicDistortion * sqrt(r2));
                }
                
                fixed2 uv = f * (i.uv - 0.5) + 0.5;
                
                fixed4 col = tex2D(_MainTex, uv);
                return col;
            }
            
            ENDCG
        }
    }
    FallBack "Diffuse"
}