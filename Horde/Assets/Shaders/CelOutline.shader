Shader "Custom/CelOutline" 
{
	Properties
	{
		_MainTex("Texture", 2D) = "grey" {}
		_LightCells("Lighting Cells", 2D) = "white"{}
		_Color("Color", Color) = (0,0,0,0)
		_OutlineColor("Outline Color", Color) = (0, 0, 0, 0)
		_Extrusion("Outline Width", Range(-0.1, 0.1)) = .05
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		Cull Front
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows vertex:vert

		struct Input
		{
			float2 uv_MainTex;
		};

		fixed4 _OutlineColor;
		float _Extrusion;

		void vert(inout appdata_full v)
		{
			if (_Extrusion > 0)
			{
				v.vertex.xyz += v.normal * _Extrusion;
			}
		}

		void surf(Input IN, inout SurfaceOutputStandard o) 
		{
			fixed4 c = _OutlineColor;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG

		Cull Back
		CGPROGRAM
		#pragma surface surf SimpleLambert vertex:vert

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
		};

		sampler2D _MainTex;
		float _Extrusion;

		void vert(inout appdata_full v)
		{
			if (_Extrusion < 0)
			{
				v.vertex.xyz += v.normal * _Extrusion;
			}
		}

		void surf(Input IN, inout SurfaceOutput o) 
		{
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb + _Color;
		}
	ENDCG
	}
	Fallback "Diffuse"
}