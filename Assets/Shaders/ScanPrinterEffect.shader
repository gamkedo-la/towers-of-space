Shader "Custom/ScanPrinterEffect" {
	Properties {
		_LeadingEdgeColor ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_ConstructY ("Scan Height", Range(-1, 1)) = 0.5
		_ConstructGap ("Scan Width", Range(-1, 1)) = 0.1
		_ConstructColor ("Scan Color", Color) = (0.5, 0.5, 0.5, 1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		Cull Off

		CGPROGRAM
		#include "UnityPBSLighting.cginc"
		// Custom lighting model, and auto generate custom shadow pass
		#pragma surface surf Custom addshadow
		
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float3 viewDir;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _LeadingEdgeColor;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		float _ConstructY;
		float _ConstructGap;
		//float _ConstructGapInv = 1 / _ConstructGap;
		fixed4 _ConstructColor;
		int building;
		float3 viewDir;
		float fade;

		inline void LightingCustom_GI(SurfaceOutputStandard s, UnityGIInput data, inout UnityGI gi)
		{
			LightingStandard_GI(s, data, gi);
		}

		inline half4 LightingCustom(SurfaceOutputStandard s, half3 lightDir, UnityGI gi)
		{
			if (dot(s.Normal, viewDir) < 0)
				return _LeadingEdgeColor;

			if (building)
				return _LeadingEdgeColor * (1 - fade) + _ConstructColor * fade; // Unlit

			return LightingStandard(s, lightDir, gi) * fade + _ConstructColor * (1 - fade); // Unity5 PBR
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			viewDir = IN.viewDir;
			fade = 0;

			float s = +sin((IN.worldPos.x * IN.worldPos.z) * 30 + _Time[3] * 3 + o.Normal) / 120;

			if (IN.worldPos.y > _ConstructY + s + _ConstructGap) {
				discard;
			}

			if (IN.worldPos.y < s + _ConstructY) {
				fade = clamp((_ConstructY - IN.worldPos.y + s) / _ConstructGap, 0, 1);
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex);// * _LeadingEdgeColor;
				o.Albedo = c.rgb;
				o.Alpha = c.a;
				building = 0;
			}
			else {
				fade = clamp((_ConstructY - IN.worldPos.y + s + _ConstructGap) / _ConstructGap, 0, 1);
				o.Albedo = _LeadingEdgeColor.rgb;
				o.Alpha = _LeadingEdgeColor.a;
				building = 1;
			}

			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
