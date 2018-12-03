Shader "Unlit/ResourceBar"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}

		_Range("Fill Amount", Range(0,1)) = 0.5

		_Color("Color", Color) = (1,1,1,1)
		_Alpha("Alpha", Range(0,1)) = 0.7
		_WaveHeightMultiplier("Wave Height Multiplier", Range(0.5,2)) = 1
		_WaveFrequencyMultiplier("Wave Frequency", Range(0.5, 2)) = 1
		_EmissionColor("Emission Color", Color) = (0,0,0,0)
		_EmissionBandSize("Emission Band Size", Range(0, 0.1)) = 0.1
		_StartingOffset("Starting Offset", Vector) = (0,0,0,0)
		_ScrollSpeed("Texture Scroll Speed", Range(0.2, 4)) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 100

		Pass
		{
			blend SrcAlpha OneMinusSrcAlpha
			zWrite Off
			Cull Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			float _Range;
			float _WaveHeightMultiplier;
			float4 _EmissionColor;
			float _WaveFrequencyMultiplier;
			float _EmissionBandSize;
			float _Alpha;
			float4 _StartingOffset;
			float _ScrollSpeed;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv2 = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float heightClip = sin(i.uv2.x * 20 * _WaveFrequencyMultiplier + _Time.w) * 0.01 * _WaveHeightMultiplier + (1 - _Range);
				heightClip += sin(i.uv2.x * 15 * _WaveFrequencyMultiplier + _Time.w * 3) * 0.01 * _WaveHeightMultiplier + (1 - _Range);
				heightClip /= 2;
				clip((1 - i.uv2.y) - heightClip);

				half2 newUV = i.uv + half2(_Time.x, _Time.x) * _ScrollSpeed + _StartingOffset.xy;
				fixed4 col = tex2D(_MainTex, newUV);
				newUV = i.uv * .5 + half2(-_Time.x * .7, -_Time.x * .8) * _ScrollSpeed + _StartingOffset.xy;
				col *= tex2D(_MainTex, newUV);
				newUV = i.uv * 1.2 + half2(_Time.x * 1.9, -_Time.x * 1.2) * _ScrollSpeed + _StartingOffset.xy;
				col *= tex2D(_MainTex, newUV);
				newUV = i.uv * 1.6 + half2(-_Time.x * 4.5, _Time.x * .3) * _ScrollSpeed + _StartingOffset.xy;
				col *= tex2D(_MainTex, newUV);

				col *= _Color;
				col += _EmissionColor * (clamp((_EmissionBandSize - ((1 - heightClip) - i.uv2.y)), 0, 1) / _EmissionBandSize);

				col.a = _Alpha;

				return col;
			}
			ENDCG
		}
	}
}
