using General;
using TMPro;
using UnityEngine;

namespace Managers.RenderBuffers {
    public sealed partial class RenderBufferManager {
        [SerializeField] TextMeshProUGUI minText;
        [SerializeField] TextMeshProUGUI maxText;
        void InitializeSliders() { ChangeIntensity(1); }
        void ChangeIntensity(float intensity) {
            heatmapMaterial.SetFloat(SPID._Intensity, intensity);
            minText.SetText(0.ToString());
            maxText.SetText($"{((int) (1000 / intensity)).ToString()}+");
        }
    }
}