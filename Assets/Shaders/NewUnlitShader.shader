Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
						#pragma geometry geom
						#pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

						[maxvertexcount(9)]
						void geom(triangle v2f IN[3], inout TriangleStream<v2f> triStream)
						{
							v2f o;

							o.uv = (IN[0].uv + IN[1].uv + IN[2].uv) / 3;

							o.vertex = IN[0].vertex;
							triStream.Append(o);

							o.vertex = (IN[0].vertex + IN[1].vertex) / 2;
							triStream.Append(o);

							o.vertex = (IN[0].vertex + IN[2].vertex) / 2;
							triStream.Append(o);

							o.vertex = (IN[1].vertex + IN[2].vertex) / 2;
							triStream.Append(o);

							o.vertex = IN[2].vertex;
							triStream.Append(o);

							triStream.RestartStrip();

							o.vertex = (IN[0].vertex + IN[1].vertex) / 2;
							triStream.Append(o);

							o.vertex = IN[1].vertex;
							triStream.Append(o);

							o.vertex = (IN[1].vertex + IN[2].vertex) / 2;
							triStream.Append(o);

						}

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
