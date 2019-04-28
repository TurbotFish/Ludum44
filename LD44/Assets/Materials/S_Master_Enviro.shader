// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Master_Enviro"
{
	Properties
	{
		_Color("Color", Color) = (0,0,0,0)
		_Smoothness("Smoothness", Float) = 0
		_Metallic("Metallic", Float) = 0
		_Gradient_Factor("Gradient_Factor", Float) = 0.5
		[Toggle]_Wind("Wind", Float) = 1
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma exclude_renderers xbox360 xboxone ps4 psp2 n3ds wiiu 
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _Wind;
		uniform sampler2D _TextureSample0;
		uniform float4 _Color;
		uniform float _Gradient_Factor;
		uniform float _Metallic;
		uniform float _Smoothness;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 panner24 = ( _Time.y * float2( 0,-0.2 ) + ( v.texcoord.xy * float2( 0.5,-0.5 ) ));
			v.vertex.xyz += lerp(float3(0,0,0),( ( ( sin( ( _Time.y * 2.0 ) ) * ( tex2Dlod( _TextureSample0, float4( panner24, 0, 0.0) ).r * v.texcoord.xy.y ) ) * float3(1,0,0) ) * 0.5 ),_Wind);
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 lerpResult14 = lerp( ( _Color * _Gradient_Factor ) , _Color , i.uv_texcoord.y);
			o.Albedo = lerpResult14.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16600
38;105;1906;690;3118.446;827.4111;2.937557;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-707.9022,-159.5451;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-386.6255,452.0032;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.5,-0.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;32;-365.595,264.0096;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;24;-249.7478,450.2712;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.2;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;23;-67.8222,419.0839;Float;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;False;0;None;e28dc97a9541e3642a48c0e3886688c5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-171.4178,258.7509;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;45;-13.35254,261.9045;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;257.9114,419.0837;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;312.1316,260.5352;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;20;219.3279,529.2145;Float;False;Constant;_Vector0;Vector 0;5;0;Create;True;0;0;False;0;1,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ColorNode;4;-226.7278,-136;Float;False;Property;_Color;Color;0;0;Create;True;0;0;False;0;0,0,0,0;0.8191704,0.9245283,0.4055713,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;12;-177.4307,35.65174;Float;False;Property;_Gradient_Factor;Gradient_Factor;3;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;420.7783,523.0414;Float;False;Constant;_Float0;Float 0;7;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;429.1523,414.3975;Float;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;110.7216,-109.2998;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;588.843,422.549;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector3Node;22;523.8125,240.1894;Float;False;Constant;_Vector1;Vector 1;6;0;Create;True;0;0;False;0;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ToggleSwitchNode;21;780.5641,249.6641;Float;False;Property;_Wind;Wind;4;0;Create;True;0;0;False;0;1;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;14;291.875,-120.6141;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;5;261,57;Float;False;Property;_Smoothness;Smoothness;1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;263,145;Float;False;Property;_Metallic;Metallic;2;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1120.495,-152.8435;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Custom/Master_Enviro;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;False;False;False;False;False;False;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;25;0;9;0
WireConnection;24;0;25;0
WireConnection;24;1;32;0
WireConnection;23;1;24;0
WireConnection;47;0;32;0
WireConnection;45;0;47;0
WireConnection;26;0;23;1
WireConnection;26;1;9;2
WireConnection;46;0;45;0
WireConnection;46;1;26;0
WireConnection;19;0;46;0
WireConnection;19;1;20;0
WireConnection;10;0;4;0
WireConnection;10;1;12;0
WireConnection;27;0;19;0
WireConnection;27;1;28;0
WireConnection;21;0;22;0
WireConnection;21;1;27;0
WireConnection;14;0;10;0
WireConnection;14;1;4;0
WireConnection;14;2;9;2
WireConnection;0;0;14;0
WireConnection;0;3;6;0
WireConnection;0;4;5;0
WireConnection;0;11;21;0
ASEEND*/
//CHKSM=72F3348E3505B4B8818D7A079AFB9432D70EC325