Shader "Custom/Flag"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
				_DefaultOffsetX("DefaultOffsetX", Float) = 1
				_DefaultFrequency("DefaultFrequency", Range(0, 1.3)) = 1
				_DefaultAmplitude("DefaultAmplitude", Range(0, 5.0)) = 1
				_Axis("Axis", Vector) = (0,0,0,0)
				_Speed("Speed", Range(0, 20.0)) = 1
				_Frequency("Frequency", Range(0, 1.3)) = 1
				_Amplitude("Amplitude", Range(0, 0.1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        //#pragma surface surf Standard fullforwardshadows
				#pragma surface surf Lambert vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _Color;

				float _DefaultOffsetX;
				float _DefaultFrequency;
				float _DefaultAmplitude;
				float _Axis;
				float _Speed;
				float _Frequency;
				float _Amplitude;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

				void vert(inout appdata_full v)
				{
					float f = abs(_Axis.x - v.vertex.x);
					v.vertex.y += sin((v.vertex.x+ _DefaultOffsetX)* _DefaultFrequency) * _DefaultAmplitude;
					v.vertex.y += cos((v.vertex.x + _Time.y * _Speed) * _Frequency) * _Amplitude*f*f;
					//o.pos = UnityObjectToClipPos(v.vertex);
					//o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				}

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
