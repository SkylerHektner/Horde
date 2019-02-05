Shader "Custom/RefractiveWater"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_NoiseScale("Wave Height", Range(0,4)) = 0.5
		_NoiseFrequency("Wave Frequency", Range(0,32)) = 0.5
		_NoiseOffset("Wave Offset", Vector) = (0,0,0,0)
		_SpecularIntensity("Specular Reflection Intensity", Range(0,1)) = 1
	}
		SubShader
	{
		Tags {"RenderType" = "Transparent" "Queue" = "Transparent" }
		Cull Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		#pragma surface surf SimpleSpecular vertex:vert alpha

		#include "noiseSimplex.cginc"

		sampler2D _MainTex;

		struct Input
		{
			float2 uv_MainTex;
			float4 grabUV;
			float4 refract;
		};

		struct appdata
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float3 tangent: TANGENT;
			float2 texcoord : TEXCOORD0;
		};

		float _NoiseScale;
		float _NoiseFrequency;
		float4 _NoiseOffset;
		void vert(inout appdata v, out Input o)
		{
			float3 v0 = v.vertex.xyz;
			float3 bitangent = cross(v.normal, v.tangent.xyz);
			float3 v1 = v0 + v.tangent.xyz * 0.01;
			float3 v2 = v0 + bitangent * 0.01;

			_NoiseOffset *= _Time.x;

			float ns0 = _NoiseScale * snoise(float3(v0.x + _NoiseOffset.x, v0.y + _NoiseOffset.y, v0.z + _NoiseOffset.z) * _NoiseFrequency);
			v0.xyz += ((ns0 + 1) / 2) * v.normal * 0.1;

			float ns1 = _NoiseScale * snoise(float3(v1.x + _NoiseOffset.x, v1.y + _NoiseOffset.y, v1.z + _NoiseOffset.z) * _NoiseFrequency);
			v1.xyz += ((ns1 + 1) / 2) * v.normal * 0.1;

			float ns2 = _NoiseScale * snoise(float3(v2.x + _NoiseOffset.x, v2.y + _NoiseOffset.y, v2.z + _NoiseOffset.z) * _NoiseFrequency);
			v2.xyz += ((ns2 + 1) / 2) * v.normal * 0.1;

			float3 vn = cross(v2 - v0, v1 - v0);

			v.normal = normalize(-vn);
			v.vertex.xyz = v0;

			UNITY_INITIALIZE_OUTPUT(Input, o);
		}

		float _SpecularIntensity;
		half4 LightingSimpleSpecular(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
			half3 h = normalize(lightDir + viewDir);

			half diff = max(0, dot(s.Normal, lightDir));

			float nh = max(0, dot(s.Normal, h));
			float spec = pow(nh, 200.0 * (1 - _SpecularIntensity));

			half4 c;
			c.rgb = (s.Albedo + _LightColor0.rgb * spec);
			c.a = s.Alpha;
			return c;
		}

		fixed4 _Color;
		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 c = _Color;
			o.Albedo = c.rgb;
			o.Alpha = _Color.a;
		}
		ENDCG
	}
		FallBack "Diffuse"
}
