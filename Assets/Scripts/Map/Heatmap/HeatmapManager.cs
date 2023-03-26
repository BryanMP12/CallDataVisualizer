using UnityEngine;

namespace Map.Heatmap {
    public class HeatmapManager : MonoBehaviour {
        [SerializeField] Material heatmapMaterial;
        [SerializeField] ComputeShader computeShader;
        static HeatmapManager instance;
        void Awake() => instance = this;
        public static void GenerateHeatmap() => instance.GenerateHeatmapInternal();
        void GenerateHeatmapInternal() {
            Texture heatTexture = HeatmapGenerator.GenerateHeatMap(computeShader);
            heatmapMaterial.SetTexture("_InputTexture", heatTexture);
        }
    }
}