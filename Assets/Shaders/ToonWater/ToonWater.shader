// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Toon/Water"
{
	Properties
	{
		_DepthMaxDistance("Depth Max Distance", Float) = 1
		_ShallowWaterColor("Shallow Water Color", Color) = (0.325,0.807,0.971,0.7254902)
		_DeepWaterColor("Deep Water Color", Color) = (0.086,0.407,1,0.7490196)
		_FoamColor("Foam Color", Color) = (1,1,1,0)
		_FoamMaxDistance("Foam Max Distance", Float) = 0.4
		_FoamMinDistance("Foam Min Distance", Float) = 0.2
		_FoamNoise("Foam Noise", 2D) = "white" {}
		_NoiseCutoff("Noise Cutoff", Float) = 0.777
		_FoamDistortionTexture("Foam Distortion Texture", 2D) = "white" {}
		_FoamDistortionAmount("Foam Distortion Amount", Range( 0 , 1)) = 0.27
		_FoamNoiseScroll("Foam Noise Scroll", Vector) = (0.03,0.03,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float4 screenPos;
			float2 uv_texcoord;
		};

		uniform float4 _ShallowWaterColor;
		uniform float4 _DeepWaterColor;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DepthMaxDistance;
		uniform float4 _FoamColor;
		uniform float _FoamMaxDistance;
		uniform float _FoamMinDistance;
		uniform sampler2D _CameraNormalsTexture;
		uniform float _NoiseCutoff;
		uniform sampler2D _FoamNoise;
		uniform float2 _FoamNoiseScroll;
		uniform sampler2D _FoamDistortionTexture;
		uniform float4 _FoamDistortionTexture_ST;
		uniform float _FoamDistortionAmount;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float eyeDepth15 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos ))));
			float temp_output_8_0 = ( eyeDepth15 - ase_screenPos.w );
			float4 lerpResult18 = lerp( _ShallowWaterColor , _DeepWaterColor , saturate( ( temp_output_8_0 / _DepthMaxDistance ) ));
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float4 tex2DNode50 = tex2D( _CameraNormalsTexture, ase_screenPosNorm.xy );
			float3 decodeViewNormalStereo53 = DecodeViewNormalStereo( tex2DNode50 );
			float dotResult55 = dot( float4( decodeViewNormalStereo53 , 0.0 ) , tex2DNode50 );
			float lerpResult59 = lerp( _FoamMaxDistance , _FoamMinDistance , saturate( dotResult55 ));
			float temp_output_29_0 = ( saturate( ( temp_output_8_0 / lerpResult59 ) ) * _NoiseCutoff );
			float4 temp_cast_3 = (( temp_output_29_0 - 0.01 )).xxxx;
			float4 temp_cast_4 = (( temp_output_29_0 + 0.01 )).xxxx;
			float2 panner31 = ( _Time.y * _FoamNoiseScroll + i.uv_texcoord);
			float2 uv_FoamDistortionTexture = i.uv_texcoord * _FoamDistortionTexture_ST.xy + _FoamDistortionTexture_ST.zw;
			float4 tex2DNode38 = tex2D( _FoamDistortionTexture, uv_FoamDistortionTexture );
			float2 appendResult67 = (float2(tex2DNode38.r , tex2DNode38.g));
			float4 smoothstepResult76 = smoothstep( temp_cast_3 , temp_cast_4 , tex2D( _FoamNoise, ( panner31 + ( (float2( -1,-1 ) + (appendResult67 - float2( 0,0 )) * (float2( 1,1 ) - float2( -1,-1 )) / (float2( 1,1 ) - float2( 0,0 ))) * _FoamDistortionAmount ).y ) ));
			float4 lerpResult69 = lerp( lerpResult18 , _FoamColor , smoothstepResult76);
			o.Emission = lerpResult69.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16600
