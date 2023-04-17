using General;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers.CensusTracts {
    public partial class CensusTractManager {
        //true is count, false is ratio
        ColorRampType colorRampMode;
        //for count
        float maxVal;
        [SerializeField] Slider minSlider;
        [SerializeField] Slider maxSlider;
        [SerializeField] TextMeshProUGUI minText;
        [SerializeField] TextMeshProUGUI maxText;
        void InitializeSliders() {
            minSlider.onValueChanged.AddListener(OnMinValueChange);
            maxSlider.onValueChanged.AddListener(OnMaxValueChange);
            censusTractMaterial.SetFloat(SPID._Min, 0);
            censusTractMaterial.SetFloat(SPID._Max, 1);
        }
        enum ColorRampType { Count, Ratio }
        void UpdateColorRamp(ColorRampType type, float max) {
            colorRampMode = type;
            maxVal = max;
            OnMinValueChange(minSlider.value);
            OnMaxValueChange(maxSlider.value);
        }
        void OnMinValueChange(float sliderVal) {
            if (colorRampMode == ColorRampType.Count) {
                minText.SetText($"{(maxVal * sliderVal):##.00-}");
                censusTractMaterial.SetFloat(SPID._Min, sliderVal);
            } else {
                minText.SetText($"{sliderVal:##.00}%-");
                censusTractMaterial.SetFloat(SPID._Min, sliderVal);
            }
        }
        void OnMaxValueChange(float sliderVal) {
            if (colorRampMode == ColorRampType.Count) {
                maxText.SetText($"{(maxVal * sliderVal):##.00}+");
                censusTractMaterial.SetFloat(SPID._Max, sliderVal);
            } else {
                maxText.SetText($"{sliderVal:##.00}%+");
                censusTractMaterial.SetFloat(SPID._Max, sliderVal);
            }
        }
    }
}