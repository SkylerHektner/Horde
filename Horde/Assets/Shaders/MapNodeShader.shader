Shader "Custom/MapNode" {
	Properties
	{
		_Color("Main Color", Color) = (.5,.5,.5,1)
		_RimValue("Rim value", Range(0, 2)) = 0.5
	}
	SubShader{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		Blend Off

		CGPROGRAM
		#pragma surface surf Lambert alpha

		float4 _Color;
		fixed _RimValue;

		struct Input 
		{
			float2 uv_MainTex;
			float3 viewDir;
			float3 worldNormal;
		};

		void surf(Input IN, inout SurfaceOutput o) 
		{
			half4 c = _Color;
			o.Albedo = c.rgb;
			float3 normal = normalize(IN.worldNormal);
			float3 dir = normalize(IN.viewDir);
			float val = abs(dot(dir, normal));
			float val2 = pow(val, 5);
			float rim = val * val * _RimValue;
			rim += val2 * val2 * _RimValue;
			o.Alpha = clamp(c.a * rim, 0, 1);
		}
		ENDCG
	}
	FallBack "Diffuse"
}