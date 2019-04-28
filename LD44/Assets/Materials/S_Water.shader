// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader " s"
{
	Properties
	{
		_FoamDepth("Foam Depth", Float) = 0
		_Color("Color", Color) = (0.1254902,0.2352941,0.3294118,1)
		_FoamFalloff("Foam Falloff", Float) = 0
		_Texture0("Texture 0", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma exclude_renderers xbox360 xboxone ps4 psp2 n3ds wiiu 
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform float4 _Color;
		uniform sampler2D _Texture0;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _FoamDepth;
		uniform float _FoamFalloff;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( ( sin( _Time.y ) * ase_vertexNormal ) * float3( 0.01,0.01,0.01 ) );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord28 = i.uv_texcoord * float2( 2,2 );
			float2 panner53 = ( 2.0 * _Time.y * float2( 0.02,0.02 ) + ( uv_TexCoord28 * float2( 2,2 ) ));
			float4 lerpResult27 = lerp( _Color , ( _Color * 1.5 ) , tex2D( _Texture0, ( uv_TexCoord28 + ( tex2D( _Texture0, panner53 ).g * 0.2 ) ) ).r);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float eyeDepth46 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos ))));
			float4 lerpResult31 = lerp( lerpResult27 , float4(0.7685564,0.8716248,0.9528302,1) , saturate( pow( ( abs( ( eyeDepth46 - ase_screenPos.w ) ) + _FoamDepth ) , _FoamFalloff ) ));
			o.Albedo = lerpResult31.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16600
131;528;1906;999;328.2819;-73.32665;1.297679;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;28;-464.9338,193.1351;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;2,2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-375.03,-89.80332;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;2,2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;51;-264.7467,-282.4105;Float;True;Property;_Texture0;Texture 0;3;0;Create;True;0;0;False;0;None;20ff95245891dca4d84df8b8f9305165;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.PannerNode;53;-232.1279,-92.90987;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.02,0.02;False;1;FLOAT;2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;45;-501.6519,455.7067;Float;False;1;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenDepthNode;46;-288.5251,455.7067;Float;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;52;2.418269,-282.4105;Float;True;Property;_TextureSample1;Texture Sample 1;3;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;153.0866,-83.59019;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;47;-72.75869,452.4485;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;71.64263,536.2787;Float;False;Property;_FoamDepth;Foam Depth;0;0;Create;True;0;0;False;0;0;0.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;48;127.0078,452.4485;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;54;353.4602,188.2345;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;39;263.6424,584.2786;Float;False;Property;_FoamFalloff;Foam Falloff;2;0;Create;True;0;0;False;0;0;-116.88;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;58;599.9974,54.25591;Float;False;Constant;_Float0;Float 0;3;0;Create;True;0;0;False;0;1.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;40;263.6424,456.2794;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;26;505.7504,165.2302;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;None;20ff95245891dca4d84df8b8f9305165;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;24;572.1252,-188.5217;Float;False;Property;_Color;Color;1;0;Create;True;0;0;False;0;0.1254902,0.2352941,0.3294118,1;0.2663314,0.4124723,0.5377358,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;64;648.8701,947.962;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;765.0804,-18.35232;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.8;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;49;772.3799,193.9805;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;41;439.6424,472.2793;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;60;792.7714,561.6865;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinOpNode;62;781.5358,826.7638;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;27;907.5559,-45.63458;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;42;591.5538,466.7277;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;32;574.676,-352.2191;Float;False;Constant;_Color1;Color 1;0;0;Create;True;0;0;False;0;0.7685564,0.8716248,0.9528302,1;0,0,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;1031.533,370.6773;Float;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;31;1089.395,-46.51933;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;1201.154,370.538;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0.01,0.01,0.01;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1369.416,-46.63724;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard; s;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Translucent;0.5;True;True;0;False;Opaque;;Transparent;All;True;True;True;True;True;True;True;False;False;False;False;False;False;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;56;0;28;0
WireConnection;53;0;56;0
WireConnection;46;0;45;0
WireConnection;52;0;51;0
WireConnection;52;1;53;0
WireConnection;55;0;52;2
WireConnection;47;0;46;0
WireConnection;47;1;45;4
WireConnection;48;0;47;0
WireConnection;54;0;28;0
WireConnection;54;1;55;0
WireConnection;40;0;48;0
WireConnection;40;1;38;0
WireConnection;26;0;51;0
WireConnection;26;1;54;0
WireConnection;57;0;24;0
WireConnection;57;1;58;0
WireConnection;49;0;26;1
WireConnection;41;0;40;0
WireConnection;41;1;39;0
WireConnection;62;0;64;0
WireConnection;27;0;24;0
WireConnection;27;1;57;0
WireConnection;27;2;49;0
WireConnection;42;0;41;0
WireConnection;59;0;62;0
WireConnection;59;1;60;0
WireConnection;31;0;27;0
WireConnection;31;1;32;0
WireConnection;31;2;42;0
WireConnection;61;0;59;0
WireConnection;0;0;31;0
WireConnection;0;11;61;0
ASEEND*/
//CHKSM=AD056CED0570486AB5E4C4803EC192CE788E1C1C