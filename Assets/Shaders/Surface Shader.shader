Shader "Custom/Surface Shader" {
	Properties{
		_ValueNoiseTex("バリューノイズ", 2D) = "white" {}
		_ParlinNoiseTex("パーリンノイズ", 2D) = "white" {}
	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			#pragma surface surf Lambert

			sampler2D _ValueNoiseTex;
			sampler2D _ParlinNoiseTex;

			struct Input {
				float2 uv_MainTex;
				half4 color : COLOR;
			};

			float random(fixed2 p) 
			{
				return frac(sin(dot(p, fixed2(12.9898, 78.233))) * 43758.5453);
			}

			void surf(Input IN, inout SurfaceOutput o) {

				half4 c = tex2D(_ValueNoiseTex, IN.uv_MainTex * 10000);
				c += tex2D(_ValueNoiseTex, IN.uv_MainTex * 1000);
				c /= 2;
				float r = (tex2D(_ParlinNoiseTex, IN.uv_MainTex * 100)* IN.color.r).r;
				//if (IN.color.r != 0 && r > 0.1f)
				//{
				//	o.Albedo = float3(38.0f / 255.0f, 74.0f / 255.0f, 46 / 255.0f)* tex2D(_ValueNoiseTex, IN.uv_MainTex * 10000);
				//}
				//else
				//{
				//	o.Albedo = float3(204.0f / 255.0f, 195.0f / 255.0f, 161 / 255.0f) * tex2D(_ValueNoiseTex, IN.uv_MainTex * 50000);
				//}
				half h = IN.color.r + tex2D(_ParlinNoiseTex, IN.uv_MainTex * 100).r;
				h *= IN.color.r;
				if (h > 1) { h = 1; }
				half4 grass = float4(38.0f / 255.0f, 74.0f / 255.0f, 46 / 255.0f,1.0f) * tex2D(_ValueNoiseTex, IN.uv_MainTex * 10000) * 1.8f;
				half4 sand = (float4(204.0f / 255.0f, 195.0f / 255.0f, 161 / 255.0f, 1.0f) * tex2D(_ValueNoiseTex, IN.uv_MainTex * 50000) - (tex2D(_ParlinNoiseTex, IN.uv_MainTex * 100) * 0.1f)) * 2.0f;
				half4 dart = (float4(160.0f / 255.0f, 131.0f / 255.0f, 112 / 255.0f, 1.0f) * tex2D(_ValueNoiseTex, IN.uv_MainTex * 50000) - (tex2D(_ParlinNoiseTex, IN.uv_MainTex * 100)*0.1f))* 2.0f;
				o.Albedo = lerp(sand, dart,h);
				//o.Albedo = c.rgb * IN.color.rgb;
				//o.Albedo *= 1.8f;
				o.Alpha = c.a * IN.color.a;
			}
			ENDCG
	}
		FallBack "Diffuse"
}