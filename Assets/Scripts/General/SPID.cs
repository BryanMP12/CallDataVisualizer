using UnityEngine;

namespace General {
    public static class SPID {
        public static readonly int _Points = Shader.PropertyToID("_Points");
        public static readonly int _PointCount = Shader.PropertyToID("_PointCount");
        public static readonly int _Radius = Shader.PropertyToID("_Radius");
        public static readonly int _Output = Shader.PropertyToID("_Output");
        public static readonly int _ColorArray = Shader.PropertyToID("_ColorArray");
        public static readonly int _RatioArray = Shader.PropertyToID("_RatioArray");
        public static readonly int _WriteDescription = Shader.PropertyToID("_WriteDescription");
        public static readonly int _Min = Shader.PropertyToID("_Min");
        public static readonly int _Max = Shader.PropertyToID("_Max");
        public static readonly int _RenderPriority = Shader.PropertyToID("_RenderPriority");
        public static readonly int _RenderNonOfficerInitiated = Shader.PropertyToID("_RenderNonOfficerInitiated");
        public static readonly int _RenderOfficerInitiated = Shader.PropertyToID("_RenderOfficerInitiated");
        public static readonly int _InputTexture = Shader.PropertyToID("_InputTexture");
        public static readonly int _VarsToRender = Shader.PropertyToID("_VarsToRender");
        public static readonly int _Intensity = Shader.PropertyToID("_Intensity");
        public static readonly int _Threshold = Shader.PropertyToID("_Threshold");
        public static readonly int _Map = Shader.PropertyToID("_Map");
    }
}