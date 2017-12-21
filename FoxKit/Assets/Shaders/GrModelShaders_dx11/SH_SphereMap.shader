Shader "GrModelShaders_dx11/SH_SphereMap"
{
	Properties
	{
		_Exposure ("Exposure", Range(0.0001, 10.0)) = 1.0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
// Upgrade NOTE: excluded shader from DX11, OpenGL ES 2.0 because it uses unsized arrays
//#pragma exclude_renderers d3d11 gles
// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it uses non-square matrices
//#pragma exclude_renderers gles
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			typedef float4x4			Matrix4x4;
			typedef float4x4			TMatrix4x4;

			inline float4 ApplyMatrix(Matrix4x4 mat, float4 vec)
			{
				return mul(mat, vec);
			}
			inline float4 ApplyMatrixT(TMatrix4x4 mat, float4 vec)
			{
				return mul(vec, mat);
			}
			inline float3 ApplyMatrixT(TMatrix4x4 mat, float3 vec)
			{
				return mul(float4(vec, 0), mat).xyz;
			}

			TMatrix4x4 _ParamSH[4];
			TMatrix4x4 _ParamSHSky[3];

			float _Exposure;

			#define DG_ENABLE_HALFPIXELOFFSET
			#define ENCODE_SPHEREMAP_NORMAL

			#define SPHEREMAP_PIXEL_SIZE (1.0/256.0)
			#define SPHEREMAP_HALF_SIZE (8.0)

			inline void NScreenCoordinateToDrawCoordinate(float2 inPosition, out float4 outPosition)
			{
				outPosition.xy = inPosition.xy * float2(2.0, -2.0) + float2(-1.0, 1.0);
				outPosition.zw = 1.0;
			}

			inline void NAddTexelOffset(float2 inPosition, out float2 outPosition)
			{
				#ifdef DG_ENABLE_HALFPIXELOFFSET
					outPosition.xy = inPosition.xy;
				#else
					outPosition.xy = inPosition.xy + float2(0.5 / SPHEREMAP_HALF_SIZE, -0.5 / SPHEREMAP_HALF_SIZE);
				#endif
			}
			
			float3 GetSphereMapping(float2 clipCoord)
			{
				float3 normal;

				const half	areaCorrectScale = 0.8125h;
				clipCoord *= (1.0f / areaCorrectScale);

				normal.xy = float2(-clipCoord.x, clipCoord.y);
				normal.z = -sqrt(max(0, 1 - normal.x * normal.x - normal.y * normal.y));

				float r2 = dot(normal.xy, normal.xy);
				normal.z = min(2 * r2 - 1, 1);
				normal.xy = normalize(normal.xy) * sqrt(1 - normal.z*normal.z);

				return normal;
			}

			inline void NDrawSphericalMap(float2 inClip, out half4 outRGB)
			{
				float exposure = 0.0h;
				/*#ifndef FOR_DEBUG_VIEW
					#ifdef DG_HIGH_PRECISION_LACC
						exposure = g_psScene.m_exposure.z;
					#else
						exposure = g_psScene.m_exposure.x;
					#endif
				#else
						exposure = g_psScene.m_exposure.x;
				#endif*/

				exposure = _Exposure;// 0.01f;// 1.0f; // TODO
				
				float3 viewNormal = GetSphereMapping(inClip.xy);
				float3 worldNormal = ApplyMatrixT(UNITY_MATRIX_V, float4(viewNormal, 0)).xyz;
				float4 cartesian = float4(-worldNormal.xz, worldNormal.y, 1);

				half occlusion;
				outRGB.r = (half)dot(cartesian*exposure, ApplyMatrixT(_ParamSH[0], cartesian));
				outRGB.g = (half)dot(cartesian*exposure, ApplyMatrixT(_ParamSH[1], cartesian));
				outRGB.b = (half)dot(cartesian*exposure, ApplyMatrixT(_ParamSH[2], cartesian));
				occlusion = (half)dot(cartesian, ApplyMatrixT(_ParamSH[3], cartesian));

				half3 skylight;
				skylight.r = (half)dot(cartesian*exposure, ApplyMatrixT(_ParamSHSky[0], cartesian));
				skylight.g = (half)dot(cartesian*exposure, ApplyMatrixT(_ParamSHSky[1], cartesian));
				skylight.b = (half)dot(cartesian*exposure, ApplyMatrixT(_ParamSHSky[2], cartesian));
				occlusion = (half)saturate(occlusion);

				outRGB.rgb += skylight * occlusion;
				outRGB.a = occlusion;
			}
						
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//UNITY_TRANSFER_FOG(o,o.vertex);

				float2 NScreenCoordinateToDrawCoordinate_drawCoordinate_inPosition;
				float4 NScreenCoordinateToDrawCoordinate_drawCoordinate_outPosition;
				NScreenCoordinateToDrawCoordinate_drawCoordinate_inPosition = o.vertex.xy;
				NScreenCoordinateToDrawCoordinate(NScreenCoordinateToDrawCoordinate_drawCoordinate_inPosition, NScreenCoordinateToDrawCoordinate_drawCoordinate_outPosition);

				float2 NAddTexelOffset_texelOffset_inPosition;
				float2 NAddTexelOffset_texelOffset_outPosition;
				NAddTexelOffset_texelOffset_inPosition = NScreenCoordinateToDrawCoordinate_drawCoordinate_outPosition.xy;
				NAddTexelOffset(NAddTexelOffset_texelOffset_inPosition, NAddTexelOffset_texelOffset_outPosition);

				o.uv = NAddTexelOffset_texelOffset_outPosition.xy;
				o.vertex.xyzw = NScreenCoordinateToDrawCoordinate_drawCoordinate_outPosition.xyzw;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2	NDrawSphericalMap_drawSphericalMap_inClip;
				half4 NDrawSphericalMap_drawSphericalMap_outRGB;
				NDrawSphericalMap_drawSphericalMap_inClip = i.uv.xy;

				NDrawSphericalMap(NDrawSphericalMap_drawSphericalMap_inClip, NDrawSphericalMap_drawSphericalMap_outRGB);
				return NDrawSphericalMap_drawSphericalMap_outRGB;
			}
			ENDCG
		}
	}
}
