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

			void surf(Input IN, inout SurfaceOutput o) {

				half4 c = tex2D(_ValueNoiseTex, IN.uv_MainTex * 10000);
				c += tex2D(_ValueNoiseTex, IN.uv_MainTex * 1000);
				c /= 2;
				o.Albedo = c.rgb * IN.color.rgb;
				o.Alpha = c.a * IN.color.a;
			}
			ENDCG
	}
		FallBack "Diffuse"
}