479.2;3.2;868;835;2301.694;742.8371;3.785778;True;False
Node;AmplifyShaderEditor.ScreenPosInputsNode;60;-2283.61,736.0151;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;50;-2042.196,724.5908;Float;True;Global;_CameraNormalsTexture;_CameraNormalsTexture;9;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DecodeViewNormalStereoHlpNode;53;-1722.095,714.5593;Float;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;55;-1428.553,769.5988;Float;False;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;38;-1388.563,1090.573;Float;True;Property;_FoamDistortionTexture;Foam Distortion Texture;8;0;Create;True;0;0;False;0;c9d197b5867b3644287de7fe3b07921a;c9d197b5867b3644287de7fe3b07921a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;57;-1352.291,652.0594;Float;False;Property;_FoamMinDistance;Foam Min Distance;5;0;Create;True;0;0;False;0;0.2;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-1362.545,558.4915;Float;False;Property;_FoamMaxDistance;Foam Max Distance;4;0;Create;True;0;0;False;0;0.4;0.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;67;-1090.472,1117.731;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;6;-1326.077,339.9545;Float;False;1;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenDepthNode;15;-1349.77,249.468;Float;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;56;-1285.015,760.1213;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;43;-943.4131,1115.86;Float;False;5;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,1;False;3;FLOAT2;-1,-1;False;4;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-1006.129,1302.126;Float;False;Property;_FoamDistortionAmount;Foam Distortion Amount;10;0;Create;True;0;0;False;0;0.27;0.27;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;59;-1052.829,572.4916;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;8;-1056.338,322.8779;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;30;-799.157,832.3273;Float;False;Property;_FoamNoiseScroll;Foam Noise Scroll;11;0;Create;True;0;0;False;0;0.03,0.03;0.03,0.03;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TimeNode;35;-805.5624,959.0181;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;32;-796.0358,707.398;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;26;-883.165,500.1308;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-729.4578,1119.137;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-733.3998,592.0444;Float;False;Property;_NoiseCutoff;Noise Cutoff;7;0;Create;True;0;0;False;0;0.777;0.777;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;48;-556.2304,1006.652;Float;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;4;-812.7282,386.1684;Float;False;Property;_DepthMaxDistance;Depth Max Distance;0;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;28;-696.0646,504.6308;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;31;-505.5565,852.0757;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;40;-300.3101,903.0743;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;73;-469.9573,601.4857;Float;False;Constant;_SMOOTHSTEP_AA;SMOOTHSTEP_AA;12;0;Create;True;0;0;False;0;0.01;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;16;-565.0136,313.5282;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-507.8835,476.8073;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;21;-178.7442,878.2126;Float;True;Property;_FoamNoise;Foam Noise;6;0;Create;True;0;0;False;0;56e82a3daf689be48af98146254f88bf;56e82a3daf689be48af98146254f88bf;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;74;-106.272,481.4301;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;27;-408.4492,319.7826;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;17;-752.8459,118.6544;Float;False;Property;_DeepWaterColor;Deep Water Color;2;0;Create;True;0;0;False;0;0.086,0.407,1,0.7490196;0.086,0.407,1,0.7490196;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;75;-99.97072,593.2784;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;2;-754.7977,-105.6778;Float;False;Property;_ShallowWaterColor;Shallow Water Color;1;0;Create;True;0;0;False;0;0.325,0.807,0.971,0.7254902;0.325,0.807,0.971,0.7254902;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;76;161.5373,655.7094;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;61;118.3081,404.4646;Float;False;Property;_FoamColor;Foam Color;3;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;18;-178.8847,253.0125;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;69;482.3768,404.583;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;672.4348,356.8459;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Toon/Water;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Translucent;0.5;True;True;0;True;Opaque;;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0.63;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;4;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;50;1;60;0
WireConnection;53;0;50;0
WireConnection;55;0;53;0
WireConnection;55;1;50;0
WireConnection;67;0;38;1
WireConnection;67;1;38;2
WireConnection;56;0;55;0
WireConnection;43;0;67;0
WireConnection;59;0;58;0
WireConnection;59;1;57;0
WireConnection;59;2;56;0
WireConnection;8;0;15;0
WireConnection;8;1;6;4
WireConnection;26;0;8;0
WireConnection;26;1;59;0
WireConnection;41;0;43;0
WireConnection;41;1;39;0
WireConnection;48;0;41;0
WireConnection;28;0;26;0
WireConnection;31;0;32;0
WireConnection;31;2;30;0
WireConnection;31;1;35;2
WireConnection;40;0;31;0
WireConnection;40;1;48;1
WireConnection;16;0;8;0
WireConnection;16;1;4;0
WireConnection;29;0;28;0
WireConnection;29;1;24;0
WireConnection;21;1;40;0
WireConnection;74;0;29;0
WireConnection;74;1;73;0
WireConnection;27;0;16;0
WireConnection;75;0;29;0
WireConnection;75;1;73;0
WireConnection;76;0;21;0
WireConnection;76;1;74;0
WireConnection;76;2;75;0
WireConnection;18;0;2;0
WireConnection;18;1;17;0
WireConnection;18;2;27;0
WireConnection;69;0;18;0
WireConnection;69;1;61;0
WireConnection;69;2;76;0
WireConnection;0;2;69;0
ASEEND*/
//CHKSM=377AC83F8532ACE80CFD7A21EA6C784DFBF74FE9