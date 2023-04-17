Shader "Map/CensusShader"
{
    Properties
    {
        _ColorRamp ("Color Ramp", 2D) = "white" {}
        _Min ("Min", Range(0, 1)) = 0
        _Max ("Max", Range(0, 1)) = 1
    }
    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float _RatioArray[300];
            sampler2D _ColorRamp;
            float _Min, _Max;

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                const float val = (clamp(_Min, _Max, _RatioArray[i.uv.x]) - _Min) / (_Max - _Min);
                fixed3 col = tex2D(_ColorRamp, val).rgb;
                return fixed4(col.rgb, 0.2);
            }
            ENDCG
        }
    }
}