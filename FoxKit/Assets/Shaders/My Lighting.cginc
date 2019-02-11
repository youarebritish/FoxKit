#if !defined(MY_LIGHTING_INCLUDED)
#define MY_LIGHTING_INCLUDED

#include "My Lighting Input.cginc"

#if !defined(ALBEDO_FUNCTION)
	#define ALBEDO_FUNCTION GetAlbedo
#endif

void ComputeVertexLightColor (inout InterpolatorsVertex i) {
	#if defined(VERTEXLIGHT_ON)
		i.vertexLightColor = Shade4PointLights(
			unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
			unity_LightColor[0].rgb, unity_LightColor[1].rgb,
			unity_LightColor[2].rgb, unity_LightColor[3].rgb,
			unity_4LightAtten0, i.worldPos.xyz, i.normal
		);
	#endif
}

float3 CreateBinormal (float3 normal, float3 tangent, float binormalSign) {
	return cross(normal, tangent.xyz) *
		(binormalSign * unity_WorldTransformParams.w);
}

InterpolatorsVertex MyVertexProgram (VertexData v) {
	InterpolatorsVertex i;
	UNITY_INITIALIZE_OUTPUT(InterpolatorsVertex, i);
	UNITY_SETUP_INSTANCE_ID(v);
	UNITY_TRANSFER_INSTANCE_ID(v, i);

	i.uv.xy = TRANSFORM_TEX(v.uv, _AlbedoTexture);

	#if VERTEX_DISPLACEMENT
		float displacement = tex2Dlod(_DisplacementMap, float4(i.uv.xy, 0, 0)).g;
		displacement = ((_ParallaxStrengthMax - _ParallaxStrengthMin) * displacement) + _ParallaxStrengthMin;
		
		v.vertex.y += displacement;
		
		float texelSize = 1.0f / 2048.0f;
		float4 h;
		h.x = tex2Dlod(_DisplacementMap, i.uv + float4(texelSize * float2(0, -1), 0, 0)).r;
		h.y = tex2Dlod(_DisplacementMap, i.uv + float4(texelSize * float2(-1, 0), 0, 0)).r;
		h.z = tex2Dlod(_DisplacementMap, i.uv + float4(texelSize * float2(1, 0), 0, 0)).r;
		h.w = tex2Dlod(_DisplacementMap, i.uv + float4(texelSize * float2(0, 1), 0, 0)).r;

		v.normal.z = h.x - h.w;
		v.normal.x = h.y - h.z;
		v.normal.y = 2;
		
		v.normal = normalize(v.normal).rbg;
	#endif

	i.pos = UnityObjectToClipPos(v.vertex);
	i.worldPos.xyz = mul(unity_ObjectToWorld, v.vertex);
	#if FOG_DEPTH
		i.worldPos.w = i.pos.z;
	#endif
	i.normal = UnityObjectToWorldNormal(v.normal);

	#if defined(BINORMAL_PER_FRAGMENT)
		i.tangent = float4(UnityObjectToWorldDir(v.tangent.xyz), v.tangent.w);
	#else
		i.tangent = UnityObjectToWorldDir(v.tangent.xyz);
		i.binormal = CreateBinormal(i.normal, i.tangent, v.tangent.w);
	#endif

	#if defined(LIGHTMAP_ON) || ADDITIONAL_MASKED_DIRECTIONAL_SHADOWS
		i.lightmapUV = v.uv1 * unity_LightmapST.xy + unity_LightmapST.zw;
	#endif

	#if defined(DYNAMICLIGHTMAP_ON)
		i.dynamicLightmapUV =
			v.uv2 * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
	#endif

	UNITY_TRANSFER_SHADOW(i, v.uv1);

	ComputeVertexLightColor(i);

	#if defined (_PARALLAX_MAP)
		#if defined(PARALLAX_SUPPORT_SCALED_DYNAMIC_BATCHING)
			v.tangent.xyz = normalize(v.tangent.xyz);
			v.normal = normalize(v.normal);
		#endif
		float3x3 objectToTangent = float3x3(
			v.tangent.xyz,
			cross(v.normal, v.tangent.xyz) * v.tangent.w,
			v.normal
		);
		i.tangentViewDir = mul(objectToTangent, ObjSpaceViewDir(v.vertex));
	#endif

	return i;
}

