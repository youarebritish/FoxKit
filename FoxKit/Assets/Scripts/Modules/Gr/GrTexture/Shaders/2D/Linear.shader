Shader "Hidden/FoxKit/GrTexturePreviews/2D/Linear"
{
	Properties
	{
		_MainTex ("", 2D) = "white" {}
		_Swizzle ("", Vector) = (1, 1, 1, 0)
		_Full ("", Int) = 0
		_MipLevel("", Int) = 0
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"

			uniform Texture2D _MainTex;
			SamplerState my_point_repeat_sampler;
			float3 _Swizzle;
			uint _Full;
			uint _MipLevel;

			half3 GammaCorrection(float3 linearRGB)
			{
				float3 mask = step(linearRGB, 0.0031308);
				return (half3) ((mask * (linearRGB * 12.92)) + ((1 - mask) * (1.055 * pow(max(linearRGB, 0.00001), (1.0 / 2.4)) - 0.055)));
			}

			float4 frag(v2f_img i) : SV_TARGET
			{
				float4 colour = _MainTex.SampleLevel(my_point_repeat_sampler, i.uv, _MipLevel);
				colour.rgb = lerp(colour.rgb, dot(colour.rgb, _Swizzle).xxx, _Full);
				//colour.rgb = GammaCorrection(colour.rgb);
				return colour;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
