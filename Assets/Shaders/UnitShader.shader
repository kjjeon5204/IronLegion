Shader "Custom/UnitShader" {
	Properties {
		_Color("Color (RGBA)", 2D) = "white"{}
		_Normal("Normal", 2D) = "bump"{}
		_FactionMask("Faction Mask", 2D) = "black"{}
		_FactionColor("Faction Color (may be changed internally)", Color) = (0,1,1,1)
		_GlowMask("Glow Mask", 2D) = "black"{}
		_Shininess("Shininess", Range(0.01,1)) = 0.5
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _Color;
		sampler2D _Normal;
		sampler2D _FactionMask;
		sampler2D _GlowMask;
		float _Shininess;
		float4 _FactionColor;

		struct Input {
			float2 uv_Color;
			float3 viewDir;
			INTERNAL_DATA
		};

		void surf (Input IN, inout SurfaceOutput o)
		{
			float4 c = tex2D(_Color, IN.uv_Color);
			float fa = tex2D(_FactionMask, IN.uv_Color).r;

			o.Albedo = c.a*(1-fa)*c.rgb + (1-c.a)*fa*_FactionColor.rgb;
			o.Normal = UnpackNormal(tex2D(_Normal, IN.uv_Color));
			o.Specular = float3(1,1,1);
			o.Emission = o.Albedo*max(
					pow(dot(normalize(IN.viewDir),o.Normal),1000.0f*_Shininess),
					tex2D(_GlowMask, IN.uv_Color).r
					);
			o.Alpha = max(c.a,fa);
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
