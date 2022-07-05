Shader "Hidden/ColorReplacement"
{
	Properties
	{	
		[HideInInspector] _MainTex ("Texture", 2D) = "white" {}

		// The mask color. This is the color that you replace with something.
		_MaskColor("Mask Color", Color) = (1,1,1,1)

		// _MaskTex and _ReplaceColor are the things that you replace the _MaskColor with. 
		// You can use both the texture and the color,or just one.
		_MaskTex("Mask Texture", 2D) = "white" {}
		_ReplaceColor("Replace Color", Color) = (1,0,1,1)

		// Tolerance of the mask. Higher number = replaces more. If you want the cleanest
		// results, use as low number as possible.
		_Diff("Tolerance", Range(0.0, 1.0)) = 0.05
	}
	SubShader
	{

        Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			sampler2D	_MainTex;
			float4		_MainTexure_ST;
			float4		_MaskTex_ST;
			sampler2D	_MaskTex;
			float4		_MaskColor;
			float4		_ReplaceColor;
			float		_Diff;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD1;
			};

			struct v2f
			{
				float4 position : SV_POSITION;
				float4 screenPosition : TEXCOORD0;
				float2 uv : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.position = UnityObjectToClipPos(v.vertex);
				o.screenPosition = ComputeScreenPos(o.position);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{				
				float2 textureCoordinate = i.screenPosition.xy / i.screenPosition.w;
				float aspect = _ScreenParams.x / _ScreenParams.y;
				textureCoordinate.x = textureCoordinate.x * aspect;
				textureCoordinate = TRANSFORM_TEX(textureCoordinate, _MaskTex);

				fixed4 col = tex2D(_MainTex, i.uv);
						
				if (col.r > _MaskColor.r - _Diff && col.r < _MaskColor.r + _Diff &&
					col.g > _MaskColor.g - _Diff && col.g < _MaskColor.g + _Diff &&
					col.b > _MaskColor.b - _Diff && col.b < _MaskColor.b + _Diff)
				{
					col = tex2D(_MaskTex, textureCoordinate);
					col.rgb *= _ReplaceColor;
				}

				return col;
			}
			ENDCG
		}
	}
}