float FadeShadows (Interpolators i, float attenuation) {
	#if HANDLE_SHADOWS_BLENDING_IN_GI || ADDITIONAL_MASKED_DIRECTIONAL_SHADOWS
		// UNITY_LIGHT_ATTENUATION doesn't fade shadows for us.
		#if ADDITIONAL_MASKED_DIRECTIONAL_SHADOWS
			attenuation = SHADOW_ATTENUATION(i);
		#endif
		float viewZ =
			dot(_WorldSpaceCameraPos - i.worldPos, UNITY_MATRIX_V[2].xyz);
		float shadowFadeDistance =
			UnityComputeShadowFadeDistance(i.worldPos, viewZ);
		float shadowFade = UnityComputeShadowFade(shadowFadeDistance);
		float bakedAttenuation =
			UnitySampleBakedOcclusion(i.lightmapUV, i.worldPos);
		attenuation = UnityMixRealtimeAndBakedShadows(
			attenuation, bakedAttenuation, shadowFade
		);
	#endif

	return attenuation;
}

UnityLight CreateLight (Interpolators i) {
	UnityLight light;

	#if defined(DEFERRED_PASS) || SUBTRACTIVE_LIGHTING
		light.dir = float3(0, 1, 0);
		light.color = 0;
	#else
		#if defined(POINT) || defined(POINT_COOKIE) || defined(SPOT)
			light.dir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos.xyz);
		#else
			light.dir = _WorldSpaceLightPos0.xyz;
		#endif

		UNITY_LIGHT_ATTENUATION(attenuation, i, i.worldPos.xyz);
		attenuation = FadeShadows(i, attenuation);

		light.color = _LightColor0.rgb * attenuation;
	#endif

	return light;
}

float3 BoxProjection (
	float3 direction, float3 position,
	float4 cubemapPosition, float3 boxMin, float3 boxMax
) {
	#if UNITY_SPECCUBE_BOX_PROJECTION
		UNITY_BRANCH
		if (cubemapPosition.w > 0) {
			float3 factors =
				((direction > 0 ? boxMax : boxMin) - position) / direction;
			float scalar = min(min(factors.x, factors.y), factors.z);
			direction = direction * scalar + (position - cubemapPosition);
		}
	#endif
	return direction;
}

void ApplySubtractiveLighting (
	Interpolators i, inout UnityIndirect indirectLight
) {
	#if SUBTRACTIVE_LIGHTING
		UNITY_LIGHT_ATTENUATION(attenuation, i, i.worldPos.xyz);
		attenuation = FadeShadows(i, attenuation);

		float ndotl = saturate(dot(i.normal, _WorldSpaceLightPos0.xyz));
		float3 shadowedLightEstimate =
			ndotl * (1 - attenuation) * _LightColor0.rgb;
		float3 subtractedLight = indirectLight.diffuse - shadowedLightEstimate;
		subtractedLight = max(subtractedLight, unity_ShadowColor.rgb);
		subtractedLight =
			lerp(subtractedLight, indirectLight.diffuse, _LightShadowData.x);
		indirectLight.diffuse = min(subtractedLight, indirectLight.diffuse);
	#endif
}

