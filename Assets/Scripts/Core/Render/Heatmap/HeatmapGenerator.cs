using Core.Points;
using General;
using UnityEngine;

namespace Core.Render.Heatmap {
    public static class HeatmapGenerator {
        static ComputeShader computeShader;
        static ComputeBuffer pointBuffer;
        static ComputeBuffer drawVarsBuffer; //0, 1, 2, 3, 4, 5, nonOfficer, officer
        static ComputeBuffer drawDescriptionsBuffer;
        static RenderTexture texture;
        static int KernelHandle;
        public static Texture InitializeHeatMap(ComputeShader shader) {
            computeShader = shader;
            int totalPointCount = PointsHolder.TotalPointCount;
            {
                PointProperties[] properties = new PointProperties[totalPointCount];
                for (int i = 0; i < PointsHolder.Points.Length; i++) {
                    Point p = PointsHolder.Points[i];
                    properties[i] = new PointProperties(Dims.CoordToPort(p.Longitude, p.Latitude), p.Priority, p.DescriptionIndex, p.OfficerInitiated);
                }
                pointBuffer = new ComputeBuffer(totalPointCount, PointProperties.Stride);
                pointBuffer.SetData(properties);
            }
            {
                drawVarsBuffer = new ComputeBuffer(8, sizeof(int));
                SetVarsToRender(new bool[8] {true, true, true, true, true, true, true, true});
            }
            {
                drawDescriptionsBuffer = new ComputeBuffer(PointsHolder.Descriptions.Length, sizeof(int));
                bool[] descriptionsToRender = new bool[PointsHolder.Descriptions.Length];
                for (int i = 0; i < descriptionsToRender.Length; i++) descriptionsToRender[i] = true;
                SetDescriptionsToRender(descriptionsToRender);
            }

            texture = new RenderTexture(Dims.Width, Dims.Height, 1) {
                enableRandomWrite = true,
                format = RenderTextureFormat.RFloat
            };
            texture.Create();

            KernelHandle = computeShader.FindKernel("CSMain");
            computeShader.SetTexture(KernelHandle, SPID._Output, texture);
            computeShader.SetBuffer(KernelHandle, SPID._Points, pointBuffer);
            computeShader.SetInt(SPID._PointCount, totalPointCount);
            computeShader.SetFloat(SPID._Radius, 15);

            RenderHeatmap();

            return texture;
        }
        public static void RenderHeatmap() { computeShader.Dispatch(KernelHandle, Dims.Width / 8, Dims.Height / 8, 1); }
        public static void ReleaseBuffer() {
            pointBuffer?.Release();
            drawVarsBuffer?.Release();
            drawDescriptionsBuffer?.Release();
            if (texture != null) texture.Release();
        }
        //0, 1, 2, 3, 4, 5, NonOfficer, Officer
        public static void SetVarsToRender(bool[] vars) {
            int[] properties = new int[8];
            for (int i = 0; i < 8; i++) properties[i] = vars[i] ? 1 : 0;
            drawVarsBuffer.SetData(properties);
            computeShader.SetBuffer(KernelHandle, SPID._VarsToRender, drawVarsBuffer);
        }
        public static void SetDescriptionsToRender(bool[] descriptionsToRender) {
            int[] descriptions = new int[descriptionsToRender.Length];
            for (int i = 0; i < descriptions.Length; i++) descriptions[i] = descriptionsToRender[i] ? 1 : 0;
            drawDescriptionsBuffer.SetData(descriptions);
            computeShader.SetBuffer(KernelHandle, SPID._WriteDescription, drawDescriptionsBuffer);
        }
    }
}