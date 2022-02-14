// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effect/EXMT_SD_UV_panner"
{
	Properties
	{
		_speed("speed", Float) = 0
		_wide("wide", Float) = 2
		_hei("hei", Float) = 4
		[Toggle(_KEYWORD0_ON)] _Keyword0("Keyword 0", Float) = 1
		_slider("slider", Float) = 0
		_x_size("x_size", Float) = 1
		_color_mult("color_mult", Float) = 1
		_offset("offset", Float) = 0
		_sequence("sequence", 2D) = "white" {}
		[HDR]_Color0("Color 0", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _KEYWORD0_ON
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float _color_mult;
		uniform sampler2D _sequence;
		uniform float _wide;
		uniform float _hei;
		uniform float _slider;
		uniform float _speed;
		uniform float _x_size;
		uniform float _offset;
		uniform float4 _Color0;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float x11 = _wide;
			float temp_output_91_0 = ( 1.0 / x11 );
			float y12 = _hei;
			float temp_output_92_0 = ( 1.0 / y12 );
			float2 appendResult51 = (float2(temp_output_91_0 , temp_output_92_0));
			float count14 = ( _wide * _hei );
			#ifdef _KEYWORD0_ON
				float staticSwitch15 = ( count14 * frac( ( _Time.y * _speed ) ) );
			#else
				float staticSwitch15 = _slider;
			#endif
			float temp_output_23_0 = floor( fmod( staticSwitch15 , x11 ) );
			float2 appendResult28 = (float2(temp_output_23_0 , -floor( ( fmod( staticSwitch15 , ( y12 * x11 ) ) / x11 ) )));
			float y_recip49 = temp_output_92_0;
			float2 appendResult72 = (float2(0.0 , ( ( y12 - 1.0 ) * y_recip49 )));
			float2 appendResult57 = (float2(( i.uv_texcoord.x * _x_size ) , i.uv_texcoord.y));
			float2 appendResult58 = (float2(_wide , _hei));
			float2 appendResult64 = (float2(( abs( _x_size ) * 0.1 * _offset ) , 0.0));
			float2 temp_output_66_0 = ( appendResult72 + ( appendResult57 / appendResult58 ) + appendResult64 );
			float4 tex2DNode83 = tex2D( _sequence, ( ( appendResult51 * appendResult28 ) + temp_output_66_0 ) );
			o.Emission = ( max( _color_mult , 1.0 ) * tex2DNode83 * _Color0 ).rgb;
			float clampResult76 = clamp( ( _wide * (temp_output_66_0).x ) , 0.0 , 1.0 );
			o.Alpha = ( tex2DNode83.a * ceil( frac( clampResult76 ) ) * min( _color_mult , 1.0 ) * i.vertexColor.a );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				half4 color : COLOR0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.vertexColor = IN.color;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17500
-7;221;1920;767;762.1034;381.2687;1.404049;True;True
Node;AmplifyShaderEditor.RangedFloatNode;9;-2897.792,201.8908;Inherit;False;Property;_hei;hei;2;0;Create;True;0;0;False;0;4;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-2908.061,89.96764;Inherit;False;Property;_wide;wide;1;0;Create;True;0;0;False;0;2;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-2657.829,506.0703;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;2;-2857.959,-650.5372;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;12;-2683.717,167.5558;Inherit;False;y;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-2858.559,-395.1373;Inherit;False;Property;_speed;speed;0;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;32;-1161.589,-922.6558;Inherit;False;12;y;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;90;-1157.406,-1337.347;Inherit;False;Constant;_Float5;Float 5;9;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-2595.959,-517.5372;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;14;-2449.902,376.0703;Float;False;count;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;5;-2438.866,-532.1635;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;92;-859.7071,-1050.048;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;7;-2447.865,-665.1636;Inherit;False;14;count;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;11;-2691.208,42.46441;Inherit;False;x;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;20;-2174.11,-352.3583;Inherit;False;11;x;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;21;-2175.462,-429.3583;Inherit;False;12;y;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-2241.565,275.6854;Inherit;False;Property;_x_size;x_size;5;0;Create;True;0;0;False;0;1;1.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-2168.879,-745.5931;Inherit;False;Property;_slider;slider;4;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;67;-2012.723,-188.885;Inherit;False;12;y;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;68;-2006.548,-89.48953;Inherit;False;Constant;_Float2;Float 2;6;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;54;-2290.565,152.6854;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;49;-631.352,-1144.997;Float;False;y_recip;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-2180.865,-654.6635;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;-1983.565,148.6854;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;69;-1771.713,-205.6318;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;71;-1747.803,24.63271;Inherit;False;49;y_recip;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;15;-1930.079,-727.4929;Inherit;True;Property;_Keyword0;Keyword 0;3;0;Create;True;0;0;False;0;0;1;1;True;;Toggle;2;Key0;Key1;Create;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;59;-1799.565,419.6854;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-1732.565,663.6854;Inherit;True;Constant;_Float0;Float 0;6;0;Create;True;0;0;False;0;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-1727.565,884.6854;Inherit;True;Property;_offset;offset;7;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-1947.879,-421.5931;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;-1310.754,-149.9209;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-1504.565,496.6854;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;73;-1311.638,-250.2055;Inherit;False;Constant;_Float3;Float 3;6;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;25;-1543.502,-316.0943;Inherit;False;11;x;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.FmodOpNode;17;-1646.279,-439.993;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-1440.565,717.6854;Inherit;False;Constant;_Float1;Float 1;6;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;57;-1690.565,240.6854;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;58;-2243.565,405.6854;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;60;-1345.565,248.6854;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;64;-1271.889,518.7731;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;24;-1360.179,-446.9931;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;72;-978.2907,-178.4232;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;19;-1655.878,-612.8931;Inherit;False;11;x;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;31;-1173.569,-1124.167;Inherit;False;11;x;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.FloorOpNode;26;-1204.079,-438.9932;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;66;-510.1382,-193.504;Inherit;True;3;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FmodOpNode;18;-1450.98,-694.9927;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;27;-1048.679,-451.9931;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FloorOpNode;23;-1187.679,-688.3926;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;91;-866.2059,-1281.448;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;74;-491.2468,155.2621;Inherit;True;True;False;False;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;51;-501.3167,-1048.413;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;-495.1653,421.6082;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;28;-819.679,-556.1931;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-305.8396,-599.6423;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;76;-462.197,685.8174;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;52;-42.21795,-400.8024;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-107.197,41.81741;Inherit;False;Property;_color_mult;color_mult;6;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;77;-444.197,861.8174;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;81;-189.197,169.8174;Inherit;False;Constant;_Float4;Float 4;7;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;86;229.1843,770.6634;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;93;199.1389,-399.0875;Inherit;False;Property;_Color0;Color 0;9;1;[HDR];Create;True;0;0;False;0;0,0,0,0;1,1,1,0.5019608;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMinOpNode;84;348.1843,462.6634;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CeilOpNode;78;-451.197,995.8174;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;80;68.80298,14.81741;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;83;-28.59391,-208.6826;Inherit;True;Property;_sequence;sequence;8;0;Create;True;0;0;False;0;-1;None;527dd74022b67614589c4efcb9cb70c4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;29;-900.8213,-669.5052;Float;False;x_fmode;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;50;-592.318,-1295.314;Float;False;x_recip;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;85;516.0708,661.7012;Inherit;True;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;475.2159,-207.8963;Inherit;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;702.9397,308.8494;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Effect/EXMT_SD_UV_panner;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;13;0;8;0
WireConnection;13;1;9;0
WireConnection;12;0;9;0
WireConnection;3;0;2;2
WireConnection;3;1;4;0
WireConnection;14;0;13;0
WireConnection;5;0;3;0
WireConnection;92;0;90;0
WireConnection;92;1;32;0
WireConnection;11;0;8;0
WireConnection;49;0;92;0
WireConnection;6;0;7;0
WireConnection;6;1;5;0
WireConnection;55;0;54;1
WireConnection;55;1;56;0
WireConnection;69;0;67;0
WireConnection;69;1;68;0
WireConnection;15;1;16;0
WireConnection;15;0;6;0
WireConnection;59;0;56;0
WireConnection;22;0;21;0
WireConnection;22;1;20;0
WireConnection;70;0;69;0
WireConnection;70;1;71;0
WireConnection;61;0;59;0
WireConnection;61;1;62;0
WireConnection;61;2;63;0
WireConnection;17;0;15;0
WireConnection;17;1;22;0
WireConnection;57;0;55;0
WireConnection;57;1;54;2
WireConnection;58;0;8;0
WireConnection;58;1;9;0
WireConnection;60;0;57;0
WireConnection;60;1;58;0
WireConnection;64;0;61;0
WireConnection;64;1;65;0
WireConnection;24;0;17;0
WireConnection;24;1;25;0
WireConnection;72;0;73;0
WireConnection;72;1;70;0
WireConnection;26;0;24;0
WireConnection;66;0;72;0
WireConnection;66;1;60;0
WireConnection;66;2;64;0
WireConnection;18;0;15;0
WireConnection;18;1;19;0
WireConnection;27;0;26;0
WireConnection;23;0;18;0
WireConnection;91;0;90;0
WireConnection;91;1;31;0
WireConnection;74;0;66;0
WireConnection;51;0;91;0
WireConnection;51;1;92;0
WireConnection;75;0;8;0
WireConnection;75;1;74;0
WireConnection;28;0;23;0
WireConnection;28;1;27;0
WireConnection;30;0;51;0
WireConnection;30;1;28;0
WireConnection;76;0;75;0
WireConnection;52;0;30;0
WireConnection;52;1;66;0
WireConnection;77;0;76;0
WireConnection;84;0;79;0
WireConnection;84;1;81;0
WireConnection;78;0;77;0
WireConnection;80;0;79;0
WireConnection;80;1;81;0
WireConnection;83;1;52;0
WireConnection;29;0;23;0
WireConnection;50;0;91;0
WireConnection;85;0;83;4
WireConnection;85;1;78;0
WireConnection;85;2;84;0
WireConnection;85;3;86;4
WireConnection;82;0;80;0
WireConnection;82;1;83;0
WireConnection;82;2;93;0
WireConnection;0;2;82;0
WireConnection;0;9;85;0
ASEEND*/
//CHKSM=230562256A0D58AA960D313452E50E3185942887