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

                // ��⿡ ���� �� ���� ��� ����
                fixed3 targetColor = _Brightness > 0 ? fixed3(1,1,1) : fixed3(0,0,0);
                float strength = abs(_Brightness);

                // RGB�� ��⿡ ���� ���� �Ǵ� ������� ����
                fixed3 finalRGB = lerp(texColor.rgb, targetColor, strength);

                // ���Ĵ� ���� ���� �� SpriteRenderer.color.a
                float finalAlpha = texColor.a * i.color.a;

                return fixed4(finalRGB, finalAlpha);
            }
            ENDCG
        }
    }
}
