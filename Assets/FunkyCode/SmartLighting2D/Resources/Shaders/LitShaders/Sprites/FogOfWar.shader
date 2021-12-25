﻿Shader "Light2D/Sprites/FogOfWar"
{
	Properties
	{
		[HideInInspector] _MainTex ("Sprite Texture", 2D) = "white" {}
 
		_Color ("Color", Color) = (1,1,1,1)
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

		Pass {

		CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "../../LitShaders/LitCore.cginc"
			
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
                float2 worldPos : TEXCOORD1;
			};
			
			sampler2D _MainTex;
			fixed4 _Color;

			v2f vert(appdata_t IN)
			{
				v2f OUT;

				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
                
                OUT.worldPos = mul (unity_ObjectToWorld, IN.vertex);

				return OUT;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 spritePixel = tex2D (_MainTex, IN.texcoord) * IN.color;  

				spritePixel.a *= SL2D_FogOfWar(IN.worldPos);

				spritePixel.rgb *= spritePixel.a;

				return spritePixel;
			}

		    ENDCG
		}
	}
}