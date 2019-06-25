Shader "Sample/SineWave"
{
    Properties
    {
        _Flat("_Flat", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }


        Pass
        {
						//Tags { "LightMode" = "ShadowCaster" }

            CGPROGRAM
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag
            #pragma target 5.0
						#pragma multi_compile_shadowcaster

            #include "UnityCG.cginc"

            float _Flat;

            struct appdata
            {
                float4 pos : POSITION;
                float3 normal : NORMAL;
            };

            struct v2g
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 normal : TEXCOORD1;
            };

            struct g2f
            {
                float4 pos : SV_POSITION;
                float3 normal : TEXCOORD0;
								//V2F_SHADOW_CASTER;
            };

            v2g vert (appdata v)
            {
                v2g o;
                v.pos.y += sin(v.pos.x * 5.0 + _Time.x * 5.0) * cos(v.pos.z * 5.0 + _Time.x * 10.0);
                o.pos = UnityObjectToClipPos(v.pos);
                o.worldPos = mul(unity_ObjectToWorld, v.pos).xyz;
                o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            [maxvertexcount(3)]
            void geom (triangle v2g input[3], inout TriangleStream<g2f> outStream)
            {
                float3 wp0 = input[0].worldPos;
                float3 wp1 = input[1].worldPos;
                float3 wp2 = input[2].worldPos;
                float3 normal = normalize(cross(wp1 - wp0, wp2 - wp1));

                g2f output0;
                output0.pos = input[0].pos;
                output0.normal = lerp(input[0].normal, normal, _Flat); 
								//TRANSFER_SHADOW_CASTER(output0)

                g2f output1;
                output1.pos = input[1].pos;
                output1.normal = lerp(input[1].normal, normal, _Flat); 
								//TRANSFER_SHADOW_CASTER(output1)

                g2f output2;
                output2.pos = input[2].pos;
                output2.normal = lerp(input[2].normal, normal, _Flat); 
								//TRANSFER_SHADOW_CASTER(output2)

                outStream.Append(output0);
                outStream.Append(output1);
                outStream.Append(output2);
            }

            fixed4 frag (g2f i) : COLOR
            {
								//SHADOW_CASTER_FRAGMENT(i)
                return fixed4(i.normal * 0.5 + 0.5, 1.0);
            }

            ENDCG
        }
    }
}
