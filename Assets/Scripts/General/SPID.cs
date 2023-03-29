using UnityEngine;

namespace General {
    public static class SPID {
        public static readonly int _Points = Shader.PropertyToID("_Points");
        public static readonly int _PointCount = Shader.PropertyToID("_PointCount");
        public static readonly int _Radius = Shader.PropertyToID("_Radius");
        public static readonly int _Output = Shader.PropertyToID("_Output");
        public static readonly int _ColorArray = Shader.PropertyToID("_ColorArray");
        public static readonly int _WriteDescription = Shader.PropertyToID("_WriteDescription");
        public static readonly int _RenderPriority = Shader.PropertyToID("_RenderPriority");
        public static readonly int _RenderNonOfficerInitiated = Shader.PropertyToID("_RenderNonOfficerInitiated");
        public static readonly int _RenderOfficerInitiated = Shader.PropertyToID("_RenderOfficerInitiated");
        public static readonly int _InputTexture = Shader.PropertyToID("_InputTexture");
        public static readonly int _VarsToRender = Shader.PropertyToID("_VarsToRender");
    }
}