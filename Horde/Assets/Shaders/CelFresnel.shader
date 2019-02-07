﻿Shader "Custom/CelFresnel"
{
	Properties
	{
		_MainTex("Texture", 2D) = "grey" {}
		_LightCells("Lighting Cells", 2D) = "white"{}
		_Color("Color", Color) = (0,0,0,0)
		_FrezColor("Fresnel Color", Color) = (1,1,1,1)
		_FrezPower("Fresnel Power", Range(0,16)) = 1
	}

		SubShader
		{
			Tags{ "RenderType" = "Opaque" }
			Cull Back
			CGPROGRAM
			#pragma surface surf SimpleLambert

			sampler2D _LightCells;
			float4 _Color;

			half4 LightingSimpleLambert(SurfaceOutput s, half3 lightDir, half atten)
			{
				half NdotL = dot(s.Normal, lightDir);
				NdotL = clamp(NdotL, .01, 1);
				half4 c;
				c.rgb = s.Albedo * _LightColor0.rgb * (tex2D(_LightCells, half2(NdotL, 0)).g * atten);
				c.a = s.Alpha;
				return c;
			}

			struct Input
			{
				float2 uv_MainTex;
				float3 worldNormal;
				float3 viewDir;
			};

			sampler2D _MainTex;

			fixed4 _FrezColor;
			half _FrezPower;
			void surf(Input IN, inout SurfaceOutput o)
			{
				o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb + _Color;

				float3 normal = normalize(IN.worldNormal);
				float3 dir = normalize(IN.viewDir);
				float val = 1 - (abs(dot(dir, normal)));
				float rim = val * val * _FrezPower;
				o.Albedo += rim * _FrezColor;
			}
		ENDCG
		}
			Fallback "Diffuse"
}