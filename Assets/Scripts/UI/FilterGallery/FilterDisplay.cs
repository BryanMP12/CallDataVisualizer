using System;
using System.Text;
using General;
using UI.Gallery;
using UnityEngine;

namespace UI.FilterGallery {
    public sealed class FilterDisplay : PooledGallery<FilterElementRep>, IAddon {
        Canvas canvas;
        [Header("Titles")] [SerializeField] SwitchButton nameOrderButton;
        [SerializeField] SwitchButton totalCountOrderButton;
        [SerializeField] SwitchButton nonOfficerInitiatedOrderButton;
        [SerializeField] SwitchButton officerInitiatedOrderButton;
        [SerializeField] SwitchButton[] priorityOrderButton;
        [SerializeField] SwitchButton descriptionToggleButton;
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
        public void InitializeOrders(Action nameOrderT, Action countOrderT, Action nonOfficerOrderT,
            Action officerOrderT, Action[] priorityOrderT) {
            nameOrderButton.SetAction(delegate {
                ResetAllOrderButtonVisuals();
                nameOrderT.Invoke();
                nameOrderButton.SetVisual(1);
            });
            totalCountOrderButton.SetAction(delegate {
                ResetAllOrderButtonVisuals();
                countOrderT.Invoke();
                {
                    StringBuilder builder = new StringBuilder();
                    StringBuilder priorityBuilder = new StringBuilder();
                    int count = 0;
                    const int maxCount = 30;
                    foreach (FilterElementRep rep in Reps) {
                        priorityBuilder.Clear();
                        priorityBuilder.Append("$");
                        for (int i = 5; i >= 0; i--) {
                            if (rep.PriorityCount[i] == 0) continue;
                            priorityBuilder.Append($"{i}|{rep.PriorityCount[i]}\\ ");
                        }
                        priorityBuilder.Append("$");
                        builder.Append(
                            $"{rep.Name.Replace("&", "\\&")} & {rep.TotalCount} & {rep.NonOfficerInitiatedCount()} & {priorityBuilder.ToString()} &  \\\\ \n");
                        
                        count++;
                        if (count >= maxCount) break;
                    }
                    Debug.Log(builder.ToString());
                }
                totalCountOrderButton.SetVisual(1);
            });
            nonOfficerInitiatedOrderButton.SetAction(delegate {
                ResetAllOrderButtonVisuals();
                nonOfficerOrderT.Invoke();
                nonOfficerInitiatedOrderButton.SetVisual(1);
            });
            officerInitiatedOrderButton.SetAction(delegate {
                ResetAllOrderButtonVisuals();
                officerOrderT.Invoke();
                officerInitiatedOrderButton.SetVisual(1);
            });
            for (int i = 0; i < 6; i++) {
                int index = i;
                priorityOrderButton[i].SetAction(delegate {
                    ResetAllOrderButtonVisuals();
                    priorityOrderT[index].Invoke();
                    priorityOrderButton[index].SetVisual(1);
                });
            }
            descriptionToggleButton.SetAction(delegate {
                toggled = !toggled;
                foreach (FilterElementRep rep in Reps)
                    if (toggled != rep.SwitchedOn)
                        rep.Switch();
            });
        }
        bool toggled;
        public void ResetAllOrderButtonVisuals() {
            nameOrderButton.ResetVisual();
            totalCountOrderButton.ResetVisual();
            nonOfficerInitiatedOrderButton.ResetVisual();
            officerInitiatedOrderButton.ResetVisual();
            for (int i = 0; i < 6; i++) priorityOrderButton[i].ResetVisual();
        }
        public void InitializeToggles(Func<bool> nonOfficerInitiatedT, Func<bool> officerInitiatedT,
            Func<bool>[] priorityT) {
            nonOfficerInitiatedToggle.SetAction(delegate {
                nonOfficerInitiatedToggle.SetVisual(nonOfficerInitiatedT.Invoke());
            });
            officerInitiatedToggle.SetAction(delegate {
                officerInitiatedToggle.SetVisual(officerInitiatedT.Invoke());
            });
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
            InputManager.ScrollDeltaUI += MoveOffset;
            FilterElementRep.SetRectLimits(new Vector2(-parent.rect.height, 0));
            MoveOffset(0);
        }
        public void DisableDisplay() {
            canvas.enabled = Enabled = false;
            InputManager.ScrollDeltaUI -= MoveOffset;
        }
        public bool Enabled { get; set; }
    }
}