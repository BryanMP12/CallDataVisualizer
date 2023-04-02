using System;
using General;
using UnityEngine;
using UnityEngine.UI;

namespace UI.RenderLayerToolbar {
    public class RenderDisplay : MonoBehaviour {
        [SerializeField] SwitchButton showPointSwitch;
        [SerializeField] Slider pointSizeSlider;

        [SerializeField] SwitchButton showHeatmapSwitch;
        [SerializeField] Button updateHeatmapButton;
        [SerializeField] Slider intensitySlider;
        [SerializeField] Slider thresholdSlider;

        [SerializeField] SwitchButton cameraSwitch;

        public void InitializePointButtonAction(Func<bool> pointAction, Action<float> sliderAction) {
            showPointSwitch.SetAction(delegate { showPointSwitch.SetVisual(pointAction.Invoke()); });
            pointSizeSlider.onValueChanged.AddListener(sliderAction.Invoke);
        }
        public void InitializeHeatmapButtonAction(Func<bool> heatmapAction, Action updateHeatmapAction, Action<float> intensityChange, Action<float> thresholdAction) {
            showHeatmapSwitch.SetAction(delegate { showHeatmapSwitch.SetVisual(heatmapAction.Invoke()); });
            updateHeatmapButton.onClick.AddListener(updateHeatmapAction.Invoke);
            intensitySlider.onValueChanged.AddListener(intensityChange.Invoke);
            thresholdSlider.onValueChanged.AddListener(thresholdAction.Invoke);
        }
        void Awake() {
            cameraSwitch.SetAction(delegate { cameraSwitch.SetVisual(CameraControl.ToggleCameraMovementState()); });
        }
    }
}