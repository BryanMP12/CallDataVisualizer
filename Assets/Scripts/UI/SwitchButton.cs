using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class SwitchButton : MonoBehaviour {
        TextMeshProUGUI text;
        RawImage image;
        Button button;
        [SerializeField] SwitchType type;
        Action OnSwitch;
        static readonly Color[] onOffColors = new Color[2] {new Color(1f, 0.1f, 0.3f), new Color(0.1f, 1, 0.3f)};
        static readonly Color[] oneOrderingColors = new Color[2] {Color.white, new Color(1, 0.5f, 0.1f)};
        static readonly Color[] twoOrderingColors = new Color[3] {Color.white, new Color(1, 0.5f, 0.1f), new Color(0.5f, 0.1f, 1),};
        void Awake() {
            text = GetComponentInChildren<TextMeshProUGUI>();
            image = GetComponent<RawImage>();
            button = GetComponent<Button>();
            button.onClick.AddListener(delegate { OnSwitch?.Invoke(); }); 
        }
        public void SetAction(Action a) => OnSwitch = a;
        void OnDisable() {
            button.onClick.RemoveAllListeners();
            OnSwitch = null;
        }
        public void SetVisual(bool b) => SetVisual(b ? 1 : 0);
        public void SetVisual(int num) {
            if (type is SwitchType.ON_OFF) text.SetText(num == 0 ? "OFF" : "ON");
            image.color =
                type switch {
                    SwitchType.ON_OFF       => onOffColors[num],
                    SwitchType.ONE_ORDERING => oneOrderingColors[num],
                    SwitchType.TWO_ORDERING => twoOrderingColors[num],
                    _                       => Color.black
                };
        }
        public void ForceReset() => SetVisual(0);
        enum SwitchType { ON_OFF, ONE_ORDERING, TWO_ORDERING }
    }
}