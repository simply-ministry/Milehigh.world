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

        [Header(Emissive & Bloom)]
        _EmissiveColor("Emissive Color", Color) = (0, 0, 0, 1)
        _EmissiveMap("Emissive Map", 2D) = "black" {}
        _EmissiveLuminance("Emissive Luminance", Float) = 1.0

        [Header(Cinematic Effects)]
        _FresnelPower("Fresnel Power", Range(0.1, 10.0)) = 2.0
        _FresnelColor("Fresnel Color", Color) = (0.0, 0.0, 0.0, 0.0)
        _FresnelIntensity("Fresnel Intensity", Range(0.0, 1.0)) = 0.0

        [Header(Advanced Options)]
        [ToggleUI] _AlphaClip("Alpha Clipping", Float) = 0
        _Cutoff("Alpha Cutoff", Range(0, 1)) = 0.5
        [Enum(UnityEngine.Rendering.CullMode)] _CullMode("Cull Mode", Float) = 2 // Back
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

            Cull[_CullMode]

            HLSLPROGRAM
            #pragma target 4.5
            #pragma multi_compile_instancing
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VEC
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _SCREEN_SPACE_OCCLUSION
            #pragma multi_compile _ _FOG
            #pragma multi_compile _ _ALPHATEST_ON

            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.hdrp/ShaderLibrary/Shared.hlsl"
            #include "Packages/com.unity.render-pipelines.hdrp/ShaderLibrary/Lit.hlsl"

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
                float4 _EmissiveColor;
                float _EmissiveLuminance;
                float _FresnelPower;
                float4 _FresnelColor;
                float _FresnelIntensity;
                float _Cutoff;
            CBUFFER_END

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float3 positionWS   : TEXCOORD0;
                float3 normalWS     : TEXCOORD1;
                float4 tangentWS    : TEXCOORD2;
                float2 uv           : TEXCOORD3;
                float2 detailUV     : TEXCOORD4;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            Varyings Vert(AttributesMesh input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);

                output.positionWS = TransformObjectToWorld(input.positionOS.xyz);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.tangentWS = float4(TransformObjectToWorldDir(input.tangentOS.xyz), input.tangentOS.w);
                output.positionCS = TransformWorldToHClip(output.positionWS);
                output.uv = input.uv0;
                output.detailUV = input.uv0 * _DetailScale;
                return output;
            }

            void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 viewWS, inout SurfaceData surfaceData, inout BuiltinData builtinData)
            {
                float3 normalWS = normalize(fragInputs.normalWS);
                float4 tangentWS = fragInputs.tangentWS;
                float3x3 TBN = CreateTangentToWorld(normalWS, tangentWS.xyz, tangentWS.w > 0.0f);

                float2 parallaxUV = fragInputs.uv;
                if (_ParallaxHeight > 0.001)
                {
                    float3 viewDirTS = mul(TBN, viewWS);
                    float heightSample = SAMPLE_TEXTURE2D(_ParallaxMap, sampler_ParallaxMap, fragInputs.uv).r;
                    parallaxUV -= viewDirTS.xy * (heightSample * _ParallaxHeight);
                    parallaxUV = saturate(parallaxUV);
                }

                float4 baseColor = SAMPLE_TEXTURE2D(_BaseColorMap, sampler_BaseColorMap, parallaxUV) * _BaseColor;

                #ifdef _ALPHATEST_ON
                    if (baseColor.a < _Cutoff) discard;
                #endif

                float4 normalMapTex = SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, parallaxUV);
                float occlusion = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, parallaxUV).r;
                float4 detailAlbedo = SAMPLE_TEXTURE2D(_DetailAlbedoMap, sampler_DetailAlbedoMap, fragInputs.detailUV);
                float4 detailNormalMapTex = SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, fragInputs.detailUV);
                float detailSmoothness = SAMPLE_TEXTURE2D(_DetailSmoothnessMap, sampler_DetailSmoothnessMap, fragInputs.detailUV).r;

                surfaceData.albedo = baseColor.rgb * detailAlbedo.rgb;

                float3 baseNormalTS = UnpackNormalmap(normalMapTex, _NormalScale);
                float3 detailNormalTS = UnpackNormalmap(detailNormalMapTex, 1.0);
                float3 combinedNormalTS = normalize(float3(baseNormalTS.xy + detailNormalTS.xy, baseNormalTS.z * detailNormalTS.z));
                surfaceData.normalWS = TransformTangentToWorld(combinedNormalTS, TBN);

                surfaceData.metallic = _Metallic;
                surfaceData.smoothness = _Smoothness * detailSmoothness;
                surfaceData.ao = lerp(1.0, occlusion, _OcclusionStrength);

                float3 emissiveColor = _EmissiveColor.rgb * SAMPLE_TEXTURE2D(_EmissiveMap, sampler_EmissiveMap, fragInputs.uv).rgb * _EmissiveLuminance;
                float fresnel = pow(1.0 - saturate(dot(surfaceData.normalWS, viewWS)), _FresnelPower);
                surfaceData.emissive = emissiveColor + (_FresnelColor.rgb * fresnel * _FresnelIntensity);

                builtinData.opacity = baseColor.a;
            }

            #include "Packages/com.unity.render-pipelines.hdrp/ShaderLibrary/ShaderPass.hlsl"

            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }
            Cull[_CullMode]
            HLSLPROGRAM
            #pragma target 4.5
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_instancing
            #include "Packages/com.unity.render-pipelines.hdrp/ShaderLibrary/Shared.hlsl"
            #include "Packages/com.unity.render-pipelines.hdrp/ShaderLibrary/Shadow/ShadowCaster.hlsl"
            ENDHLSL
        }

        Pass
        {
            Name "DepthOnly"
            Tags { "LightMode" = "DepthOnly" }
            Cull[_CullMode]
            HLSLPROGRAM
            #pragma target 4.5
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_instancing
            #include "Packages/com.unity.render-pipelines.hdrp/ShaderLibrary/Shared.hlsl"
            #include "Packages/com.unity.render-pipelines.hdrp/ShaderLibrary/DepthOnly.hlsl"
            ENDHLSL
        }
    }
    FallBack "Hidden/HDRP/Lit"
}