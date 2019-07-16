﻿Shader "Custom/Surface Shader" {
	Properties{
		_GrassTex("草のテクスチャ", 2D) = "white" {}
		_SandTex("砂のテクスチャ", 2D) = "white" {}
		_CliffTex("崖のテクスチャ", 2D) = "white" {}
		_ValueNoiseTex("バリューノイズ", 2D) = "white" {}
		_PerlinNoiseTex("パーリンノイズ", 2D) = "white" {}
		//_BumpMap("normal", 2D) = "white" {}
		_MinDist("Min Distance", Range(0, 5000)) = 10
		_MaxDist("Max Distance", Range(0.1, 5000)) = 25
		_TessFactor("Tessellation", Range(1, 50)) = 10
		_Displacement("Displacement", Range(0, 1.0)) = 0.3
	}

		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM

			#pragma surface surf BlinnPhong addshadow fullforwardshadows vertex:vert tessellate:tessDistance nolightmap
			//#pragma surface surf Lambert vertex:vert tessellate:tessDistance nolightmap
			#include "Tessellation.cginc"

			sampler2D _GrassTex;
			sampler2D _SandTex;
			sampler2D _CliffTex;
			sampler2D _ValueNoiseTex;
			sampler2D _PerlinNoiseTex;
			//sampler2D _BumpMap;
			float _TessFactor;
			float _Displacement;
			float _MinDist;
			float _MaxDist;

			struct Input {
				float2 uv_MainTex;
				float2 uv_BumpMap;
				half4 color : COLOR;
				float3 worldPos;
			};

			float random(fixed2 p) 
			{
				return frac(sin(dot(p, fixed2(12.9898, 78.233))) * 43758.5453);
			}

			half4 NoiseTex(half2 uv,float colorR)
			{
				half h = colorR + tex2D(_PerlinNoiseTex, uv * 100).r;
				h *= colorR;
				if (h > 1) { h = 1; }
				half4 grass = float4(38.0f / 255.0f, 74.0f / 255.0f, 46 / 255.0f, 1.0f) * tex2D(_ValueNoiseTex, uv * 10000) * 1.8f;
				half4 sand = (float4(204.0f / 255.0f, 195.0f / 255.0f, 161 / 255.0f, 1.0f) * tex2D(_ValueNoiseTex, uv * 50000) - (tex2D(_PerlinNoiseTex, uv * 100) * 0.1f)) * 2.0f;
				half4 dart = (float4(160.0f / 255.0f, 131.0f / 255.0f, 112 / 255.0f, 1.0f) * tex2D(_ValueNoiseTex, uv * 50000) - (tex2D(_PerlinNoiseTex, uv * 100) * 0.1f)) * 2.0f;
				return lerp(sand, dart, h);
			}

			half4 UseTex(half2 uv,half4 color)
			{

				half h = color.r + tex2D(_PerlinNoiseTex, uv * 100).r;
				h *= color.r;
				if (h > 1) { h = 1; }
				half4 grass = (tex2D(_GrassTex, uv * 1000) + tex2D(_GrassTex, uv * 10)) / 2;
				//half4 grass = tex2D(_GrassTex, uv * 1000);
				half4 sand = tex2D(_SandTex, uv * 1000);
				half4 cliff = (tex2D(_CliffTex, uv * 1000)+ tex2D(_CliffTex, uv * 10))/2;
				half4 result = lerp(sand, grass, h);

				result = lerp(result,cliff, color.g);

				return result;
			}

			float4 tessDistance(appdata_full v0, appdata_full v1, appdata_full v2)
			{
				return UnityDistanceBasedTess(v0.vertex, v1.vertex, v2.vertex, _MinDist, _MaxDist, _TessFactor);
			}

			void vert(inout appdata_full v) {
				//float d = tex2Dlod(_GrassTex, float4(v.texcoord.xy, 0, 0)).r * _Displacement;
				//v.vertex.xyz = v.normal * d;
				v.vertex.y = (tex2Dlod(_PerlinNoiseTex, float4(v.texcoord.xy, 0, 0)).r-0.5f) * _Displacement;
				
			}

			void surf(Input IN, inout SurfaceOutput o) 
			{

				half4 c = tex2D(_ValueNoiseTex, IN.uv_MainTex * 10000);
				c += tex2D(_ValueNoiseTex, IN.uv_MainTex * 1000);
				c /= 2;
				float r = (tex2D(_PerlinNoiseTex, IN.uv_MainTex * 100)* IN.color.r).r;
				//if (IN.color.r != 0 && r > 0.1f)
				//{
				//	o.Albedo = float3(38.0f / 255.0f, 74.0f / 255.0f, 46 / 255.0f)* tex2D(_ValueNoiseTex, IN.uv_MainTex * 10000);
				//}
				//else
				//{
				//	o.Albedo = float3(204.0f / 255.0f, 195.0f / 255.0f, 161 / 255.0f) * tex2D(_ValueNoiseTex, IN.uv_MainTex * 50000);
				//}
				o.Albedo = UseTex(IN.uv_MainTex, IN.color);
				//o.Albedo = c.rgb * IN.color.rgb;
				//o.Albedo *= 1.8f;
				o.Alpha = c.a * IN.color.a;
				//o.Normal = UnpackNormal((tex2D(_BumpMap, IN.uv_BumpMap * 1000)+ tex2D(_BumpMap, IN.uv_BumpMap * 10))/2);
			}
			ENDCG
	}
		FallBack "Diffuse"
}

