Shader "UI/OutlineOnly"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0.2, 0.6, 1, 1) // light blue
        _OutlineThickness ("Outline Thickness", Float) = 1.0
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

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color    : COLOR;   // UI vertex color (CanvasGroup, Image tint, etc.)
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

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.color = v.color; // pass canvas alpha down
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float alpha = tex2D(_MainTex, i.texcoord).a;

                fixed4 col;

                if (alpha > 0.5)
                {
                    // Inside → black
                    col = fixed4(0, 0, 0, 1);
                }
                else
                {
                    // Outline check
                    float2 offset = _MainTex_TexelSize.xy * _OutlineThickness;
                    float neighborAlpha = 0.0;

                    neighborAlpha += tex2D(_MainTex, i.texcoord + float2(offset.x, 0)).a;
                    neighborAlpha += tex2D(_MainTex, i.texcoord - float2(offset.x, 0)).a;
                    neighborAlpha += tex2D(_MainTex, i.texcoord + float2(0, offset.y)).a;
                    neighborAlpha += tex2D(_MainTex, i.texcoord - float2(0, offset.y)).a;

                    if (neighborAlpha > 0.0)
                    {
                        col = _OutlineColor;
                    }
                    else
                    {
                        col = fixed4(0, 0, 0, 0);
                    }
                }

                // ✅ Respect UI tint & CanvasGroup alpha
                col *= i.color;

                return col;
            }
            ENDCG
        }
    }
}
