// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Master"
{
	Properties
	{
		_Diffuse("Diffuse", 2D) = "white" {}
		_Color("Color", Color) = (0,0,0,0)
		_Smoothness("Smoothness", Float) = 0
		_Metallic("Metallic", Float) = 0
		_Gradient_Factor("Gradient_Factor", Float) = 0.5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma exclude_renderers xbox360 xboxone ps4 psp2 n3ds wiiu 
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Diffuse;
		uniform float4 _Diffuse_ST;
		uniform float _Gradient_Factor;
		uniform float4 _Color;
		uniform float _Metallic;
		uniform float _Smoothness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Diffuse = i.uv_texcoord * _Diffuse_ST.xy + _Diffuse_ST.zw;
			float4 tex2DNode1 = tex2D( _Diffuse, uv_Diffuse );
			float4 lerpResult14 = lerp( ( tex2DNode1 * _Gradient_Factor ) , tex2DNode1 , i.uv_texcoord.y);
			float4 temp_output_2_0 = ( lerpResult14 * _Color );
			o.Albedo = temp_output_2_0.rgb;
			o.Emission = ( temp_output_2_0 * 0.25 ).rgb;
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
302;551;1906;1011;1550.152;-112.2274;2.01416;True;True
Node;AmplifyShaderEditor.SamplerNode;1;-485.8466,9.346548;Float;True;Property;_Diffuse;Diffuse;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;12;-402.6721,219.3101;Float;False;Property;_Gradient_Factor;Gradient_Factor;4;0;Create;True;0;0;False;0;0.5;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-428.1879,280.5476;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-135.3113,50.10192;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;4;-50,-136;Float;False;Property;_Color;Color;1;0;Create;True;0;0;False;0;0,0,0,0;1,0.1179245,0.1179245,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;14;38.91156,37.05486;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;248,-50;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;8;245,-148;Float;False;Constant;_Emission_Factor;Emission_Factor;4;0;Create;True;0;0;False;0;0.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;261,57;Float;False;Property;_Smoothness;Smoothness;2;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;287,133;Float;False;Property;_Metallic;Metallic;3;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;414,-49;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.07843138;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;683,-52;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Custom/Master;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;False;False;False;False;False;False;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;10;0;1;0
WireConnection;10;1;12;0
WireConnection;14;0;10;0
WireConnection;14;1;1;0
WireConnection;14;2;9;2
WireConnection;2;0;14;0
WireConnection;2;1;4;0
WireConnection;7;0;2;0
WireConnection;7;1;8;0
WireConnection;0;0;2;0
WireConnection;0;2;7;0
WireConnection;0;3;6;0
WireConnection;0;4;5;0
ASEEND*/
//CHKSM=5B1C5A74DA82E98F0816390FAD39B4FFD57AC6D8