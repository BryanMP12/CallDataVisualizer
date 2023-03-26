Shader "Map/UI/InvisibleStencilWriter"
{
    Properties
    {
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}
        [IntRange] _StencilRef ("Stencil Reference", Range(0, 255)) = 0
    }
    SubShader
    {

        Pass
        {
            Stencil
            {
                Ref [_StencilRef]
                Comp NotEqual
                Pass Replace
            }
            ColorMask 0

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 vert(float4 vertex : POSITION) : SV_POSITION
            {
                return UnityObjectToClipPos(vertex);
            }

            void frag()
            {
            }
            ENDCG
        }
    }
}