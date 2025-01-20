Shader "Custom/URP_Dissolve_Particle" {
    Properties {
        _MainTex ("Main Texture", 2D) = "white" {}
        _DissolveThreshold ("Dissolve Threshold", Range(0, 1)) = 0.5
        _EdgeColor ("Edge Color", Color) = (1, 0, 0, 1)
    }
    SubShader {
        Tags { "RenderPipeline" = "UniversalRenderPipeline" }
        Pass {
            Name "MainPass"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // Declare texture and sampler
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float _DissolveThreshold;
            float4 _EdgeColor;

            struct Attributes {
                float4 positionOS : POSITION; // Object space position
                float2 uv : TEXCOORD0;       // UV coordinates
            };

            struct Varyings {
                float4 positionHCS : SV_POSITION; // Homogeneous clip space position
                float2 uv : TEXCOORD0;           // UV coordinates
            };

            Varyings vert(Attributes v) {
                Varyings o;

                // Transform object space to world space, then to clip space
                float4 positionWS = mul(unity_ObjectToWorld, v.positionOS); // Object to World
                o.positionHCS = mul(UnityGetViewProjectionMatrix(), positionWS); // World to Clip

                o.uv = v.uv;
                return o;
            }

            half4 frag(Varyings i) : SV_Target {
                // Sample the texture
                float4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                // Simulate dissolve by comparing the dissolve threshold
                float dissolveFactor = texColor.r; // Use the red channel for dissolve
                if (dissolveFactor < _DissolveThreshold) {
                    // Return edge color if below threshold
                    return _EdgeColor;
                }

                // Return original texture color if above threshold
                return texColor;
            }
            ENDHLSL
        }
    }
    Fallback "Hidden/InternalErrorShader"
}