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
                int officer_initiated;
            };

            StructuredBuffer<Point> _Points;
            float4 _ColorArray[6];
            //0 if no draw, 1 if draw
            StructuredBuffer<int> _WriteDescription; //200
            //0, 1, 2, 3, 4, 5, NonOfficerInitiated, OfficerInitiated
            StructuredBuffer<int> _VarsToRender; //8
            #define RenderNonOfficerInitiated _VarsToRender[6] == 1
            #define RenderOfficerInitiated _VarsToRender[7] == 1
            // float _RenderPriority[6] = {1, 1, 1, 1, 1, 1};
            // float _WriteDescription[200];
            // float _RenderNonOfficerInitiated = 1;
            // float _RenderOfficerInitiated = 1;

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