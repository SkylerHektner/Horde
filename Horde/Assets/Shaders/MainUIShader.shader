Shader "Unlit/MainUIShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_SecondaryTex("Secondary Texture", 2D) = "white" {}
		_SecondaryTexUV("Secondary Tex Tiling", Vector) = (0,0,0,0)
		_SecondaryAlpha("Secondary Alpha", Range(0,1)) = 1
		_SecondaryTexScroll("Scroll Speed", Range(0,100)) = 50
		_TertiaryTex("Tertiary Tex", 2D) = "white" {}
		_TertiaryTexUV("Tertiary Tex Tiling", Vector) = (0,0,0,0)
		_TertiaryTexOffset("Tertiary Tex Offset", Vector) = (0,0,0,0)
		_TertiaryAlpha("Tertiary Alpha", Range(0,1)) = 1
		_TertiaryBloomSpeed("Bloom Speed", Range(0,1)) = 0.5
		_TertiaryBloomMagnitude("Bloom Magnitude", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderQueue" = "UI" }

        Pass
        {
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
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

			sampler2D _SecondaryTex;
			float4 _SecondaryTex_ST;
			float4 _SecondaryTexUV;

			sampler2D _TertiaryTex;
			float4 _TertiaryTex_ST;
			float4 _TertiaryTexUV;
			float4 _TertiaryTexOffset;

			float _SecondaryTexScroll;
			float _TertiaryBloomSpeed;
			float _TertiaryBloomMagnitude;
			float _SecondaryAlpha;
			float _TertiaryAlpha;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				// apply layer 1 tex
                fixed4 col = tex2D(_MainTex, i.uv);

				// apply layer 2 tex
				col *= (tex2D(_SecondaryTex, i.uv * _SecondaryTexUV.xy + _SecondaryTexScroll * _Time.x) * _SecondaryAlpha) + (col * (1 - _SecondaryAlpha));

				// apply layer 3 tex
				float2 uv3 = i.uv * _TertiaryTexUV.xy + _TertiaryTexOffset.xy;
				uv3.x += (uv3.x - 0.5) * sin(_Time.w * _TertiaryBloomSpeed) * _TertiaryBloomMagnitude;
				uv3.y += (uv3.y - 0.5) * sin(_Time.w * _TertiaryBloomSpeed) * _TertiaryBloomMagnitude;
				col *= (tex2D(_TertiaryTex, uv3) * _TertiaryAlpha) + (col * (1 - _TertiaryAlpha));

				// output color
                return col;
            }
            ENDCG
        }
    }
}
