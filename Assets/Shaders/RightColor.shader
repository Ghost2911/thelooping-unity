
Shader "Custom/RightColor"
{
	Properties {
		[PerRendererData]_MainTex ("Sprite Texture", 2D) = "white" {}
		_Cutoff("Shadow alpha cutoff", Range(0,1)) = 0.5
		_Color ("Tint", Color) = (1,1,1,1)
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
		Lighting Off

		CGPROGRAM

		#pragma surface surf Lambert vertex:vert

		sampler2D _MainTex;
		fixed _Cutoff;
		fixed4 _Color;

		struct Input
		{
			float2 uv_MainTex;
			fixed4 color;
		};

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
			v.normal = mul(v.normal, rotationMatrix);
		}

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex)*_Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			clip(o.Alpha - _Cutoff);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
