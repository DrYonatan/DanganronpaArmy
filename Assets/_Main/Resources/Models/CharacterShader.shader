Shader "Custom/Unlit Cutout With Shadows"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags
        {
            "Queue" = "AlphaTest"
            "RenderType" = "TransparentCutout"
        }

        // =========================================================
        // Visible pass: completely unaffected by lighting
        // =========================================================
        Pass
        {
            Tags { "LightMode" = "ForwardBase" }

            Cull Off
            ZWrite On

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _Cutoff;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;

                // Discard transparent pixels
                clip(col.a - _Cutoff);

                return col;
            }

            ENDCG
        }

        // =========================================================
        // Shadow pass: makes the cutout cast shadows
        // =========================================================
        Pass
        {
            Tags { "LightMode" = "ShadowCaster" }

            Cull Off
            ZWrite On
            ZTest LEqual

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Cutoff;

            struct appdata
            {
                float4 vertex : POSITION;

                // Required by TRANSFER_SHADOW_CASTER_NORMALOFFSET
                float3 normal : NORMAL;

                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                V2F_SHADOW_CASTER;

                float2 uv : TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;

                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                fixed alpha = tex2D(_MainTex, i.uv).a;

                // Transparent pixels do not cast shadows
                clip(alpha - _Cutoff);

                SHADOW_CASTER_FRAGMENT(i);
            }

            ENDCG
        }
    }
}