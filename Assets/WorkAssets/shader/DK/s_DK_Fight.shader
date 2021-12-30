
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'


Shader "TopJoy/DK/Fight" {
Properties {
		_MainTex ("bodyTex ", 2D) = "white" {}		
		_SSSTex("bodyTex_dark", 2D) = "white" {}
		_SecTex ("faceTex ", 2D) = "white" {}
		_SpecAndAOTexAndInnerLineColorTex ("Spec Mask (B) AO Mask(G) Inner Line Color (R)", 2D) = "white" {}
		
		//---
		_shadeSoft("aoIndependentShadeSoft",Range(0,1))=0
		_aoScale("aoScale",Range(0,10))=2.5
		_fresnelshadowRange("fresnelShadowRange",Range(-1,1))=-1
		_darkAdd("darkAdd",Range(0,1))=0.1
		_aoAdd("aoAdd",Range(0,1))=0

		_innerLineScale("innerLineScale",Range(0,1))=0.5
		_innerLineMaxAdd("innerLineMaxAdd",Range(0,2))=0
		_diffuseOffset("diffuseOffset",Range(0,0.9))=0

		//spec
		_SpecColor ("specColor", Color) = (1, 1, 1, 1)
		_Shininess ("Shininess", Range(0.01,2)) = 0.04
		_SpecSmooth ("Smoothness", Range(0,1)) = 1
		//outline
		_OutlineColor ("Outline Color", Color) = (0, 0, 0, 1.0)
		_OutlineMinWidth("OutlineMinWidth", Float) = 0.2
		_OutlineMaxWidth("OutlineMaxWidth", Float) = 0.4
		_MaxCameraDistance("MaxCameraDistance", Float) = 199
		_MinCameraDistance("MinCameraDistance", Float) = 66.6
		//rim
		_MainColor("MainColor",Color)=(1,0.98,0.9725,1)
		_RimColor1("RimColor",Color)=(0.8235,0.19215,0,1)
		_RimRange1("RimRange",Range(-1,1))=0.3
		_RimSoft("RimSoft",Range(0,0.49))=0
		_RimOffset1("RimOffset",Range(-1,1))=-0.027
		[Toggle]_RimToggle("_RimToggle",Float)=0
		_XRayColor("XRay Color", Color) = (0, 1, 0.7, 0.25)
}

SubShader {	
	LOD 1000
	Tags { "RenderType" = "Opaque" "Queue" = "Geometry+100"}
	//UsePass "TopJoy/DK/FightEx/FIGHTERXRAY"
	UsePass "TopJoy/DK/FightEx/FIGHTER"
	UsePass "Hidden/TopJoy/Utility/ToonyOutLine/OUTLINE"
	
}

SubShader{
	Tags { "RenderType" = "Opaque" "Queue" = "Geometry"}
	LOD 600
	UsePass "TopJoy/DK/FightEx/FIGHTER"
}
CustomEditor "DKShaderGUIFight"
}
