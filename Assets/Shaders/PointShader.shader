Shader "Map/InstancedPointShader"
{
    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
        Tags
        {
            "RenderType" = "Transparent"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            struct Point
            {
                float2 position;
                int priority;
                int description_index;
            };

            StructuredBuffer<Point> _Points;
            uniform float4 _ColorArray[6];
            //0 if no draw, 1 if draw
            float _WriteDescription[200];

            v2f vert(appdata_t i, const uint instanceID: SV_InstanceID)
            {
                v2f o;
                const float3 position = float3(_Points[instanceID].position + i.vertex.xy, 0);
                o.vertex = UnityObjectToClipPos(position);
                o.color = _ColorArray[_Points[instanceID].priority];
                o.color.a = _WriteDescription[_Points[instanceID].description_index];
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return i.color;
            }
            ENDCG
        }
    }
}