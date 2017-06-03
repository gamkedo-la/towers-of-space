Shader "Custom/ScanPrinterEffect" {
	Properties {
		[HDR] _LeadingEdgeColor ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap ("Normal Map", 2D) = "bump" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_ConstructY ("Scan Height", Range(-1, 1)) = 0.5
		_ConstructGap ("Scan Width", Range(-1, 1)) = 0.1
		_ConstructColor ("Scan Color", Color) = (0.5, 0.5, 0.5, 1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		//Back face shader
		Cull Front
		CGPROGRAM
		#pragma surface surf Unlit
		#pragma target 3.0

		struct Input {
			float3 worldPos;
			float3 viewDir;
		};

		float _ConstructY;
		float _ConstructGap;
		fixed4 _LeadingEdgeColor;
		float3 viewDir;

		fixed4 LightingUnlit(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			if (dot(s.Normal, viewDir) < 0)
				return _LeadingEdgeColor;

			fixed4 c;
			c.rgb = s.Albedo; 
			c.a = s.Alpha;
			return c;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			viewDir = IN.viewDir;

			float s = +sin((IN.worldPos.x * IN.worldPos.z) * 30 + _Time[3] * 3 + o.Normal) / 120;

			if (IN.worldPos.y > _ConstructY + s + _ConstructGap) {
				discard;
			}

			o.Albedo = _LeadingEdgeColor.rgb;
			o.Alpha = _LeadingEdgeColor.a;
		}
		ENDCG

		//Front face shader
		Cull Back
		CGPROGRAM
		#include "UnityPBSLighting.cginc"
		// Custom lighting model, and auto generate custom shadow pass
		#pragma surface surf Custom addshadow
		
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float2 uv_BumpMap;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _LeadingEdgeColor;

		float _ConstructY;
		float _ConstructGap;
		fixed4 _ConstructColor;
		int building;
		float fade;

		inline void LightingCustom_GI(SurfaceOutputStandard s, UnityGIInput data, inout UnityGI gi)
		{
			LightingStandard_GI(s, data, gi);
		}

		inline half4 LightingCustom(SurfaceOutputStandard s, half3 lightDir, UnityGI gi)
		{
			if (building)
				return _LeadingEdgeColor * (1 - fade) + _ConstructColor * fade; // Leading edge fade into trailing edge

			return LightingStandard(s, lightDir, gi) * fade + _ConstructColor * (1 - fade); // Trailing fade into Unity5 PBR
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fade = 0;

			float s = +sin((IN.worldPos.x * IN.worldPos.z) * 30 + _Time[3] * 3 + o.Normal) / 120;

			if (IN.worldPos.y > _ConstructY + s + _ConstructGap) {
				discard;
			}

			if (IN.worldPos.y < s + _ConstructY) { //Trailing edge fade into texture
				fade = clamp((_ConstructY - IN.worldPos.y + s) / _ConstructGap, 0, 1);
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex);// * _LeadingEdgeColor;
				o.Albedo = c.rgb;
				o.Alpha = c.a;

				o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));

				building = 0;
			}
			else { //Leading edge fade into trailing edge
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
