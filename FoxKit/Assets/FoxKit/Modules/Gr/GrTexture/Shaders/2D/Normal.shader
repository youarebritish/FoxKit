Shader "Hidden/FoxKit/GrTexturePreviews/2D/Normal"
{
	Properties
	{
		_MainTex ("", 2D) = "white" {}
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
			uint _MipLevel;

			half3 GammaCorrection(float3 linearRGB)
			{
				float3 mask = step(linearRGB, 0.0031308);
				return (half3) ((mask * (linearRGB * 12.92)) + ((1 - mask) * (1.055 * pow(max(linearRGB, 0.00001), (1.0 / 2.4)) - 0.055)));
			}

			inline half3 DecodeNormalTexture(half4 color)
			{
				half3 normal;

				normal.xyz = half3(color.agb) /** 2.0h - 1.0h*/;

				//half tmp = half(saturate(1.0h - normal.x * normal.x - normal.y * normal.y) + 0.0001h);

				normal.z = 1; //half(tmp * rsqrt(tmp));

				return normal;
			}

			float4 frag(v2f_img i) : SV_TARGET
			{
				float4 colour = _MainTex.SampleLevel(my_point_repeat_sampler, i.uv, _MipLevel);
				colour.rgb = DecodeNormalTexture(colour);
				//colour.rgb = GammaCorrection(colour.rgb);
				colour.w = 1;
				return colour;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
