Shader "Custom/UI_Brightness"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            
            float GetV(float3 c) 
            {
                return max(c.r, max(c.g, c.b));
            }


            float ApplyBrightnessChannel(float base, float brightness)
            {
                float blend = brightness * 0.5 + 0.5;

                float g1 = base - base * base;
                float g2 = sqrt(base) - base;
                float g  = lerp(g1, g2, step(0.5, blend));

                float soft = base + brightness * g;

                return lerp(base, soft, 1.65);
            }

            float4 ApplyBrightness(float4 base, float brightness) 
            {
                return float4(
                    ApplyBrightnessChannel(base.r, brightness),
                    ApplyBrightnessChannel(base.g, brightness),
                    ApplyBrightnessChannel(base.b, brightness),
                    base.a
                );
            }



            struct appdata_t
            {
                float4 vertex   : POSITION;
                float2 texcoord : TEXCOORD0;
                fixed4 color    : COLOR;   // UI Image.color
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv     : TEXCOORD0;
                fixed4 color  : COLOR;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv     = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.color  = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 color0 = tex2D(_MainTex, i.uv);
                fixed4 color1 = i.color;

                fixed brightness = GetV(color1.rgb) * 2 - 1;
                
                return ApplyBrightness(color0, brightness);
            }
            ENDCG
        }
    }
}
