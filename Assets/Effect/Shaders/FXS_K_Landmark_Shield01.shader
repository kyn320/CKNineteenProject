// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "CustomFX/K/Landmark_Shield"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[ASEBegin]_Fres_Scale("Fres_Scale", Range( 0 , 10)) = 1
		_Fres_Power("Fres_Power", Range( 0 , 10)) = 1
		[HDR]_Main_Frescolor("Main_Frescolor", Color) = (0,0,0,0)
		_Offset("Offset", Range( 0 , 1)) = 0
		_Mask_Offset("Mask_Offset", Range( -13 , 10)) = 0
		_Mask_NoiseTex("Mask_NoiseTex", 2D) = "white" {}
		_Step("Step", Range( 0 , 1)) = 0.15
		_Noise_Pow("Noise_Pow", Range( 1 , 10)) = 0
		_Noise_Mult("Noise_Mult", Range( 1 , 10)) = 0
		_Step_Gap("Step_Gap", Range( 0 , 0.5)) = 0
		[HDR]_Gap_Color("Gap_Color", Color) = (0,0,0,0)
		_Noise_Vpanner("Noise_Vpanner", Range( -0.5 , 0.5)) = 0
		_Fres_NoiseTex("Fres_NoiseTex", 2D) = "white" {}
		_FresNoise_Vpanner("FresNoise_Vpanner", Range( -0.5 , 0.5)) = 0
		_FresNoise2_Vpanner("FresNoise2_Vpanner", Range( -0.5 , 0.5)) = 0
		_FresNoise_Pow("FresNoise_Pow", Range( 1 , 10)) = 0
		_FresNoise_Mult("FresNoise_Mult", Range( 1 , 10)) = 0
		_FresNoise2_Pow("FresNoise2_Pow", Range( 1 , 10)) = 0
		_FresNoise2_Mult("FresNoise2_Mult", Range( 1 , 10)) = 0
		[Toggle]_isInside("is Inside", Float) = 1
		_InsideMask_Pow("InsideMask_Pow", Range( 1 , 10)) = 0
		_InsideMask_Mult("InsideMask_Mult", Range( 1 , 10)) = 0
		_Inside_Pow("Inside_Pow", Range( 0 , 10)) = 1
		_Inside_Mult("Inside_Mult", Range( 0 , 10)) = 0.1
		[Toggle(_USE_CUSTOM_ON)] _Use_Custom("Use_Custom", Float) = 0
		_HexaTex("HexaTex", 2D) = "white" {}
		_Hexa_ScrollSpeed("Hexa_ScrollSpeed", Range( -1 , 1)) = 0
		_Hexa_Wavecount("Hexa_Wavecount", Range( 0 , 30)) = 0
		_Hexa_Maskpow("Hexa_Maskpow", Range( 1 , 20)) = 7.71
		_Hexa_MaskMult("Hexa_MaskMult", Range( 0 , 10)) = 0
		_Hexa_BackOpacity("Hexa_BackOpacity", Range( 0 , 0.4)) = 0.7
		_Hexa_VmaskPow("Hexa_VmaskPow", Range( 1 , 30)) = 0
		_Hexa_Vmask_mult("Hexa_Vmask_mult", Range( 1 , 10)) = 0
		_Hexa_Vpanner("Hexa_Vpanner", Range( -1 , 1)) = 0
		[ASEEnd]_Hexa_Upanner("Hexa_Upanner", Range( -1 , 1)) = 0

		[HideInInspector]_QueueOffset("_QueueOffset", Float) = 0
        [HideInInspector]_QueueControl("_QueueControl", Float) = -1
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25
	}

	SubShader
	{
		LOD 0

		
		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" }
		
		Cull Back
		AlphaToMask Off
		
		HLSLINCLUDE
		#pragma target 3.0

		#pragma prefer_hlslcc gles
		#pragma exclude_renderers d3d11_9x 

		#ifndef ASE_TESS_FUNCS
		#define ASE_TESS_FUNCS
		float4 FixedTess( float tessValue )
		{
			return tessValue;
		}
		
		float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
		{
			float3 wpos = mul(o2w,vertex).xyz;
			float dist = distance (wpos, cameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
			return f;
		}

		float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
		{
			float4 tess;
			tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
			tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
			tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
			tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
			return tess;
		}

		float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
		{
			float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
			float len = distance(wpos0, wpos1);
			float f = max(len * scParams.y / (edgeLen * dist), 1.0);
			return f;
		}

		float DistanceFromPlane (float3 pos, float4 plane)
		{
			float d = dot (float4(pos,1.0f), plane);
			return d;
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlane(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlane(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlane(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlane(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
			return !all (planeTest);
		}

		float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
		{
			float3 f;
			f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
			f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
			f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

			return CalcTriEdgeTessFactors (f);
		}

		float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;
			tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
			tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
			tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
			tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			return tess;
		}

		float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;

			if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
			{
				tess = 0.0f;
			}
			else
			{
				tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
				tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
				tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
				tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			}
			return tess;
		}
		#endif //ASE_TESS_FUNCS

		ENDHLSL

		
		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="UniversalForwardOnly" }
			
			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZWrite On
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			
			#define _RECEIVE_SHADOWS_OFF 1
			#define ASE_SRP_VERSION 999999
			#define REQUIRE_DEPTH_TEXTURE 1

			
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma shader_feature _ _SAMPLE_GI
			#pragma multi_compile _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
			#pragma multi_compile _ DEBUG_DISPLAY
			#define SHADERPASS SHADERPASS_UNLIT


			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging3D.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceData.hlsl"


			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_VERT_NORMAL
			#pragma shader_feature_local _USE_CUSTOM_ON


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				#ifdef ASE_FOG
				float fogFactor : TEXCOORD2;
				#endif
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				float4 ase_texcoord6 : TEXCOORD6;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _Main_Frescolor;
			float4 _Gap_Color;
			float4 _Mask_NoiseTex_ST;
			float4 _Fres_NoiseTex_ST;
			float4 _HexaTex_ST;
			float _Hexa_Upanner;
			float _Hexa_Vpanner;
			float _Hexa_BackOpacity;
			float _Hexa_ScrollSpeed;
			float _Hexa_Maskpow;
			float _Offset;
			float _Hexa_MaskMult;
			float _Hexa_VmaskPow;
			float _Hexa_Vmask_mult;
			float _Inside_Pow;
			float _Inside_Mult;
			float _Hexa_Wavecount;
			float _FresNoise2_Mult;
			float _FresNoise2_Vpanner;
			float _InsideMask_Pow;
			float _FresNoise_Mult;
			float _FresNoise_Pow;
			float _FresNoise_Vpanner;
			float _Fres_Power;
			float _Fres_Scale;
			float _isInside;
			float _Step_Gap;
			float _Step;
			float _Noise_Mult;
			float _Noise_Pow;
			float _Noise_Vpanner;
			float _Mask_Offset;
			float _FresNoise2_Pow;
			float _InsideMask_Mult;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _Mask_NoiseTex;
			sampler2D _Fres_NoiseTex;
			uniform float4 _CameraDepthTexture_TexelSize;
			sampler2D _HexaTex;


						
			VertexOutput VertexFunction ( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord5.xyz = ase_worldNormal;
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord6 = screenPos;
				
				o.ase_texcoord3 = v.vertex;
				o.ase_texcoord4 = v.ase_texcoord;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord5.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				VertexPositionInputs vertexInput = (VertexPositionInputs)0;
				vertexInput.positionWS = positionWS;
				vertexInput.positionCS = positionCS;
				o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				#ifdef ASE_FOG
				o.fogFactor = ComputeFogFactor( positionCS.z );
				#endif
				o.clipPos = positionCS;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag ( VertexOutput IN , FRONT_FACE_TYPE ase_vface : FRONT_FACE_SEMANTIC ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif
				#ifdef _USE_CUSTOM_ON
				float staticSwitch173 = IN.ase_texcoord4.z;
				#else
				float staticSwitch173 = _Mask_Offset;
				#endif
				float2 appendResult111 = (float2(0.0 , _Noise_Vpanner));
				float2 uv_Mask_NoiseTex = IN.ase_texcoord4.xy * _Mask_NoiseTex_ST.xy + _Mask_NoiseTex_ST.zw;
				float2 panner107 = ( 1.0 * _Time.y * appendResult111 + uv_Mask_NoiseTex);
				float temp_output_86_0 = saturate( ( ( IN.ase_texcoord3.xyz.y + staticSwitch173 ) + ( pow( tex2D( _Mask_NoiseTex, panner107 ).r , _Noise_Pow ) * _Noise_Mult ) ) );
				float temp_output_80_0 = ( 1.0 - step( temp_output_86_0 , _Step ) );
				float temp_output_93_0 = ( temp_output_80_0 - ( 1.0 - step( temp_output_86_0 , ( _Step + _Step_Gap ) ) ) );
				
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldNormal = IN.ase_texcoord5.xyz;
				float fresnelNdotV10 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode10 = ( 0.0 + _Fres_Scale * pow( 1.0 - fresnelNdotV10, _Fres_Power ) );
				float2 appendResult127 = (float2(0.0 , _FresNoise_Vpanner));
				float2 uv_Fres_NoiseTex = IN.ase_texcoord4.xy * _Fres_NoiseTex_ST.xy + _Fres_NoiseTex_ST.zw;
				float2 panner125 = ( 1.0 * _Time.y * appendResult127 + uv_Fres_NoiseTex);
				float temp_output_133_0 = ( pow( tex2D( _Fres_NoiseTex, panner125 ).r , _FresNoise_Pow ) * _FresNoise_Mult );
				float2 appendResult140 = (float2(0.0 , _FresNoise2_Vpanner));
				float2 texCoord139 = IN.ase_texcoord4.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner141 = ( 1.0 * _Time.y * appendResult140 + texCoord139);
				float temp_output_143_0 = ( pow( tex2D( _Fres_NoiseTex, panner141 ).r , _FresNoise2_Pow ) * _FresNoise2_Mult );
				float4 screenPos = IN.ase_texcoord6;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float eyeDepth15 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm.xy ),_ZBufferParams);
				float2 appendResult183 = (float2(_Hexa_Upanner , _Hexa_Vpanner));
				float2 uv_HexaTex = IN.ase_texcoord4.xy * _HexaTex_ST.xy + _HexaTex_ST.zw;
				float2 panner188 = ( 1.0 * _Time.y * appendResult183 + uv_HexaTex);
				float4 tex2DNode192 = tex2D( _HexaTex, panner188 );
				float2 texCoord175 = IN.ase_texcoord4.xy * float2( 1,1 ) + float2( 0,0 );
				float mulTime176 = _TimeParameters.x * _Hexa_ScrollSpeed;
				float temp_output_205_0 = saturate( ( ( tex2DNode192.r * _Hexa_BackOpacity ) + ( ( saturate( tex2DNode192.r ) * saturate( ( pow( abs( sin( ( ( texCoord175.y + mulTime176 ) * _Hexa_Wavecount ) ) ) , _Hexa_Maskpow ) * _Hexa_MaskMult ) ) ) * saturate( ( pow( ( 1.0 - texCoord175.y ) , _Hexa_VmaskPow ) * _Hexa_Vmask_mult ) ) ) ) );
				float2 texCoord162 = IN.ase_texcoord4.xy * float2( 1,1 ) + float2( 0,0 );
				float switchResult112 = (((ase_vface>0)?(saturate( saturate( ( ( (( _isInside )?( saturate( ( saturate( ( pow( ( temp_output_133_0 + temp_output_143_0 ) , _Inside_Pow ) * _Inside_Mult * saturate( ( pow( ( 1.0 - texCoord162.y ) , _InsideMask_Pow ) * _InsideMask_Mult ) ) ) ) + temp_output_205_0 ) ) ):( saturate( ( saturate( ( saturate( ( fresnelNode10 * saturate( ( fresnelNode10 + temp_output_133_0 + temp_output_143_0 ) ) ) ) + saturate( ( 1.0 - ( eyeDepth15 + -screenPos.w + _Offset ) ) ) ) ) + temp_output_205_0 ) ) )) * saturate( temp_output_80_0 ) ) + temp_output_93_0 ) ) )):(saturate( saturate( temp_output_93_0 ) ))));
				
				float3 BakedAlbedo = 0;
				float3 BakedEmission = 0;
				float3 Color = ( _Main_Frescolor + ( _Gap_Color * temp_output_93_0 ) ).rgb;
				float Alpha = switchResult112;
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;

				#ifdef _ALPHATEST_ON
					clip( Alpha - AlphaClipThreshold );
				#endif

				#if defined(_DBUFFER)
					ApplyDecalToBaseColor(IN.clipPos, Color);
				#endif

				#if defined(_ALPHAPREMULTIPLY_ON)
				Color *= Alpha;
				#endif


				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				#ifdef ASE_FOG
					Color = MixFog( Color, IN.fogFactor );
				#endif

				return half4( Color, Alpha );
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			ZWrite On
			ColorMask 0
			AlphaToMask Off

			HLSLPROGRAM
			
			#define _RECEIVE_SHADOWS_OFF 1
			#define ASE_SRP_VERSION 999999
			#define REQUIRE_DEPTH_TEXTURE 1

			
			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_VERT_NORMAL
			#pragma shader_feature_local _USE_CUSTOM_ON


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _Main_Frescolor;
			float4 _Gap_Color;
			float4 _Mask_NoiseTex_ST;
			float4 _Fres_NoiseTex_ST;
			float4 _HexaTex_ST;
			float _Hexa_Upanner;
			float _Hexa_Vpanner;
			float _Hexa_BackOpacity;
			float _Hexa_ScrollSpeed;
			float _Hexa_Maskpow;
			float _Offset;
			float _Hexa_MaskMult;
			float _Hexa_VmaskPow;
			float _Hexa_Vmask_mult;
			float _Inside_Pow;
			float _Inside_Mult;
			float _Hexa_Wavecount;
			float _FresNoise2_Mult;
			float _FresNoise2_Vpanner;
			float _InsideMask_Pow;
			float _FresNoise_Mult;
			float _FresNoise_Pow;
			float _FresNoise_Vpanner;
			float _Fres_Power;
			float _Fres_Scale;
			float _isInside;
			float _Step_Gap;
			float _Step;
			float _Noise_Mult;
			float _Noise_Pow;
			float _Noise_Vpanner;
			float _Mask_Offset;
			float _FresNoise2_Pow;
			float _InsideMask_Mult;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _Fres_NoiseTex;
			uniform float4 _CameraDepthTexture_TexelSize;
			sampler2D _HexaTex;
			sampler2D _Mask_NoiseTex;


			
			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord2.xyz = ase_worldNormal;
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord4 = screenPos;
				
				o.ase_texcoord3 = v.ase_texcoord;
				o.ase_texcoord5 = v.vertex;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				o.clipPos = TransformWorldToHClip( positionWS );
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = o.clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN , FRONT_FACE_TYPE ase_vface : FRONT_FACE_SEMANTIC ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldNormal = IN.ase_texcoord2.xyz;
				float fresnelNdotV10 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode10 = ( 0.0 + _Fres_Scale * pow( 1.0 - fresnelNdotV10, _Fres_Power ) );
				float2 appendResult127 = (float2(0.0 , _FresNoise_Vpanner));
				float2 uv_Fres_NoiseTex = IN.ase_texcoord3.xy * _Fres_NoiseTex_ST.xy + _Fres_NoiseTex_ST.zw;
				float2 panner125 = ( 1.0 * _Time.y * appendResult127 + uv_Fres_NoiseTex);
				float temp_output_133_0 = ( pow( tex2D( _Fres_NoiseTex, panner125 ).r , _FresNoise_Pow ) * _FresNoise_Mult );
				float2 appendResult140 = (float2(0.0 , _FresNoise2_Vpanner));
				float2 texCoord139 = IN.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner141 = ( 1.0 * _Time.y * appendResult140 + texCoord139);
				float temp_output_143_0 = ( pow( tex2D( _Fres_NoiseTex, panner141 ).r , _FresNoise2_Pow ) * _FresNoise2_Mult );
				float4 screenPos = IN.ase_texcoord4;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float eyeDepth15 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm.xy ),_ZBufferParams);
				float2 appendResult183 = (float2(_Hexa_Upanner , _Hexa_Vpanner));
				float2 uv_HexaTex = IN.ase_texcoord3.xy * _HexaTex_ST.xy + _HexaTex_ST.zw;
				float2 panner188 = ( 1.0 * _Time.y * appendResult183 + uv_HexaTex);
				float4 tex2DNode192 = tex2D( _HexaTex, panner188 );
				float2 texCoord175 = IN.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float mulTime176 = _TimeParameters.x * _Hexa_ScrollSpeed;
				float temp_output_205_0 = saturate( ( ( tex2DNode192.r * _Hexa_BackOpacity ) + ( ( saturate( tex2DNode192.r ) * saturate( ( pow( abs( sin( ( ( texCoord175.y + mulTime176 ) * _Hexa_Wavecount ) ) ) , _Hexa_Maskpow ) * _Hexa_MaskMult ) ) ) * saturate( ( pow( ( 1.0 - texCoord175.y ) , _Hexa_VmaskPow ) * _Hexa_Vmask_mult ) ) ) ) );
				float2 texCoord162 = IN.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				#ifdef _USE_CUSTOM_ON
				float staticSwitch173 = IN.ase_texcoord3.z;
				#else
				float staticSwitch173 = _Mask_Offset;
				#endif
				float2 appendResult111 = (float2(0.0 , _Noise_Vpanner));
				float2 uv_Mask_NoiseTex = IN.ase_texcoord3.xy * _Mask_NoiseTex_ST.xy + _Mask_NoiseTex_ST.zw;
				float2 panner107 = ( 1.0 * _Time.y * appendResult111 + uv_Mask_NoiseTex);
				float temp_output_86_0 = saturate( ( ( IN.ase_texcoord5.xyz.y + staticSwitch173 ) + ( pow( tex2D( _Mask_NoiseTex, panner107 ).r , _Noise_Pow ) * _Noise_Mult ) ) );
				float temp_output_80_0 = ( 1.0 - step( temp_output_86_0 , _Step ) );
				float temp_output_93_0 = ( temp_output_80_0 - ( 1.0 - step( temp_output_86_0 , ( _Step + _Step_Gap ) ) ) );
				float switchResult112 = (((ase_vface>0)?(saturate( saturate( ( ( (( _isInside )?( saturate( ( saturate( ( pow( ( temp_output_133_0 + temp_output_143_0 ) , _Inside_Pow ) * _Inside_Mult * saturate( ( pow( ( 1.0 - texCoord162.y ) , _InsideMask_Pow ) * _InsideMask_Mult ) ) ) ) + temp_output_205_0 ) ) ):( saturate( ( saturate( ( saturate( ( fresnelNode10 * saturate( ( fresnelNode10 + temp_output_133_0 + temp_output_143_0 ) ) ) ) + saturate( ( 1.0 - ( eyeDepth15 + -screenPos.w + _Offset ) ) ) ) ) + temp_output_205_0 ) ) )) * saturate( temp_output_80_0 ) ) + temp_output_93_0 ) ) )):(saturate( saturate( temp_output_93_0 ) ))));
				
				float Alpha = switchResult112;
				float AlphaClipThreshold = 0.5;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				return 0;
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "Universal2D"
			Tags { "LightMode"="Universal2D" }
			
			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZWrite On
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			
			#define _RECEIVE_SHADOWS_OFF 1
			#define ASE_SRP_VERSION 999999
			#define REQUIRE_DEPTH_TEXTURE 1

			
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma shader_feature _ _SAMPLE_GI
			#pragma multi_compile _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
			#pragma multi_compile _ DEBUG_DISPLAY
			#define SHADERPASS SHADERPASS_UNLIT


			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging3D.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceData.hlsl"


			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_VERT_NORMAL
			#pragma shader_feature_local _USE_CUSTOM_ON


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				#ifdef ASE_FOG
				float fogFactor : TEXCOORD2;
				#endif
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				float4 ase_texcoord6 : TEXCOORD6;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _Main_Frescolor;
			float4 _Gap_Color;
			float4 _Mask_NoiseTex_ST;
			float4 _Fres_NoiseTex_ST;
			float4 _HexaTex_ST;
			float _Hexa_Upanner;
			float _Hexa_Vpanner;
			float _Hexa_BackOpacity;
			float _Hexa_ScrollSpeed;
			float _Hexa_Maskpow;
			float _Offset;
			float _Hexa_MaskMult;
			float _Hexa_VmaskPow;
			float _Hexa_Vmask_mult;
			float _Inside_Pow;
			float _Inside_Mult;
			float _Hexa_Wavecount;
			float _FresNoise2_Mult;
			float _FresNoise2_Vpanner;
			float _InsideMask_Pow;
			float _FresNoise_Mult;
			float _FresNoise_Pow;
			float _FresNoise_Vpanner;
			float _Fres_Power;
			float _Fres_Scale;
			float _isInside;
			float _Step_Gap;
			float _Step;
			float _Noise_Mult;
			float _Noise_Pow;
			float _Noise_Vpanner;
			float _Mask_Offset;
			float _FresNoise2_Pow;
			float _InsideMask_Mult;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _Mask_NoiseTex;
			sampler2D _Fres_NoiseTex;
			uniform float4 _CameraDepthTexture_TexelSize;
			sampler2D _HexaTex;


						
			VertexOutput VertexFunction ( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord5.xyz = ase_worldNormal;
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord6 = screenPos;
				
				o.ase_texcoord3 = v.vertex;
				o.ase_texcoord4 = v.ase_texcoord;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord5.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				VertexPositionInputs vertexInput = (VertexPositionInputs)0;
				vertexInput.positionWS = positionWS;
				vertexInput.positionCS = positionCS;
				o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				#ifdef ASE_FOG
				o.fogFactor = ComputeFogFactor( positionCS.z );
				#endif
				o.clipPos = positionCS;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag ( VertexOutput IN , FRONT_FACE_TYPE ase_vface : FRONT_FACE_SEMANTIC ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif
				#ifdef _USE_CUSTOM_ON
				float staticSwitch173 = IN.ase_texcoord4.z;
				#else
				float staticSwitch173 = _Mask_Offset;
				#endif
				float2 appendResult111 = (float2(0.0 , _Noise_Vpanner));
				float2 uv_Mask_NoiseTex = IN.ase_texcoord4.xy * _Mask_NoiseTex_ST.xy + _Mask_NoiseTex_ST.zw;
				float2 panner107 = ( 1.0 * _Time.y * appendResult111 + uv_Mask_NoiseTex);
				float temp_output_86_0 = saturate( ( ( IN.ase_texcoord3.xyz.y + staticSwitch173 ) + ( pow( tex2D( _Mask_NoiseTex, panner107 ).r , _Noise_Pow ) * _Noise_Mult ) ) );
				float temp_output_80_0 = ( 1.0 - step( temp_output_86_0 , _Step ) );
				float temp_output_93_0 = ( temp_output_80_0 - ( 1.0 - step( temp_output_86_0 , ( _Step + _Step_Gap ) ) ) );
				
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldNormal = IN.ase_texcoord5.xyz;
				float fresnelNdotV10 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode10 = ( 0.0 + _Fres_Scale * pow( 1.0 - fresnelNdotV10, _Fres_Power ) );
				float2 appendResult127 = (float2(0.0 , _FresNoise_Vpanner));
				float2 uv_Fres_NoiseTex = IN.ase_texcoord4.xy * _Fres_NoiseTex_ST.xy + _Fres_NoiseTex_ST.zw;
				float2 panner125 = ( 1.0 * _Time.y * appendResult127 + uv_Fres_NoiseTex);
				float temp_output_133_0 = ( pow( tex2D( _Fres_NoiseTex, panner125 ).r , _FresNoise_Pow ) * _FresNoise_Mult );
				float2 appendResult140 = (float2(0.0 , _FresNoise2_Vpanner));
				float2 texCoord139 = IN.ase_texcoord4.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner141 = ( 1.0 * _Time.y * appendResult140 + texCoord139);
				float temp_output_143_0 = ( pow( tex2D( _Fres_NoiseTex, panner141 ).r , _FresNoise2_Pow ) * _FresNoise2_Mult );
				float4 screenPos = IN.ase_texcoord6;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float eyeDepth15 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm.xy ),_ZBufferParams);
				float2 appendResult183 = (float2(_Hexa_Upanner , _Hexa_Vpanner));
				float2 uv_HexaTex = IN.ase_texcoord4.xy * _HexaTex_ST.xy + _HexaTex_ST.zw;
				float2 panner188 = ( 1.0 * _Time.y * appendResult183 + uv_HexaTex);
				float4 tex2DNode192 = tex2D( _HexaTex, panner188 );
				float2 texCoord175 = IN.ase_texcoord4.xy * float2( 1,1 ) + float2( 0,0 );
				float mulTime176 = _TimeParameters.x * _Hexa_ScrollSpeed;
				float temp_output_205_0 = saturate( ( ( tex2DNode192.r * _Hexa_BackOpacity ) + ( ( saturate( tex2DNode192.r ) * saturate( ( pow( abs( sin( ( ( texCoord175.y + mulTime176 ) * _Hexa_Wavecount ) ) ) , _Hexa_Maskpow ) * _Hexa_MaskMult ) ) ) * saturate( ( pow( ( 1.0 - texCoord175.y ) , _Hexa_VmaskPow ) * _Hexa_Vmask_mult ) ) ) ) );
				float2 texCoord162 = IN.ase_texcoord4.xy * float2( 1,1 ) + float2( 0,0 );
				float switchResult112 = (((ase_vface>0)?(saturate( saturate( ( ( (( _isInside )?( saturate( ( saturate( ( pow( ( temp_output_133_0 + temp_output_143_0 ) , _Inside_Pow ) * _Inside_Mult * saturate( ( pow( ( 1.0 - texCoord162.y ) , _InsideMask_Pow ) * _InsideMask_Mult ) ) ) ) + temp_output_205_0 ) ) ):( saturate( ( saturate( ( saturate( ( fresnelNode10 * saturate( ( fresnelNode10 + temp_output_133_0 + temp_output_143_0 ) ) ) ) + saturate( ( 1.0 - ( eyeDepth15 + -screenPos.w + _Offset ) ) ) ) ) + temp_output_205_0 ) ) )) * saturate( temp_output_80_0 ) ) + temp_output_93_0 ) ) )):(saturate( saturate( temp_output_93_0 ) ))));
				
				float3 BakedAlbedo = 0;
				float3 BakedEmission = 0;
				float3 Color = ( _Main_Frescolor + ( _Gap_Color * temp_output_93_0 ) ).rgb;
				float Alpha = switchResult112;
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;

				#ifdef _ALPHATEST_ON
					clip( Alpha - AlphaClipThreshold );
				#endif

				#if defined(_DBUFFER)
					ApplyDecalToBaseColor(IN.clipPos, Color);
				#endif

				#if defined(_ALPHAPREMULTIPLY_ON)
				Color *= Alpha;
				#endif


				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				#ifdef ASE_FOG
					Color = MixFog( Color, IN.fogFactor );
				#endif

				return half4( Color, Alpha );
			}

			ENDHLSL
		}


		
        Pass
        {
			
            Name "SceneSelectionPass"
            Tags { "LightMode"="SceneSelectionPass" }
        
			Cull Off

			HLSLPROGRAM
        
			#define _RECEIVE_SHADOWS_OFF 1
			#define ASE_SRP_VERSION 999999
			#define REQUIRE_DEPTH_TEXTURE 1

        
			#pragma only_renderers d3d11 glcore gles gles3 
			#pragma vertex vert
			#pragma fragment frag

			#define ATTRIBUTES_NEED_NORMAL
			#define ATTRIBUTES_NEED_TANGENT
			#define SHADERPASS SHADERPASS_DEPTHONLY

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
			#define ASE_NEEDS_VERT_NORMAL
			#pragma shader_feature_local _USE_CUSTOM_ON


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
        
			CBUFFER_START(UnityPerMaterial)
			float4 _Main_Frescolor;
			float4 _Gap_Color;
			float4 _Mask_NoiseTex_ST;
			float4 _Fres_NoiseTex_ST;
			float4 _HexaTex_ST;
			float _Hexa_Upanner;
			float _Hexa_Vpanner;
			float _Hexa_BackOpacity;
			float _Hexa_ScrollSpeed;
			float _Hexa_Maskpow;
			float _Offset;
			float _Hexa_MaskMult;
			float _Hexa_VmaskPow;
			float _Hexa_Vmask_mult;
			float _Inside_Pow;
			float _Inside_Mult;
			float _Hexa_Wavecount;
			float _FresNoise2_Mult;
			float _FresNoise2_Vpanner;
			float _InsideMask_Pow;
			float _FresNoise_Mult;
			float _FresNoise_Pow;
			float _FresNoise_Vpanner;
			float _Fres_Power;
			float _Fres_Scale;
			float _isInside;
			float _Step_Gap;
			float _Step;
			float _Noise_Mult;
			float _Noise_Pow;
			float _Noise_Vpanner;
			float _Mask_Offset;
			float _FresNoise2_Pow;
			float _InsideMask_Mult;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			sampler2D _Fres_NoiseTex;
			uniform float4 _CameraDepthTexture_TexelSize;
			sampler2D _HexaTex;
			sampler2D _Mask_NoiseTex;


			
			int _ObjectId;
			int _PassValue;

			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};
        
			VertexOutput VertexFunction(VertexInput v  )
			{
				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				o.ase_texcoord.xyz = ase_worldPos;
				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord1.xyz = ase_worldNormal;
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord3 = screenPos;
				
				o.ase_texcoord2 = v.ase_texcoord;
				o.ase_texcoord4 = v.vertex;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.w = 0;
				o.ase_texcoord1.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				o.clipPos = TransformWorldToHClip(positionWS);
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif
			
			half4 frag(VertexOutput IN , FRONT_FACE_TYPE ase_vface : FRONT_FACE_SEMANTIC) : SV_TARGET
			{
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float3 ase_worldPos = IN.ase_texcoord.xyz;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - ase_worldPos );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldNormal = IN.ase_texcoord1.xyz;
				float fresnelNdotV10 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode10 = ( 0.0 + _Fres_Scale * pow( 1.0 - fresnelNdotV10, _Fres_Power ) );
				float2 appendResult127 = (float2(0.0 , _FresNoise_Vpanner));
				float2 uv_Fres_NoiseTex = IN.ase_texcoord2.xy * _Fres_NoiseTex_ST.xy + _Fres_NoiseTex_ST.zw;
				float2 panner125 = ( 1.0 * _Time.y * appendResult127 + uv_Fres_NoiseTex);
				float temp_output_133_0 = ( pow( tex2D( _Fres_NoiseTex, panner125 ).r , _FresNoise_Pow ) * _FresNoise_Mult );
				float2 appendResult140 = (float2(0.0 , _FresNoise2_Vpanner));
				float2 texCoord139 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner141 = ( 1.0 * _Time.y * appendResult140 + texCoord139);
				float temp_output_143_0 = ( pow( tex2D( _Fres_NoiseTex, panner141 ).r , _FresNoise2_Pow ) * _FresNoise2_Mult );
				float4 screenPos = IN.ase_texcoord3;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float eyeDepth15 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm.xy ),_ZBufferParams);
				float2 appendResult183 = (float2(_Hexa_Upanner , _Hexa_Vpanner));
				float2 uv_HexaTex = IN.ase_texcoord2.xy * _HexaTex_ST.xy + _HexaTex_ST.zw;
				float2 panner188 = ( 1.0 * _Time.y * appendResult183 + uv_HexaTex);
				float4 tex2DNode192 = tex2D( _HexaTex, panner188 );
				float2 texCoord175 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float mulTime176 = _TimeParameters.x * _Hexa_ScrollSpeed;
				float temp_output_205_0 = saturate( ( ( tex2DNode192.r * _Hexa_BackOpacity ) + ( ( saturate( tex2DNode192.r ) * saturate( ( pow( abs( sin( ( ( texCoord175.y + mulTime176 ) * _Hexa_Wavecount ) ) ) , _Hexa_Maskpow ) * _Hexa_MaskMult ) ) ) * saturate( ( pow( ( 1.0 - texCoord175.y ) , _Hexa_VmaskPow ) * _Hexa_Vmask_mult ) ) ) ) );
				float2 texCoord162 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				#ifdef _USE_CUSTOM_ON
				float staticSwitch173 = IN.ase_texcoord2.z;
				#else
				float staticSwitch173 = _Mask_Offset;
				#endif
				float2 appendResult111 = (float2(0.0 , _Noise_Vpanner));
				float2 uv_Mask_NoiseTex = IN.ase_texcoord2.xy * _Mask_NoiseTex_ST.xy + _Mask_NoiseTex_ST.zw;
				float2 panner107 = ( 1.0 * _Time.y * appendResult111 + uv_Mask_NoiseTex);
				float temp_output_86_0 = saturate( ( ( IN.ase_texcoord4.xyz.y + staticSwitch173 ) + ( pow( tex2D( _Mask_NoiseTex, panner107 ).r , _Noise_Pow ) * _Noise_Mult ) ) );
				float temp_output_80_0 = ( 1.0 - step( temp_output_86_0 , _Step ) );
				float temp_output_93_0 = ( temp_output_80_0 - ( 1.0 - step( temp_output_86_0 , ( _Step + _Step_Gap ) ) ) );
				float switchResult112 = (((ase_vface>0)?(saturate( saturate( ( ( (( _isInside )?( saturate( ( saturate( ( pow( ( temp_output_133_0 + temp_output_143_0 ) , _Inside_Pow ) * _Inside_Mult * saturate( ( pow( ( 1.0 - texCoord162.y ) , _InsideMask_Pow ) * _InsideMask_Mult ) ) ) ) + temp_output_205_0 ) ) ):( saturate( ( saturate( ( saturate( ( fresnelNode10 * saturate( ( fresnelNode10 + temp_output_133_0 + temp_output_143_0 ) ) ) ) + saturate( ( 1.0 - ( eyeDepth15 + -screenPos.w + _Offset ) ) ) ) ) + temp_output_205_0 ) ) )) * saturate( temp_output_80_0 ) ) + temp_output_93_0 ) ) )):(saturate( saturate( temp_output_93_0 ) ))));
				
				surfaceDescription.Alpha = switchResult112;
				surfaceDescription.AlphaClipThreshold = 0.5;


				#if _ALPHATEST_ON
					float alphaClipThreshold = 0.01f;
					#if ALPHA_CLIP_THRESHOLD
						alphaClipThreshold = surfaceDescription.AlphaClipThreshold;
					#endif
					clip(surfaceDescription.Alpha - alphaClipThreshold);
				#endif

				half4 outColor = half4(_ObjectId, _PassValue, 1.0, 1.0);
				return outColor;
			}

			ENDHLSL
        }

		
        Pass
        {
			
            Name "ScenePickingPass"
            Tags { "LightMode"="Picking" }
        
			HLSLPROGRAM

			#define _RECEIVE_SHADOWS_OFF 1
			#define ASE_SRP_VERSION 999999
			#define REQUIRE_DEPTH_TEXTURE 1


			#pragma only_renderers d3d11 glcore gles gles3 
			#pragma vertex vert
			#pragma fragment frag

        
			#define ATTRIBUTES_NEED_NORMAL
			#define ATTRIBUTES_NEED_TANGENT
			#define SHADERPASS SHADERPASS_DEPTHONLY
			

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
			#define ASE_NEEDS_VERT_NORMAL
			#pragma shader_feature_local _USE_CUSTOM_ON


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
        
			CBUFFER_START(UnityPerMaterial)
			float4 _Main_Frescolor;
			float4 _Gap_Color;
			float4 _Mask_NoiseTex_ST;
			float4 _Fres_NoiseTex_ST;
			float4 _HexaTex_ST;
			float _Hexa_Upanner;
			float _Hexa_Vpanner;
			float _Hexa_BackOpacity;
			float _Hexa_ScrollSpeed;
			float _Hexa_Maskpow;
			float _Offset;
			float _Hexa_MaskMult;
			float _Hexa_VmaskPow;
			float _Hexa_Vmask_mult;
			float _Inside_Pow;
			float _Inside_Mult;
			float _Hexa_Wavecount;
			float _FresNoise2_Mult;
			float _FresNoise2_Vpanner;
			float _InsideMask_Pow;
			float _FresNoise_Mult;
			float _FresNoise_Pow;
			float _FresNoise_Vpanner;
			float _Fres_Power;
			float _Fres_Scale;
			float _isInside;
			float _Step_Gap;
			float _Step;
			float _Noise_Mult;
			float _Noise_Pow;
			float _Noise_Vpanner;
			float _Mask_Offset;
			float _FresNoise2_Pow;
			float _InsideMask_Mult;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			sampler2D _Fres_NoiseTex;
			uniform float4 _CameraDepthTexture_TexelSize;
			sampler2D _HexaTex;
			sampler2D _Mask_NoiseTex;


			
        
			float4 _SelectionID;

        
			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};
        
			VertexOutput VertexFunction(VertexInput v  )
			{
				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				o.ase_texcoord.xyz = ase_worldPos;
				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord1.xyz = ase_worldNormal;
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord3 = screenPos;
				
				o.ase_texcoord2 = v.ase_texcoord;
				o.ase_texcoord4 = v.vertex;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.w = 0;
				o.ase_texcoord1.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				o.clipPos = TransformWorldToHClip(positionWS);
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN , FRONT_FACE_TYPE ase_vface : FRONT_FACE_SEMANTIC) : SV_TARGET
			{
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float3 ase_worldPos = IN.ase_texcoord.xyz;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - ase_worldPos );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldNormal = IN.ase_texcoord1.xyz;
				float fresnelNdotV10 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode10 = ( 0.0 + _Fres_Scale * pow( 1.0 - fresnelNdotV10, _Fres_Power ) );
				float2 appendResult127 = (float2(0.0 , _FresNoise_Vpanner));
				float2 uv_Fres_NoiseTex = IN.ase_texcoord2.xy * _Fres_NoiseTex_ST.xy + _Fres_NoiseTex_ST.zw;
				float2 panner125 = ( 1.0 * _Time.y * appendResult127 + uv_Fres_NoiseTex);
				float temp_output_133_0 = ( pow( tex2D( _Fres_NoiseTex, panner125 ).r , _FresNoise_Pow ) * _FresNoise_Mult );
				float2 appendResult140 = (float2(0.0 , _FresNoise2_Vpanner));
				float2 texCoord139 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner141 = ( 1.0 * _Time.y * appendResult140 + texCoord139);
				float temp_output_143_0 = ( pow( tex2D( _Fres_NoiseTex, panner141 ).r , _FresNoise2_Pow ) * _FresNoise2_Mult );
				float4 screenPos = IN.ase_texcoord3;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float eyeDepth15 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm.xy ),_ZBufferParams);
				float2 appendResult183 = (float2(_Hexa_Upanner , _Hexa_Vpanner));
				float2 uv_HexaTex = IN.ase_texcoord2.xy * _HexaTex_ST.xy + _HexaTex_ST.zw;
				float2 panner188 = ( 1.0 * _Time.y * appendResult183 + uv_HexaTex);
				float4 tex2DNode192 = tex2D( _HexaTex, panner188 );
				float2 texCoord175 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float mulTime176 = _TimeParameters.x * _Hexa_ScrollSpeed;
				float temp_output_205_0 = saturate( ( ( tex2DNode192.r * _Hexa_BackOpacity ) + ( ( saturate( tex2DNode192.r ) * saturate( ( pow( abs( sin( ( ( texCoord175.y + mulTime176 ) * _Hexa_Wavecount ) ) ) , _Hexa_Maskpow ) * _Hexa_MaskMult ) ) ) * saturate( ( pow( ( 1.0 - texCoord175.y ) , _Hexa_VmaskPow ) * _Hexa_Vmask_mult ) ) ) ) );
				float2 texCoord162 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				#ifdef _USE_CUSTOM_ON
				float staticSwitch173 = IN.ase_texcoord2.z;
				#else
				float staticSwitch173 = _Mask_Offset;
				#endif
				float2 appendResult111 = (float2(0.0 , _Noise_Vpanner));
				float2 uv_Mask_NoiseTex = IN.ase_texcoord2.xy * _Mask_NoiseTex_ST.xy + _Mask_NoiseTex_ST.zw;
				float2 panner107 = ( 1.0 * _Time.y * appendResult111 + uv_Mask_NoiseTex);
				float temp_output_86_0 = saturate( ( ( IN.ase_texcoord4.xyz.y + staticSwitch173 ) + ( pow( tex2D( _Mask_NoiseTex, panner107 ).r , _Noise_Pow ) * _Noise_Mult ) ) );
				float temp_output_80_0 = ( 1.0 - step( temp_output_86_0 , _Step ) );
				float temp_output_93_0 = ( temp_output_80_0 - ( 1.0 - step( temp_output_86_0 , ( _Step + _Step_Gap ) ) ) );
				float switchResult112 = (((ase_vface>0)?(saturate( saturate( ( ( (( _isInside )?( saturate( ( saturate( ( pow( ( temp_output_133_0 + temp_output_143_0 ) , _Inside_Pow ) * _Inside_Mult * saturate( ( pow( ( 1.0 - texCoord162.y ) , _InsideMask_Pow ) * _InsideMask_Mult ) ) ) ) + temp_output_205_0 ) ) ):( saturate( ( saturate( ( saturate( ( fresnelNode10 * saturate( ( fresnelNode10 + temp_output_133_0 + temp_output_143_0 ) ) ) ) + saturate( ( 1.0 - ( eyeDepth15 + -screenPos.w + _Offset ) ) ) ) ) + temp_output_205_0 ) ) )) * saturate( temp_output_80_0 ) ) + temp_output_93_0 ) ) )):(saturate( saturate( temp_output_93_0 ) ))));
				
				surfaceDescription.Alpha = switchResult112;
				surfaceDescription.AlphaClipThreshold = 0.5;


				#if _ALPHATEST_ON
					float alphaClipThreshold = 0.01f;
					#if ALPHA_CLIP_THRESHOLD
						alphaClipThreshold = surfaceDescription.AlphaClipThreshold;
					#endif
					clip(surfaceDescription.Alpha - alphaClipThreshold);
				#endif

				half4 outColor = 0;
				outColor = _SelectionID;
				
				return outColor;
			}
        
			ENDHLSL
        }
		
		
        Pass
        {
			
            Name "DepthNormals"
            Tags { "LightMode"="DepthNormalsOnly" }

			ZTest LEqual
			ZWrite On

        
			HLSLPROGRAM
			
			#define _RECEIVE_SHADOWS_OFF 1
			#define ASE_SRP_VERSION 999999
			#define REQUIRE_DEPTH_TEXTURE 1

			
			#pragma only_renderers d3d11 glcore gles gles3 
			#pragma multi_compile_fog
			#pragma instancing_options renderinglayer
			#pragma vertex vert
			#pragma fragment frag

        
			#define ATTRIBUTES_NEED_NORMAL
			#define ATTRIBUTES_NEED_TANGENT
			#define VARYINGS_NEED_NORMAL_WS

			#define SHADERPASS SHADERPASS_DEPTHNORMALSONLY

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
			#pragma shader_feature_local _USE_CUSTOM_ON


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float3 normalWS : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
        
			CBUFFER_START(UnityPerMaterial)
			float4 _Main_Frescolor;
			float4 _Gap_Color;
			float4 _Mask_NoiseTex_ST;
			float4 _Fres_NoiseTex_ST;
			float4 _HexaTex_ST;
			float _Hexa_Upanner;
			float _Hexa_Vpanner;
			float _Hexa_BackOpacity;
			float _Hexa_ScrollSpeed;
			float _Hexa_Maskpow;
			float _Offset;
			float _Hexa_MaskMult;
			float _Hexa_VmaskPow;
			float _Hexa_Vmask_mult;
			float _Inside_Pow;
			float _Inside_Mult;
			float _Hexa_Wavecount;
			float _FresNoise2_Mult;
			float _FresNoise2_Vpanner;
			float _InsideMask_Pow;
			float _FresNoise_Mult;
			float _FresNoise_Pow;
			float _FresNoise_Vpanner;
			float _Fres_Power;
			float _Fres_Scale;
			float _isInside;
			float _Step_Gap;
			float _Step;
			float _Noise_Mult;
			float _Noise_Pow;
			float _Noise_Vpanner;
			float _Mask_Offset;
			float _FresNoise2_Pow;
			float _InsideMask_Mult;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _Fres_NoiseTex;
			uniform float4 _CameraDepthTexture_TexelSize;
			sampler2D _HexaTex;
			sampler2D _Mask_NoiseTex;


			      
			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};
        
			VertexOutput VertexFunction(VertexInput v  )
			{
				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				o.ase_texcoord1.xyz = ase_worldPos;
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord3 = screenPos;
				
				o.ase_texcoord2 = v.ase_texcoord;
				o.ase_texcoord4 = v.vertex;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float3 normalWS = TransformObjectToWorldNormal(v.ase_normal);

				o.clipPos = TransformWorldToHClip(positionWS);
				o.normalWS.xyz =  normalWS;

				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN , FRONT_FACE_TYPE ase_vface : FRONT_FACE_SEMANTIC) : SV_TARGET
			{
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float3 ase_worldPos = IN.ase_texcoord1.xyz;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - ase_worldPos );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float fresnelNdotV10 = dot( IN.normalWS, ase_worldViewDir );
				float fresnelNode10 = ( 0.0 + _Fres_Scale * pow( 1.0 - fresnelNdotV10, _Fres_Power ) );
				float2 appendResult127 = (float2(0.0 , _FresNoise_Vpanner));
				float2 uv_Fres_NoiseTex = IN.ase_texcoord2.xy * _Fres_NoiseTex_ST.xy + _Fres_NoiseTex_ST.zw;
				float2 panner125 = ( 1.0 * _Time.y * appendResult127 + uv_Fres_NoiseTex);
				float temp_output_133_0 = ( pow( tex2D( _Fres_NoiseTex, panner125 ).r , _FresNoise_Pow ) * _FresNoise_Mult );
				float2 appendResult140 = (float2(0.0 , _FresNoise2_Vpanner));
				float2 texCoord139 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner141 = ( 1.0 * _Time.y * appendResult140 + texCoord139);
				float temp_output_143_0 = ( pow( tex2D( _Fres_NoiseTex, panner141 ).r , _FresNoise2_Pow ) * _FresNoise2_Mult );
				float4 screenPos = IN.ase_texcoord3;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float eyeDepth15 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm.xy ),_ZBufferParams);
				float2 appendResult183 = (float2(_Hexa_Upanner , _Hexa_Vpanner));
				float2 uv_HexaTex = IN.ase_texcoord2.xy * _HexaTex_ST.xy + _HexaTex_ST.zw;
				float2 panner188 = ( 1.0 * _Time.y * appendResult183 + uv_HexaTex);
				float4 tex2DNode192 = tex2D( _HexaTex, panner188 );
				float2 texCoord175 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float mulTime176 = _TimeParameters.x * _Hexa_ScrollSpeed;
				float temp_output_205_0 = saturate( ( ( tex2DNode192.r * _Hexa_BackOpacity ) + ( ( saturate( tex2DNode192.r ) * saturate( ( pow( abs( sin( ( ( texCoord175.y + mulTime176 ) * _Hexa_Wavecount ) ) ) , _Hexa_Maskpow ) * _Hexa_MaskMult ) ) ) * saturate( ( pow( ( 1.0 - texCoord175.y ) , _Hexa_VmaskPow ) * _Hexa_Vmask_mult ) ) ) ) );
				float2 texCoord162 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				#ifdef _USE_CUSTOM_ON
				float staticSwitch173 = IN.ase_texcoord2.z;
				#else
				float staticSwitch173 = _Mask_Offset;
				#endif
				float2 appendResult111 = (float2(0.0 , _Noise_Vpanner));
				float2 uv_Mask_NoiseTex = IN.ase_texcoord2.xy * _Mask_NoiseTex_ST.xy + _Mask_NoiseTex_ST.zw;
				float2 panner107 = ( 1.0 * _Time.y * appendResult111 + uv_Mask_NoiseTex);
				float temp_output_86_0 = saturate( ( ( IN.ase_texcoord4.xyz.y + staticSwitch173 ) + ( pow( tex2D( _Mask_NoiseTex, panner107 ).r , _Noise_Pow ) * _Noise_Mult ) ) );
				float temp_output_80_0 = ( 1.0 - step( temp_output_86_0 , _Step ) );
				float temp_output_93_0 = ( temp_output_80_0 - ( 1.0 - step( temp_output_86_0 , ( _Step + _Step_Gap ) ) ) );
				float switchResult112 = (((ase_vface>0)?(saturate( saturate( ( ( (( _isInside )?( saturate( ( saturate( ( pow( ( temp_output_133_0 + temp_output_143_0 ) , _Inside_Pow ) * _Inside_Mult * saturate( ( pow( ( 1.0 - texCoord162.y ) , _InsideMask_Pow ) * _InsideMask_Mult ) ) ) ) + temp_output_205_0 ) ) ):( saturate( ( saturate( ( saturate( ( fresnelNode10 * saturate( ( fresnelNode10 + temp_output_133_0 + temp_output_143_0 ) ) ) ) + saturate( ( 1.0 - ( eyeDepth15 + -screenPos.w + _Offset ) ) ) ) ) + temp_output_205_0 ) ) )) * saturate( temp_output_80_0 ) ) + temp_output_93_0 ) ) )):(saturate( saturate( temp_output_93_0 ) ))));
				
				surfaceDescription.Alpha = switchResult112;
				surfaceDescription.AlphaClipThreshold = 0.5;

				#if _ALPHATEST_ON
					clip(surfaceDescription.Alpha - surfaceDescription.AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				float3 normalWS = IN.normalWS;
				return half4(NormalizeNormalPerPixel(normalWS), 0.0);

			}
        
			ENDHLSL
        }

		
        Pass
        {
			
            Name "DepthNormalsOnly"
            Tags { "LightMode"="DepthNormalsOnly" }
        
			ZTest LEqual
			ZWrite On
        
        
			HLSLPROGRAM
        
			#define _RECEIVE_SHADOWS_OFF 1
			#define ASE_SRP_VERSION 999999
			#define REQUIRE_DEPTH_TEXTURE 1

        
			#pragma exclude_renderers glcore gles gles3 
			#pragma vertex vert
			#pragma fragment frag
        
			#define ATTRIBUTES_NEED_NORMAL
			#define ATTRIBUTES_NEED_TANGENT
			#define ATTRIBUTES_NEED_TEXCOORD1
			#define VARYINGS_NEED_NORMAL_WS
			#define VARYINGS_NEED_TANGENT_WS
        
			#define SHADERPASS SHADERPASS_DEPTHNORMALSONLY
        
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
			#pragma shader_feature_local _USE_CUSTOM_ON


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float3 normalWS : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
        
			CBUFFER_START(UnityPerMaterial)
			float4 _Main_Frescolor;
			float4 _Gap_Color;
			float4 _Mask_NoiseTex_ST;
			float4 _Fres_NoiseTex_ST;
			float4 _HexaTex_ST;
			float _Hexa_Upanner;
			float _Hexa_Vpanner;
			float _Hexa_BackOpacity;
			float _Hexa_ScrollSpeed;
			float _Hexa_Maskpow;
			float _Offset;
			float _Hexa_MaskMult;
			float _Hexa_VmaskPow;
			float _Hexa_Vmask_mult;
			float _Inside_Pow;
			float _Inside_Mult;
			float _Hexa_Wavecount;
			float _FresNoise2_Mult;
			float _FresNoise2_Vpanner;
			float _InsideMask_Pow;
			float _FresNoise_Mult;
			float _FresNoise_Pow;
			float _FresNoise_Vpanner;
			float _Fres_Power;
			float _Fres_Scale;
			float _isInside;
			float _Step_Gap;
			float _Step;
			float _Noise_Mult;
			float _Noise_Pow;
			float _Noise_Vpanner;
			float _Mask_Offset;
			float _FresNoise2_Pow;
			float _InsideMask_Mult;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _Fres_NoiseTex;
			uniform float4 _CameraDepthTexture_TexelSize;
			sampler2D _HexaTex;
			sampler2D _Mask_NoiseTex;


			
			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};
      
			VertexOutput VertexFunction(VertexInput v  )
			{
				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				o.ase_texcoord1.xyz = ase_worldPos;
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord3 = screenPos;
				
				o.ase_texcoord2 = v.ase_texcoord;
				o.ase_texcoord4 = v.vertex;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float3 normalWS = TransformObjectToWorldNormal(v.ase_normal);

				o.clipPos = TransformWorldToHClip(positionWS);
				o.normalWS.xyz =  normalWS;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN , FRONT_FACE_TYPE ase_vface : FRONT_FACE_SEMANTIC) : SV_TARGET
			{
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float3 ase_worldPos = IN.ase_texcoord1.xyz;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - ase_worldPos );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float fresnelNdotV10 = dot( IN.normalWS, ase_worldViewDir );
				float fresnelNode10 = ( 0.0 + _Fres_Scale * pow( 1.0 - fresnelNdotV10, _Fres_Power ) );
				float2 appendResult127 = (float2(0.0 , _FresNoise_Vpanner));
				float2 uv_Fres_NoiseTex = IN.ase_texcoord2.xy * _Fres_NoiseTex_ST.xy + _Fres_NoiseTex_ST.zw;
				float2 panner125 = ( 1.0 * _Time.y * appendResult127 + uv_Fres_NoiseTex);
				float temp_output_133_0 = ( pow( tex2D( _Fres_NoiseTex, panner125 ).r , _FresNoise_Pow ) * _FresNoise_Mult );
				float2 appendResult140 = (float2(0.0 , _FresNoise2_Vpanner));
				float2 texCoord139 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner141 = ( 1.0 * _Time.y * appendResult140 + texCoord139);
				float temp_output_143_0 = ( pow( tex2D( _Fres_NoiseTex, panner141 ).r , _FresNoise2_Pow ) * _FresNoise2_Mult );
				float4 screenPos = IN.ase_texcoord3;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float eyeDepth15 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm.xy ),_ZBufferParams);
				float2 appendResult183 = (float2(_Hexa_Upanner , _Hexa_Vpanner));
				float2 uv_HexaTex = IN.ase_texcoord2.xy * _HexaTex_ST.xy + _HexaTex_ST.zw;
				float2 panner188 = ( 1.0 * _Time.y * appendResult183 + uv_HexaTex);
				float4 tex2DNode192 = tex2D( _HexaTex, panner188 );
				float2 texCoord175 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float mulTime176 = _TimeParameters.x * _Hexa_ScrollSpeed;
				float temp_output_205_0 = saturate( ( ( tex2DNode192.r * _Hexa_BackOpacity ) + ( ( saturate( tex2DNode192.r ) * saturate( ( pow( abs( sin( ( ( texCoord175.y + mulTime176 ) * _Hexa_Wavecount ) ) ) , _Hexa_Maskpow ) * _Hexa_MaskMult ) ) ) * saturate( ( pow( ( 1.0 - texCoord175.y ) , _Hexa_VmaskPow ) * _Hexa_Vmask_mult ) ) ) ) );
				float2 texCoord162 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				#ifdef _USE_CUSTOM_ON
				float staticSwitch173 = IN.ase_texcoord2.z;
				#else
				float staticSwitch173 = _Mask_Offset;
				#endif
				float2 appendResult111 = (float2(0.0 , _Noise_Vpanner));
				float2 uv_Mask_NoiseTex = IN.ase_texcoord2.xy * _Mask_NoiseTex_ST.xy + _Mask_NoiseTex_ST.zw;
				float2 panner107 = ( 1.0 * _Time.y * appendResult111 + uv_Mask_NoiseTex);
				float temp_output_86_0 = saturate( ( ( IN.ase_texcoord4.xyz.y + staticSwitch173 ) + ( pow( tex2D( _Mask_NoiseTex, panner107 ).r , _Noise_Pow ) * _Noise_Mult ) ) );
				float temp_output_80_0 = ( 1.0 - step( temp_output_86_0 , _Step ) );
				float temp_output_93_0 = ( temp_output_80_0 - ( 1.0 - step( temp_output_86_0 , ( _Step + _Step_Gap ) ) ) );
				float switchResult112 = (((ase_vface>0)?(saturate( saturate( ( ( (( _isInside )?( saturate( ( saturate( ( pow( ( temp_output_133_0 + temp_output_143_0 ) , _Inside_Pow ) * _Inside_Mult * saturate( ( pow( ( 1.0 - texCoord162.y ) , _InsideMask_Pow ) * _InsideMask_Mult ) ) ) ) + temp_output_205_0 ) ) ):( saturate( ( saturate( ( saturate( ( fresnelNode10 * saturate( ( fresnelNode10 + temp_output_133_0 + temp_output_143_0 ) ) ) ) + saturate( ( 1.0 - ( eyeDepth15 + -screenPos.w + _Offset ) ) ) ) ) + temp_output_205_0 ) ) )) * saturate( temp_output_80_0 ) ) + temp_output_93_0 ) ) )):(saturate( saturate( temp_output_93_0 ) ))));
				
				surfaceDescription.Alpha = switchResult112;
				surfaceDescription.AlphaClipThreshold = 0.5;
				
				#if _ALPHATEST_ON
					clip(surfaceDescription.Alpha - surfaceDescription.AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				float3 normalWS = IN.normalWS;
				return half4(NormalizeNormalPerPixel(normalWS), 0.0);

			}

			ENDHLSL
        }
		
	}
	
	CustomEditor "UnityEditor.ShaderGraphUnlitGUI"
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=18935
0;-2;1920;1021;1187.019;-1197.392;1.3;True;False
Node;AmplifyShaderEditor.CommentaryNode;88;-1607.248,-1487.358;Inherit;False;2574.743;1011.763;Fresnel;43;40;36;37;39;41;43;34;38;42;52;14;51;130;22;146;50;49;15;129;19;133;143;16;10;12;142;135;11;145;134;123;136;144;132;141;125;139;140;127;124;138;126;53;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;174;-885.5957,1985.634;Inherit;False;Property;_Hexa_ScrollSpeed;Hexa_ScrollSpeed;26;0;Create;True;0;0;0;False;0;False;0;-0.122;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;126;-1459.323,-893.9302;Inherit;False;Property;_FresNoise_Vpanner;FresNoise_Vpanner;13;0;Create;True;0;0;0;False;0;False;0;-0.185;-0.5;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;138;-1534.307,-630.5772;Inherit;False;Property;_FresNoise2_Vpanner;FresNoise2_Vpanner;14;0;Create;True;0;0;0;False;0;False;0;-0.306;-0.5;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;175;-797.1067,1756.447;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;176;-638.5945,1886.832;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;124;-1383.401,-1143.697;Inherit;False;0;123;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;127;-1164.323,-961.9303;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;140;-1258.887,-658.6119;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;139;-1353.385,-781.3431;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;178;-443.5946,1819.233;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;177;-449.0037,2062.505;Inherit;False;Property;_Hexa_Wavecount;Hexa_Wavecount;27;0;Create;True;0;0;0;False;0;False;0;7.3;0;30;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;125;-1121.323,-1125.931;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;141;-1128.307,-774.5771;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;179;-204.8035,1828.603;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;136;-931.3232,-801.9302;Inherit;True;Property;_Fres2;Fres2;12;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;123;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinOpNode;182;-43.6037,1824.704;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;180;-569.0665,1692.355;Inherit;False;Property;_Hexa_Upanner;Hexa_Upanner;34;0;Create;True;0;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;144;-908.5701,-603.5359;Inherit;False;Property;_FresNoise2_Pow;FresNoise2_Pow;17;0;Create;True;0;0;0;False;0;False;0;1;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;123;-917.6946,-1129.374;Inherit;True;Property;_Fres_NoiseTex;Fres_NoiseTex;12;0;Create;True;0;0;0;False;0;False;-1;None;442ceef19115ca64cb036e9b9e1924dc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;181;-547.0666,1756.355;Inherit;False;Property;_Hexa_Vpanner;Hexa_Vpanner;33;0;Create;True;0;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;132;-886.3234,-916.9302;Inherit;False;Property;_FresNoise_Pow;FresNoise_Pow;15;0;Create;True;0;0;0;False;0;False;0;1;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;147;-1533.791,479.2608;Inherit;False;2445.23;751.1824;Mask;25;108;111;62;107;57;81;82;92;71;91;90;80;94;60;93;56;84;54;55;85;73;86;78;172;173;;1,1,1,1;0;0
Node;AmplifyShaderEditor.PowerNode;134;-614.3234,-1038.931;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;135;-555.3234,-926.9303;Inherit;False;Property;_FresNoise_Mult;FresNoise_Mult;16;0;Create;True;0;0;0;False;0;False;0;5.46;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;108;-1483.791,969.6884;Inherit;False;Property;_Noise_Vpanner;Noise_Vpanner;11;0;Create;True;0;0;0;False;0;False;0;0.066;-0.5;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-872.4099,-1355.706;Inherit;False;Property;_Fres_Power;Fres_Power;1;0;Create;True;0;0;0;False;0;False;1;2.82;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;142;-611.6531,-744.9157;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;145;-580.6531,-634.9157;Inherit;False;Property;_FresNoise2_Mult;FresNoise2_Mult;18;0;Create;True;0;0;0;False;0;False;0;3.45;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;185;-113.9039,2102.104;Inherit;False;Property;_Hexa_Maskpow;Hexa_Maskpow;28;0;Create;True;0;0;0;False;0;False;7.71;3.13;1;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-869.0412,-1438.358;Inherit;False;Property;_Fres_Scale;Fres_Scale;0;0;Create;True;0;0;0;False;0;False;1;1.2;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;184;-439.2916,1555.239;Inherit;False;0;192;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;183;-250.0666,1696.355;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.AbsOpNode;186;-57.51056,2183.264;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;187;841.5297,1816.024;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;162;-544.8406,222.2045;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;143;-442.6531,-756.9161;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;111;-1171.791,904.6882;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;62;-1265.224,779.5551;Inherit;False;0;57;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;133;-445.3235,-1050.931;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;10;-566.3886,-1421.955;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;189;794.6416,2041.019;Inherit;False;Property;_Hexa_VmaskPow;Hexa_VmaskPow;31;0;Create;True;0;0;0;False;0;False;0;8.4;1;30;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;190;159.1955,1880.603;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;188;-196.5585,1565.019;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;191;163.1953,2169.204;Inherit;False;Property;_Hexa_MaskMult;Hexa_MaskMult;29;0;Create;True;0;0;0;False;0;False;0;0.32;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;16;-211.6713,-1009.477;Float;True;1;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;155;-112.0058,65.51614;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;49;110.4665,-894.3002;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;159;-252.0068,276.5161;Inherit;False;Property;_InsideMask_Pow;InsideMask_Pow;20;0;Create;True;0;0;0;False;0;False;0;3.19;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-198.9097,-780.2431;Inherit;False;Property;_Offset;Offset;3;0;Create;True;0;0;0;False;0;False;0;0.257;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;107;-1016.79,840.6877;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;129;-242.3233,-1255.931;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;194;1039.042,1836.919;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;193;1096.242,2043.619;Inherit;False;Property;_Hexa_Vmask_mult;Hexa_Vmask_mult;32;0;Create;True;0;0;0;False;0;False;0;10;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;195;404.284,1907.495;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;192;39.70812,1537.039;Inherit;True;Property;_HexaTex;HexaTex;25;0;Create;True;0;0;0;False;0;False;-1;None;e1c30023b4732624d81b17b8f683dab3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenDepthNode;15;56.52849,-985.5764;Inherit;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;160;51.99323,288.516;Inherit;False;Property;_InsideMask_Mult;InsideMask_Mult;21;0;Create;True;0;0;0;False;0;False;0;1.64;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;81;-750.7007,1052.576;Inherit;False;Property;_Noise_Pow;Noise_Pow;7;0;Create;True;0;0;0;False;0;False;0;1;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;146;-25.65305,-1270.417;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;156;112.9934,88.51612;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;57;-840.2852,812.2582;Inherit;True;Property;_Mask_NoiseTex;Mask_NoiseTex;5;0;Create;True;0;0;0;False;0;False;-1;None;f862f97ef17a2504c953a715421baf56;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;197;562.5026,1561.686;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;198;1200.242,1862.919;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;196;600.2336,1896.615;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;50;262.4665,-904.3002;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;172;-1437.26,513.5687;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;56;-1102.241,669.9534;Inherit;False;Property;_Mask_Offset;Mask_Offset;4;0;Create;True;0;0;0;False;0;False;0;-0.06;-13;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;54;-827.6486,529.2607;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;158;276.9935,89.51612;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;173;-1125.53,555.3945;Inherit;False;Property;_Use_Custom;Use_Custom;24;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;22;389.2584,-865.3641;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;163;-120.6326,-419.262;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;130;118.6767,-1306.931;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;84;-515.6241,774.4344;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;167;-83.1265,-129.871;Inherit;False;Property;_Inside_Pow;Inside_Pow;22;0;Create;True;0;0;0;False;0;False;1;1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;201;161.8576,1754.347;Inherit;False;Property;_Hexa_BackOpacity;Hexa_BackOpacity;30;0;Create;True;0;0;0;False;0;False;0.7;0.016;0;0.4;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;199;1361.985,1845.268;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;200;815.2046,1559.601;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-442.1032,892.6191;Inherit;False;Property;_Noise_Mult;Noise_Mult;8;0;Create;True;0;0;0;False;0;False;0;1;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;85;-370.8968,678.4781;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;55;-595.8226,553.9156;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;169;68.07339,-37.07097;Inherit;False;Property;_Inside_Mult;Inside_Mult;23;0;Create;True;0;0;0;False;0;False;0.1;0.33;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;166;142.4735,-310.6709;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;14;520.4866,-1021.754;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;51;539.3961,-898.1892;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;204;858.7616,1316.485;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;202;1141.213,1505.471;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;171;432.5106,54.00055;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;52;677.3964,-996.1885;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;168;411.7338,-241.871;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;73;-225.1375,594.9976;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;203;1099.097,1248.2;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;71;-156.3553,898.7952;Inherit;False;Property;_Step;Step;6;0;Create;True;0;0;0;False;0;False;0.15;0.07941176;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;92;-201.5605,1040.443;Inherit;False;Property;_Step_Gap;Step_Gap;9;0;Create;True;0;0;0;False;0;False;0;0.08235294;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;86;-23.69445,617.1736;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;53;800.7968,-994.3885;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;170;689.738,-194.0259;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;205;1355.359,1329.628;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;211;799.8027,-106.335;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;78;133.5767,609.5876;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;210;990.1367,-411.7062;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;91;120.4394,968.4434;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;212;895.2736,-20.25478;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;213;1081.255,-281.5729;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;90;251.4397,974.4434;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;80;371.5766,609.5876;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;149;1082.436,-3.622567;Inherit;False;Property;_isInside;is Inside;19;0;Create;True;0;0;0;False;0;False;1;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;60;662.2643,622.9445;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;94;461.4397,976.4434;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;965.0417,492.4459;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;93;676.4398,938.2433;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;105;1186.875,504.2141;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;122;1246.941,971.613;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;106;1406.771,439.9672;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;208;1503.972,1044.08;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;209;1703.972,467.0801;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;95;1272.113,762.6319;Inherit;False;Property;_Gap_Color;Gap_Color;10;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;3.477292,3.477292,15.8134,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;101;2092.309,608.8467;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SwitchByFaceNode;112;1853.835,480.3297;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;96;1684.633,902.6089;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;13;1708.359,651.3319;Inherit;False;Property;_Main_Frescolor;Main_Frescolor;2;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;2.017456,4.371155,12.84447,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;40;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;SceneSelectionPass;0;6;SceneSelectionPass;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=SceneSelectionPass;False;True;4;d3d11;glcore;gles;gles3;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;43;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthNormalsOnly;0;9;DepthNormalsOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=DepthNormalsOnly;False;True;15;d3d9;d3d11_9x;d3d11;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;34;-771.6044,-1355.861;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;0;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;37;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;39;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;Universal2D;0;5;Universal2D;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;2;5;False;-1;10;False;-1;3;1;False;-1;10;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;0;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=Universal2D;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;42;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthNormals;0;8;DepthNormals;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=DepthNormalsOnly;False;True;4;d3d11;glcore;gles;gles3;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;41;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ScenePickingPass;0;7;ScenePickingPass;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Picking;False;True;4;d3d11;glcore;gles;gles3;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;38;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;35;2289.87,509.0774;Float;False;True;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;3;CustomFX/K/Landmark_Shield;2992e84f91cbeb14eab234972e07ea9d;True;Forward;0;1;Forward;8;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;2;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;True;True;2;5;False;-1;10;False;-1;3;1;False;-1;10;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;True;0;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=UniversalForwardOnly;False;False;0;Hidden/InternalErrorShader;0;0;Standard;22;Surface;1;638008954140501348;  Blend;0;0;Two Sided;1;638009289882386704;Cast Shadows;0;638008955851648027;  Use Shadow Threshold;0;0;Receive Shadows;0;638008955858203263;GPU Instancing;0;638008955855647326;LOD CrossFade;0;0;Built-in Fog;0;0;DOTS Instancing;0;0;Meta Pass;0;0;Extra Pre Pass;0;0;Tessellation;0;0;  Phong;0;0;  Strength;0.5,False,-1;0;  Type;0;0;  Tess;16,False,-1;0;  Min;10,False,-1;0;  Max;25,False,-1;0;  Edge Length;16,False,-1;0;  Max Displacement;25,False,-1;0;Vertex Position,InvertActionOnDeselection;1;0;0;10;False;True;False;True;False;True;True;True;True;True;False;;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;36;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
WireConnection;176;0;174;0
WireConnection;127;1;126;0
WireConnection;140;1;138;0
WireConnection;178;0;175;2
WireConnection;178;1;176;0
WireConnection;125;0;124;0
WireConnection;125;2;127;0
WireConnection;141;0;139;0
WireConnection;141;2;140;0
WireConnection;179;0;178;0
WireConnection;179;1;177;0
WireConnection;136;1;141;0
WireConnection;182;0;179;0
WireConnection;123;1;125;0
WireConnection;134;0;123;1
WireConnection;134;1;132;0
WireConnection;142;0;136;1
WireConnection;142;1;144;0
WireConnection;183;0;180;0
WireConnection;183;1;181;0
WireConnection;186;0;182;0
WireConnection;187;0;175;2
WireConnection;143;0;142;0
WireConnection;143;1;145;0
WireConnection;111;1;108;0
WireConnection;133;0;134;0
WireConnection;133;1;135;0
WireConnection;10;2;12;0
WireConnection;10;3;11;0
WireConnection;190;0;186;0
WireConnection;190;1;185;0
WireConnection;188;0;184;0
WireConnection;188;2;183;0
WireConnection;155;0;162;2
WireConnection;49;0;16;4
WireConnection;107;0;62;0
WireConnection;107;2;111;0
WireConnection;129;0;10;0
WireConnection;129;1;133;0
WireConnection;129;2;143;0
WireConnection;194;0;187;0
WireConnection;194;1;189;0
WireConnection;195;0;190;0
WireConnection;195;1;191;0
WireConnection;192;1;188;0
WireConnection;146;0;129;0
WireConnection;156;0;155;0
WireConnection;156;1;159;0
WireConnection;57;1;107;0
WireConnection;197;0;192;1
WireConnection;198;0;194;0
WireConnection;198;1;193;0
WireConnection;196;0;195;0
WireConnection;50;0;15;0
WireConnection;50;1;49;0
WireConnection;50;2;19;0
WireConnection;158;0;156;0
WireConnection;158;1;160;0
WireConnection;173;1;56;0
WireConnection;173;0;172;3
WireConnection;22;0;50;0
WireConnection;163;0;133;0
WireConnection;163;1;143;0
WireConnection;130;0;10;0
WireConnection;130;1;146;0
WireConnection;84;0;57;1
WireConnection;84;1;81;0
WireConnection;199;0;198;0
WireConnection;200;0;197;0
WireConnection;200;1;196;0
WireConnection;85;0;84;0
WireConnection;85;1;82;0
WireConnection;55;0;54;2
WireConnection;55;1;173;0
WireConnection;166;0;163;0
WireConnection;166;1;167;0
WireConnection;14;0;130;0
WireConnection;51;0;22;0
WireConnection;204;0;192;1
WireConnection;204;1;201;0
WireConnection;202;0;200;0
WireConnection;202;1;199;0
WireConnection;171;0;158;0
WireConnection;52;0;14;0
WireConnection;52;1;51;0
WireConnection;168;0;166;0
WireConnection;168;1;169;0
WireConnection;168;2;171;0
WireConnection;73;0;55;0
WireConnection;73;1;85;0
WireConnection;203;0;204;0
WireConnection;203;1;202;0
WireConnection;86;0;73;0
WireConnection;53;0;52;0
WireConnection;170;0;168;0
WireConnection;205;0;203;0
WireConnection;211;0;170;0
WireConnection;211;1;205;0
WireConnection;78;0;86;0
WireConnection;78;1;71;0
WireConnection;210;0;53;0
WireConnection;210;1;205;0
WireConnection;91;0;71;0
WireConnection;91;1;92;0
WireConnection;212;0;211;0
WireConnection;213;0;210;0
WireConnection;90;0;86;0
WireConnection;90;1;91;0
WireConnection;80;0;78;0
WireConnection;149;0;213;0
WireConnection;149;1;212;0
WireConnection;60;0;80;0
WireConnection;94;0;90;0
WireConnection;68;0;149;0
WireConnection;68;1;60;0
WireConnection;93;0;80;0
WireConnection;93;1;94;0
WireConnection;105;0;68;0
WireConnection;105;1;93;0
WireConnection;122;0;93;0
WireConnection;106;0;105;0
WireConnection;208;0;122;0
WireConnection;209;0;106;0
WireConnection;101;0;13;0
WireConnection;101;1;96;0
WireConnection;112;0;209;0
WireConnection;112;1;208;0
WireConnection;96;0;95;0
WireConnection;96;1;93;0
WireConnection;35;2;101;0
WireConnection;35;3;112;0
ASEEND*/
//CHKSM=9ACBBB2BCCE89C2F8E0B5981454A564303E24089