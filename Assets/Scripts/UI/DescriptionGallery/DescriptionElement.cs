using System;
using TMPro;
using UI.Gallery;
using UnityEngine;
using UnityEngine.UI;

namespace UI.DescriptionGallery {
    public class DescriptionElement : MonoBehaviour, IGalleryElement {
        [SerializeField] TextImage nameText;
        [SerializeField] TextImage countText;
        [SerializeField] TextImage officerInitiatedCountText;
        [SerializeField] TextImage[] priorityText;
        [SerializeField] TextImage switchButtonText;
        [SerializeField] Button switchButton;
        RectTransform rect;
        DescriptionElementRep descriptionRep;
        public void InitializeDisplay() {
            switchButton.onClick.AddListener(SwitchState);
            rect = GetComponent<RectTransform>();
        }
        void SwitchState() {
            if (descriptionRep == null) return;
            SetSwitchedState(descriptionRep.Switch());
        }
        public void SetPosition(float position) => rect.anchoredPosition = new Vector2(0, position);
        public void SetVisualState(bool state) {
            //Is this needed since I have stencil draw?
            //Otherwise I have to call GetComponentsInChildren<Graphic> in awake and turn off all images
        }
        public void SetRep(ElementRep rep) {
            descriptionRep = (DescriptionElementRep) rep;
            nameText.SetText(descriptionRep.Name);
            countText.SetNumber(descriptionRep.TotalCount);
            officerInitiatedCountText.SetNumber(descriptionRep.OfficerInitiatedCount);
            for (int i = 0; i < 6; i++) priorityText[i].SetNumber(descriptionRep.PriorityCount[i]);
            SetSwitchedState(descriptionRep.SwitchedOn);
        }
        void SetSwitchedState(bool on) {
            if (on) {
                switchButtonText.SetText("ON");
                switchButtonText.SetColor(Color.black, new Color(0.1f, 1, 0.3f));
            } else {
                switchButtonText.SetText("OFF");
                switchButtonText.SetColor(Color.black, new Color(1, 0.1f, 0.3f));
            }
        }
        public void RemoveRep() => descriptionRep = null;
        [Serializable]
        struct TextImage {
            public TextMeshProUGUI text;
            public RawImage image;
            public void SetText(string t) => text.SetText(t);
            public void SetNumber(int num) {
                text.SetText(num.ToString());
                if (num == 0) image.color = new Color(0.2f, 0.2f, 0.2f, 1);
                else image.color = Color.white;
            }
            public void SetColor(Color textColor, Color imageColor) {
                text.color = textColor;
                image.color = imageColor;
            }
        }
    }
}