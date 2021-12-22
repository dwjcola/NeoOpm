﻿Shader "OPM/ToonShader-Transparent"
{
	Properties
	{
		_tint("Main Color",COLOR) = (1,1,1,1)
		_MainTex("Main Texture ", 2D) = "white" {}
		_SSSTex("Shadow Texture", 2D) = "white" {}

		_SpecAndAOTexAndInnerLineColorTex("Inner Line Color (R) AO Mask(G) Spec Mask (B) Line Spec Mask(A)", 2D) = "white" {}

		//spec
		_SpecColor("SpecColor", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess("Shininess", Range(0.01,2)) = 0.1
		_SpecSmooth("Smoothness", Range(0,1)) = 0.05
		_SpecColor1("Line SpecColor", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess1("Line Shininess", Range(0.01,2)) = 0.1
		_SpecSmooth1("Line Smoothness", Range(0,1)) = 0.05
        _MaskDataTex1("遮罩数据图1 用于存储渲染数据  Rim Mask (R)  Outline Mask (G)  Outline Camera Dis (B)  Outline Mask Z Offset (A)", 2D) = "white" {}
		//rim
		_RimColor("Rim Color", Color) = (0.8,0.8,0.8,0.6)
		_Bias("Rim Bias", Range(-1,1)) = 0
		_Scale("Rim Scale", Range(0,10)) = 1
		_Power("Rim Power", Range(0,10)) = 5


		//---
		_shadeSoft("AoIndependentShadeSoft",Range(0,1)) = 0
		_aoScale("AoScale",Range(0,10)) = 1
		_innerLineScale("InnerLineScale",Range(0,1)) = 1
		
		//---
		_shadowColor("First Shadow Color",COLOR) = (0.45,0.45,0.45,1)
		_Brightness1("First Shadow Color Brightness", Float) = 1    //调整亮度
		_Saturation1("First Shadow Color Saturation", Float) = 1    //调整饱和度
		_Contrast1("First Shadow Color Contrast", Float) = 1        //调整对比度 
		_twoShadowColor("Second Shadow Color",color) = (0.5,0.5,0.5,1)
		_Brightness2("Second Shadow Color Brightness", Float) = 1    //调整亮度
		_Saturation2("Second Shadow Color Saturation", Float) = 1    //调整饱和度
		_Contrast2("Second Shadow Color Contrast", Float) = 1        //调整对比度 
		_towShadowRange("Second Shadow Boundary",Range(0,3)) = 1
	}
		/*
		•R：判断rim边缘光的区域，值越小rim效果越弱，0的时候没有rim效果。
		•G：对应到Camera的距离，轮廓线的在哪个范围膨胀的系数
		•B：轮廓线的 Z Offset值
		•A：轮廓线的粗细系数。0.5是标准，1是最粗，0的话就没有轮廓线。
		遮罩数据图1 用于存储渲染数据  Rim Mask (R)  Outline Mask (G)  Outline Camera Dis (B)  Outline Mask Z Offset (A)
		*/
	SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue"="Transparent" }
		LOD 100
		Pass
            {
                ZWrite On
                Cull Front
                //意味着不写入任何颜色通道
                ColorMask 0
            }
		Pass
            {
                Tags{"LightMode" = "ForwardBase" }
                Cull Front
                ZWrite Off
                //Blend One OneMinusSrcAlpha
                Blend SrcAlpha OneMinusSrcAlpha
                //Blend SrcAlpha OneMinusSrcAlpha
                //BlendOp Max
                //Blend One One*/
                //Blend  OneMinusSrcAlpha	One
                //Blend 1 SrcAlpha OneMinusSrcAlpha 	
                //Blend One One//OneMinusSrcAlpha		
                //ColorMask 0
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                // make fog work
                //#pragma multi_compile_fog
                
            
                
                #include "UnityCG.cginc"			
                #include "AutoLight.cginc"
                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    half3 normal : NORMAL;
                    fixed4 color : COLOR;
                    
                };
    
                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    //UNITY_FOG_COORDS(1)
                    float4 pos : SV_POSITION;
                    float3 worldPos: TEXCOORD1;
                    half3 worldNormal: NORMAL;
                    fixed4 Color : TEXCOORD2;
                    SHADOW_COORDS(3)
                };
    
                fixed4 _Color;
                sampler2D _MainTex;
                sampler2D _MaskDataTex1;
                half4 _MainTex_ST;
                sampler2D _SpecAndAOTexAndInnerLineColorTex;
                sampler2D _SSSTex;
                fixed _Shininess;
                fixed4 _RimColor;
                fixed _RimMin;
                fixed _RimMax;
                half _RimBright;
                fixed _SpecSmooth;
                fixed _shadeSoft;
                fixed4 _SpecColor;
                half _aoScale;
                fixed _innerLineScale;
                fixed4 _SpecColor1;
                half	_Shininess1;
                half	_SpecSmooth1;
                
                fixed4 _tint;
                
                fixed _BriDarkConst;
            
                fixed4 _twoShadowColor;
                half	_towShadowRange;
                fixed4 _shadowColor;
                half _Bias;
                half _Scale;
                half _Power;
                fixed4 _OutlineColor;
                half _Brightness1;
                half _Saturation1;
                half _Contrast1;
                
                half _Brightness2;
                half _Saturation2;
                half _Contrast2;
                
                v2f vert (appdata v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                    o.worldNormal = UnityObjectToWorldNormal(-v.normal);
                    o.Color = v.color;
                    TRANSFER_SHADOW(o);
                    return o;
                }
                
                fixed4 frag (v2f i) : SV_Target
                {
                    half3 lightDir;
                    
                    lightDir = _WorldSpaceLightPos0.xyz;
                    
                    fixed3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos);
    
                    fixed shadow = SHADOW_ATTENUATION(i);
    
                    
                    fixed4 _main4 = tex2D(_MainTex, i.uv);
                    fixed3 _main = _main4.rgb;
                    
                    fixed4 specAndAOAndInnerLineColor = tex2D(_SpecAndAOTexAndInnerLineColorTex, i.uv);
                    fixed specMask = specAndAOAndInnerLineColor.b;
                    fixed specMask1=specAndAOAndInnerLineColor.a;
                    half ao = specAndAOAndInnerLineColor.g;
                    half innerLine = specAndAOAndInnerLineColor.r;
    
                    ao = lerp(1,ao,_aoScale);
                    half NdotL = dot(lightDir, i.worldNormal);
                    half NdotV = dot(viewDir, i.worldNormal);
                    half3 halfVector = normalize(lightDir + viewDir);
                    half NdotH = dot(i.worldNormal, halfVector);
                    
                
    
                    half NdotLrim = dot(lightDir, i.worldNormal);
                    // ---shadowcolor
                    fixed4 sssTex = tex2D(_SSSTex, i.uv);
                    half3 shadowColor = sssTex.rgb*_shadowColor.rgb;
                    fixed3 shadowColorfinal = shadowColor * _Brightness1;			
                    fixed gray = 0.2125 * shadowColor.r + 0.7154 * shadowColor.g + 0.0721 * shadowColor.b;
                    fixed3 grayColor = fixed3(gray, gray, gray);
                    shadowColorfinal = lerp(grayColor, shadowColorfinal, _Saturation1);			
                    fixed3 avgColor = fixed3(0.5, 0.5, 0.5);				
                    shadowColorfinal = lerp(avgColor, shadowColorfinal, _Contrast1);
    
    
                    half3 twoShadowColor = sssTex.rgb*_twoShadowColor.rgb;
                    fixed3 twoshadowColorfinal = twoShadowColor * _Brightness2;
                    fixed gray2 = 0.2125 * twoShadowColor.r + 0.7154 * twoShadowColor.g + 0.0721 * twoShadowColor.b;
                    fixed3 grayColor2 = fixed3(gray2, gray2, gray2);
                    twoshadowColorfinal = lerp(grayColor2, twoshadowColorfinal, _Saturation2);
                    fixed3 avgColor2= fixed3(0.5, 0.5, 0.5);
                    twoshadowColorfinal = lerp(avgColor2, twoshadowColorfinal, _Contrast2);
                    //----rim
                    half rim = (_Bias + _Scale * pow(1.0 - NdotV, _Power));
                    rim *= saturate(-NdotLrim);
                    fixed3 rimColor = saturate( rim*_RimColor)*tex2D(_MaskDataTex1, i.uv).r;
                    
                    //----shadeColor
                    half shade = smoothstep(0,(1 - ao) + _shadeSoft,NdotL)*shadow;
                    shade = lerp((shade)*(ao),float3(1,1,1),shade);
    
                    half shade1 = smoothstep(0, (1 - ao) + _shadeSoft, NdotL + _towShadowRange)*shadow;
                    shade1 = lerp((shade1)*(ao), float3(1, 1, 1), shade1);
    
                    
    
                    half3 shadeColor = lerp(shadowColorfinal, _main,shade);
                    //shadeColor *= shadeColor2;
                    half3 shadeColor2 = lerp(twoshadowColorfinal, shadeColor, lerp(1, shade1, sssTex.a));
                    //----innerLineColor
                    innerLine = lerp(innerLine,saturate(innerLine ),shade);
                    half3 innerLineColor = float3(innerLine, innerLine, innerLine);//  _OutlineColor*innerLine;
                    //----specular
                    half Gloss = specMask * _SpecColor.a;
                    half spec = pow(max(0, NdotH), _Shininess*128.0) * Gloss * 2.0;
                    spec = smoothstep(0.5 - _SpecSmooth * 0.5, 0.5 + _SpecSmooth * 0.5, spec);
                    half Gloss1 = specMask1 * _SpecColor1.a;
                    half spec1 = pow(max(0, NdotH), _Shininess1*128.0) * Gloss1 * 2.0;
                    spec1 = smoothstep(0.5 - _SpecSmooth1 * 0.5, 0.5 + _SpecSmooth1 * 0.5, spec1);
                    //spec += spec1;
                    
    
                    half3 specColor = _SpecColor.rgb *spec;// specAndAOAndInnerLineColor.a;
                    half3 specColor1 = _SpecColor1.rgb *spec1;
                    fixed4 c;
                    c.rgb =saturate( saturate(saturate(shadeColor2 +specColor)+specColor1)+rimColor);
                    
                    c.rgb = lerp(c.rgb, innerLineColor*c.rgb, _innerLineScale);
                    
                    c.rgb *= _tint.rgb;
                    
                    c.a = _main4.a;
                    
                    return c;// fixed4(innerLine, innerLine, innerLine, 1);// 
                }
                ENDCG
            }
        Pass
            {
                Tags{"LightMode" = "ForwardBase" }
                Cull Back
                ZWrite Off
                Blend SrcAlpha OneMinusSrcAlpha
                /*BlendOp Max
                Blend One One*/
			    //Blend  OneMinusSrcAlpha	One
                //Blend 1 SrcAlpha OneMinusSrcAlpha 	
                //Blend One One//OneMinusSrcAlpha		
                //ColorMask B 1
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                // make fog work
                //#pragma multi_compile_fog
                
            
                
                #include "UnityCG.cginc"			
                #include "AutoLight.cginc"
                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    half3 normal : NORMAL;
                    fixed4 color : COLOR;
                    
                };
    
                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    //UNITY_FOG_COORDS(1)
                    float4 pos : SV_POSITION;
                    float3 worldPos: TEXCOORD1;
                    half3 worldNormal: NORMAL;
                    fixed4 Color : TEXCOORD2;
                    SHADOW_COORDS(3)
                };
    
                fixed4 _Color;
                sampler2D _MainTex;
                sampler2D _MaskDataTex1;
                half4 _MainTex_ST;
                sampler2D _SpecAndAOTexAndInnerLineColorTex;
                sampler2D _SSSTex;
                fixed _Shininess;
                fixed4 _RimColor;
                fixed _RimMin;
                fixed _RimMax;
                half _RimBright;
                fixed _SpecSmooth;
                fixed _shadeSoft;
                fixed4 _SpecColor;
                half _aoScale;
                fixed _innerLineScale;
                fixed4 _SpecColor1;
                half	_Shininess1;
                half	_SpecSmooth1;
                
                fixed4 _tint;
                
                fixed _BriDarkConst;
            
                fixed4 _twoShadowColor;
                half	_towShadowRange;
                fixed4 _shadowColor;
                half _Bias;
                half _Scale;
                half _Power;
                fixed4 _OutlineColor;
                half _Brightness1;
                half _Saturation1;
                half _Contrast1;
                
                half _Brightness2;
                half _Saturation2;
                half _Contrast2;
                
                v2f vert (appdata v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                    o.worldNormal = UnityObjectToWorldNormal(v.normal);
                    o.Color = v.color;
                    TRANSFER_SHADOW(o);
                    return o;
                }
                
                fixed4 frag (v2f i) : SV_Target
                {
                    half3 lightDir;
                    
                    lightDir = _WorldSpaceLightPos0.xyz;
                    
                    fixed3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos);
    
                    fixed shadow = SHADOW_ATTENUATION(i);
    
                    
                    fixed4 _main4 = tex2D(_MainTex, i.uv);
                    fixed3 _main = _main4.rgb;
                    
                    fixed4 specAndAOAndInnerLineColor = tex2D(_SpecAndAOTexAndInnerLineColorTex, i.uv);
                    fixed specMask = specAndAOAndInnerLineColor.b;
                    fixed specMask1=specAndAOAndInnerLineColor.a;
                    half ao = specAndAOAndInnerLineColor.g;
                    half innerLine = specAndAOAndInnerLineColor.r;
    
                    ao = lerp(1,ao,_aoScale);
                    half NdotL = dot(lightDir, i.worldNormal);
                    half NdotV = dot(viewDir, i.worldNormal);
                    half3 halfVector = normalize(lightDir + viewDir);
                    half NdotH = dot(i.worldNormal, halfVector);
                    
                
    
                    half NdotLrim = dot(lightDir, i.worldNormal);
                    // ---shadowcolor
                    fixed4 sssTex = tex2D(_SSSTex, i.uv);
                    half3 shadowColor = sssTex.rgb*_shadowColor.rgb;
                    fixed3 shadowColorfinal = shadowColor * _Brightness1;			
                    fixed gray = 0.2125 * shadowColor.r + 0.7154 * shadowColor.g + 0.0721 * shadowColor.b;
                    fixed3 grayColor = fixed3(gray, gray, gray);
                    shadowColorfinal = lerp(grayColor, shadowColorfinal, _Saturation1);			
                    fixed3 avgColor = fixed3(0.5, 0.5, 0.5);				
                    shadowColorfinal = lerp(avgColor, shadowColorfinal, _Contrast1);
    
    
                    half3 twoShadowColor = sssTex.rgb*_twoShadowColor.rgb;
                    fixed3 twoshadowColorfinal = twoShadowColor * _Brightness2;
                    fixed gray2 = 0.2125 * twoShadowColor.r + 0.7154 * twoShadowColor.g + 0.0721 * twoShadowColor.b;
                    fixed3 grayColor2 = fixed3(gray2, gray2, gray2);
                    twoshadowColorfinal = lerp(grayColor2, twoshadowColorfinal, _Saturation2);
                    fixed3 avgColor2= fixed3(0.5, 0.5, 0.5);
                    twoshadowColorfinal = lerp(avgColor2, twoshadowColorfinal, _Contrast2);
                    //----rim
                    half rim = (_Bias + _Scale * pow(1.0 - NdotV, _Power));
                    rim *= saturate(-NdotLrim);
                    fixed3 rimColor = saturate( rim*_RimColor)*tex2D(_MaskDataTex1, i.uv).r;
                    
                    //----shadeColor
                    half shade = smoothstep(0,(1 - ao) + _shadeSoft,NdotL)*shadow;
                    shade = lerp((shade)*(ao),float3(1,1,1),shade);
    
                    half shade1 = smoothstep(0, (1 - ao) + _shadeSoft, NdotL + _towShadowRange)*shadow;
                    shade1 = lerp((shade1)*(ao), float3(1, 1, 1), shade1);
    
                    
    
                    half3 shadeColor = lerp(shadowColorfinal, _main,shade);
                    //shadeColor *= shadeColor2;
                    half3 shadeColor2 = lerp(twoshadowColorfinal, shadeColor, lerp(1, shade1, sssTex.a));
                    //----innerLineColor
                    innerLine = lerp(innerLine,saturate(innerLine ),shade);
                    half3 innerLineColor = float3(innerLine, innerLine, innerLine);//  _OutlineColor*innerLine;
                    //----specular
                    half Gloss = specMask * _SpecColor.a;
                    half spec = pow(max(0, NdotH), _Shininess*128.0) * Gloss * 2.0;
                    spec = smoothstep(0.5 - _SpecSmooth * 0.5, 0.5 + _SpecSmooth * 0.5, spec);
                    half Gloss1 = specMask1 * _SpecColor1.a;
                    half spec1 = pow(max(0, NdotH), _Shininess1*128.0) * Gloss1 * 2.0;
                    spec1 = smoothstep(0.5 - _SpecSmooth1 * 0.5, 0.5 + _SpecSmooth1 * 0.5, spec1);
                    //spec += spec1;
                    
    
                    half3 specColor = _SpecColor.rgb *spec;// specAndAOAndInnerLineColor.a;
                    half3 specColor1 = _SpecColor1.rgb *spec1;
                    fixed4 c;
                    c.rgb =saturate( saturate(saturate(shadeColor2 +specColor)+specColor1)+rimColor);
                    
                    c.rgb = lerp(c.rgb, innerLineColor*c.rgb, _innerLineScale);
                    
                    c.rgb *= _tint.rgb;
                    
                    c.a = _main4.a;
                    
                    return c;// fixed4(innerLine, innerLine, innerLine, 1);// 
                }
                ENDCG
            }
		/*Pass
			{
				Tags{ "LightMode" = "ForwardAdd" }
					Blend One One
					CGPROGRAM
					#pragma multi_compile_fwdadd
					#pragma vertex vert
					#pragma fragment frag
					#include "Lighting.cginc"
					#include "UnityCG.cginc"
	                #include "AutoLight.cginc"


					sampler2D _MainTex;
					float4 _MainTex_ST;

					struct a2v
					{
						float4 vertex : POSITION;
						float3 normal : NORMAL;
						float2 uv:TEXCOORD0;
					};

					struct v2f
					{
						float4 pos : SV_POSITION;
						float3 worldNormal : TEXCOORD1;
						float2 uv:TEXCOORD0;
						float3 worldPos:TEXCOORD2;

					};

					v2f vert(a2v v)
					{
						v2f o;
						o.pos = UnityObjectToClipPos(v.vertex);
						o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);
						o.uv = TRANSFORM_TEX(v.uv, _MainTex);
						o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

						return o;
					}

					fixed4 frag(v2f v) : SV_Target
					{
						fixed3 worldNormal = normalize(v.worldNormal);
					#ifdef USING_DIRECTIONAL_LIGHT
					float3 worldLightDir = _WorldSpaceLightPos0;
					#else
						float3 worldLightDir = normalize(_WorldSpaceLightPos0 - v.worldPos);
					#endif

					


					fixed3 diffuse = _LightColor0 * (saturate(saturate(dot(worldNormal, worldLightDir))*0.5 + 0.5));
					//fixed3 viewDir = normalize(_WorldSpaceCameraPos.xyz - v.worldPos.xyz);
					//fixed3 halfDir = normalize(worldLightDir + viewDir);

					fixed3 color = saturate(diffuse );
					fixed4 c = tex2D(_MainTex, v.uv);
					fixed3 finCol = (c*color).rgb;
					#ifdef USING_DIRECTIONAL_LIGHT
										 fixed atten = 1.0;
									#else
										 #if defined (POINT)
											 float3 lightCoord = mul(unity_WorldToLight, float4(v.worldPos, 1)).xyz;
									  fixed atten = (tex2D(_LightTexture0, dot(lightCoord, lightCoord).rr).UNITY_ATTEN_CHANNEL)* (saturate(saturate(dot(worldNormal, worldLightDir)*0.5 + 0.5)));
										 #elif defined (SPOT)
												 float4 lightCoord = mul(unity_WorldToLight, float4(v.worldPos, 1));
											fixed atten = ((lightCoord.z > 0) * tex2D(_LightTexture0, lightCoord.xy / lightCoord.w + 0.5).w * tex2D(_LightTextureB0, dot(lightCoord, lightCoord).rr).UNITY_ATTEN_CHANNEL)* (saturate(saturate(dot(worldNormal, worldLightDir))));
										 #else
												fixed atten = 1.0;
										#endif
								   #endif


					return fixed4(finCol*atten * 2, 1);
					}
						ENDCG
			}*/
        /*Pass
            {
                Name "ShadowCaster"
                Tags { "LightMode" = "ShadowCaster" }

                ZWrite On ZTest LEqual Cull Off

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 2.0
                #pragma multi_compile_shadowcaster
                #include "UnityCG.cginc"

                struct v2f {
                    V2F_SHADOW_CASTER;
                    UNITY_VERTEX_OUTPUT_STEREO
                };

                v2f vert(appdata_base v)
                {
                    v2f o;
                    UNITY_SETUP_INSTANCE_ID(v);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                    TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                    return o;
                }

                float4 frag(v2f i) : SV_Target
                {
                    SHADOW_CASTER_FRAGMENT(i)
                }
                ENDCG
            }*/
	}
	
	
}
