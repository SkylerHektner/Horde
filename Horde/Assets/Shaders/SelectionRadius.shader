Shader "Unlit/SelectionRadius"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Alpha ("Alpha", Range(0,1)) = 0.5
		_Color("Color", Color) = (0,0,0,0)
		_BloomSpeed("Bloom Speed", Range(.2,2)) = 1
		_BloomFadeOffset("Bloom Fade Offset", Range(0,1)) = 0.65
		_BloomFadeMagnitude("Bloom Fade Magnitude", Range(0,1)) = 0.5
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
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
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Alpha;
			half4 _Color;
			float _BloomSpeed;
			float _BloomFadeOffset;
			float _BloomFadeMagnitude;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				i.uv.x -= _Time.y * _BloomSpeed;
				fixed4 col = tex2D(_MainTex, i.uv) * _Color;
				col.a *= _Alpha * (abs(sin((_Time.y * _BloomSpeed + 0.65) * 3.14)) * _BloomFadeMagnitude + (1 - _BloomFadeMagnitude));
				return col;
			}
			ENDCG
		}
	}
}
