Shader "Custom/CelFresnel"
{
	Properties
	{
		_MainTex("Texture", 2D) = "grey" {}
		_NormalMap("Normal Map", 2D) = "bump" {}
		_EmitMap("Emission Map", 2D) = "black" {}
		_LightCells("Lighting Cells", 2D) = "white"{}
		_Color("Color", Color) = (0,0,0,0)
		_FrezColor("Fresnel Color", Color) = (1,1,1,1)
		_FrezPower("Fresnel Power", Range(0,10)) = 4
		_FrezStrength("Fresnel Strength", Range(0,4)) = 0
		_EmitIntensity("Emission Strength", Range(0,2)) = 1
	}

		SubShader
		{
			Tags{ "RenderType" = "Opaque" }
			CGPROGRAM
			#pragma surface surf SimpleLambert fullforwardshadows finalcolor:fresnel

			struct Input
			{
				float2 uv_MainTex;
				float2 uv2_NormalMap;
				float2 uv3_EmitMap;
				float3 viewDir;
				float3 worldNormal; INTERNAL_DATA
			};

			struct SurfaceOutputCustom
			{
				fixed3 Albedo;  // diffuse color
				fixed3 Normal;  // tangent space normal, if written
				fixed3 Emission;
				half Specular;  // specular power in 0..1 range
				fixed Gloss;    // specular intensity
				fixed Alpha;    // alpha for transparencies
				fixed3 worldNormal;
			};

			sampler2D _LightCells;
			float4 _Color;
			half4 LightingSimpleLambert(SurfaceOutputCustom s, half3 lightDir, half atten)
			{
				half NdotL = dot(s.Normal, lightDir);
				NdotL = clamp(NdotL, .2, 1);
				half4 c;
				c.rgb = s.Albedo * _LightColor0.rgb * tex2D(_LightCells, half2(NdotL, 0)).g * atten;
				c.a = s.Alpha;
				return c;
			}

			fixed4 _FrezColor;
			half _FrezPower;
			half _FrezStrength;
			void fresnel(Input IN, SurfaceOutputCustom o, inout fixed4 color)
			{
				float3 normal = normalize(o.worldNormal);
				float3 dir = normalize(IN.viewDir);
				float val = 1 - (abs(dot(dir, normal)));
				float rim = pow(val, _FrezPower) * _FrezStrength;
				rim = (rim * 10 - frac(rim * 10)) / 10;
					
				color += rim * _FrezColor;
			}

			sampler2D _MainTex;
			sampler2D _NormalMap;
			sampler2D _EmitMap;
			half _EmitIntensity;
			void surf(Input IN, inout SurfaceOutputCustom o)
			{
				WorldNormalVector(IN, o.Normal);
				o.worldNormal = o.Normal;

				o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _Color;

				o.Emission = tex2D(_EmitMap, IN.uv3_EmitMap) * _EmitIntensity;

				o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv2_NormalMap));
			}
		ENDCG
		}
			Fallback "Diffuse"
}