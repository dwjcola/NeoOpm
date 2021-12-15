Shader "Custom_Shader/Outlight"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LightColor ("Light Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _Size ("Size", Int) = 1 
    }

    SubShader {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }
        Blend SrcAlpha OneMinusSrcAlpha

        CGINCLUDE
        #include "UnityCG.cginc"

        struct v2f {
            float4 pos : SV_POSITION;
            float2 uv: TEXCOORD0;
        };

        sampler2D _MainTex;
        float4 _MainTex_ST;
        half4 _MainTex_TexelSize;
        float4 _LightColor;
        int _Size;

        ENDCG

        Pass {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            v2f vert(appdata_img IN)
            {
                v2f OUT;
                OUT.pos = UnityObjectToClipPos(IN.vertex);
                OUT.uv = TRANSFORM_TEX(IN.texcoord, _MainTex);
                return OUT;
            }

            /*fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 color = tex2D(_MainTex, IN.uv);
                fixed4 c = _LightColor;
                float sum = tex2D(_MainTex, IN.uv).a;
                for (int i = 1; i <= _Size; i ++) {
                    sum += tex2D(_MainTex, IN.uv + _MainTex_TexelSize.xy * half2(i, 0)).a;
                    sum += tex2D(_MainTex, IN.uv + _MainTex_TexelSize.xy * half2(-1 * i, 0)).a;
                    sum += tex2D(_MainTex, IN.uv + _MainTex_TexelSize.xy * half2(0, 1 * i)).a;
                    sum += tex2D(_MainTex, IN.uv + _MainTex_TexelSize.xy * half2(0, -1 * i)).a;
                }

                c.a = sum / (4 * _Size + 1);

                return step(0.1, color.a) * color + step(0.1, 1-color.a) * c;
            }*/
            fixed4 frag(v2f IN) : SV_TARGET
            {
                fixed4 color = tex2D(_MainTex, IN.uv);
                 if (_Size > 0) 
                {
                    fixed4 c = _LightColor;
                    float sum = tex2D(_MainTex, IN.uv).a;
                    for (int i = -_Size ; i <= _Size; i ++) {
                        for (int j = -_Size; j <= _Size; j ++) {
                            if (abs(i) != abs(j))
                            {
                                sum += tex2D(_MainTex, IN.uv + _MainTex_TexelSize.xy * half2(i, j)).a;
                            }       
                        }
                    }
    
                    c.a = sum / (_Size * _Size * 2);
    
                    return step(0.1, color.a) * color + step(0.1, 1-color.a) * c;
                }else
                {
                    return color;
                }
                
            }

            ENDCG
        }
    } 
}