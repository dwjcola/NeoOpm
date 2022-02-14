// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effect/EXMT_Common_AH_xin"
{
	Properties
	{
		_Main_tex("Main_tex", 2D) = "white" {}
		_mask_text("mask_text", 2D) = "white" {}
		_main_mask_text("main_mask_text", 2D) = "white" {}
		_distort_TEX("distort_TEX", 2D) = "white" {}
		_dissolve_TEX("dissolve_TEX", 2D) = "white" {}
		_flow_map("flow_map", 2D) = "white" {}
		_vertex_tex("vertex_tex", 2D) = "white" {}
		_Main_angle("Main_angle", Float) = 12.72
		_mask_angle("mask_angle", Float) = 12.72
		[HDR]_Main_Color("Main_Color", Color) = (1,1,1,1)
		[HDR]_edge_color("edge_color", Color) = (1,1,1,1)
		_Main_U("Main_U", Float) = 0
		_Main_V("Main_V", Float) = 0
		_mask_U("mask_U", Float) = 0
		_mask_V("mask_V", Float) = 0
		_Masksuofang1("Masksuofang", Float) = 1
		_Main_mask_U("Main_mask_U", Float) = 0
		_Main_mask_V("Main_mask_V", Float) = 0
		_dis_U("dis_U", Float) = 0
		_dis_V("dis_V", Float) = 0
		_dissolve_U("dissolve_U", Float) = 0
		_dissolve_V("dissolve_V", Float) = 0
		_vertex_U("vertex_U", Float) = 0
		_vertex_V("vertex_V", Float) = 0
		_dis_MU("dis_MU", Float) = 0
		_dissolve_MU("dissolve_MU", Range( -0.1 , 1)) = 0.55
		_soft_hard_MU("soft_hard_MU", Range( 0 , 0.5)) = 0.5
		_edge_mu("edge_mu", Float) = -0.07
		_power_M("power_M", Float) = 1
		_beizeng_M("beizeng_M", Float) = 1
		_beizeng_M_2("beizeng_M_2", Float) = 1
		_hunhe_M("hunhe_M", Float) = 0.5
		[Toggle(_PARTICAL_UV_CUS1_XY_ON)] _partical_UV_cus1_xy("partical_UV_cus1_xy", Float) = 0
		[Toggle(_PARTICAL_DISSOLVE_CUS1_W_ON)] _partical_dissolve_cus1_w("partical_dissolve_cus1_w", Float) = 0
		[Toggle(_PARTICAL_DISTORT_CUS1_Z_ON)] _partical_distort_cus1_z("partical_distort_cus1_z", Float) = 0
		[Toggle(_PARTICAL_VERTEX_CUS2_X_ON)] _partical_vertex_cus2_x("partical_vertex_cus2_x", Float) = 0
		[Toggle(_PARTICAL_FL_SF_CUS2_Y_ON)] _partical_FL_SF_cus2_y("partical_FL_SF_cus2_y", Float) = 0
		_FL_MU("FL_MU", Float) = 1
		_FL_SF("FL_SF", Float) = 0
		_vertex_MU("vertex_MU", Float) = 0
		[HideInInspector] _texcoord3( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord2( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _PARTICAL_VERTEX_CUS2_X_ON
		#pragma shader_feature_local _PARTICAL_UV_CUS1_XY_ON
		#pragma shader_feature_local _PARTICAL_DISTORT_CUS1_Z_ON
		#pragma shader_feature_local _PARTICAL_FL_SF_CUS2_Y_ON
		#pragma shader_feature_local _PARTICAL_DISSOLVE_CUS1_W_ON
		#pragma surface surf Standard alpha:fade keepalpha noshadow vertex:vertexDataFunc 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float2 uv_texcoord;
			float4 uv2_tex4coord2;
			float2 uv3_texcoord3;
			float4 vertexColor : COLOR;
		};

		uniform float _vertex_MU;
		uniform sampler2D _vertex_tex;
		uniform float _vertex_U;
		uniform float _vertex_V;
		uniform float4 _vertex_tex_ST;
		uniform sampler2D _Main_tex;
		uniform float _Main_U;
		uniform float _Main_V;
		uniform float4 _Main_tex_ST;
		uniform sampler2D _distort_TEX;
		uniform float _dis_U;
		uniform float _dis_V;
		uniform float4 _distort_TEX_ST;
		uniform float _dis_MU;
		uniform float _Main_angle;
		uniform sampler2D _flow_map;
		uniform float4 _flow_map_ST;
		uniform float _FL_MU;
		uniform float _FL_SF;
		uniform float _power_M;
		uniform float _beizeng_M;
		uniform float _beizeng_M_2;
		uniform float _hunhe_M;
		uniform sampler2D _mask_text;
		uniform float _mask_U;
		uniform float _mask_V;
		uniform float4 _mask_text_ST;
		uniform float _mask_angle;
		uniform sampler2D _main_mask_text;
		uniform float _Main_mask_U;
		uniform float _Main_mask_V;
		uniform float4 _main_mask_text_ST;
		uniform float _Masksuofang1;
		uniform float4 _Main_Color;
		uniform float _soft_hard_MU;
		uniform sampler2D _dissolve_TEX;
		uniform float _dissolve_U;
		uniform float _dissolve_V;
		uniform float4 _dissolve_TEX_ST;
		uniform float _dissolve_MU;
		uniform float _edge_mu;
		uniform float4 _edge_color;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			#ifdef _PARTICAL_VERTEX_CUS2_X_ON
				float staticSwitch131 = v.texcoord2.xy.x;
			#else
				float staticSwitch131 = _vertex_MU;
			#endif
			float2 appendResult4_g75 = (float2(_vertex_U , _vertex_V));
			float2 uv0_vertex_tex = v.texcoord.xy * _vertex_tex_ST.xy + _vertex_tex_ST.zw;
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( staticSwitch131 * ( tex2Dlod( _vertex_tex, float4( ( ( appendResult4_g75 * _Time.y ) + uv0_vertex_tex ), 0, 0.0) ) * float4( ase_vertexNormal , 0.0 ) ) * v.color.a ).xyz;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 appendResult4_g63 = (float2(_Main_U , _Main_V));
			float2 uv0_Main_tex = i.uv_texcoord * _Main_tex_ST.xy + _Main_tex_ST.zw;
			float2 appendResult80 = (float2(i.uv2_tex4coord2.x , i.uv2_tex4coord2.y));
			#ifdef _PARTICAL_UV_CUS1_XY_ON
				float2 staticSwitch78 = ( ( appendResult80 + uv0_Main_tex ) * 1.0 );
			#else
				float2 staticSwitch78 = ( ( appendResult4_g63 * _Time.y ) + uv0_Main_tex );
			#endif
			float2 appendResult4_g64 = (float2(_dis_U , _dis_V));
			float2 uv0_distort_TEX = i.uv_texcoord * _distort_TEX_ST.xy + _distort_TEX_ST.zw;
			float3 temp_cast_1 = (tex2D( _distort_TEX, ( ( appendResult4_g64 * _Time.y ) + uv0_distort_TEX ) ).r).xxx;
			float3 desaturateInitialColor2_g66 = temp_cast_1;
			float desaturateDot2_g66 = dot( desaturateInitialColor2_g66, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar2_g66 = lerp( desaturateInitialColor2_g66, desaturateDot2_g66.xxx, 0.0 );
			#ifdef _PARTICAL_DISTORT_CUS1_Z_ON
				float staticSwitch88 = i.uv2_tex4coord2.z;
			#else
				float staticSwitch88 = _dis_MU;
			#endif
			float3 lerpResult3_g66 = lerp( float3( staticSwitch78 ,  0.0 ) , desaturateVar2_g66 , staticSwitch88);
			float3 temp_output_25_0 = lerpResult3_g66;
			float cos65 = cos( _Main_angle );
			float sin65 = sin( _Main_angle );
			float2 rotator65 = mul( temp_output_25_0.xy - float2( 0.5,0.5 ) , float2x2( cos65 , -sin65 , sin65 , cos65 )) + float2( 0.5,0.5 );
			float2 uv_flow_map = i.uv_texcoord * _flow_map_ST.xy + _flow_map_ST.zw;
			float4 tex2DNode117 = tex2D( _flow_map, uv_flow_map );
			float2 temp_cast_3 = (_FL_MU).xx;
			#ifdef _PARTICAL_FL_SF_CUS2_Y_ON
				float staticSwitch133 = i.uv3_texcoord3.y;
			#else
				float staticSwitch133 = _FL_SF;
			#endif
			float2 lerpResult121 = lerp( rotator65 , pow( (tex2DNode117).rg , temp_cast_3 ) , staticSwitch133);
			float4 temp_output_9_0_g73 = tex2D( _Main_tex, lerpResult121 );
			float4 temp_cast_5 = (_power_M).xxxx;
			float4 lerpResult6_g73 = lerp( ( pow( temp_output_9_0_g73 , temp_cast_5 ) * _beizeng_M ) , ( temp_output_9_0_g73 * _beizeng_M_2 ) , _hunhe_M);
			float2 appendResult4_g67 = (float2(_mask_U , _mask_V));
			float2 uv0_mask_text = i.uv_texcoord * _mask_text_ST.xy + _mask_text_ST.zw;
			float cos114 = cos( _mask_angle );
			float sin114 = sin( _mask_angle );
			float2 rotator114 = mul( ( ( appendResult4_g67 * _Time.y ) + uv0_mask_text ) - float2( 0.5,0.5 ) , float2x2( cos114 , -sin114 , sin114 , cos114 )) + float2( 0.5,0.5 );
			float2 appendResult4_g70 = (float2(_Main_mask_U , _Main_mask_V));
			float2 uv0_main_mask_text = i.uv_texcoord * _main_mask_text_ST.xy + _main_mask_text_ST.zw;
			float4 break2_g77 = ( ( lerpResult6_g73 * tex2D( _mask_text, rotator114 ).a ) * tex2D( _main_mask_text, ( ( appendResult4_g70 * _Time.y ) + ( uv0_main_mask_text + ( ( uv0_main_mask_text - float2( 0.5,0.5 ) ) * _Masksuofang1 ) ) ) ).a );
			float4 appendResult4_g77 = (float4(break2_g77.x , break2_g77.y , break2_g77.z , 0.0));
			float4 appendResult11_g77 = (float4(i.vertexColor.r , i.vertexColor.g , i.vertexColor.b , 0.0));
			float temp_output_10_0_g74 = _soft_hard_MU;
			float2 appendResult4_g65 = (float2(_dissolve_U , _dissolve_V));
			float2 uv0_dissolve_TEX = i.uv_texcoord * _dissolve_TEX_ST.xy + _dissolve_TEX_ST.zw;
			float2 temp_cast_8 = (_FL_MU).xx;
			float3 lerpResult127 = lerp( ( ( temp_output_25_0 + float3( ( ( appendResult4_g65 * _Time.y ) + uv0_dissolve_TEX ) ,  0.0 ) ) * 0.5 ) , float3( pow( (tex2DNode117).rg , temp_cast_8 ) ,  0.0 ) , staticSwitch133);
			float4 tex2DNode30 = tex2D( _dissolve_TEX, lerpResult127.xy );
			#ifdef _PARTICAL_DISSOLVE_CUS1_W_ON
				float staticSwitch89 = i.uv2_tex4coord2.w;
			#else
				float staticSwitch89 = _dissolve_MU;
			#endif
			float clampResult8_g74 = clamp( ( tex2DNode30.r + 1.0 + ( staticSwitch89 * -2.0 ) ) , 0.0 , 1.0 );
			float smoothstepResult9_g74 = smoothstep( ( 1.0 - temp_output_10_0_g74 ) , temp_output_10_0_g74 , clampResult8_g74);
			float temp_output_3_0_g71 = tex2DNode30.r;
			float temp_output_6_0_g71 = staticSwitch89;
			float4 temp_output_37_0 = ( smoothstepResult9_g74 + ( ( step( temp_output_3_0_g71 , temp_output_6_0_g71 ) - step( ( _edge_mu + temp_output_3_0_g71 ) , temp_output_6_0_g71 ) ) * _edge_color ) );
			o.Emission = ( ( appendResult4_g77 * appendResult11_g77 * _Main_Color ) * temp_output_37_0 ).xyz;
			float3 desaturateInitialColor129 = temp_output_37_0.rgb;
			float desaturateDot129 = dot( desaturateInitialColor129, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar129 = lerp( desaturateInitialColor129, desaturateDot129.xxx, 0.0 );
			o.Alpha = ( ( break2_g77.w * i.vertexColor.a * _Main_Color.a ) * desaturateVar129 ).x;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17500
255;171;1920;691;1774.908;1431.931;1.39599;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;79;-1770.349,-459.1182;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;86;-1769.458,-84.73914;Inherit;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;80;-1430.491,-422.7841;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;85;-2463.627,-143.4908;Inherit;False;0;19;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;20;-1546.689,108.6185;Inherit;False;Property;_Main_U;Main_U;14;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-1564.747,205.3845;Inherit;False;Property;_Main_V;Main_V;15;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-1409.669,-275.6861;Inherit;False;Constant;_Float0;Float 0;22;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;87;-1280.395,-405.834;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-2197.92,-78.91058;Inherit;False;Property;_dis_V;dis_V;22;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-2146.149,-198.6013;Inherit;False;Property;_dis_U;dis_U;21;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;84;-1341.123,77.9743;Inherit;False;EXMT_UV_panner;-1;;63;18c96d2969f0a90438f43aa405811725;0;3;1;FLOAT;0;False;11;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;-1112.911,-291.9524;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;83;-1942.449,-171.7169;Inherit;False;EXMT_UV_panner;-1;;64;18c96d2969f0a90438f43aa405811725;0;3;1;FLOAT;0;False;11;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-1238.125,194.5348;Inherit;False;Property;_dis_MU;dis_MU;27;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;78;-1030.629,-193.0968;Inherit;False;Property;_partical_UV_cus1_xy;partical_UV_cus1_xy;35;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;91;-1724.448,869.333;Inherit;False;Property;_dissolve_V;dissolve_V;24;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;90;-1763.111,564.0601;Inherit;False;Property;_dissolve_U;dissolve_U;23;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;19;-1409.697,-188.6763;Inherit;True;Property;_distort_TEX;distort_TEX;3;0;Create;True;0;0;False;0;-1;None;cd460ee4ac5c1e746b7a734cc7cc64dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;92;-1707.018,687.2321;Inherit;False;0;30;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;88;-1174.77,237.9722;Inherit;False;Property;_partical_distort_cus1_z;partical_distort_cus1_z;37;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;25;-974.6308,44.14669;Inherit;False;EXMT_distort;-1;;66;70e18f33b5562434b94b267a099eb1a0;0;3;1;FLOAT;0;False;6;FLOAT2;0,0;False;5;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;117;-2320,-992;Inherit;True;Property;_flow_map;flow_map;5;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;93;-1359.948,462.1018;Inherit;False;EXMT_UV_panner;-1;;65;18c96d2969f0a90438f43aa405811725;0;3;1;FLOAT;0;False;11;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;125;-1427.833,868.1499;Inherit;True;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;120;-1859.52,-581.1208;Inherit;False;Property;_FL_MU;FL_MU;40;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;135;-1204.052,-1049.617;Inherit;False;Constant;_Vector3;Vector 2;41;0;Create;True;0;0;False;0;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;96;-733.44,350.0591;Inherit;False;Constant;_Float2;Float 2;26;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;136;-1279.141,-1296.056;Inherit;False;0;104;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;130;-1468.183,-573.892;Inherit;False;2;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;94;-768.3109,236.694;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;122;-1418.418,-679.7394;Inherit;False;Property;_FL_SF;FL_SF;41;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;133;-1217.571,-606.3892;Inherit;False;Property;_partical_FL_SF_cus2_y;partical_FL_SF_cus2_y;39;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;118;-1872,-1008;Inherit;True;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-656.5101,9.019891;Inherit;False;Property;_Main_angle;Main_angle;7;0;Create;True;0;0;False;0;12.72;12.72;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;126;-1092.914,942.8964;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;68;-1173.566,-340.3214;Inherit;False;Constant;_Vector0;Vector 0;18;0;Create;True;0;0;False;0;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleSubtractOpNode;138;-919.808,-1169.23;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;-573.0869,229.3261;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;139;-994.4058,-964.7292;Inherit;False;Property;_Masksuofang1;Masksuofang;18;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;105;-666.8149,-833.8475;Inherit;False;0;103;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RotatorNode;65;-784.1646,-316.6392;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;127;-598.3329,841.7478;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;107;-401.1077,-769.2673;Inherit;False;Property;_mask_V;mask_V;17;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-977.3979,465.198;Inherit;False;Property;_dissolve_MU;dissolve_MU;28;0;Create;True;0;0;False;0;0.55;0.45;-0.1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;106;-349.3367,-888.958;Inherit;False;Property;_mask_U;mask_U;16;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;140;-793.7634,-1049.616;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PowerNode;119;-1568,-976;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;115;-491.4792,-559.9149;Inherit;False;Constant;_Vector1;Vector 1;18;0;Create;True;0;0;False;0;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleAddOpNode;137;-684.4394,-1206.529;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;121;-857.5717,-660.6627;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;109;-386.8851,-1174.409;Inherit;False;Property;_Main_mask_U;Main_mask_U;19;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;89;-695.1069,416.7949;Inherit;False;Property;_partical_dissolve_cus1_w;partical_dissolve_cus1_w;36;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;30;-352.6824,213.2201;Inherit;True;Property;_dissolve_TEX;dissolve_TEX;4;0;Create;True;0;0;False;0;-1;e28dc97a9541e3642a48c0e3886688c5;cd460ee4ac5c1e746b7a734cc7cc64dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;116;-349.5297,-439.7245;Inherit;False;Property;_mask_angle;mask_angle;8;0;Create;True;0;0;False;0;12.72;12.72;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;108;-183.1575,-845.9058;Inherit;False;EXMT_UV_panner;-1;;67;18c96d2969f0a90438f43aa405811725;0;3;1;FLOAT;0;False;11;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;110;-438.6561,-1054.718;Inherit;False;Property;_Main_mask_V;Main_mask_V;20;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-892.3596,708.5739;Inherit;False;Property;_edge_mu;edge_mu;30;0;Create;True;0;0;False;0;-0.07;-0.07;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-248.4012,-141.8071;Inherit;False;Property;_hunhe_M;hunhe_M;34;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;111;-190.1852,-1148.524;Inherit;False;EXMT_UV_panner;-1;;70;18c96d2969f0a90438f43aa405811725;0;3;1;FLOAT;0;False;11;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;1;-223.7021,-47.83193;Inherit;True;Property;_Main_tex;Main_tex;0;0;Create;True;0;0;False;0;-1;80ab37a9e4f49c842903bb43bdd7bcd2;80ab37a9e4f49c842903bb43bdd7bcd2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RotatorNode;114;-187.7476,-632.8104;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;39;-354.5692,800.7449;Inherit;False;Property;_edge_color;edge_color;11;1;[HDR];Create;True;0;0;False;0;1,1,1,1;1.782434,0.9705402,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;56;-258.5917,509.6009;Inherit;True;EXMT_lightedge;12;;71;f8e3c03e426d10a41bfb23a93b05fa62;0;3;1;FLOAT;0;False;3;FLOAT;0;False;6;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-289.8012,-334.207;Inherit;False;Property;_beizeng_M_2;beizeng_M_2;33;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-836.0576,594.7905;Inherit;False;Property;_soft_hard_MU;soft_hard_MU;29;0;Create;True;0;0;False;0;0.5;0.5;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;60;-153.3013,-419.407;Inherit;False;Property;_power_M;power_M;31;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-241.8012,-236.3071;Inherit;False;Property;_beizeng_M;beizeng_M;32;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;104;76.80192,-1080.818;Inherit;True;Property;_main_mask_text;main_mask_text;2;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;57;66.65718,301.7664;Inherit;True;EXMT_dissolve;-1;;74;f00b8e177aae37f4ca9c5f5bed782be4;0;3;1;FLOAT;0;False;6;FLOAT;0;False;10;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;98;-1.04126,989.1202;Inherit;False;Property;_vertex_U;vertex_U;25;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;64;35.35141,-375.9955;Inherit;True;EXMT_refine;-1;;73;49b551c9610a30d44a2f60000afa9996;0;5;10;FLOAT;0;False;11;FLOAT;0;False;12;FLOAT;0;False;13;FLOAT;0;False;9;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;99;-52.81226,1108.811;Inherit;False;Property;_vertex_V;vertex_V;26;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;106.2301,628.4189;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;97;-185.5932,1024.888;Inherit;False;0;75;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;103;21.98745,-629.5115;Inherit;True;Property;_mask_text;mask_text;1;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;132;-772.2037,1076.512;Inherit;False;Property;_vertex_MU;vertex_MU;42;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;100;202.6587,1016.005;Inherit;False;EXMT_UV_panner;-1;;75;18c96d2969f0a90438f43aa405811725;0;3;1;FLOAT;0;False;11;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;102;454.0707,-380.5563;Inherit;False;EXMT_Main_mask;-1;;76;7007ff822379e0f4e8e4b5160216a57a;0;3;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;37;386.98,193.4738;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DesaturateOpNode;129;858.7037,328.7821;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;113;749.3052,-281.0145;Inherit;True;EXMT_Partcl_color;9;;77;07015bb29b7c6744ba7071eddda1e547;0;1;1;FLOAT4;0,0,0,0;False;2;FLOAT4;0;FLOAT;9
Node;AmplifyShaderEditor.SamplerNode;75;427.8615,680.6552;Inherit;True;Property;_vertex_tex;vertex_tex;6;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;131;-464.5064,1090.798;Inherit;False;Property;_partical_vertex_cus2_x;partical_vertex_cus2_x;38;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;74;1183.589,540.7884;Inherit;False;EXMT_vertex_panner;-1;;78;437eca5473efb7f4d9764486838f5158;0;2;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;1222.706,-14.24733;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;128;1214.499,197.9149;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;134;1679.229,131.842;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Effect/EXMT_Common_AH_xin;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;80;0;79;1
WireConnection;80;1;79;2
WireConnection;87;0;80;0
WireConnection;87;1;86;0
WireConnection;84;1;20;0
WireConnection;84;11;86;0
WireConnection;84;2;21;0
WireConnection;81;0;87;0
WireConnection;81;1;82;0
WireConnection;83;1;26;0
WireConnection;83;11;85;0
WireConnection;83;2;27;0
WireConnection;78;1;84;0
WireConnection;78;0;81;0
WireConnection;19;1;83;0
WireConnection;88;1;23;0
WireConnection;88;0;79;3
WireConnection;25;1;19;1
WireConnection;25;6;78;0
WireConnection;25;5;88;0
WireConnection;93;1;90;0
WireConnection;93;11;92;0
WireConnection;93;2;91;0
WireConnection;125;0;117;0
WireConnection;94;0;25;0
WireConnection;94;1;93;0
WireConnection;133;1;122;0
WireConnection;133;0;130;2
WireConnection;118;0;117;0
WireConnection;126;0;125;0
WireConnection;126;1;120;0
WireConnection;138;0;136;0
WireConnection;138;1;135;0
WireConnection;95;0;94;0
WireConnection;95;1;96;0
WireConnection;65;0;25;0
WireConnection;65;1;68;0
WireConnection;65;2;69;0
WireConnection;127;0;95;0
WireConnection;127;1;126;0
WireConnection;127;2;133;0
WireConnection;140;0;138;0
WireConnection;140;1;139;0
WireConnection;119;0;118;0
WireConnection;119;1;120;0
WireConnection;137;0;136;0
WireConnection;137;1;140;0
WireConnection;121;0;65;0
WireConnection;121;1;119;0
WireConnection;121;2;133;0
WireConnection;89;1;31;0
WireConnection;89;0;79;4
WireConnection;30;1;127;0
WireConnection;108;1;106;0
WireConnection;108;11;105;0
WireConnection;108;2;107;0
WireConnection;111;1;109;0
WireConnection;111;11;137;0
WireConnection;111;2;110;0
WireConnection;1;1;121;0
WireConnection;114;0;108;0
WireConnection;114;1;115;0
WireConnection;114;2;116;0
WireConnection;56;1;35;0
WireConnection;56;3;30;1
WireConnection;56;6;89;0
WireConnection;104;1;111;0
WireConnection;57;1;30;1
WireConnection;57;6;89;0
WireConnection;57;10;32;0
WireConnection;64;10;60;0
WireConnection;64;11;61;0
WireConnection;64;12;62;0
WireConnection;64;13;63;0
WireConnection;64;9;1;0
WireConnection;38;0;56;0
WireConnection;38;1;39;0
WireConnection;103;1;114;0
WireConnection;100;1;98;0
WireConnection;100;11;97;0
WireConnection;100;2;99;0
WireConnection;102;1;64;0
WireConnection;102;2;103;4
WireConnection;102;5;104;4
WireConnection;37;0;57;0
WireConnection;37;1;38;0
WireConnection;129;0;37;0
WireConnection;113;1;102;0
WireConnection;75;1;100;0
WireConnection;131;1;132;0
WireConnection;131;0;130;1
WireConnection;74;1;75;0
WireConnection;74;2;131;0
WireConnection;55;0;113;0
WireConnection;55;1;37;0
WireConnection;128;0;113;9
WireConnection;128;1;129;0
WireConnection;134;2;55;0
WireConnection;134;9;128;0
WireConnection;134;11;74;0
ASEEND*/
//CHKSM=8974D389551AFA54BCE11C112252F8CCD29CCE5F