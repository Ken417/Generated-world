Shader "Custom/VoronoiNoise"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
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

				const float2x2 myt = float2x2(.12121212, .13131313, -.13131313, .12121212);
				const float2 mys = float2(1e4, 1e6);

				float2 rhash(float2 uv) {
					uv = mul(uv, myt);
					uv *= mys;
					return frac(frac(uv / mys) * uv);
				}

				float3 hash(float3 p) {
					return frac(sin(float3(dot(p, float3(1.0, 57.0, 113.0)),
						dot(p, float3(57.0, 113.0, 1.0)),
						dot(p, float3(113.0, 1.0, 57.0)))) *
						43758.5453);
				}

				float voronoi2d(float2 po) {
					float2 p = floor(po);
					float2 f = frac(po);
					float res = 0.0;
					for (int j = -1; j <= 1; j++) {
						for (int i = -1; i <= 1; i++) {
							float2 b = float2(i, j);
							float2 r = float2(b)-f + rhash(p + b);
							res += 1. / pow(dot(r, r), 8.);
						}
					}
					return pow(abs(1. / res), 0.0625);
				}

        void surf (Input IN, inout SurfaceOutputStandard o)
				{
					// Albedo comes from a texture tinted by color
					float f = voronoi2d(IN.uv_MainTex*1000);
					o.Albedo = float3(f,f,f);
					// Metallic and smoothness come from slider variables
					o.Metallic = _Metallic;
					o.Smoothness = _Glossiness;
					o.Alpha =1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
