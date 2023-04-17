using UI.Gallery;
using UnityEngine;

namespace UI.FilterGallery {
    public class FilterElement : MonoBehaviour, IGalleryElement {
        [SerializeField] TextImage nameText;
        [SerializeField] TextImage countText;
        [SerializeField] TextImage nonOfficerInitiatedCountText;
        [SerializeField] TextImage officerInitiatedCountText;
        [SerializeField] TextImage[] priorityText;
        [SerializeField] SwitchButton switchButton;
        RectTransform rect;
        FilterElementRep filterRep;
        public void InitializeDisplay() {
            switchButton.SetAction(SwitchState);
            rect = GetComponent<RectTransform>();
        }
        public void UpdateVisual(bool val) {
            switchButton.SetVisual(val);
        }
        void SwitchState() => filterRep?.Switch();
        public void SetPosition(float position) => rect.anchoredPosition = new Vector2(0, position);
        public void SetVisualState(bool state) {
            //Is this needed since I have stencil draw?
            //Otherwise I have to call GetComponentsInChildren<Graphic> in awake and turn off all images
        }
        public void SetRep(ElementRep rep) {
            filterRep = (FilterElementRep) rep;
            nameText.SetText(filterRep.Name);
            countText.SetNumber(filterRep.TotalCount);
            officerInitiatedCountText.SetNumber(filterRep.OfficerInitiatedCount);
            nonOfficerInitiatedCountText.SetNumber(filterRep.NonOfficerInitiatedCount());
            for (int i = 0; i < 6; i++) priorityText[i].SetNumber(filterRep.PriorityCount[i]);
            switchButton.SetVisual(filterRep.SwitchedOn);
        }
        public void RemoveRep() => filterRep = null;
    }
}