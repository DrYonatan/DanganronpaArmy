Shader "Unlit/BWWaveRipple"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Radius ("Radius", Float) = 0
        _Width ("Width", Float) = 0.15
        _DistortStrength ("Distortion Strength", Float) = 0.015
        _WaveFrequency ("Wave Frequency", Float) = 25
    }

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Radius;
            float _Width;
            float _DistortStrength;
            float _WaveFrequency;

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
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                float2 center = float2(0.5, 0.5);
                float dist = distance(uv, center);

                // -------------------------
                // RIPPLED DISTORTION
                // -------------------------
                float ripple = sin((dist - _Radius) * _WaveFrequency) * _DistortStrength;
                uv += normalize(uv - center) * ripple;

                // sample distorted color
                float3 col = tex2D(_MainTex, uv).rgb;

                // -------------------------
                // GRAYSCALE TRANSITION MASK
                // -------------------------
                float mask = smoothstep(_Radius, _Radius - _Width, dist);
                float gray = dot(col, float3(0.299, 0.587, 0.114));
                float3 bw = float3(gray, gray, gray);
                float3 mixed = lerp(col, bw, mask);

                return float4(mixed, 1);
            }
            ENDCG
        }
    }
}
