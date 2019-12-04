Shader "Custom/Sprite Outline"
{
	Properties
	{
		_MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_OutlineThickness("Outline Thickness", Range (0.0, 0.1)) = 0.0
		_OutlineColor ("Outline Color", Color) = (1,1,1,1)
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;
			sampler2D _MainTex;
			
			fixed4 _OutlineColor;
			fixed _OutlineThickness;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				return OUT;
			}

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);
				return color;
			}

			fixed4 frag(v2f IN) : COLOR
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
				c.rgb *= c.a;
				
				fixed4 outlineC = _OutlineColor;
                outlineC.rgb *= outlineC.a;
                //outlineC.a *= ceil(c.a);

                if (c.a == 0.0)
				{
                    fixed upAlpha = SampleSpriteTexture ( IN.texcoord + fixed2(0, _OutlineThickness)).a;
                    fixed downAlpha = SampleSpriteTexture ( IN.texcoord - fixed2(0, _OutlineThickness)).a;
                    fixed rightAlpha = SampleSpriteTexture ( IN.texcoord + fixed2(_OutlineThickness, 0)).a;
                    fixed leftAlpha = SampleSpriteTexture ( IN.texcoord - fixed2(_OutlineThickness, 0)).a;
                    
                    fixed upRightAlpha = SampleSpriteTexture ( IN.texcoord - fixed2(_OutlineThickness, _OutlineThickness)).a;
                    fixed upLeftAlpha = SampleSpriteTexture ( IN.texcoord - fixed2(_OutlineThickness, -_OutlineThickness)).a;
                    fixed downRightAlpha = SampleSpriteTexture ( IN.texcoord - fixed2(-_OutlineThickness, _OutlineThickness)).a;
                    fixed downLeftAlpha = SampleSpriteTexture ( IN.texcoord - fixed2(-_OutlineThickness, -_OutlineThickness)).a;
				
                
				    if (upAlpha != 0.0|| downAlpha != 0.0 || rightAlpha != 0.0 || leftAlpha != 0.0 || upRightAlpha != 0.0 || upLeftAlpha != 0.0 || downLeftAlpha != 0.0 || downRightAlpha != 0.0)
				        return outlineC;    
				}
				
				return c;
			}
		ENDCG
		}
	}
}