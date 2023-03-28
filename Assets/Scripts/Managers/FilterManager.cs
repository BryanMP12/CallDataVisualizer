using System;
using Core.PointHolder;
using Core.Render.Points;
using UI.FilterGallery;
using UnityEngine;

namespace Managers {
    public class FilterManager : MonoBehaviour {
        FilterDisplay filterDisplay;
        bool[] descriptionsToRender;
        bool renderNonOfficerInitiated = true;
        bool renderOfficerInitiated = true;
        readonly bool[] renderPriority = {true, true, true, true, true, true};
        void Awake() => filterDisplay = GetComponent<FilterDisplay>();
        void OnEnable() => PointsHolder.DataSet += OnDataSet;
        void OnDisable() => PointsHolder.DataSet -= OnDataSet;
        void OnDataSet() {
            descriptionsToRender = new bool[PointsHolder.Descriptions.Length];
            filterDisplay.SetCount(PointsHolder.TotalPointCount, PointsHolder.NonOfficerInitiatedCount, PointsHolder.OfficerInitiatedCount, PointsHolder.PriorityCounts);
            for (int i = 0; i < PointsHolder.Descriptions.Length; i++) {
                DescriptionCount dc = PointsHolder.Descriptions[i];
                filterDisplay.AddRep(NewRep(dc, i));
                descriptionsToRender[i] = true;
            }
            Func<bool>[] priorityToggleFunctions = new Func<bool>[6];
            for (int i = 0; i < 6; i++) {
                int index = i;
                priorityToggleFunctions[i] = () => TogglePriority(index);
            }
            filterDisplay.InitializeToggles(ToggleNonOfficerInitiated, ToggleOfficerInitiated, priorityToggleFunctions);
        }
        bool ToggleNonOfficerInitiated() {
            renderNonOfficerInitiated = !renderNonOfficerInitiated;
            PointRenderer.SetRenderNonOfficerInitiatedState(renderNonOfficerInitiated);
            return renderNonOfficerInitiated;
        }
        bool ToggleOfficerInitiated() {
            renderOfficerInitiated = !renderOfficerInitiated;
            PointRenderer.SetRenderOfficerInitiatedState(renderOfficerInitiated);
            return renderOfficerInitiated;
        }
        bool TogglePriority(int index) {
            renderPriority[index] = !renderPriority[index];
            PointRenderer.SetPrioritiesToRender(renderPriority);
            return renderPriority[index];
        }
        void UpdateDescriptionToRender(int index, bool state) {
            descriptionsToRender[index] = state;
            PointRenderer.SetDescriptions(descriptionsToRender);
        }
        FilterElementRep NewRep(DescriptionCount dc, int index) =>
            new FilterElementRep(dc.Name, dc.TotalCount, dc.OfficerInitiatedCount, dc.PriorityCount, delegate(bool b) { UpdateDescriptionToRender(index, b); });
    }
}