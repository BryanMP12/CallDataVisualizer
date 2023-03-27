namespace Core.Render.Heatmap {
    public static class HeatmapGenerator {
        // public static Texture GenerateHeatMap(ComputeShader shader) {
        //     int totalPointCount = PointsHolder.TotalPointCount;
        //     Vector2[] properties = new Vector2[FilterManager.FilteredListCount()];
        //     int index = 0;
        //     foreach (Point p in FilterManager.FilteredList()) {
        //         properties[index] = Dims.CoordToPort(p.Longitude, p.Latitude);
        //         index++;
        //     }
        //     ComputeBuffer buffer = new ComputeBuffer(totalPointCount, sizeof(float) * 2);
        //     buffer.SetData(properties);
        //
        //     int kernelHandle = shader.FindKernel("CSMain");
        //     RenderTexture tex = new RenderTexture(Dims.Width, Dims.Height, 1) {
        //         enableRandomWrite = true,
        //         format = RenderTextureFormat.RFloat
        //     };
        //     tex.Create();
        //
        //     shader.SetTexture(kernelHandle, "output", tex);
        //     shader.SetBuffer(kernelHandle, "points", buffer);
        //     shader.SetInt("point_count", totalPointCount);
        //     shader.SetFloat("radius", 15);
        //     shader.Dispatch(kernelHandle, Dims.Width / 8, Dims.Height / 8, 1);
        //
        //     buffer.Release();
        //     return tex;
        // }
    }
}