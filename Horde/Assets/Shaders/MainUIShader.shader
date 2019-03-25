Shader "Unlit/MainUIShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_SecondaryTex("Secondary Texture", 2D) = "white" {}
		_SecondaryTexScroll("Scroll Speed", Range(0,100)) = 50
		_TertiaryTex("Tertiary Texture", 2D) = "white" {}
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
				float2 uv2 : TEXCOORD1;
				float2 uv3 : TEXCOORD2;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float2 uv3 : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

			sampler2D _SecondaryTex;
			float4 _SecondaryTex_ST;

			sampler2D _TertiaryTex;
			float4 _TertiaryTex_ST;

			float _SecondaryTexScroll;
			float _TertiaryBloomSpeed;
			float _TertiaryBloomMagnitude;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv2 = TRANSFORM_TEX(v.uv2, _SecondaryTex);
				o.uv3 = TRANSFORM_TEX(v.uv3, _TertiaryTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
				col *= tex2D(_SecondaryTex, i.uv2 + _SecondaryTexScroll * _Time.x);
				i.uv3.x += (i.uv3.x - 0.5) * sin(_Time.w * _TertiaryBloomSpeed) * _TertiaryBloomMagnitude;
				i.uv3.y += (i.uv3.y - 0.5) * sin(_Time.w * _TertiaryBloomSpeed) * _TertiaryBloomMagnitude;
				col *= tex2D(_TertiaryTex, i.uv3);
                return col;
            }
            ENDCG
        }
    }
}
