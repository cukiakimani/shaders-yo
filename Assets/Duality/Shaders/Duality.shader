Shader "Custom/Duality"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0

		_RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
		_RimPower ("Rim Power", Range(0.5,8.0)) = 3.0

		_NoiseTex ("Albedo (RGB)", 2D) = "white" {}
		_Extrusion ("Extrusion Amount", Range(0,1)) = 0.1
		_DistortionOrigin ("DistrotionOrigin", Vector) = (0.0,0.0,0.0)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _NoiseTex;

		struct Input
		{
			float2 uv_NoiseTex;
			float3 viewDir;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float4 _RimColor;
		float3 _DistortionOrigin;
		float _RimPower;

		float _Extrusion;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
		// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		float map(float value, float initialMin, float initialMax, float destinationMin, float destinationMax)
		{
			float t = (value - initialMin) / (initialMax - initialMin);
			return lerp(destinationMin, destinationMax, t);
		}
		
		void vert(inout appdata_full v) 
		{
			float4 noise = tex2Dlod(_NoiseTex, v.texcoord);

			float3 direction = normalize(_DistortionOrigin - v.vertex.xyz);

			// v.vertex.xyz += direction * -map(noise.r, 0.0, 1.0, -5.0, 5.0) * _Extrusion;
			v.vertex.xyz += v.normal * map(noise.r, 0.0, 1.0, -5.0, 5.0) * _Extrusion;
		}

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			// Albedo comes from a texture tinted by color
			o.Albedo = _Color;
			// o.Albedo = tex2D (_NoiseTex, IN.uv_NoiseTex).rgb;

			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;

			half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
			o.Emission = _RimColor.rgb * pow (rim, _RimPower);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
