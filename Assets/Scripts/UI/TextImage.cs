using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    [Serializable]
    internal struct TextImage {
        public TextMeshProUGUI text;
        public RawImage image;
        public void SetText(string t) => text.SetText(t);
        public void SetNumber(int num) {
            text.SetText(num.ToString());
            if (num == 0) image.color = new Color(0.4f, 0.4f, 0.4f, 1);
            else image.color = Color.white;
        }
        public void SetColor(Color textColor, Color imageColor) {
            text.color = textColor;
            image.color = imageColor;
        }
    }
    [Serializable]
    internal struct ImageButton {
        public RawImage image;
        public Button button;
        public void SubscribeAction(Action action) => button.onClick.AddListener(delegate { action?.Invoke(); });
        public void UnsubscribeButton() => button.onClick.RemoveAllListeners();
    }
}