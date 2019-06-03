Shader "Simulation/Spring"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Frequency ("Frequency", Float) = 2.0
		_PercentageDecay ("Percentage Decay", Float) = 0.76
		_TimeDecay ("Time Decay", Float) = 0.452
		_DeltaTime ("Delta Time", Float) = 0.02

		_DistortPositionX ("Distort Position X", Range(0.0, 1.0)) = 0.5
		_DistortPositionY ("Distort Position Y", Range(0.0, 1.0)) = 0.5
		_Radius ("Distortion Radius", Range(0.0, 0.5)) = 0.3

	}
	SubShader
	{
		
		// Required to work
		ZTest Always Cull Off ZWrite Off
		Fog{ Mode off }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			float _Frequency;
			float _PercentageDecay;
			float _TimeDecay;
			float _DeltaTime;
			half4 _DistortPosition;
			half _Radius;
			half _DistortPositionX;
			half _DistortPositionY;

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
			};

			v2f vert(appdata_base v) 
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}
			

			float map(float value, float initialMin, float initialMax, float destinationMin, float destinationMax)
			{
				float t = (value - initialMin) / (initialMax - initialMin);
				return lerp(destinationMin, destinationMax, t);
			}

			float2 SpringLerp(float x, float v, float xt, float zeta, float omega, float dt)
			{
				// x - position
				// y - velocity

				float f = 1.0f + 2.0f * dt * zeta * omega;
				float oo = omega * omega;
				float hoo = dt * oo;
				float hhoo = dt * hoo;
				float detInv = 1.0f / (f + hhoo);
				float detX = f * x + dt * v + hhoo * xt;
				float detV = v + hoo * (xt - x);
				v = map(detV * detInv, -50, 50, 0, 1);
				x = map(detX * detInv, -5, 5, 0, 1); 
				return float2(x, v);
			}

			float4 frag (v2f i) : SV_Target
			{
				// r - stores the current position
				// g - stores the velocity
				// b - stores the target position

				half4 c = tex2D(_MainTex, i.uv);

				float2 p = float2(_DistortPositionX, _DistortPositionY);
				float dist = distance(i.uv, p);

				if (dist < _Radius)
				{
					float t = dist / _Radius;
					t = sqrt(1 - t * t);
					float offset = lerp(.5, 1, t);
					return float4(offset, 0.0, c.b, 0.0);
				}
				else
				{
					

					float omega = 2 * 3.14 * _Frequency;
					float zeta = log(_PercentageDecay) / (-omega * _TimeDecay);

					float x = map(c.r, 0, 1, -5, 5);
					float v = map(c.g, 0, 1, -50, 50);
					float2 spr = SpringLerp(x, v, c.b, zeta, omega, _DeltaTime);

					return float4(spr.xy, 0, 1);
				}
			}
			
			ENDCG
		}
	}
}
