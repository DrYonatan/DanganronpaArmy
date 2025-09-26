Shader "UI/OutlineGlow"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0.2, 0.6, 1, 1)
        _OutlineThickness ("Outline Thickness", Range(1,10)) = 3.0
        _GlowStrength ("Glow Strength", Range(0, 5)) = 1.5
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
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            #define MAX_THICKNESS 10
            #define NUM_SAMPLES 8

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color    : COLOR;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color    : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float4 _OutlineColor;
            float _OutlineThickness;
            float _GlowStrength;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float centerAlpha = tex2D(_MainTex, i.texcoord).a;

                // Inside sprite = black fill
                if (centerAlpha > 0.5)
                    return fixed4(0, 0, 0, 1) * i.color;

                float glow = 0.0;
                int thickness = (int)min(_OutlineThickness, MAX_THICKNESS);

                // 8 directions around the pixel
                [unroll]
                for (int s = 0; s < NUM_SAMPLES; s++)
                {
                    float angle = 6.283185 * (s / (float)NUM_SAMPLES);
                    float2 dir = float2(cos(angle), sin(angle));

                    // fixed max loop, compiler happy
                    [unroll]
                    for (int r = 1; r <= MAX_THICKNESS; r++)
                    {
                        if (r > thickness) break;

                        float2 offset = dir * r * _MainTex_TexelSize.xy;
                        float a = tex2D(_MainTex, i.texcoord + offset).a;

                        // falloff: further away contributes less
                        glow += a * (1.0 - (r / (float)thickness));
                    }
                }

                glow /= (NUM_SAMPLES * thickness);

                if (glow > 0.0)
                {
                    fixed4 col = _OutlineColor;
                    col.a *= glow * _GlowStrength;
                    col *= i.color;
                    return col;
                }

                return fixed4(0, 0, 0, 0);
            }
            ENDCG
        }
    }
}
