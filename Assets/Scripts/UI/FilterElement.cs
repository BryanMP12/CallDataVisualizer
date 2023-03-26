using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class FilterElement : MonoBehaviour {
        Button button;
        RawImage buttonImage;
        [SerializeField] TextMeshProUGUI filterNameText;
        [SerializeField] TextMeshProUGUI countText;
        const float ButtonWidth = 125;
        const float ButtonHeight = 30;
        void Awake() {
            button = GetComponent<Button>();
            buttonImage = GetComponent<RawImage>();
        }
        public void Initialize(int column, int row, string filterName, int count, Action clickAction) {
            GetComponent<RectTransform>().position = new Vector2(column * ButtonWidth, (row + 1) * ButtonHeight);
            filterNameText.SetText(filterName);
            countText.SetText(count.ToString());
            button.onClick.AddListener(delegate {
                clickAction?.Invoke();
                buttonImage.color = (buttonImage.color.r > 0.5f) ? new Color(0.5f, 0.5f, 0.5f, 1) : Color.white;
            });
        }
        void OnDisable() { button.onClick.RemoveAllListeners(); }
    }
}