Shader "Custom/DistortionFlow" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		[NoScaleOffset] _FlowMap("Flow (RG, A noise)", 2D) = "black" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_RMult("Red multiplier", Float) = 1
		_GMult("Green multiplier", Float) = 1
		_BMult("Blue multiplier", Float) = 1
	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			#pragma surface surf Standard fullforwardshadows
			#pragma target 3.0

			sampler2D _MainTex, _FlowMap;

			struct Input {
				float2 uv_MainTex;
			};

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;


			float2 FlowUV(float2 uv, float2 flowVector, float time) {
				return uv - flowVector * time;
			}

			float3 FlowUVW(float2 uv, float2 flowVector, float time) {
				float progress = frac(time);
				float3 uvw;
				uvw.xy = uv - flowVector * progress;
				uvw.z = 1;
				return uvw;
			}

			void surf(Input IN, inout SurfaceOutputStandard o) {
				float2 flowVector = tex2D(_FlowMap, IN.uv_MainTex).rg * 2 - 1;
				float noise = tex2D(_FlowMap, IN.uv_MainTex).a;
				float time = _Time.y + noise;
				float3 uvw = FlowUVW(IN.uv_MainTex, flowVector, time);
				fixed4 c = tex2D(_MainTex, uvw.xy) * uvw.z * _Color;

				//c.r = 2.0 * c.r;
				c.r = 0.8;
				c.b = 0;
				o.Albedo = c.rgb;
				//o.Albedo = float3(flowVector, 0);
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;
			}
			ENDCG
		}

			FallBack "Diffuse"
}
