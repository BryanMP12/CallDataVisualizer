using System;
using TMPro;
using UnityEngine;

namespace UI.CensusTracts {
    public sealed class TractViewerElement : MonoBehaviour {
        SwitchButton switchButton;
        TextMeshProUGUI titleNameText;
        TextMeshProUGUI totalValue;
        TextMeshProUGUI averageValue;
        TextMeshProUGUI selectedTractValue;
        void Awake() {
            switchButton = GetComponent<SwitchButton>();
            titleNameText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            totalValue = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            averageValue = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            selectedTractValue = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        }
        public void SetButtonAction(Action action) => switchButton.SetAction(action);
        public void SetVisual(int val) => switchButton.SetVisual(val);
        public void SetTotalAndAverage(int total, int count) {
            totalValue.SetText(total.ToString());
            averageValue.SetText(((float) total / count).ToString("##.00"));
        }
        public void SetSelectedValue(int value) => selectedTractValue.SetText(value.ToString());
    }
}