Shader "Map/PointRenderer"
{
    Properties
    {
        _Circle ("Circle", 2D) = "white" {}
    }
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct Point
            {
                float2 position;
                int priority;
                int description_index;
                int officer_initiated;
            };


            sampler2D _Circle;
            StructuredBuffer<Point> _Points;
            float4 _ColorArray[6];
            //0 if no draw, 1 if draw
            StructuredBuffer<int> _WriteDescription; //200
            //0, 1, 2, 3, 4, 5, NonOfficerInitiated, OfficerInitiated
            StructuredBuffer<int> _VarsToRender; //8
            #define RenderNonOfficerInitiated _VarsToRender[6] == 1
            #define RenderOfficerInitiated _VarsToRender[7] == 1

            v2f vert(appdata_t i, const uint instanceID: SV_InstanceID)
            {
                v2f o;
                const float3 position = float3(_Points[instanceID].position + i.vertex.xy, 0);
                o.vertex = UnityObjectToClipPos(position);
                const int priority_index = _Points[instanceID].priority;
                o.color = _ColorArray[priority_index];
                const bool officer_initiated = _Points[instanceID].officer_initiated > 0.5;
                const bool render =
                    _WriteDescription[_Points[instanceID].description_index] == 1
                    && (!officer_initiated && RenderNonOfficerInitiated || officer_initiated && RenderOfficerInitiated)
                    && _VarsToRender[priority_index] == 1;
                o.color.a = render ? 1 : 0;
                o.uv = i.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return tex2D(_Circle, i.uv).a * i.color;
            }
            ENDCG
        }
    }
}