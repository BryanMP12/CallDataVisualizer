using Core.PointHolder;
using General;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Render.Heatmap {
    public class HeatmapManager : MonoBehaviour {
        [SerializeField] Material heatmapMaterial;
        [SerializeField] ComputeShader computeShader;
        [SerializeField] Button showHeatmap;
        [SerializeField] MeshRenderer mesh;
        static HeatmapManager instance;
        void OnEnable() {
            PointsHolder.DataSet += Initialize; 
            showHeatmap.onClick.AddListener(delegate { mesh.enabled = !mesh.enabled;});
        }
        void OnDisable() {
            PointsHolder.DataSet -= Initialize;
            HeatmapGenerator.ReleaseBuffer();
        }
        void Initialize() {
            Texture heatTexture = HeatmapGenerator.InitializeHeatMap(computeShader);
            heatmapMaterial.SetTexture(SPID._InputTexture, heatTexture);
        }
        [ContextMenu("UpdateHeatmap")]
        public void UpdateHeatmap() => HeatmapGenerator.RenderHeatmap();
    }
}