UnityIndirect CreateIndirectLight (Interpolators i, float3 viewDir) {
	UnityIndirect indirectLight;
	indirectLight.diffuse = 0;
	indirectLight.specular = 0;

	#if defined(VERTEXLIGHT_ON)
		indirectLight.diffuse = i.vertexLightColor;
	#endif

	#if defined(FORWARD_BASE_PASS) || defined(DEFERRED_PASS)
		#if defined(LIGHTMAP_ON)
			indirectLight.diffuse =
				DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lightmapUV));
			
			#if defined(DIRLIGHTMAP_COMBINED)
				float4 lightmapDirection = UNITY_SAMPLE_TEX2D_SAMPLER(
					unity_LightmapInd, unity_Lightmap, i.lightmapUV
				);
				indirectLight.diffuse = DecodeDirectionalLightmap(
					indirectLight.diffuse, lightmapDirection, i.normal
				);
			#endif

			ApplySubtractiveLighting(i, indirectLight);
		#endif

		#if defined(DYNAMICLIGHTMAP_ON)
			float3 dynamicLightDiffuse = DecodeRealtimeLightmap(
				UNITY_SAMPLE_TEX2D(unity_DynamicLightmap, i.dynamicLightmapUV)
			);

			#if defined(DIRLIGHTMAP_COMBINED)
				float4 dynamicLightmapDirection = UNITY_SAMPLE_TEX2D_SAMPLER(
					unity_DynamicDirectionality, unity_DynamicLightmap,
					i.dynamicLightmapUV
				);
            	indirectLight.diffuse += DecodeDirectionalLightmap(
            		dynamicLightDiffuse, dynamicLightmapDirection, i.normal
            	);
			#else
				indirectLight.diffuse += dynamicLightDiffuse;
			#endif
		#endif

		#if !defined(LIGHTMAP_ON) && !defined(DYNAMICLIGHTMAP_ON)
			#if UNITY_LIGHT_PROBE_PROXY_VOLUME
				if (unity_ProbeVolumeParams.x == 1) {
					indirectLight.diffuse = SHEvalLinearL0L1_SampleProbeVolume(
						float4(i.normal, 1), i.worldPos
					);
					indirectLight.diffuse = max(0, indirectLight.diffuse);
					#if defined(UNITY_COLORSPACE_GAMMA)
			            indirectLight.diffuse =
			            	LinearToGammaSpace(indirectLight.diffuse);
			        #endif
				}
				else {
					indirectLight.diffuse +=
						max(0, ShadeSH9(float4(i.normal, 1)));
				}
			#else
				indirectLight.diffuse += max(0, ShadeSH9(float4(i.normal, 1)));
			#endif
		#endif

		float3 reflectionDir = reflect(-viewDir, i.normal);
		Unity_GlossyEnvironmentData envData;
		envData.roughness = 1 - GetSmoothness(i);
		envData.reflUVW = BoxProjection(
			reflectionDir, i.worldPos.xyz,
			unity_SpecCube0_ProbePosition,
			unity_SpecCube0_BoxMin, unity_SpecCube0_BoxMax
		);
		float3 probe0 = Unity_GlossyEnvironment(
			UNITY_PASS_TEXCUBE(unity_SpecCube0), unity_SpecCube0_HDR, envData
		);
		envData.reflUVW = BoxProjection(
			reflectionDir, i.worldPos.xyz,
			unity_SpecCube1_ProbePosition,
			unity_SpecCube1_BoxMin, unity_SpecCube1_BoxMax
		);
		#if UNITY_SPECCUBE_BLENDING
			float interpolator = unity_SpecCube0_BoxMin.w;
			UNITY_BRANCH
			if (interpolator < 0.99999) {
				float3 probe1 = Unity_GlossyEnvironment(
					UNITY_PASS_TEXCUBE_SAMPLER(unity_SpecCube1, unity_SpecCube0),
					unity_SpecCube0_HDR, envData
				);
				indirectLight.specular = lerp(probe1, probe0, interpolator);
			}
			else {
				indirectLight.specular = probe0;
			}
		#else
			indirectLight.specular = probe0;
		#endif

		float occlusion = GetOcclusion(i);
		indirectLight.diffuse *= occlusion;
		indirectLight.specular *= occlusion;

		#if defined(DEFERRED_PASS) && UNITY_ENABLE_REFLECTION_BUFFERS
			indirectLight.specular = 0;
		#endif
	#endif

	return indirectLight;
}

float GetParallaxHeight(float2 uv) {
	return tex2D(_ParallaxMap, uv).g;
}

void InitializeFragmentNormal(inout Interpolators i) {
	float3 tangentSpaceNormal = GetTangentSpaceNormal(i);

	#if defined(BINORMAL_PER_FRAGMENT)
		float3 binormal = CreateBinormal(i.normal, i.tangent.xyz, i.tangent.w);
	#else
		float3 binormal = i.binormal;
	#endif
	
	i.normal = normalize(
		tangentSpaceNormal.x * i.tangent +
		tangentSpaceNormal.y * binormal +
		tangentSpaceNormal.z * i.normal
	);
}

