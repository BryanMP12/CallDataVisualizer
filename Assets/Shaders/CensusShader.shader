Shader "Map/CensusShader"
{
    Properties
    {
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}
        _ColorLow ("Color Low", Color) = (0, 0, 0, 0)
        _ColorHigh ("Color High", Color) = (0, 0, 0, 0)
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

            fixed4 _ColorLow;
            fixed4 _ColorHigh;
            float _RatioArray[300];
            
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return lerp(_ColorLow, _ColorHigh, _RatioArray[i.uv.x]);
            }
            ENDCG
        }
    }
}
