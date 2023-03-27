using Core.PointHolder;
using Core.Render.Points;
using UI.DescriptionGallery;
using UnityEngine;

namespace Managers {
    public class DescriptionManager : MonoBehaviour {
        DescriptionDisplay descriptionDisplay;
        bool[] descriptionsToRender;
        void Awake() => descriptionDisplay = GetComponent<DescriptionDisplay>();
        void OnEnable() => PointsHolder.DataSet += OnDataSet;
        void OnDisable() => PointsHolder.DataSet -= OnDataSet;
        void OnDataSet() {
            descriptionsToRender = new bool[PointsHolder.Descriptions.Length];
            for (int i = 0; i < PointsHolder.Descriptions.Length; i++) {
                DescriptionCount dc = PointsHolder.Descriptions[i];
                descriptionDisplay.AddRep(NewRep(dc, i));
                descriptionsToRender[i] = true;
            }
        }
        void UpdateDescriptionToRender(int index, bool state) {
            descriptionsToRender[index] = state;
            PointRenderer.SetDescriptions(descriptionsToRender);
        }
        DescriptionElementRep NewRep(DescriptionCount dc, int index) =>
            new DescriptionElementRep(dc.Name, dc.TotalCount, dc.OfficerInitiatedCount, dc.PriorityCount, delegate(bool b) { UpdateDescriptionToRender(index, b); });
    }
}