float4 ApplyFog (float4 color, Interpolators i) {
	#if FOG_ON
		float viewDistance = length(_WorldSpaceCameraPos - i.worldPos.xyz);
		#if FOG_DEPTH
			viewDistance = UNITY_Z_0_FAR_FROM_CLIPSPACE(i.worldPos.w);
		#endif
		UNITY_CALC_FOG_FACTOR_RAW(viewDistance);
		float3 fogColor = 0;
		#if defined(FORWARD_BASE_PASS)
			fogColor = unity_FogColor.rgb;
		#endif
		color.rgb = lerp(fogColor, color.rgb, saturate(unityFogFactor));
	#endif
	return color;
}

float2 ParallaxOffset (float2 uv, float2 viewDir) {
	float height = GetParallaxHeight(uv);
	height -= 0.5;
	height *= _ParallaxStrengthMin;
	return viewDir * height;
}

float2 ParallaxRaymarching (float2 uv, float2 viewDir) {
	#if !defined(PARALLAX_RAYMARCHING_STEPS)
		#define PARALLAX_RAYMARCHING_STEPS 10
	#endif
	float2 uvOffset = 0;
	float stepSize = 1.0 / PARALLAX_RAYMARCHING_STEPS;
	float2 uvDelta = viewDir * (stepSize * _ParallaxStrengthMin);

	float stepHeight = 1;
	float surfaceHeight = GetParallaxHeight(uv);

	float2 prevUVOffset = uvOffset;
	float prevStepHeight = stepHeight;
	float prevSurfaceHeight = surfaceHeight;

	for (
		int i = 1;
		i < PARALLAX_RAYMARCHING_STEPS && stepHeight > surfaceHeight;
		i++
	) {
		prevUVOffset = uvOffset;
		prevStepHeight = stepHeight;
		prevSurfaceHeight = surfaceHeight;
		
		uvOffset -= uvDelta;
		stepHeight -= stepSize;
		surfaceHeight = GetParallaxHeight(uv + uvOffset);
	}

	#if !defined(PARALLAX_RAYMARCHING_SEARCH_STEPS)
		#define PARALLAX_RAYMARCHING_SEARCH_STEPS 0
	#endif
	#if PARALLAX_RAYMARCHING_SEARCH_STEPS > 0
		for (int i = 0; i < PARALLAX_RAYMARCHING_SEARCH_STEPS; i++) {
			uvDelta *= 0.5;
			stepSize *= 0.5;

			if (stepHeight < surfaceHeight) {
				uvOffset += uvDelta;
				stepHeight += stepSize;
			}
			else {
				uvOffset -= uvDelta;
				stepHeight -= stepSize;
			}
			surfaceHeight = GetParallaxHeight(uv + uvOffset);
		}
	#elif defined(PARALLAX_RAYMARCHING_INTERPOLATE)
		float prevDifference = prevStepHeight - prevSurfaceHeight;
		float difference = surfaceHeight - stepHeight;
		float t = prevDifference / (prevDifference + difference);
		uvOffset = prevUVOffset - uvDelta * t;
	#endif

	return uvOffset;
}

void ApplyParallax (inout Interpolators i) {
	#if defined(_PARALLAX_MAP)
		i.tangentViewDir = normalize(i.tangentViewDir);
		#if !defined(PARALLAX_OFFSET_LIMITING)
			#if !defined(PARALLAX_BIAS)
				#define PARALLAX_BIAS 0.42
			#endif
			i.tangentViewDir.xy /= (i.tangentViewDir.z + PARALLAX_BIAS);
		#endif

		#if !defined(PARALLAX_FUNCTION)
			#define PARALLAX_FUNCTION ParallaxOffset
		#endif
		float2 uvOffset = PARALLAX_FUNCTION(i.uv.xy, i.tangentViewDir.xy);
		i.uv.xy += uvOffset;
		i.uv.zw += uvOffset * (_DetailTex_ST.xy / _AlbedoTexture_ST.xy);
	#endif
}

struct FragmentOutput {
	#if defined(DEFERRED_PASS)
		float4 gBuffer0 : SV_Target0;
		float4 gBuffer1 : SV_Target1;
		float4 gBuffer2 : SV_Target2;
		float4 gBuffer3 : SV_Target3;

		#if defined(SHADOWS_SHADOWMASK) && (UNITY_ALLOWED_MRT_COUNT > 4)
			float4 gBuffer4 : SV_Target4;
		#endif
	#else
		float4 color : SV_Target;
	#endif
};

float _Tiling;

