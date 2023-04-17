using Core.Points;
using Core.Render.Heatmap;
using Core.Render.Points;
using UnityEngine;

namespace Core.Render {
    public static class PointRenderBuffer {
        //0, 1, 2, 3, 4, 5, nonOfficer, officer
        static readonly bool[] VarsToRender = new bool[8] {true, true, true, true, true, true, true, true};
        static ComputeBuffer pointBuffer;
        static ComputeBuffer drawDescriptionsBuffer;
        static ComputeBuffer drawVarsBuffer;
        public static void InitializeBuffers() {
            {
                drawVarsBuffer = new ComputeBuffer(8, sizeof(int));
                SetVarsToRender();
            }
            {
                //Initialize buffer with the given population
                int pointCount = PointsHolder.TotalPointCount;
                PointProperties[] properties = new PointProperties[pointCount];
                for (int i = 0; i < pointCount; i++) {
                    Point p = PointsHolder.Points[i];
                    properties[i] = new PointProperties(Dims.CoordToPort(p.Longitude, p.Latitude), p.Priority, p.DescriptionIndex, p.OfficerInitiated);
                }
                pointBuffer = new ComputeBuffer(pointCount, PointProperties.Stride);
                pointBuffer.SetData(properties);
                HeatmapGenerator.SetPointsBuffer(pointBuffer);
                PointRenderer.SetPointsBuffer(pointBuffer);
            }
            {
                drawDescriptionsBuffer = new ComputeBuffer(PointsHolder.Descriptions.Length, sizeof(int));
                bool[] properties = new bool[PointsHolder.Descriptions.Length];
                for (int i = 0; i < properties.Length; i++) properties[i] = true;
                SetDescriptionsToRender(properties);
            }
        }
        public static void SetDescriptionsToRender(bool[] descriptionsToRender) {
            int[] descriptions = new int[descriptionsToRender.Length];
            for (int i = 0; i < descriptions.Length; i++) descriptions[i] = descriptionsToRender[i] ? 1 : 0;
            drawDescriptionsBuffer.SetData(descriptions);
            HeatmapGenerator.SetDescriptionsToRender(drawDescriptionsBuffer);
            PointRenderer.SetDescriptionsToRender(drawDescriptionsBuffer);
        }
        static void SetVarsToRender() {
            int[] properties = new int[8];
            for (int i = 0; i < 8; i++) properties[i] = VarsToRender[i] ? 1 : 0;
            drawVarsBuffer.SetData(properties);
            HeatmapGenerator.SetVarsToRender(drawVarsBuffer);
            PointRenderer.SetVarsToRender(drawVarsBuffer);
        }
        public static void SetPrioritiesToRender(bool[] priorityToRender) {
            for (int i = 0; i < 6; i++) VarsToRender[i] = priorityToRender[i];
            SetVarsToRender();
        }
        public static void SetNonOfficerInitiatedToRender(bool b) {
            VarsToRender[6] = b;
            SetVarsToRender();
        }
        public static void SetOfficerInitiatedToRender(bool b) {
            VarsToRender[7] = b;
            SetVarsToRender();
        }
        public static void ReleaseBuffers() {
            drawDescriptionsBuffer?.Release();
            drawDescriptionsBuffer = null;
            pointBuffer?.Release();
            pointBuffer = null;
            drawVarsBuffer?.Release();
            drawVarsBuffer = null;
        }
    }
}