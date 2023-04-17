using Core.Points;
using General;
using UnityEngine;

namespace Core.Render.Heatmap {
    public static class HeatmapGenerator {
        static ComputeShader computeShader;
        static RenderTexture texture;
        static int KernelHandle;
        public static Texture InitializeHeatMap(ComputeShader shader) {
            computeShader = shader;
            int totalPointCount = PointsHolder.TotalPointCount;

            texture = new RenderTexture(Dims.Width, Dims.Height, 1) {
                enableRandomWrite = true,
                format = RenderTextureFormat.RFloat
            };
            texture.Create();

            KernelHandle = computeShader.FindKernel("CSMain");
            computeShader.SetTexture(KernelHandle, SPID._Output, texture);
            computeShader.SetInt(SPID._PointCount, totalPointCount);
            computeShader.SetFloat(SPID._Radius, 15);

            return texture;
        }
        public static void RenderHeatmap() { computeShader.Dispatch(KernelHandle, Dims.Width / 8, Dims.Height / 8, 1); }
        public static void ReleaseBuffer() {
            if (texture != null) texture.Release();
        }
        //0, 1, 2, 3, 4, 5, NonOfficer, Officer
        public static void SetVarsToRender(ComputeBuffer drawVarsBuffer) => computeShader.SetBuffer(KernelHandle, SPID._VarsToRender, drawVarsBuffer);
        public static void SetDescriptionsToRender(ComputeBuffer descriptionsBuffer) => computeShader.SetBuffer(KernelHandle, SPID._WriteDescription, descriptionsBuffer);
        public static void SetPointsBuffer(ComputeBuffer pointsBuffer) => computeShader.SetBuffer(KernelHandle, SPID._Points, pointsBuffer);
    }
}