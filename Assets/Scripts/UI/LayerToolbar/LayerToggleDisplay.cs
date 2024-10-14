using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.LayerToolbar {
    public sealed class LayerToggleDisplay : MonoBehaviour, IAddon {
        [SerializeField] SwitchButton showPointSwitch;
        [SerializeField] Slider pointSizeSlider;
        [SerializeField] SwitchButton showHeatmapSwitch;
        [SerializeField] Button updateHeatmapButton;
        [SerializeField] Slider intensity100Slider;
        [SerializeField] Slider intensity10Slider;
        [SerializeField] Slider intensity1Slider;
        [SerializeField] SwitchButton tractFillSwitch;
        [SerializeField] SwitchButton tractOutlineSwitch;
        [SerializeField] SwitchButton tractDigraphSwitch;
        Canvas canvas;
        public bool Enabled { get; set; }
        public void EnableDisplay() => canvas.enabled = Enabled = true;
        public void DisableDisplay() => canvas.enabled = Enabled = false;
        public void InitializePointButtonAction(Func<bool> pointAction, Action<float> sliderAction) {
            showPointSwitch.SetAction(delegate { showPointSwitch.SetVisual(pointAction.Invoke()); });
            pointSizeSlider.onValueChanged.AddListener(sliderAction.Invoke);
        }
        Action<float> intensityChanged;
        void UpdateIntensity() {
            float val = 0;
            val += intensity100Slider.value;
            val += intensity10Slider.value;
            val += intensity1Slider.value;
            intensityChanged?.Invoke(val);
        }
        public void InitializeHeatmapButtonAction(Func<bool> heatmapAction, Action updateHeatmapAction, Action<float> intensityChange, Action<float> thresholdAction) {
            showHeatmapSwitch.SetAction(delegate { showHeatmapSwitch.SetVisual(heatmapAction.Invoke()); });
            updateHeatmapButton.onClick.AddListener(updateHeatmapAction.Invoke);
            intensityChanged = intensityChange;
            intensity100Slider.onValueChanged.AddListener(_ => UpdateIntensity());
            intensity10Slider.onValueChanged.AddListener(_ => UpdateIntensity());
            intensity1Slider.onValueChanged.AddListener(_ => UpdateIntensity());
        }
        public void InitializeTractButtonAction(Func<bool> fillAction, Func<bool> outlineAction, Func<bool> digraphAction) {
            tractFillSwitch.SetAction(delegate { tractFillSwitch.SetVisual(fillAction.Invoke()); });
            tractOutlineSwitch.SetAction(delegate { tractOutlineSwitch.SetVisual(outlineAction.Invoke()); });
            tractDigraphSwitch.SetAction(delegate { tractDigraphSwitch.SetVisual(digraphAction.Invoke()); });
        }
        void Awake() { canvas = GetComponent<Canvas>(); }
    }
}