Shader "Custom/Unlit/FieldColor"
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
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
								float4 color : COLOR;
								float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
								if (v.vertex.y < 5)
								{
									o.color = float4(0, 154.0f/255.0f, 237.0f/255.0f, 1);
								}
								else
								{
									float hight = 40;
									o.color = float4(v.vertex.y / hight, v.vertex.y / hight + 0.5f, v.vertex.y / hight, 1);
								}
								o.vertex = UnityObjectToClipPos(v.vertex);
								return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return i.color;
            }
            ENDCG
        }
    }
}
