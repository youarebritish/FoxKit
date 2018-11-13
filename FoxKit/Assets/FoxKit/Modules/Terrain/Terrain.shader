Shader "Custom/Terrain" {
	Properties {
		_Color ("Color", Color) = (0.5, 0.5, 0.5, 1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_HeightTex ("Height (RGB)", 2D) = "black" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_MaxHeight("Max Height", Range(0, 9999)) = 50.0
		_MinHeight("Min Height", Range(0, 9999)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard vertex:vert fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _HeightTex;
		
		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		half _MaxHeight;
		half _MinHeight;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		SamplerState linear_clamp_sampler;

		void vert(inout appdata_full v) {
			float4 height = tex2Dlod(_HeightTex, float4(v.texcoord.xy, 0, 0));
			float heightOffset = ((_MaxHeight - _MinHeight) * height) + _MinHeight;

			v.vertex.y += heightOffset;
			v.texcoord = mul(UNITY_MATRIX_TEXTURE0, v.texcoord);
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = _Color; //tex2D (_MainTex, IN.uv_MainTex) * _Color;

			//float4 height = tex2D(_HeightTex, IN.uv_MainTex);
			//c = float4(height.r, height.r, height.r, 1.0);
			//c = float4(IN.uv_MainTex.x, IN.uv_MainTex.y, 0.0, 0.0);

			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
