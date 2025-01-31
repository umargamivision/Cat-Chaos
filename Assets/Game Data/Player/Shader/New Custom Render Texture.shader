Shader "Shader Graphs/Fx_dissolve_particle_apb" {
    Properties {
        [NoScaleOffset] Main_tex ("Texture", 2D) = "white" {}
        Vector1_930B327D ("Highlight_Min", Float) = -10
        Vector1_270105AC ("Highlight_Max", Float) = -1
        [ToggleUI] Boolean_9C0948F4 ("Use SoftParticleFactor?", Float) = 1
        Vector1_2C5A3101 ("Emission_Power", Float) = 1
    }
    SubShader {
        Tags { "RenderPipeline" = "UniversalPipeline" "Queue" = "Transparent" }
        Pass {
            Name "UniversalForward"
            Tags { "LightMode" = "UniversalForward" }
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D Main_tex;
            CBUFFER_START(UnityPerMaterial)
            float4 _Main_tex_ST;
            CBUFFER_END

            Varyings vert(Attributes IN) {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _Main_tex);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target {
                return tex2D(Main_tex, IN.uv);
            }
            ENDHLSL
        }
    }
}