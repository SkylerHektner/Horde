// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "PostProcessing/Greyscale"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_RMult("Red Multiplier", Range(0,1)) = .3
		_GMult("Green Multiplier", Range(0,1)) = .59
		_BMult("Blue Multiplier", Range(0,1)) = .11
		_GreyAmountRed("Greyscale Amount", Range(0,1)) = .5
		_GreyAmountGreen("Greyscale Amount", Range(0,1)) = .5
		_GreyAmountBlue("Greyscale Amount", Range(0,1)) = .5
	}
	SubShader
	{
		Tags{ "RenderType" = "Opaque" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 screenuv : TEXCOORD1;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.screenuv = ComputeScreenPos(o.pos);
				return o;
			}

			sampler2D _MainTex;
			float _RMult;
			float _GMult;
			float _BMult;
			float _GreyAmountRed;
			float _GreyAmountGreen;
			float _GreyAmountBlue;

			fixed4 frag(v2f i) : SV_Target
			{

				fixed4 col = tex2D(_MainTex, i.screenuv);
				float newCol = col.g * _GMult + col.r * _RMult * col.b * _BMult;
				col.r = col.r * (1 - _GreyAmountRed) + newCol * _GreyAmountRed;
				col.b = col.b * (1 - _GreyAmountBlue) + newCol * _GreyAmountBlue;
				col.g = col.g * (1 - _GreyAmountGreen) + newCol * _GreyAmountGreen;
				return col;
			}
			ENDCG
		}
	}
}