Shader "Hidden/CRTScreen/FullScreen"
{
    Properties
    {
        _InputTexture("Main Texture (camera)", 2D) = "white" {}
        _ScanlineIntensity("Scanline Intensity", Range(0,2)) = 0.7
        _ScanlineCount("Scanline Count", Float) = 900
        _Curvature("Curvature", Range(0,1)) = 0.20
        _ChromaticAberration("Chromatic Aberration", Range(0,0.02)) = 0.004
        _NoiseAmount("Noise Amount", Range(0,0.3)) = 0.02
        _ColorBoost("Color Boost", Range(0,4)) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Overlay" }

        Pass
        {
            Name "CRT_Fullscreen"
            ZTest Always Cull Off ZWrite Off

            HLSLPROGRAM
            #pragma target 4.5
            #pragma vertex Vert
            #pragma fragment Frag

           #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            

            // Fullscreen triangle using SV_VertexID
            struct VInput
            {
                uint vertexID : SV_VertexID;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            // Texture macros (Common.hlsl defines these helpers)
            TEXTURE2D(_InputTexture);
            SAMPLER(sampler_InputTexture);

            float _ScanlineIntensity;
            float _ScanlineCount;
            float _Curvature;
            float _ChromaticAberration;
            float _NoiseAmount;
            float _ColorBoost;

            // Vertex: generate fullscreen triangle
            Varyings Vert(VInput v)
            {
                Varyings o;
                // compute uv from vertexID (0,1,2)
                o.uv = float2((v.vertexID << 1) & 2, v.vertexID & 2);
                // convert uv to clip space pos (fullscreen triangle)
                o.positionCS = float4(o.uv * 2.0 - 1.0, 0.0, 1.0);
                // Unity's screen UV convention: flip Y
                o.uv.y = 1.0 - o.uv.y;
                return o;
            }

            // Helpers
            float rand(float2 co) { return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453); }

            float2 barrelDistort(float2 uv, float k)
            {
                float2 p = uv * 2.0 - 1.0;
                float r2 = dot(p, p);
                float factor = 1.0 + k * r2;
                p *= factor;
                return (p + 1.0) * 0.5;
            }

            float3 sampleChromatic(float2 uv)
{
    // per-channel tiny offset sample
    float2 offR = float2(_ChromaticAberration, 0);
    float2 offB = float2(-_ChromaticAberration, 0);

    // CAMBIA _MainTex por _InputTexture y sampler_MainTex por sampler_InputTexture
    float r = SAMPLE_TEXTURE2D(_InputTexture, sampler_InputTexture, uv + offR).r;
    float g = SAMPLE_TEXTURE2D(_InputTexture, sampler_InputTexture, uv).g;
    float b = SAMPLE_TEXTURE2D(_InputTexture, sampler_InputTexture, uv + offB).b;
    return float3(r, g, b);
}

            float4 Frag(Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;

                // curvature
                if (_Curvature > 0.0001)
                    uv = barrelDistort(uv, _Curvature * 1.6);

                // if outside, return black to avoid sampling invalid areas
                if (uv.x < 0.0 || uv.x > 1.0 || uv.y < 0.0 || uv.y > 1.0)
                    return float4(0,0,0,1);

                // chromatic sample
                float3 col = sampleChromatic(uv);

                // scanlines (sin across Y). scale with screen-space density via _ScanlineCount
                float scan = sin(uv.y * _ScanlineCount * 3.14159265);
                float scanMod = 1.0 - _ScanlineIntensity * (0.5 + 0.5 * scan);
                col *= scanMod;

                // noise
                //float n = (rand(uv * _Time.y) - 0.5) * _NoiseAmount;
                //col += n;

                // vignette
                float2 center = uv - 0.5;
                float dist = length(center);
                // smooth vignette factor
                float vign = 1.0 - smoothstep(0.4, 0.9, dist);
                col *= lerp(1.0, vign, 1.0);

                // color boost (HDR-friendly scaling)
                col *= _ColorBoost;

                return float4(col, 1.0);
            }

            ENDHLSL
        }
    }

    Fallback Off
}
