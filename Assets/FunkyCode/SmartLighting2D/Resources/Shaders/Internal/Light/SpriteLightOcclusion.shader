﻿Shader "Light2D/Internal/Light/SpriteLightOcclusion"
{
    Properties
    {
        _MainTex ("Sprite", 2D) = "white" {}
        _Outer("Outer", Float) = 0
        _Inner("Inner", Float) = 0
    }

    Category
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
        }
        
        Blend One One
        Cull Off
        Lighting Off
        ZWrite Off

        SubShader
        {
            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
        
                #include "UnityCG.cginc"

                sampler2D _MainTex;
                float4 _MainTex_ST;

                float _Outer;
                float _Inner;

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    fixed4 color : COLOR;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f
                {
                    float4 vertex : SV_POSITION;
                    fixed4 color : COLOR;
                    float2 texcoord : TEXCOORD0;
                    float2 xy : TEXCOORD1;
                };

                v2f vert (appdata_t v)
                {
                    v2f o;

                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.color = v.color;
                    o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                    o.xy.x = v.texcoord.x - 0.5;
                    o.xy.y = v.texcoord.y - 0.5;

                    return o;
                }

                fixed4 frag (v2f i) : SV_Target
                {
                    fixed4 sprite = tex2D(_MainTex, i.texcoord);

                    float dir = ((atan2(i.xy.y, i.xy.x)) * 57.2958 + 450 + 360) % 360;

                    float pointValue = max(0, min(1, (_Inner * 0.5 - abs(dir - 180) + _Outer) / _Outer));

                    fixed4 output = fixed4(1, 1, 1, 1);
                    output.a = (1 - sprite.a);

                    output.rgb *= sprite.rgb * 2.0f;
                    output.rgb *= pointValue;
                    output.rgb *= i.color * i.color.a * 2;
                
                    return output;
                }
                ENDCG
            }
        }
    }
}