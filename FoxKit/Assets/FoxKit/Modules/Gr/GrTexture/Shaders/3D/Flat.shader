Shader "Hidden/FoxKit/GrTexturePreviews/3D/Flat"
{
	Properties
	{
		_MainTex ("", 3D) = "white" {}
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

			half3 GammaCorrection(float3 linearRGB)
			{
				float3 mask = step(linearRGB, 0.0031308);
				return (half3) ((mask * (linearRGB * 12.92)) + ((1 - mask) * (1.055 * pow(max(linearRGB, 0.00001), (1.0 / 2.4)) - 0.055)));
			}

			inline void NUvToUvw(half2 inUv, out half3 outUvw)
			{
				const half LUT_RESOLUTION = 16;

				const half UV_W = LUT_RESOLUTION * LUT_RESOLUTION;
				const half UV_Y = LUT_RESOLUTION;

				#ifdef DG_ENABLE_HALFPIXELOFFSET
								inUv -= half2(0.5 / UV_W, 0.5 / UV_Y);
				#endif

				const half3 scale0 = half3(LUT_RESOLUTION, 1.0, LUT_RESOLUTION);
				const half3 scale1 = half3(1.0, 1.0, 1.0 / LUT_RESOLUTION);

				const half3 uvw = scale0 * inUv.xyx;

				outUvw = scale1 * half3(frac(uvw.x), uvw.y, floor(uvw.z));
			}

			float4 frag(v2f_img i) : SV_TARGET
			{
				float width;
				float height;
				float depth;
				float mipmapCount;
				_MainTex.GetDimensions(0, width, height, depth, mipmapCount);

				float3 uv;
				NUvToUvw(i.uv, uv);
				float4 colour = _MainTex.Sample(my_point_clamp_sampler, uv);
				//colour.rgb = GammaCorrection(colour.rgb);
				return colour;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
