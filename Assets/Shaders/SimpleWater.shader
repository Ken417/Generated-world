Shader "Custom/SimpleWater"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
				_Amount("Height Adjustment", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
				#pragma surface surf Standard vertex:vert alpha:fade


        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

				float _Amount;
        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
						float3 pos : POSITION;
						float4 color : COLOR;
        };


        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

				void vert(inout appdata_full v)
				{
					//float mag = (v.vertex.x * v.vertex.x) + (v.vertex.z * v.vertex.z);
					//mag = sqrt(mag);
					//v.vertex.y = sin(_Time * _Amount + mag) / 3;

					v.vertex.y += sin(v.vertex.x * 5 + _Time.x * 5.0) * cos(v.vertex.z * 5 + _Time.x * 10.0);
					v.vertex.y *= 0.001f;
					v.color = _Color;
					v.color.r += (v.vertex.y * 30);
					v.color.g += (v.vertex.y * 30);
					v.color.b += (v.vertex.y* 50);
					v.color.a = 0.9f;
				}


        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            //fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = IN.color.rgb;
            // Metallic and smoothness come from slider variables
            //o.Metallic = _Metallic;
            //o.Smoothness = _Glossiness;
            o.Alpha = IN.color.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
