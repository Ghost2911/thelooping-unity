Shader "Custom/LineShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _ScrollSpeed ("Scroll Speed", Float) = 1.0
        _Tiling ("Tiling", Float) = 1.0
    }

    SubShader
    {
        Tags { 
            "RenderType"="Transparent" 
            "Queue"="Transparent"
        }
        
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _ScrollSpeed;
            float _Tiling;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex) * _Tiling;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Сдвигаем UV по X с учетом времени
                float2 scrolledUV = i.uv;
                scrolledUV.x += _Time.y * _ScrollSpeed;
                
                // Читаем текстуру и применяем цвет
                fixed4 col = tex2D(_MainTex, scrolledUV) * _Color;
                return col;
            }
            ENDCG
        }
    }
}