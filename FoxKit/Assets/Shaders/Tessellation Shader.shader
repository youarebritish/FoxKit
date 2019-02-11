Shader "Custom/Tessellation" {

	Properties{
		_Color("Tint", Color) = (0.5, 0.5, 0.5, 1)
		_Tiling("Tiling", Float) = 10

		[NoScaleOffset] _NormalMap ("Normals", 2D) = "bump" {}
		
		_AlbedoTexture("Albedo Atlas", 2D) = "white" {}
		_NormalTexture("Normal Atlas", 2D) = "bump" {}
		_SrmTexture("SRM Atlas", 2D) = "white" {}
		_MaterialSelectMapTexture("Material Select Map", 2D) = "white" {}
		_MaterialWeightMapTexture("Material Weight Map", 2D) = "white" {}
		_MaterialIndicesTexture("Material Indices", 2D) = "white" {}

		[NoScaleOffset] _ParallaxMap ("Parallax", 2D) = "black" {}
		_ParallaxStrengthMin ("Minimum Height", Range(0, 1000)) = 188.4435
		_ParallaxStrengthMax ("Maximum Height", Range(0, 1000)) = 755.0121
			
		_TessellationUniform ("Tessellation Uniform", Range(1, 64)) = 1
		_TessellationEdgeLength ("Tessellation Edge Length", Range(1, 100)) = 25

		[HideInInspector] _SrcBlend ("_SrcBlend", Float) = 1
		[HideInInspector] _DstBlend ("_DstBlend", Float) = 0
		[HideInInspector] _ZWrite ("_ZWrite", Float) = 1
	}

	CGINCLUDE

	#define BINORMAL_PER_FRAGMENT
	#define FOG_DISTANCE

	#define VERTEX_DISPLACEMENT_INSTEAD_OF_PARALLAX

	ENDCG

	SubShader {

		Pass {
			Tags {
				"LightMode" = "ForwardBase"
			}
			Blend [_SrcBlend] [_DstBlend]
			ZWrite [_ZWrite]

			CGPROGRAM

			#pragma target 4.6

			#pragma shader_feature _ _RENDERING_CUTOUT _RENDERING_FADE _RENDERING_TRANSPARENT
			#pragma shader_feature _METALLIC_MAP
			#pragma shader_feature _ _SMOOTHNESS_ALBEDO _SMOOTHNESS_METALLIC
			#pragma shader_feature _NORMAL_MAP
			#define _PARALLAX_MAP//#pragma shader_feature _PARALLAX_MAP
			#pragma shader_feature _OCCLUSION_MAP
			#pragma shader_feature _EMISSION_MAP
			#pragma shader_feature _DETAIL_MASK
			#pragma shader_feature _DETAIL_ALBEDO_MAP
			#pragma shader_feature _DETAIL_NORMAL_MAP
			//#pragma shader_feature _TESSELLATION_EDGE
			#define _TESSELLATION_EDGE

			#pragma multi_compile _ LOD_FADE_CROSSFADE

			#pragma multi_compile_fwdbase
			#pragma multi_compile_fog

			#pragma vertex MyTessellationVertexProgram
			#pragma fragment MyFragmentProgram
			#pragma hull MyHullProgram
			#pragma domain MyDomainProgram

			#define FORWARD_BASE_PASS

			#include "My Lighting.cginc"
			#include "MyTessellation.cginc"

			ENDCG
		}

		Pass {
			Tags {
				"LightMode" = "ForwardAdd"
			}

			Blend [_SrcBlend] One
			ZWrite Off

			CGPROGRAM

			#pragma target 4.6

			#pragma shader_feature _ _RENDERING_CUTOUT _RENDERING_FADE _RENDERING_TRANSPARENT
			#pragma shader_feature _METALLIC_MAP
			#pragma shader_feature _ _SMOOTHNESS_ALBEDO _SMOOTHNESS_METALLIC
			#pragma shader_feature _NORMAL_MAP
			#pragma shader_feature _PARALLAX_MAP
			#pragma shader_feature _DETAIL_MASK
			#pragma shader_feature _DETAIL_ALBEDO_MAP
			#pragma shader_feature _DETAIL_NORMAL_MAP
			#pragma shader_feature _TESSELLATION_EDGE

			#pragma multi_compile _ LOD_FADE_CROSSFADE

			#pragma multi_compile_fwdadd_fullshadows
			#pragma multi_compile_fog
			
			#pragma vertex MyTessellationVertexProgram
			#pragma fragment MyFragmentProgram
			#pragma hull MyHullProgram
			#pragma domain MyDomainProgram

			#include "My Lighting.cginc"
			#include "MyTessellation.cginc"

			ENDCG
		}

		Pass {
			Tags {
				"LightMode" = "Deferred"
			}

			CGPROGRAM

			#pragma target 4.6
			#pragma exclude_renderers nomrt

			#pragma shader_feature _ _RENDERING_CUTOUT
			#pragma shader_feature _METALLIC_MAP
			#pragma shader_feature _ _SMOOTHNESS_ALBEDO _SMOOTHNESS_METALLIC
			#pragma shader_feature _NORMAL_MAP
			#pragma shader_feature _PARALLAX_MAP
			#pragma shader_feature _OCCLUSION_MAP
			#pragma shader_feature _EMISSION_MAP
			#pragma shader_feature _DETAIL_MASK
			#pragma shader_feature _DETAIL_ALBEDO_MAP
			#pragma shader_feature _DETAIL_NORMAL_MAP
			#pragma shader_feature _TESSELLATION_EDGE

			#pragma multi_compile _ LOD_FADE_CROSSFADE

			#pragma multi_compile_prepassfinal

			#pragma vertex MyTessellationVertexProgram
			#pragma fragment MyFragmentProgram
			#pragma hull MyHullProgram
			#pragma domain MyDomainProgram

			#define DEFERRED_PASS

			#include "My Lighting.cginc"
			#include "MyTessellation.cginc"

			ENDCG
		}

		Pass {
			Tags {
				"LightMode" = "ShadowCaster"
			}

			CGPROGRAM

			#pragma target 4.6

			#pragma shader_feature _ _RENDERING_CUTOUT _RENDERING_FADE _RENDERING_TRANSPARENT
			#pragma shader_feature _SEMITRANSPARENT_SHADOWS
			#pragma shader_feature _SMOOTHNESS_ALBEDO
			#pragma shader_feature _PARALLAX_MAP
			#pragma shader_feature _TESSELLATION_EDGE

			#pragma multi_compile _ LOD_FADE_CROSSFADE

			#pragma multi_compile_shadowcaster

			#pragma vertex MyTessellationVertexProgram
			#pragma fragment MyShadowFragmentProgram
			#pragma hull MyHullProgram
			#pragma domain MyDomainProgram

			#include "My Shadows.cginc"
			#include "MyTessellation.cginc"

			ENDCG
		}

		Pass {
			Tags {
				"LightMode" = "Meta"
			}

			Cull Off

			CGPROGRAM

			#pragma vertex MyLightmappingVertexProgram
			#pragma fragment MyLightmappingFragmentProgram

			#pragma shader_feature _METALLIC_MAP
			#pragma shader_feature _ _SMOOTHNESS_ALBEDO _SMOOTHNESS_METALLIC
			#pragma shader_feature _EMISSION_MAP
			#pragma shader_feature _DETAIL_MASK
			#pragma shader_feature _DETAIL_ALBEDO_MAP

			#include "My Lightmapping.cginc"

			ENDCG
		}
	}

	//CustomEditor "MyLightingShaderGUI"
}