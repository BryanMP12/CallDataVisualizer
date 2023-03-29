using System;
using Core.PointHolder;
using Core.Render;
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
            Func<int>[] orderPriorityFunctions = new Func<int>[6];
            for (int i = 0; i < 6; i++) {
                int index = i;
                orderPriorityFunctions[i] = () => OrderPriority(index);
            }
            filterDisplay.InitializeOrders(OrderName, OrderCount, OrderNonOfficer, OrderOfficer, orderPriorityFunctions);
        }
        //Toggle
        bool ToggleNonOfficerInitiated() {
            renderNonOfficerInitiated = !renderNonOfficerInitiated;
            RendererVariableToggle.SetNonOfficerInitiatedToRender(renderNonOfficerInitiated);
            return renderNonOfficerInitiated;
        }
        bool ToggleOfficerInitiated() {
            renderOfficerInitiated = !renderOfficerInitiated;
            RendererVariableToggle.SetOfficerInitiatedToRender(renderOfficerInitiated);
            return renderOfficerInitiated;
        }
        bool TogglePriority(int index) {
            renderPriority[index] = !renderPriority[index];
            RendererVariableToggle.SetPrioritiesToRender(renderPriority);
            return renderPriority[index];
        }
        //Comparison
        int OrderName() {
            filterDisplay.ResetAllOrderButtonVisuals();
            filterDisplay.SortReps(ByAlphabetical);
            return 1;
        }
        int OrderCount() {
            filterDisplay.ResetAllOrderButtonVisuals();
            filterDisplay.SortReps(ByCount);
            return 1;
        }
        int nonOfficerOrderIndex;
        int OrderNonOfficer() {
            filterDisplay.ResetAllOrderButtonVisuals();
            nonOfficerOrderIndex = (nonOfficerOrderIndex + 1) % 2;
            filterDisplay.SortReps(nonOfficerOrderIndex == 0 ? ByNonOfficerCount : ByNonOfficerRatio);
            return nonOfficerOrderIndex + 1;
        }
        int officerOrderIndex;
        int OrderOfficer() {
            filterDisplay.ResetAllOrderButtonVisuals();
            officerOrderIndex = (officerOrderIndex + 1) % 2;
            filterDisplay.SortReps(officerOrderIndex == 0 ? ByOfficerCount : ByOfficerRatio);
            return officerOrderIndex + 1;
        }
        readonly int[] priorityOrderIndex = new int[6];
        int OrderPriority(int priority) {
            filterDisplay.ResetAllOrderButtonVisuals();
            priorityOrderIndex[priority] = (priorityOrderIndex[priority] + 1) % 2;
            filterDisplay.SortReps(priorityOrderIndex[priority] == 0 ? ByPriorityCount[priority] : ByPriorityRatio[priority]);
            return priorityOrderIndex[priority] + 1;
        }
        
        readonly Comparison<FilterElementRep> ByAlphabetical = (x, y) => string.CompareOrdinal(x.Name, y.Name);
        readonly Comparison<FilterElementRep> ByCount = (x, y) => -x.TotalCount.CompareTo(y.TotalCount);
        readonly Comparison<FilterElementRep> ByNonOfficerCount = (x, y) => -x.NonOfficerInitiatedCount().CompareTo(y.NonOfficerInitiatedCount());
        readonly Comparison<FilterElementRep> ByNonOfficerRatio = (x, y) => -x.NonOfficerInitiatedRatio.CompareTo(y.NonOfficerInitiatedRatio);
        readonly Comparison<FilterElementRep> ByOfficerCount = (x, y) => -x.OfficerInitiatedCount.CompareTo(y.OfficerInitiatedCount);
        readonly Comparison<FilterElementRep> ByOfficerRatio = (x, y) => -x.OfficerInitiatedRatio.CompareTo(y.OfficerInitiatedRatio);
        readonly Comparison<FilterElementRep>[] ByPriorityCount = PriorityCountComparison();
        readonly Comparison<FilterElementRep>[] ByPriorityRatio = PriorityRatioComparison();
        static Comparison<FilterElementRep>[] PriorityCountComparison() {
            Comparison<FilterElementRep>[] comparisons = new Comparison<FilterElementRep>[6];
            for (int i = 0; i < 6; i++) {
                int index = i;
                comparisons[i] = (x, y) => -x.PriorityCount[index].CompareTo(y.PriorityCount[index]);
            }
            return comparisons;
        }
        static Comparison<FilterElementRep>[] PriorityRatioComparison() {
            Comparison<FilterElementRep>[] comparisons = new Comparison<FilterElementRep>[6];
            for (int i = 0; i < 6; i++) {
                int index = i;
                comparisons[i] = (x, y) => -x.PriorityRatio(index).CompareTo(y.PriorityRatio(index));
            }
            return comparisons;
        }
        //
        //New Reps
        FilterElementRep NewRep(DescriptionCount dc, int index) =>
            new FilterElementRep(dc.Name, dc.TotalCount, dc.OfficerInitiatedCount, dc.PriorityCount, delegate(bool b) { UpdateDescriptionToRender(index, b); });
        void UpdateDescriptionToRender(int index, bool state) {
            descriptionsToRender[index] = state;
            RendererVariableToggle.SetDescriptionsToRender(descriptionsToRender);
        }
        //
    }
}