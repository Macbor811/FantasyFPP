Shader "Custom/BloodSwordShader"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_BumpMap("Bumpmap", 2D) = "bump" {}
		[NoScaleOffset] _FlowMap("Flow (RG, A noise)", 2D) = "black" {}
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
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
			float2 uv_BumpMap;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		sampler2D _BumpMap, _FlowMap;

		float2 FlowUV(float2 uv, float2 flowVector, float time) {
			float progress = frac(time);
			return uv - flowVector * progress;
		}

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			float2 flowVector = tex2D(_FlowMap, IN.uv_MainTex).rg * 2 - 1;
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			float noise = tex2D(_FlowMap, IN.uv_MainTex).a;
			if (noise > 0.6) {
				c.r = 0.5;
				c.g = 0.0;
				c.b = 0.0;
				//c.g = 0.0;
			}
			//c.r = noise > 0.57 ? noise + c.r : c.r;

			o.Albedo = c.rgb;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;

		}
		ENDCG
	}
		FallBack "Diffuse"
}
