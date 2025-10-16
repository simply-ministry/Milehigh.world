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
            #include "Packages/com.unity.render-pipelines.hdrp/ShaderLibrary/Surface.hlsl"
            #include "Packages/com.unity.render-pipelines.hdrp/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.hdrp/ShaderLibrary/BSDF.hlsl"
            #include "Packages/com.unity.render-pipelines.hdrp/ShaderLibrary/Material.hlsl"

            // Declare CBuffer properties
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

            // Declare textures
            TEXTURE2D(_BaseColorMap);       SAMPLER(sampler_BaseColorMap);
            TEXTURE2D(_NormalMap);          SAMPLER(sampler_NormalMap);
            TEXTURE2D(_OcclusionMap);       SAMPLER(sampler_OcclusionMap);
            TEXTURE2D(_DetailAlbedoMap);    SAMPLER(sampler_DetailAlbedoMap);
            TEXTURE2D(_DetailNormalMap);    SAMPLER(sampler_DetailNormalMap);
            TEXTURE2D(_DetailSmoothnessMap);SAMPLER(sampler_DetailSmoothnessMap);
            TEXTURE2D(_ParallaxMap);        SAMPLER(sampler_ParallaxMap);
            TEXTURE2D(_EmissiveMap);        SAMPLER(sampler_EmissiveMap);

            struct Attributes
            {
                float3 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float4 tangentOS    : TANGENT;
                float2 uv           : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

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

            Varyings Vert(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);

                output.positionWS = TransformObjectToWorld(input.positionOS);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.tangentWS.xyz = TransformObjectToWorldDir(input.tangentOS.xyz);
                output.tangentWS.w = input.tangentOS.w;
                output.positionCS = TransformWorldToHClip(output.positionWS);

                output.uv = input.uv;
                output.detailUV = input.uv * _DetailScale;

                return output;
            }

            float4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);

                // Reconstruct the tangent frame
                float3 normalWS = normalize(input.normalWS);
                float3 tangentWS = normalize(input.tangentWS.xyz);
                float3 bitangentWS = cross(normalWS, tangentWS) * input.tangentWS.w;
                float3x3 TBN = float3x3(tangentWS, bitangentWS, normalWS);

                float3 viewDirWS = GetWorldSpaceNormalizeViewDir(input.positionWS);
                float3 viewDirTS = mul(TBN, viewDirWS);

                // Parallax Offset
                float2 parallaxUV = input.uv;
                if (_ParallaxHeight > 0.001)
                {
                    float heightSample = SAMPLE_TEXTURE2D(_ParallaxMap, sampler_ParallaxMap, input.uv).r;
                    parallaxUV -= viewDirTS.xy * (heightSample * _ParallaxHeight);
                }

                // Base Color and Albedo
                float4 baseColor = SAMPLE_TEXTURE2D(_BaseColorMap, sampler_BaseColorMap, parallaxUV) * _BaseColor;

                #if ALPHATEST_ON
                    if (baseColor.a < _Cutoff) discard;
                #endif

                // Normals
                float3 baseNormalTS = UnpackNormalScale(SAMPLE_TEXTURE2D_LOD(_NormalMap, sampler_NormalMap, parallaxUV, 0), _NormalScale);
                float3 detailNormalTS = UnpackNormal(SAMPLE_TEXTURE2D_LOD(_DetailNormalMap, sampler_DetailNormalMap, input.detailUV, 0));
                float3 combinedNormalTS = normalize(lerp(baseNormalTS, detailNormalTS, 0.5)); // Simple blend
                float3 finalNormalWS = TransformTangentToWorld(combinedNormalTS, TBN);

                // Surface properties
                float metallic = _Metallic;
                float smoothness = _Smoothness * SAMPLE_TEXTURE2D(_DetailSmoothnessMap, sampler_DetailSmoothnessMap, input.detailUV).r;
                float occlusion = lerp(1.0, SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, parallaxUV).r, _OcclusionStrength);

                // Emissive
                float3 emissiveColor = _EmissiveColor.rgb * SAMPLE_TEXTURE2D(_EmissiveMap, sampler_EmissiveMap, parallaxUV).rgb * _EmissiveLuminance;

                // Fresnel
                float fresnel = pow(1.0 - saturate(dot(finalNormalWS, viewDirWS)), _FresnelPower);
                emissiveColor += _FresnelColor.rgb * fresnel * _FresnelIntensity;

                // Prepare data for HDRP
                BuiltinData builtinData;
                ZERO_INITIALIZE(BuiltinData, builtinData);
                builtinData.positionCS = input.positionCS;
                builtinData.normalWS = finalNormalWS;
                builtinData.viewDirectionWS = viewDirWS;
                // Other fields like depth, motion vectors etc. would be set here in a full implementation

                SurfaceData surfaceData;
                ZERO_INITIALIZE(SurfaceData, surfaceData);
                surfaceData.albedo = baseColor.rgb * SAMPLE_TEXTURE2D(_DetailAlbedoMap, sampler_DetailAlbedoMap, input.detailUV).rgb;
                surfaceData.metallic = metallic;
                surfaceData.smoothness = smoothness;
                surfaceData.normalWS = finalNormalWS;
                surfaceData.ambientOcclusion = occlusion;
                surfaceData.emissive = emissiveColor;

                // In a real forward pass, you'd call HDRP lighting functions.
                // This simplified version outputs a color for demonstration.
                LightLoopContext lightLoopContext;
                ZERO_INITIALIZE(LightLoopContext, lightLoopContext);

                LightLoopOutput lightLoopOutput = EvaluateLightLoop(builtinData, surfaceData, lightLoopContext);

                float4 finalColor = float4(lightLoopOutput.diffuse + lightLoopOutput.specular + surfaceData.emissive, baseColor.a);

                return finalColor;
            }
            ENDHLSL
        }
        // ShadowCaster, DepthOnly, etc. passes would go here
    }
    FallBack "Hidden/HDRP/Lit"
}