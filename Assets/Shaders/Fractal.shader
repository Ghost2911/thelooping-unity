Shader "Custom/GruGruEffect" {
 
    SubShader {
            Tags { "RenderType"="Opaque" }
            LOD 100
 
        Pass {
            CGPROGRAM
 
                #include "UnityCG.cginc"
 
                #pragma vertex vert_img
                #pragma fragment frag
 
                #define PI 3.14159
 
                float4 frag( v2f_img i ) : COLOR {
 
                    float2 vec = i.uv.xy - float2(0.5, 0.5);
 
                    float l = length(vec);
                    float r = atan2(vec.y, vec.x) + PI;
                    float t = _Time.y*5;
                    float c = 1-sin(l*72+r+t);
                    c = step(0.5, c);
 
                    float3 rgb = float3(c,c,c);
 
                    return float4(rgb,1.0);
                }
 
            ENDCG
        }
    }
}
 