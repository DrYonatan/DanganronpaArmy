Shader "Unlit/DebateContrast"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Contrast ("Contrast", Range(0, 3)) = 1.2
        _Brightness ("Brightness", Range(-1, 1)) = 0.0
        _Tint ("Tint", Color) = (1, 0.8, 0.6, 1) // warm reddish-yellow
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            ZTest Always Cull Off ZWrite Off
            Fog { Mode Off }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Contrast;
            float _Brightness;
            fixed4 _Tint;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // Apply contrast (centered around 0.5)
                col.rgb = ((col.rgb - 0.5) * _Contrast + 0.5) + _Brightness;

                // Apply tint (warm filter)
                col.rgb *= _Tint.rgb;

                return col;
            }
            ENDCG
        }
    }
}
