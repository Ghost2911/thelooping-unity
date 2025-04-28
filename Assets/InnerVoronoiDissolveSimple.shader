Shader "Custom/InnerVoronoiDissolveSimple" {
    Properties {
        _Color ("Main Color", Color) = (1, 0.5, 0.8, 1)
        _Color2 ("Secondary Color", Color) = (1, 0.5, 0.8, 1)
        _Speed ("Animation Speed", Range(0, 5)) = 1.0
        _SizeT ("Pattern Size", Range(0, 2000)) = 1000.0
        
        // Dissolve properties
        _DissolveAmount ("Dissolve Amount", Range(0, 1)) = 0.5
        _DissolveSize ("Dissolve Cell Size", Range(0.1, 10)) = 2.0
    }
    
    SubShader {
        Tags { "RenderType"="Opaque" }
        
        Pass {
            Cull Front // Отрисовываем только внутренние стороны
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            fixed4 _Color;
            fixed4 _Color2;
            float _Speed;
            float _SizeT;
            float _DissolveAmount;
            float _DissolveSize;
            
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };
            
            float simpleRand(float2 seed) {
                return frac(sin(dot(seed, float2(12.9898, 78.233))) * 43758.5453); 
            }
            
            float dissolvePattern(float3 worldPos) {
                float3 scaledPos = worldPos / _DissolveSize;
                float3 grid = floor(scaledPos);
                return simpleRand(grid.xy + grid.z) - _DissolveAmount;
            }
            
            v2f vert(appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * _SizeT;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }
            
            fixed4 frag(v2f i) : SV_Target {
                float dissolve = dissolvePattern(i.worldPos);
                clip(dissolve);
                
                float2 grid = floor(i.uv);
                float random = simpleRand(grid);
                float movingPattern = sin(_Time.y * _Speed + random * 6.2831);
                float cells = step(0.8, movingPattern);
                
                fixed4 col = _Color * (1-cells) + (_Color2 * cells);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}