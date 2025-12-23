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

            
            float getV(float3 c) 
            {
                return max(c.r, max(c.g, c.b));
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

                float alpha = color0.a * color1.a;

                fixed value = getV(color1.rgb) * 2 - 1;
                fixed3 bw = value > 0 ? fixed3(1,1,1) : fixed3(0,0,0);
                
                fixed3 rgb = lerp(color0.rgb, bw, abs(value));


                return fixed4(rgb, alpha);
            }
            ENDCG
        }
    }
}
