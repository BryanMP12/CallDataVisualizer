using System;
using General;
using UI.Gallery;
using UnityEngine;

namespace UI.FilterGallery {
    public class FilterDisplay : PooledGallery<FilterElementRep>, IAddon {
        Canvas canvas;
        [Header("Order Tiles")] [SerializeField]
        TextImage orderCategory;
        [SerializeField] TextImage orderType;
        //
        [Header("Titles")] [SerializeField] SwitchButton nameTitle;
        [SerializeField] SwitchButton totalCountTitle;
        [SerializeField] SwitchButton nonOfficerInitiatedTitle;
        [SerializeField] SwitchButton officerInitiatedTitle;
        [SerializeField] SwitchButton[] priorityTitle;
        //
        [Header("Total Count")] [SerializeField]
        TextImage totalCountText;
        [SerializeField] TextImage nonOfficerInitiatedCountText;
        [SerializeField] TextImage officerInitiatedCountText;
        [SerializeField] TextImage[] priorityCountText;
        //
        [Header("Toggle Categories")] [SerializeField]
        SwitchButton nonOfficerInitiatedToggle;
        [SerializeField] SwitchButton officerInitiatedToggle;
        [SerializeField] SwitchButton[] priorityToggles;
        void Awake() {
            canvas = GetComponent<Canvas>();
            Enabled = false;
        }
        public void InitializeToggles(Func<bool> nonOfficerInitiatedT, Func<bool> officerInitiatedT, Func<bool>[] priorityT) {
            nonOfficerInitiatedToggle.SetAction(delegate { nonOfficerInitiatedToggle.SetVisual(nonOfficerInitiatedT.Invoke()); });
            officerInitiatedToggle.SetAction(delegate { officerInitiatedToggle.SetVisual(officerInitiatedT.Invoke()); });
            for (int i = 0; i < 6; i++) {
                int index = i;
                priorityToggles[i].SetAction(delegate { priorityToggles[index].SetVisual(priorityT[index].Invoke()); });
            }
        }
        public void SetCount(int tc, int nonOic, int oic, int[] pc) {
            totalCountText.SetNumber(tc);
            nonOfficerInitiatedCountText.SetNumber(nonOic);
            officerInitiatedCountText.SetNumber(oic);
            for (int i = 0; i < 6; i++) priorityCountText[i].SetNumber(pc[i]);
        }
        public void EnableDisplay() {
            canvas.enabled = Enabled = true;
            MouseInput.ScrollDelta += MoveOffset;
            FilterElementRep.SetRectLimits(new Vector2(-parent.rect.height, 0));
            MoveOffset(0);
        }
        public void DisableDisplay() {
            canvas.enabled = Enabled = false;
            MouseInput.ScrollDelta -= MoveOffset;
        }
        public bool Enabled { get; set; }
    }
}