Shader "Custom/SimpleInnerVoronoi" {
    Properties {
        _Color ("Main Color", Color) = (1, 0.5, 0.8, 1)
        _Speed ("Animation Speed", Range(0, 5)) = 1.0
        _SizeT ("SizeT", Range(0, 1000)) = 10.0
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
            float _Speed;
            float _SizeT;
            
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            
            // Простейший генератор псевдослучайных чисел
            float simpleRand(float2 seed) {
                return frac(sin(dot(seed, float2(12.9898, 78.233))) * 43758.5453);
            }
            
            v2f vert(appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * _SizeT; // Масштаб текстуры
                return o;
            }
            
            fixed4 frag(v2f i) : SV_Target {
                // Создаем простой ячеистый узор
                float2 grid = floor(i.uv);
                float random = simpleRand(grid);
                
                // Анимация с помощью времени
                float movingPattern = sin(_Time.y * _Speed + random * 6.2831);
                
                // Создаем эффект ячеек
                float cells = step(0.8, movingPattern);
                
                // Применяем цвет
                fixed4 col = _Color;
                col.rgb *= (0.7 + 0.3 * cells); // Затемняем/осветляем ячейки
                
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}