// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
Shader "Custom/HyperrealisticPlatformShader"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (0.5, 0.5, 0.5, 1)
        _Metallic("Metallic", Range(0, 1)) = 0.2 // Unused in this unlit shader, but kept for user consistency
        _Roughness("Roughness", Range(0, 1)) = 0.8 // Unused in this unlit shader, but kept for user consistency
        _DetailAlbedoMap("Detail Albedo Map", 2D) = "white" {}
        _DetailNormalMap("Detail Normal Map", 2D) = "bump" {}
        _DetailScale("Detail Scale", Float) = 5
        _NormalMap("Normal Map", 2D) = "bump" {}
        _EmissiveColor("Emissive Color", Color) = (0, 0.1, 0.2, 1)
        _EmissiveIntensity("Emissive Intensity", Float) = 0.1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="HDRenderPipeline" }
        Pass
        {
            Name "Unlit"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.6

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Normal/Normal.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float2 uv           : TEXCOORD0;
                float4 tangentOS    : TANGENT;
            };

            struct Varyings
            {
                float4 positionCS       : SV_POSITION;
                float3 positionWS       : TEXCOORD0;
                float2 uv               : TEXCOORD1;
                float2 detailUV         : TEXCOORD2;
                float3x3 TBN            : TEXCOORD3;
            };

            // Texture and Sampler declarations
            TEXTURE2D(_DetailAlbedoMap);
            TEXTURE2D(_NormalMap);
            TEXTURE2D(_DetailNormalMap);
            SAMPLER(sampler_linear_clamp);

            // CBUFFER for material properties
            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float _DetailScale;
                float4 _EmissiveColor;
                float _EmissiveIntensity;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings o;
                o.positionWS = TransformObjectToWorld(input.positionOS.xyz);
                o.positionCS = TransformWorldToHClip(o.positionWS);
                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);
                float3 tangentWS = TransformObjectToWorldDir(input.tangentOS.xyz);
                float3 bitangentWS = cross(normalWS, tangentWS) * input.tangentOS.w;
                o.TBN = float3x3(tangentWS, bitangentWS, normalWS);
                o.uv = input.uv;
                o.detailUV = input.uv * _DetailScale;
                return o;
            }

            float4 frag(Varyings i) : SV_Target
            {
                // Base color + detail albedo
                float4 baseCol = _BaseColor * SAMPLE_TEXTURE2D(_DetailAlbedoMap, sampler_linear_clamp, i.detailUV);

                // Normal mapping for Fresnel calculation
                float3 baseNormalTS = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap, sampler_linear_clamp, i.uv));
                float3 detailNormalTS = UnpackNormal(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_linear_clamp, i.detailUV));
                float3 finalNormalTS = BlendNormal(baseNormalTS, detailNormalTS);
                float3 n = normalize(mul(i.TBN, finalNormalTS));

                // Fresnel cinematic effect
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.positionWS);
                float fresnel = pow(1.0 - saturate(dot(n, viewDir)), 5.0);

                // Emissive glow
                float3 emissive = _EmissiveColor.rgb * _EmissiveIntensity;

                // The user's custom unlit lighting model
                float3 finalColor = baseCol.rgb + emissive + fresnel * 0.15;

                return float4(finalColor, baseCol.a);
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/HDRP/Unlit"
}