sampler2D _NormalTexture;
sampler2D _SrmTexture;

Texture2D _MaterialSelectMapTexture;
sampler2D _MaterialWeightMapTexture;
Texture2D _MaterialIndicesTexture;

SamplerState PointClampSampler;

inline void NSelectMaterials(float2 physicalUv, out half4 outMaterialIndex, out half4 outMaterialWeight, out half outTopMaterialIndex)
{
	half4 materialLists = floor(_MaterialSelectMapTexture.Sample(PointClampSampler, physicalUv) * 255.4h);
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
	outTopMaterialIndex = (half)_MaterialIndicesTexture.Sample(PointClampSampler, materialSelectTableUv).x; //(half)tex2D(_MaterialIndicesTexture, materialSelectTableUv).x;

	outMaterialWeight.x += 0.001h;

	half totalWeight = outMaterialWeight.x + outMaterialWeight.y;
	outMaterialWeight.xy = outMaterialWeight.xy / totalWeight;
}

#define MATERIAL_TEXTURE_RESOLUTION	(960.0f)
#define AVAILABLE_MIP_LEVEL_FOR_SIMPLE_MIPMAP	(5.0)	// 1024x1024 => 32x32

inline void NFetchTerrainMaterial2(float2 inVirtualUv, half4 inMaterialIndex, half4 inAlpha, out half4 outAlbedoColor, out half3 outNormalColor, out half2 outSrmColor)
{
	float2 texcoord = inVirtualUv.xy * _Tiling;

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

FragmentOutput MyFragmentProgram (Interpolators i) {
	UNITY_SETUP_INSTANCE_ID(i);
	#if defined(LOD_FADE_CROSSFADE)
		UnityApplyDitherCrossFade(i.vpos);
	#endif

	ApplyParallax(i);

	float alpha = GetAlpha(i);
	#if defined(_RENDERING_CUTOUT)
		clip(alpha - _Cutoff);
	#endif

	InitializeFragmentNormal(i);

	float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos.xyz);

	float3 specularTint;
	float oneMinusReflectivity;

	//float3 baseColor = float3(0.5, 0.5, 0.5);

	// Select materials.
	float2 physicalUv = i.uv.xy;

	half4 outMaterialIndex;
	half4 outMaterialWeight;
	half outTopMaterialIndex;
	NSelectMaterials(physicalUv, outMaterialIndex, outMaterialWeight, outTopMaterialIndex);

	half4 outAlbedo;
	half3 outNormal;
	half2 outSrm;
	NFetchTerrainMaterial2(physicalUv, outMaterialIndex, outMaterialWeight, outAlbedo, outNormal, outSrm);

	float3 albedo = DiffuseAndSpecularFromMetallic(
		outAlbedo, GetMetallic(i), specularTint, oneMinusReflectivity
	);

	float4 color = UNITY_BRDF_PBS(
		albedo, specularTint,
		oneMinusReflectivity, GetSmoothness(i),
		i.normal, viewDir,
		CreateLight(i), CreateIndirectLight(i, viewDir)
	);
	color.rgb += GetEmission(i);
	#if defined(_RENDERING_FADE) || defined(_RENDERING_TRANSPARENT)
		color.a = alpha;
	#endif

	FragmentOutput output;
	#if defined(DEFERRED_PASS)
		#if !defined(UNITY_HDR_ON)
			color.rgb = exp2(-color.rgb);
		#endif
		output.gBuffer0.rgb = albedo;
		output.gBuffer0.a = GetOcclusion(i);
		output.gBuffer1.rgb = specularTint;
		output.gBuffer1.a = GetSmoothness(i);
		output.gBuffer2 = float4(i.normal * 0.5 + 0.5, 1);
		output.gBuffer3 = color;

		#if defined(SHADOWS_SHADOWMASK) && (UNITY_ALLOWED_MRT_COUNT > 4)
			float2 shadowUV = 0;
			#if defined(LIGHTMAP_ON)
				shadowUV = i.lightmapUV;
			#endif
			output.gBuffer4 =
				UnityGetRawBakedOcclusions(shadowUV, i.worldPos.xyz);
		#endif
	#else
		output.color = ApplyFog(color, i);

		// NORMAL HACK
		//output.color.rgb = i.normal;
	#endif
		
	return output;
}

#endif