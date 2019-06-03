Shader "Custom/VertexDisplacement II"
{
	Properties
	{
		_MyColor ("Color", Color) = (0, 0, 0, 1)
		_Extrusion ("Extrusion", Range(0.0, 1.0)) = 1
		_NoiseTex ("Noise Texture", 2D) = "white" {}
	}

	SubShader
	{

		Tags
		{
			"Queue" = "Geometry"
			"RenderType" = "Opaque"
			"LightMode"="ForwardBase"
		}

		Pass 
		{
			CGPROGRAM

			#pragma vertex vert             
			#pragma fragment frag
			#include "UnityCG.cginc" // for UnityObjectToWorldNormal
			#include "UnityLightingCommon.cginc" // for _LightColor0
			#include "UnityShaderVariables.cginc"

			half4 _MyColor;
			float _Extrusion;
			sampler2D _NoiseTex;

			struct vertInput 
			{
				float4 pos : POSITION; // : after the colon are the setting up by Unity
				float2 texcoord : TEXCOORD0;
				float4 normal : NORMAL;
			};  

			struct v2f 
			{
				float4 vertex : SV_POSITION;
				half2 uv  : TEXCOORD0;
				fixed4 diff : COLOR0;
			};
			
			float map(float value, float initialMin, float initialMax, float destinationMin, float destinationMax)
			{
				float t = (value - initialMin) / (initialMax - initialMin);
				return lerp(destinationMin, destinationMax, t);
			}

			v2f vert(appdata_base v) 
			{
				v2f o;

				float4 noise = tex2Dlod(_NoiseTex, v.texcoord);
				v.vertex.xyz += v.normal * map(noise.r, 0.0, 1.0, -5.0, 5.0) * _Extrusion;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;

				// get vertex normal in world space
				half3 worldNormal = UnityObjectToWorldNormal(v.normal);

				// dot product between normal and light direction for
				// standard diffuse (Lambert) lighting
				half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));

				// factor in the light color
				o.diff = nl * _LightColor0;
				return o;
			}

			half4 frag(v2f output) : COLOR 
			{
				// debuging spring shader
				half4 c = tex2D(_NoiseTex, output.uv);
				return float4(c.r, c.g, c.b, 1.0);

				// return _MyColor * output.diff;
			}
			ENDCG
		}

	}
}
