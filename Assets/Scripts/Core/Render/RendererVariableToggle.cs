using Core.Render.Heatmap;
using Core.Render.Points;

namespace Core.Render {
    public static class RendererVariableToggle {
        //0, 1, 2, 3, 4, 5, nonOfficer, officer
        static readonly bool[] VarsToRender = new bool[8] {true, true, true, true, true, true, true, true};
        public static void SetDescriptionsToRender(bool[] descriptionsToRender) {
            HeatmapGenerator.SetDescriptionsToRender(descriptionsToRender);
            PointRenderer.SetDescriptionsToRender(descriptionsToRender);
        }
        public static void SetPrioritiesToRender(bool[] priorityToRender) {
            for (int i = 0; i < 6; i++) VarsToRender[i] = priorityToRender[i];
            HeatmapGenerator.SetVarsToRender(VarsToRender);
            PointRenderer.SetVarsToRender(VarsToRender);
        }
        public static void SetNonOfficerInitiatedToRender(bool b) {
            VarsToRender[6] = b;
            HeatmapGenerator.SetVarsToRender(VarsToRender);
            PointRenderer.SetVarsToRender(VarsToRender);
        }
        public static void SetOfficerInitiatedToRender(bool b) {
            VarsToRender[7] = b;
            HeatmapGenerator.SetVarsToRender(VarsToRender);
            PointRenderer.SetVarsToRender(VarsToRender);
        }
    }
}