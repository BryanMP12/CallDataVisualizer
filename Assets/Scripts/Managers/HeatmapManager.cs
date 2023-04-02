using Core.Points;
using Core.Render.Heatmap;
using General;
using UI.RenderLayerToolbar;
using UnityEngine;

namespace Managers {
    public class HeatmapManager : MonoBehaviour {
        [SerializeField] Material heatmapMaterial;
        [SerializeField] ComputeShader computeShader;
        [SerializeField] MeshRenderer mesh;
        [SerializeField] RenderDisplay renderDisplay;
        static HeatmapManager instance;
        void OnEnable() { PointsHolder.DataSet += Initialize; }
        void OnDisable() {
            PointsHolder.DataSet -= Initialize;
            HeatmapGenerator.ReleaseBuffer();
        }
        void Initialize() {
            Texture heatTexture = HeatmapGenerator.InitializeHeatMap(computeShader);
            heatmapMaterial.SetTexture(SPID._InputTexture, heatTexture);
            renderDisplay.InitializeHeatmapButtonAction(
                delegate { return mesh.enabled = !mesh.enabled; },
                HeatmapGenerator.RenderHeatmap,
                delegate(float f) { heatmapMaterial.SetFloat(SPID._Intensity, f); },
                delegate(float f) { heatmapMaterial.SetFloat(SPID._Threshold, f); });
        }
        [ContextMenu("UpdateHeatmap")]
        public void UpdateHeatmap() => HeatmapGenerator.RenderHeatmap();
    }
}