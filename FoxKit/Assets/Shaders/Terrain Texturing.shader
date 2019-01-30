Shader "FoxKit/Terrain Texturing"
{
	Properties
	{
		_AlbedoTexture("Albedo Atlas", 2D) = "white" {}
		_NormalTexture("Normal Atlas", 2D) = "bump" {}
		_SrmTexture("SRM Atlas", 2D) = "white" {}
		_HeightMap("Height Map", 2D) = "white" {}
		_MaterialSelectMapTexture("Material Select Map", 2D) = "white" {}
		_MaterialWeightMapTexture("Material Weight Map", 2D) = "white" {}
		_MaterialIndicesTexture("Material Indices", 2D) = "white" {}
		_HeightAmount("Height Amount", float) = 10.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _AlbedoTexture;
		sampler2D _NormalTexture;
		sampler2D _SrmTexture;

		sampler2D _HeightMap;
		sampler2D _MaterialSelectMapTexture;
		sampler2D _MaterialWeightMapTexture;
		sampler2D _MaterialIndicesTexture;

		float _HeightAmount;

		struct Input
		{
			float2 uv_HeightMap;
		};

		void vert(inout appdata_full v)
		{
			float4 height = tex2Dlod(_HeightMap, float4(v.texcoord.xy, 0, 0));
			v.vertex.y += height * _HeightAmount;
		}

		inline void NSelectMaterials(float2 physicalUv, out half4 outMaterialIndex, out half4 outMaterialWeight, out half outTopMaterialIndex)
		{
			half4 materialLists = floor(tex2D(_MaterialSelectMapTexture, physicalUv) * 255.4h);
			half4 materialWeights = tex2D(_MaterialWeightMapTexture, physicalUv);
			float4 materialAndWeight = (float4(materialLists * 1.0h / 256.0h) + float4(floor(materialWeights * 256.0h)));

			materialAndWeight.xy = materialAndWeight.x > materialAndWeight.y ? materialAndWeight.xy : materialAndWeight.yx;
			materialAndWeight.zw = materialAndWeight.z > materialAndWeight.w ? materialAndWeight.zw : materialAndWeight.wz;

			materialAndWeight.xz = materialAndWeight.x > materialAndWeight.z ? materialAndWeight.xz : materialAndWeight.zx;
			materialAndWeight.yw = materialAndWeight.y > materialAndWeight.w ? materialAndWeight.yw : materialAndWeight.wy;

			materialAndWeight.yz = materialAndWeight.y > materialAndWeight.z ? materialAndWeight.yz : materialAndWeight.zy;

			outMaterialIndex = (half4)frac(materialAndWeight) * 256.0h;
			outMaterialWeight = (half4)floor(materialAndWeight) / 256.0h;

			half2 materialSelectTableUvParam = half2(1.0h / 16.0h, 0.5h / 16.0h);
			half2 materialSelectTableUv = half2(outMaterialIndex.x * materialSelectTableUvParam.x + materialSelectTableUvParam.y, 0);
			outTopMaterialIndex = (half)tex2D(_MaterialIndicesTexture, materialSelectTableUv).x;

			outMaterialWeight.x += 0.001h;

			half totalWeight = outMaterialWeight.x + outMaterialWeight.y;
			outMaterialWeight.xy = outMaterialWeight.xy / totalWeight;
		}

#define MATERIAL_TEXTURE_RESOLUTION	(960.0f)
#define AVAILABLE_MIP_LEVEL_FOR_SIMPLE_MIPMAP	(5.0)	// 1024x1024 => 32x32

		inline void NFetchTerrainMaterial2(float2 inVirtualUv, half4 inMaterialIndex, half4 inAlpha, out half4 outAlbedoColor, out half3 outNormalColor, out half2 outSrmColor)
		{
			float2 texcoord = inVirtualUv.xy;

			half2 materialDdx = (half2)ddx(texcoord.xy);
			half2 materialDdy = (half2)ddy(texcoord.xy);
			half2 baseTexcoord = (half2)frac(texcoord);
			half maxDifferential = max(dot(materialDdx, materialDdx), dot(materialDdy, materialDdy));

			const half AVAILABLE_MIP_LEVEL = AVAILABLE_MIP_LEVEL_FOR_SIMPLE_MIPMAP;
			half materialLod = (half) min(max(0, log2(maxDifferential * MATERIAL_TEXTURE_RESOLUTION*MATERIAL_TEXTURE_RESOLUTION) * 0.5), AVAILABLE_MIP_LEVEL);

			half layer = 0;

			half atlasOffsetTop = 0;
			half atlasSize = 1.0 / 4.0;
			half atlasOffset = 32.0 / 1024.0 * atlasSize + atlasOffsetTop;
			half atlasScale = 960.0 / 1024.0 * atlasSize;

			half2 materialIndexHigh = (half2) floor(inMaterialIndex.xy / 4.0h);
			half2 materialIndexLow = inMaterialIndex.xy - materialIndexHigh * 4.0h;
			half4 materialIndex = half4(materialIndexLow, materialIndexHigh);
			float4 materialOffset = materialIndex * atlasSize + atlasOffset;
			half2 scaledTexcoord = baseTexcoord * atlasScale;
			float4 materialTexcoord = scaledTexcoord.xxyy + materialOffset;

			float4 texcoord0 = half4(materialTexcoord.xz, layer, materialLod);
			float4 texcoord1 = half4(materialTexcoord.yw, layer, materialLod);

			half4 fetchAlbedoColor = tex2D(_AlbedoTexture, texcoord0) * inAlpha.x;
			half4 rawNormal = tex2D(_NormalTexture, texcoord0);
			//half3 fetchNormalColor = UnpackNormal(half4(rawNormal.r, 1.0 - rawNormal.g, 1.0, 1.0) * inAlpha.x);
			half3 fetchNormalColor = UnpackNormal(half4(rawNormal.agb, 1.0)) * inAlpha.x;
			half2 fetchSrmColor = tex2D(_SrmTexture, texcoord0).xy * inAlpha.x;

			fetchAlbedoColor += tex2D(_AlbedoTexture, texcoord1) * inAlpha.y;
			half4 rawNormal2 = tex2D(_NormalTexture, texcoord1);
			//fetchNormalColor += UnpackNormal(half4(rawNormal2.r, 1.0 - rawNormal2.g, 1.0, 1.0)) * inAlpha.y;
			fetchNormalColor += UnpackNormal(half4(rawNormal2.agb, 1.0)) * inAlpha.y;
			fetchSrmColor += tex2D(_SrmTexture, texcoord1).xy * inAlpha.y;

			outAlbedoColor.xyzw = fetchAlbedoColor;
			outNormalColor.xyz = fetchNormalColor.xyz;
			outSrmColor.xy = fetchSrmColor.xy;
		}
		
		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			//fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = float3(0, 0, 0);

			o.Metallic = 0;
			o.Smoothness = 0;
			o.Alpha = 1;

			// Select materials.
			float2 physicalUv = IN.uv_HeightMap;

			half4 outMaterialIndex;
			half4 outMaterialWeight;
			half outTopMaterialIndex;
			NSelectMaterials(physicalUv, outMaterialIndex, outMaterialWeight, outTopMaterialIndex);

			half4 outAlbedo;
			half3 outNormal;
			half2 outSrm;
			NFetchTerrainMaterial2(physicalUv, outMaterialIndex, outMaterialWeight, outAlbedo, outNormal, outSrm);

			//o.Emission = tex2D(_HeightMap, physicalUv);
			//o.Emission = tex2D(_MaterialSelectMapTexture, physicalUv);
			//o.Emission = tex2D(_MaterialWeightMapTexture, physicalUv);
			//o.Emission = tex2D(_MaterialIndicesTexture, physicalUv);
			o.Albedo = outAlbedo;

			// Convert normal to Unity format.
			o.Normal = outNormal;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
