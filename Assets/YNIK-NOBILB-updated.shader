Shader "Unlit/NO-YNIK-Lit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _RotateAngle("RotateAngle", Range(-100, 100)) = 0
        _Offset("Offset", Range(0, 2)) = 0
        _ShadowColor("Shadow Color", Color) = (0,0,0,0.5)
        _ShadowOffset("Shadow Offset", Float) = 0.1
        _SpecularPower("Specular Power", Range(0, 50)) = 10
        _SpecularIntensity("Specular Intensity", Range(0, 1)) = 0.5
        _DiffuseIntensity("Diffuse Intensity", Range(0, 2)) = 1
    }
    SubShader
    {
        Tags {
            "LightMode"="ForwardBase" 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "DisableBatching"="True" 
        }

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        // Shadow pass
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode"="ShadowCaster" }
            
            ZWrite On
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert_shadow
            #pragma fragment frag_shadow
            #pragma multi_compile_shadowcaster
            
            #include "UnityCG.cginc"
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _RotateAngle;
            float _Offset;
            float _ShadowOffset;
            
            struct v2f_shadow {
                V2F_SHADOW_CASTER;
                float2 uv : TEXCOORD1;
            };
            
            v2f_shadow vert_shadow(appdata_base v)
            {
                v2f_shadow o;
                
                // Apply rotation
                float3 buf = v.vertex.xyz;
                buf = float3(
                    buf.x * cos(_RotateAngle) - (buf.y - _Offset) * sin(_RotateAngle),
                    buf.x * sin(_RotateAngle) + (buf.y - _Offset) * cos(_RotateAngle),
                    buf.z
                );
                v.vertex.xyz = float3(buf.x, buf.y*0.7, buf.y*0.7);
                
                // Shadow offset
                v.vertex.y -= _ShadowOffset;
                
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                return o;
            }
            
            float4 frag_shadow(v2f_shadow i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                clip(col.a - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }

        // Main lighting pass
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma multi_compile_fwdbase
            #pragma multi_compile_instancing
            
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD2;
                float3 worldNormal : TEXCOORD3;
                LIGHTING_COORDS(4,5)
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _RotateAngle;
            float _Offset;
            float _SpecularPower;
            float _SpecularIntensity;
            float _DiffuseIntensity;

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                
                // Apply rotation
                float3 buf = v.vertex.xyz;
                buf = float3(
                    buf.x * cos(_RotateAngle) - (buf.y - _Offset) * sin(_RotateAngle),
                    buf.x * sin(_RotateAngle) + (buf.y - _Offset) * cos(_RotateAngle),
                    buf.z
                );
                v.vertex.xyz = float3(buf.x, buf.y*0.7, buf.y*0.7);

                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                
                UNITY_TRANSFER_FOG(o, o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample texture
                fixed4 col = tex2D(_MainTex, i.uv);
                clip(col.a - 0.5);
                
                // Lighting calculations
                float3 worldNormal = normalize(i.worldNormal);
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                float3 halfVector = normalize(lightDir + viewDir);
                
                // Diffuse lighting
                float ndotl = max(0, dot(worldNormal, lightDir));
                float3 diffuse = _LightColor0.rgb * ndotl * _DiffuseIntensity;
                
                // Specular lighting (Blinn-Phong)
                float specular = pow(max(0, dot(worldNormal, halfVector)), _SpecularPower) * _SpecularIntensity;
                
                // Shadow attenuation
                float shadow = SHADOW_ATTENUATION(i);
                
                // Ambient light
                float3 ambient = ShadeSH9(float4(worldNormal, 1));
                
                // Combine all lighting
                float3 lighting = (diffuse + ambient + specular) * shadow;
                col.rgb *= lighting;
                
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
    Fallback "Transparent/Cutout/VertexLit"
}