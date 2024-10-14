using System;
using System.Collections;
using Core.Points;
using Core.Render;
using UI.FilterGallery;
using UnityEngine;

namespace Managers {
    public sealed class FilterManager : MonoBehaviour {
        FilterDisplay filterDisplay;
        bool[] descriptionsToRender;
        bool renderNonOfficerInitiated = true;
        bool renderOfficerInitiated = true;
        readonly bool[] renderPriority = {true, true, true, true, true, true};
        void Awake() => filterDisplay = GetComponent<FilterDisplay>();
        void OnEnable() => PointsHolder.DataSet += OnDataSet;
        void OnDisable() => PointsHolder.DataSet -= OnDataSet;
        public static event Action FilterChanged;
        bool PointIsValid(Point p) =>
            descriptionsToRender[p.DescriptionIndex] &&
            (!p.OfficerInitiated && renderNonOfficerInitiated || p.OfficerInitiated && renderOfficerInitiated) &&
            renderPriority[p.Priority];
        public bool PointIsValid(int p) => PointIsValid(PointsHolder.Points[p]);
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
            Action[] orderPriorityFunctions = new Action[6];
            for (int i = 0; i < 6; i++) {
                int index = i;
                orderPriorityFunctions[i] = () => OrderPriority(index);
            }
            filterDisplay.InitializeOrders(OrderName, OrderCount, OrderNonOfficer, OrderOfficer, orderPriorityFunctions);
        }
        //Toggle
        bool ToggleNonOfficerInitiated() {
            renderNonOfficerInitiated = !renderNonOfficerInitiated;
            PointRenderBuffer.SetNonOfficerInitiatedToRender(renderNonOfficerInitiated);
            FilterChanged?.Invoke();
            return renderNonOfficerInitiated;
        }
        bool ToggleOfficerInitiated() {
            renderOfficerInitiated = !renderOfficerInitiated;
            PointRenderBuffer.SetOfficerInitiatedToRender(renderOfficerInitiated);
            FilterChanged?.Invoke();
            return renderOfficerInitiated;
        }
        bool TogglePriority(int index) {
            renderPriority[index] = !renderPriority[index];
            PointRenderBuffer.SetPrioritiesToRender(renderPriority);
            FilterChanged?.Invoke();
            return renderPriority[index];
        }
        //Comparison
        void OrderName() {
            filterDisplay.ResetAllOrderButtonVisuals();
            filterDisplay.SortReps(ByAlphabetical);
        }
        void OrderCount() {
            filterDisplay.ResetAllOrderButtonVisuals();
            filterDisplay.SortReps(ByCount);
        }
        void OrderNonOfficer() {
            filterDisplay.ResetAllOrderButtonVisuals();
            filterDisplay.SortReps(ByNonOfficerCount);
        }
        void OrderOfficer() {
            filterDisplay.ResetAllOrderButtonVisuals();
            filterDisplay.SortReps(ByOfficerCount);
        }
        void OrderPriority(int priority) {
            filterDisplay.ResetAllOrderButtonVisuals();
            filterDisplay.SortReps(ByPriorityCount[priority]);
        }
        readonly Comparison<FilterElementRep> ByAlphabetical = (x, y) => string.CompareOrdinal(x.Name, y.Name);
        readonly Comparison<FilterElementRep> ByCount = (x, y) => -x.TotalCount.CompareTo(y.TotalCount);
        readonly Comparison<FilterElementRep> ByNonOfficerCount = (x, y) => -x.NonOfficerInitiatedCount().CompareTo(y.NonOfficerInitiatedCount());
        readonly Comparison<FilterElementRep> ByOfficerCount = (x, y) => -x.OfficerInitiatedCount.CompareTo(y.OfficerInitiatedCount);
        readonly Comparison<FilterElementRep>[] ByPriorityCount = PriorityCountComparison();
        static Comparison<FilterElementRep>[] PriorityCountComparison() {
            Comparison<FilterElementRep>[] comparisons = new Comparison<FilterElementRep>[6];
            for (int i = 0; i < 6; i++) {
                int index = i;
                comparisons[i] = (x, y) => -x.PriorityCount[index].CompareTo(y.PriorityCount[index]);
            }
            return comparisons;
        }
        //
        //New Reps
        FilterElementRep NewRep(DescriptionCount dc, int index) =>
            new FilterElementRep(dc.Name, dc.TotalCount, dc.OfficerInitiatedCount, dc.PriorityCount, delegate(bool b) { UpdateDescriptionToRender(index, b); });
        void UpdateDescriptionToRender(int index, bool state) {
            descriptionsToRender[index] = state;
            if (delayedUpdate == null) delayedUpdate = StartCoroutine(DelayedDescriptionUpdate());
        }
        //This is a bandage solution. Not elegant but works. Used to prevent buffer being updated twice in one tick
        Coroutine delayedUpdate;
        IEnumerator DelayedDescriptionUpdate() {
            yield return null;
            PointRenderBuffer.SetDescriptionsToRender(descriptionsToRender);
            delayedUpdate = null;
            FilterChanged?.Invoke();
        }
        //
    }
}