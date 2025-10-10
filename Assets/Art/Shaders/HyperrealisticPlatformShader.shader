Shader "Custom/HyperrealisticPlatformShader"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (0.5, 0.5, 0.5, 1) // Default gray
        _Metallic("Metallic", Range(0, 1)) = 0.2 // Subtle metallic sheen
        _Roughness("Roughness", Range(0, 1)) = 0.8 // Mostly rough surface
        _DetailAlbedoMap("Detail Albedo Map", 2D) = "white" {}
        _DetailNormalMap("Detail Normal Map", 2D) = "bump" {}
        _DetailSmoothnessMask("Detail Smoothness Mask", 2D) = "white" {} // Mask for smoothness variations
        _DetailScale("Detail Scale", Float) = 5 // Tiling for detail maps
        _NormalMap("Normal Map", 2D) = "bump" {}
        _ParallaxMap("Parallax Map", 2D) = "black" {}
        _ParallaxHeight("Parallax Height", Float) = 0.05
        _EmissiveColor("Emissive Color", Color) = (0, 0.1, 0.2, 1) // Dark blueish emissive
        _EmissiveIntensity("Emissive Intensity", Float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "HDRenderPipeline" }
        Pass
        {
            Name "ForwardLit"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.6

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/BSDF.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
                float4 tangentOS : TANGENT;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float2 uv : TEXCOORD2;
                float2 detailUV : TEXCOORD3;
                float3 tangentWS : TEXCOORD4;
                float3 bitangentWS : TEXCOORD5;
            };

            // Declare shader properties
            float4 _BaseColor;
            half _Metallic;
            half _Roughness;
            sampler2D _DetailAlbedoMap;
            sampler2D _DetailNormalMap;
            sampler2D _DetailSmoothnessMask;
            float _DetailScale;
            sampler2D _NormalMap;
            sampler2D _ParallaxMap;
            float _ParallaxHeight;
            float4 _EmissiveColor;
            float _EmissiveIntensity;

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.positionWS = TransformObjectToWorld(input.positionOS.xyz);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.uv = input.uv;
                output.detailUV = input.uv * _DetailScale;
                output.tangentWS = TransformObjectToWorldDir(input.tangentOS.xyz);
                output.bitangentWS = cross(output.normalWS, output.tangentWS) * input.tangentOS.w;
                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                // Parallax mapping
                float3 viewDirTS = normalize(TransformWorldToTangent(GetWorldSpaceViewDir(input.positionWS), input.normalWS, input.tangentWS, input.bitangentWS));
                float parallaxHeight = tex2D(_ParallaxMap, input.uv).r;
                float2 parallaxOffset = viewDirTS.xy * parallaxHeight * _ParallaxHeight;
                float2 parallaxUV = input.uv - parallaxOffset;
                float2 detailParallaxUV = input.detailUV - parallaxOffset;

                // Texture sampling
                float4 detailAlbedo = tex2D(_DetailAlbedoMap, detailParallaxUV);
                float4 baseColor = _BaseColor * detailAlbedo;

                float3 normalTS = UnpackNormal(tex2D(_NormalMap, parallaxUV));
                float3 detailNormalTS = UnpackNormal(tex2D(_DetailNormalMap, detailParallaxUV));
                // Blend normals using a simple overlay-like approach
                normalTS = normalize(normalTS + detailNormalTS);

                float detailSmoothness = tex2D(_DetailSmoothnessMask, detailParallaxUV).r;
                float smoothness = 1.0 - (_Roughness * (1.0 - detailSmoothness));

                // Prepare surface data for HDRP lighting
                BuiltinSurfaceData surfaceData;
                ZERO_INITIALIZE(BuiltinSurfaceData, surfaceData); // Important for HDRP
                surfaceData.baseColor = baseColor.rgb;
                surfaceData.metallic = _Metallic;
                surfaceData.smoothness = smoothness;
                surfaceData.normalWS = normalize(TransformTangentToWorld(normalTS, float3x3(input.tangentWS, input.bitangentWS, input.normalWS)));
                surfaceData.ambientOcclusion = 1.0; // Default AO

                // Lighting calculation
                BSDFData bsdfData;
                ZERO_INITIALIZE(BSDFData, bsdfData);
                GetBSDFData(surfaceData, bsdfData);

                LightLoopOutput lightLoopOutput;
                EvaluateBSDF(GetLightingContext(), input.positionWS, bsdfData, lightLoopOutput);

                float3 finalColor = lightLoopOutput.diffuse + lightLoopOutput.specular;

                // Add emissive
                finalColor += _EmissiveColor.rgb * _EmissiveIntensity;

                return float4(finalColor, baseColor.a);
            }
            ENDHLSL
        }
    }
    FallBack "HDRP/Lit"
}