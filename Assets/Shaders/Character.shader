Shader "Custom/Character" {
	Properties {
		[PerRendererData]_MainTex ("Sprite Texture", 2D) = "white" {}

		_ColorInHelmet ("Color to replace 1", Color) = (1,1,1,1)
        _ColorInBody ("Color to replace 2", Color) = (1,1,1,1)


		_ColorOutHelmet ("Replace color 1", Color) = (1,1,1,1)
        _ColorOutBody ("Replace color 2", Color) = (1,1,1,1)

	}
	SubShader {
		Tags 
		{ 
			"Queue"="Geometry"
			"RenderType"="TransparentCutout"
			"PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
		}
		LOD 50

		Cull Off
		Lighting On
		
		CGPROGRAM
  
		#pragma surface surf Lambert vertex:vert
		#include "UnityCG.cginc"

		sampler2D _MainTex;
		fixed _Cutoff;

		struct Input
		{
			float2 uv_MainTex;
			fixed4 color;
		};

		fixed4 _ColorInHelmet;
        fixed4 _ColorInBody;

		fixed4 _ColorOutHelmet;
        fixed4 _ColorOutBody;

		void vert (inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			//camera basis
			float3 forward = float3(1, 0, 0);
			float3 up = normalize(UNITY_MATRIX_V._m10_m11_m12);
			float3 right = float3(1, 0, 0);
  
			float4x4 rotationMatrix = float4x4(right,   0,
												up,      0,
												forward, 0,
												0, 0, 0, 1);

			//float offset = _Object2World._m22 / 2;
			float offset = 0;
			v.vertex = mul(v.vertex + float4(0, offset, 0, 0), rotationMatrix) + float4(0, -offset, 0, 0);
		}

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 texColor = tex2D (_MainTex, IN.uv_MainTex);

			//character painting
			fixed dist1 = floor(distance(texColor,_ColorInHelmet));
            fixed dist2 = floor(distance(texColor,_ColorInBody));
			
			fixed3 finalColor = texColor.rgb;	
			
			if (dist1==0)
				finalColor = mul(finalColor,float3(1,1,1))*_ColorOutHelmet;
			else if (dist2==0)
				finalColor = mul(finalColor,float3(1,1,1))*_ColorOutBody;

			o.Albedo = finalColor;
			o.Alpha = texColor.a;
			clip(o.Alpha - 0.5);
		}
		
		ENDCG
	}
	FallBack "Diffuse"
}
