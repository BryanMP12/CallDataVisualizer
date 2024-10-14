Shader "Map/UI/InvisibleStencilWriter"
{
    Properties
    {
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}


        [Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Stencil Comparison", Float) = 8
        [IntRange] _Stencil ("Stencil ID", Range(0, 255)) = 0
        [Enum(UnityEngine.Rendering.StencilOp)] _StencilOp ("Stencil Operation", float) = 0
        [IntRange] _StencilWriteMask ("Stencil Write Mask", Range(0, 255)) = 255
        [IntRange] _StencilReadMask ("Stencil Read Mask", Range(0, 255)) = 255
    }
    SubShader
    {

        Pass
        {
            Stencil
            {
                Ref [_Stencil]
                Comp [_StencilComp]
                Pass [_StencilOp]
                ReadMask [_StencilReadMask]
                WriteMask [_StencilWriteMask]
            }

            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f vtf) : SV_Target
            {
                half4 color = tex2D(_MainTex, vtf.texcoord) * vtf.color;
                return color;
            }
            ENDCG
        }
    }
}