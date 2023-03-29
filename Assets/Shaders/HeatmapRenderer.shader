Shader "Map/HeatmapRenderer"
{
    Properties
    {
        _InputTexture ("Texture", 2D) = "white" {}
        _ColorRamp ("Color Ramp", 2D) = "white" {}
        _Intensity ("Intensity", Range(0, 20)) = 0
        _Threshold ("Threshold", Range(0, 0.2)) = 0
    }
    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
        Tags
        {
            "RenderType"="Transparent"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _InputTexture;
            sampler2D _ColorRamp;
            float _Intensity;
            float _Threshold;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                const float heat_value = tex2D(_InputTexture, i.uv).r * _Intensity;
                const float value = saturate(heat_value);
                return fixed4(tex2D(_ColorRamp, fixed2(value, 0.5)).rgb, saturate(heat_value - _Threshold));
            }
            ENDCG
        }
    }
}