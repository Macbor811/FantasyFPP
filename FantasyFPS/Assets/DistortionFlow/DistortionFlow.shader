Shader "Custom/DistortionFlow" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		[NoScaleOffset] _FlowMap ("Flow (RG, A noise)", 2D) = "black" {}
		_Tiling ("Tiling", Float) = 1
		_Speed ("Speed", Float) = 1
		_FlowStrength ("Flow Strength", Float) = 1
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

	

		sampler2D _MainTex, _FlowMap, _DerivHeightMap;
		float  _Tiling, _Speed, _FlowStrength;
		float _HeightScale, _HeightScaleModulated;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		float3 FlowUVW(float2 uv, float2 flowVector, float tiling, float time, bool flowB) {
			float phaseOffset = flowB ? 0.5 : 0;
			float progress = frac(time + phaseOffset);
			float3 uvw;
			uvw.xy = uv - flowVector * progress;
			uvw.xy *= tiling;
			uvw.xy += phaseOffset;
			uvw.z = 1 - abs(1 - 2 * progress);
			return uvw;
		}


		void surf (Input IN, inout SurfaceOutputStandard o) {
			float3 flow = tex2D(_FlowMap, IN.uv_MainTex).rgb;
			flow.xy = flow.xy * 2 - 1;
			flow *= _FlowStrength;
			float noise = tex2D(_FlowMap, IN.uv_MainTex).a;
			float time = _Time.y * _Speed + noise;

			float3 uvwA = FlowUVW(
				IN.uv_MainTex, flow.xy, _Tiling, time, false
			);
			float3 uvwB = FlowUVW(
				IN.uv_MainTex, flow.xy, _Tiling, time, true
			);


			fixed4 texA = tex2D(_MainTex, uvwA.xy) * uvwA.z;
			fixed4 texB = tex2D(_MainTex, uvwB.xy) * uvwB.z;

			fixed4 c = (texA + texB) * _Color;
			c.r = 1;
			c.g *= 0.7;
			c.b = 0;

			o.Albedo = c.rgb;
			o.Emission = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}

	FallBack "Diffuse"
}