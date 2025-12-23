Shader "Custom/SpriteBrightness"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Brightness ("Brightness", Range(-1,1)) = 0.0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Lighting Off
        ZWrite Off
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Brightness;


            
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


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);

                return ApplyBrightness(texColor * i.color, _Brightness);
            }
            ENDCG
        }
    }
}
