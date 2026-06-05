Shader "CustomRenderTexture/EmissionStencilGeometry"
{
    Properties
    {
        _BaseMap("Base Map", 2D) = "white" {}
        _BaseColor("Base Color", Color) = (1,1,1,1)

        _EmissionMap("Emission Map", 2D) = "white" {}
        _EmissionColor("Emission Color", Color) = (1,1,1,1)

        _StencilRef("Stencil Ref", Int) = 1
    }

        SubShader
        {
            Tags { "RenderType" = "Opaque" "Queue" = "Geometry+1" }

            Stencil
            {
                Ref[_StencilRef]
                Comp Equal
                Pass Keep
            }

            Pass
            {
                Tags { "LightMode" = "UniversalForward" }

                HLSLPROGRAM
                #pragma vertex Vert
                #pragma fragment Frag
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

                struct Attributes
                {
                    float4 positionOS : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct Varyings
                {
                    float4 positionHCS : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };

                TEXTURE2D(_BaseMap);
                SAMPLER(sampler_BaseMap);

                TEXTURE2D(_EmissionMap);
                SAMPLER(sampler_EmissionMap);

                float4 _BaseColor;
                float4 _EmissionColor;

                Varyings Vert(Attributes IN)
                {
                    Varyings OUT;
                    OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                    OUT.uv = IN.uv;
                    return OUT;
                }

                half4 Frag(Varyings IN) : SV_Target
                {
                    half4 baseCol = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _BaseColor;
                    half4 emissionTex = SAMPLE_TEXTURE2D(_EmissionMap, sampler_EmissionMap, IN.uv);
                    half3 emission = emissionTex.rgb * _EmissionColor.rgb * 10.0;
                    return half4(baseCol.rgb + emission, baseCol.a);
                }

                ENDHLSL
            }
        }
}
