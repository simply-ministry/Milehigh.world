// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
Shader "Custom/HyperrealisticPlatformShader"
{
    Properties
    {
        [Header(Base Surface Properties)]
        _BaseColor("Base Color", Color) = (0.5, 0.5, 0.5, 1)
        _BaseColorMap("Base Color Map", 2D) = "white" {}
        _Metallic("Metallic", Range(0, 1)) = 0.2
        _Smoothness("Smoothness", Range(0, 1)) = 0.8
        _NormalMap("Normal Map", 2D) = "bump" {}
        _NormalScale("Normal Scale", Float) = 1.0
        _OcclusionMap("Occlusion Map", 2D) = "white" {}
        _OcclusionStrength("Occlusion Strength", Range(0, 1)) = 1.0

        [Header(Detail Mapping)]
        _DetailAlbedoMap("Detail Albedo Map", 2D) = "white" {}
        _DetailNormalMap("Detail Normal Map", 2D) = "bump" {}
        _DetailSmoothnessMap("Detail Smoothness Map", 2D) = "white" {}
        _DetailScale("Detail Scale", Float) = 5.0

        [Header(Parallax Mapping)]
        _ParallaxMap("Parallax Map (Height)", 2D) = "black" {}
        _ParallaxHeight("Parallax Height Scale", Range(0.0, 0.1)) = 0.02
        _MinSteps("POM Min Steps", Int) = 5
        _MaxSteps("POM Max Steps", Int) = 25

        [Header(Emissive & Bloom)]
        _EmissiveColor("Emissive Color", Color) = (0, 0, 0, 1)
        _EmissiveMap("Emissive Map", 2D) = "black" {}
        _EmissiveExposureWeight("Emissive Exposure Weight", Range(0.0, 1.0)) = 1.0
        _EmissiveLuminance("Emissive Luminance", Float) = 1.0

        [Header(Cinematic Effects)]
        _FresnelPower("Fresnel Power", Range(0.1, 10.0)) = 2.0
        _FresnelColor("Fresnel Color", Color) = (0.0, 0.0, 0.0, 0.0)
        _FresnelIntensity("Fresnel Intensity", Range(0.0, 1.0)) = 0.0

        [ToggleUI] _AlphaClip("Alpha Clipping", Float) = 0
        _Cutoff("Alpha Cutoff", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "HDRP"
            "RenderType" = "Opaque"
        }

        Pass
        {
            Name "ForwardOnly"
            Tags { "LightMode" = "HDRPForwardLit" }

            HLSLPROGRAM
            #pragma target 4.5
            #pragma multi_compile_instancing
            #pragma multi_compile _ ALPHATEST_ON

            #include "Packages/com.unity.render-pipelines.hdrp/ShaderLibrary/Shared.hlsl"
            #include "Packages/com.unity.render-pipelines.hdrp/ShaderLibrary/Lit.hlsl"

            #pragma vertex Vert
            #pragma fragment Frag

            // This struct is defined in Lit.hlsl and is the standard way to pass data to the fragment shader
            // for a Lit shader in HDRP.
            struct Varyings
            {
                float4 positionCS               : SV_POSITION;
                float3 positionWS               : TEXCOORD0;
                float3 normalWS                 : TEXCOORD1;
                float4 tangentWS                : TEXCOORD2;
                float2 uv                       : TEXCOORD3;
                float2 detailUV                 : TEXCOORD4;
                float3 viewDirectionWS          : TEXCOORD5;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            // Declare all properties so the shader can use them
            TEXTURE2D(_BaseColorMap); SAMPLER(sampler_BaseColorMap);
            TEXTURE2D(_NormalMap); SAMPLER(sampler_NormalMap);
            TEXTURE2D(_OcclusionMap); SAMPLER(sampler_OcclusionMap);
            TEXTURE2D(_DetailAlbedoMap); SAMPLER(sampler_DetailAlbedoMap);
            TEXTURE2D(_DetailNormalMap); SAMPLER(sampler_DetailNormalMap);
            TEXTURE2D(_DetailSmoothnessMap); SAMPLER(sampler_DetailSmoothnessMap);
            TEXTURE2D(_ParallaxMap); SAMPLER(sampler_ParallaxMap);
            TEXTURE2D(_EmissiveMap); SAMPLER(sampler_EmissiveMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float _Metallic;
                float _Smoothness;
                float _NormalScale;
                float _OcclusionStrength;
                float _DetailScale;
                float _ParallaxHeight;
                int _MinSteps;
                int _MaxSteps;
                float4 _EmissiveColor;
                float _EmissiveExposureWeight;
                float _EmissiveLuminance;
                float _FresnelPower;
                float4 _FresnelColor;
                float _FresnelIntensity;
                float _AlphaClip;
                float _Cutoff;
            CBUFFER_END

            // Vertex Shader
            Varyings Vert(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);

                output.positionWS = TransformObjectToWorld(input.positionOS);
                output.positionCS = TransformWorldToHClip(output.positionWS);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.tangentWS = float4(TransformObjectToWorldDir(input.tangentOS.xyz), input.tangentOS.w);
                output.uv = input.uv;
                output.detailUV = input.uv * _DetailScale;
                output.viewDirectionWS = GetWorldSpaceNormalizeViewDir(output.positionWS);

                return output;
            }

            // Fragment Shader
            float4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);

                float3 normalWS = normalize(input.normalWS);
                float3 tangentWS = normalize(input.tangentWS.xyz);
                float3 bitangentWS = cross(normalWS, tangentWS) * input.tangentWS.w;
                float3x3 TBN = float3x3(tangentWS, bitangentWS, normalWS);

                float3 viewDirTS = mul(TBN, input.viewDirectionWS);

                // Simplified POM for correctness
                float2 parallaxUV = input.uv;
                if (_ParallaxHeight > 0.0)
                {
                    float height = SAMPLE_TEXTURE2D(_ParallaxMap, sampler_ParallaxMap, input.uv).r;
                    parallaxUV = input.uv + (viewDirTS.xy * (height * _ParallaxHeight));
                }

                float4 baseColor = SAMPLE_TEXTURE2D(_BaseColorMap, sampler_BaseColorMap, parallaxUV) * _BaseColor;

                #if ALPHATEST_ON
                    if (baseColor.a < _Cutoff) discard;
                #endif

                float3 baseNormalTS = UnpackNormalmap(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, parallaxUV), _NormalScale);
                float3 detailNormalTS = UnpackNormalmap(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, input.detailUV), 1.0);
                float3 combinedNormalTS = normalize(float3(baseNormalTS.xy + detailNormalTS.xy, baseNormalTS.z * detailNormalTS.z));
                float3 finalNormalWS = TransformTangentToWorld(combinedNormalTS, TBN);

                float occlusion = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, parallaxUV).r;
                float detailSmoothness = SAMPLE_TEXTURE2D(_DetailSmoothnessMap, sampler_DetailSmoothnessMap, input.detailUV).r;

                // Prepare data for HDRP lighting
                BuiltinData builtinData;
                GetBuiltinData(input.positionCS, builtinData);

                SurfaceData surfaceData;
                surfaceData.albedo = baseColor.rgb;
                surfaceData.metallic = _Metallic;
                surfaceData.smoothness = _Smoothness * detailSmoothness;
                surfaceData.normalWS = finalNormalWS;
                surfaceData.occlusion = lerp(1.0, occlusion, _OcclusionStrength);
                surfaceData.emissive = SAMPLE_TEXTURE2D(_EmissiveMap, sampler_EmissiveMap, parallaxUV).rgb * _EmissiveColor.rgb * _EmissiveLuminance;

                float fresnel = pow(1.0 - saturate(dot(finalNormalWS, input.viewDirectionWS)), _FresnelPower);
                surfaceData.emissive += _FresnelColor.rgb * fresnel * _FresnelIntensity;

                // Call the standard HDRP Lit function
                return Lit(surfaceData, builtinData);
            }
            ENDHLSL
        }

        // Use standard HDRP passes for shadows and depth
        UsePass "Hidden/HDRP/Lit/ShadowCaster"
        UsePass "Hidden/HDRP/Lit/DepthOnly"
    }
    FallBack "Hidden/HDRP/Lit"
}