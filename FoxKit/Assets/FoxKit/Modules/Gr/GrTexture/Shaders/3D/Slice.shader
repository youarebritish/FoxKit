Shader "Hidden/FoxKit/GrTexturePreviews/3D/Slice"
{
	Properties
	{
		_MainTex ("", 3D) = "white" {}
		_Slice ("", Range(0, 1)) = 0
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"

			uniform Texture3D _MainTex;
			SamplerState my_point_clamp_sampler;
			float _Slice;

			half3 GammaCorrection(float3 linearRGB)
			{
				float3 mask = step(linearRGB, 0.0031308);
				return (half3) ((mask * (linearRGB * 12.92)) + ((1 - mask) * (1.055 * pow(max(linearRGB, 0.00001), (1.0 / 2.4)) - 0.055)));
			}

			float4 frag(v2f_img i) : SV_TARGET
			{
				float4 colour = _MainTex.Sample(my_point_clamp_sampler, float3(i.uv.x, i.uv.y, _Slice));
				return colour;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
