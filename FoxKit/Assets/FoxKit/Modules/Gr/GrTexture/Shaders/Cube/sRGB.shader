Shader "Hidden/FoxKit/GrTexturePreviews/Cube/sRGB"
{
	Properties
	{
		_MainTex ("", Cube) = "white" {}
		_MipLevel ("", Int) = 0
		_Alpha ("", Int) = 0
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			uniform TextureCube _MainTex;
			SamplerState my_point_repeat_sampler;
			uint _MipLevel;
			uint _Alpha;
			
			struct v2f
			{
				float4 position : SV_Position;
				float3 normal : NORMAL;
				float3 worldPosition : TEXCOORD0;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.worldPosition = mul(unity_ObjectToWorld, v.vertex);
				o.position = mul(UNITY_MATRIX_VP, o.worldPosition);
				o.normal = UnityObjectToClipPos(v.normal);

				return o;
			}

			half3 GammaCorrection(float3 linearRGB)
			{
				float3 mask = step(linearRGB, 0.0031308);
				return (half3) ((mask * (linearRGB * 12.92)) + ((1 - mask) * (1.055 * pow(max(linearRGB, 0.00001), (1.0 / 2.4)) - 0.055)));
			}

			float4 frag(v2f i) : SV_TARGET
			{
				float4 colour = float4(_MainTex.SampleLevel(my_point_repeat_sampler, reflect(normalize(i.worldPosition - _WorldSpaceCameraPos), normalize(i.normal)), _MipLevel).rgb, 1);
				colour.rgb = GammaCorrection(colour.rgb);

				return float4(1, 0, 0, 1);
				//return colour;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
