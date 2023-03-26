using System.Collections.Generic;
using Map.PointHolder;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Map.Filter {
    public class FilterManager : MonoBehaviour {
        [SerializeField] GameObject filterElementPrefab;
        [SerializeField] RectTransform filterUI;
        [SerializeField] Button openFilterButton;
        CanvasGroup filterGroup;
        static FilterManager instance;
        //Priority Filter
        readonly bool[] priorityFilter = new bool[6];
        bool[] descriptionsToShow;
        bool showOfficerInitiated;
        bool showNonOfficerInitiated;
        void Awake() => instance = this;
        public static void Initialize() => instance.InitializeInternal();
        void InitializeInternal() {
            filterGroup = filterUI.GetComponent<CanvasGroup>();
            filterGroup.alpha = 0;
            openFilterButton.onClick.AddListener(delegate { filterGroup.alpha = 1 - filterGroup.alpha; });

            //Priority Buttons
            for (int i = 0; i < 6; i++) {
                priorityFilter[i] = true;
                FilterElement fe = Instantiate(filterElementPrefab, filterUI).GetComponent<FilterElement>();
                int index = i;
                fe.Initialize(0, i, $"Priority: {i.ToString()}", PointsHolder.PriorityCounts[i], delegate { priorityFilter[index] = !priorityFilter[index]; });
            }
            //Description To Show
            NameCount[] dts = PointsHolder.Descriptions;
            int descriptionCount = dts.Length;
            descriptionsToShow = new bool[dts.Length];
            for (int i = 0; i < descriptionCount; i++) {
                descriptionsToShow[i] = true;
                FilterElement fe = Instantiate(filterElementPrefab, filterUI).GetComponent<FilterElement>();
                int index = i;
                fe.Initialize(1, i, dts[i].Name, dts[i].TotalCount, delegate { descriptionsToShow[index] = !descriptionsToShow[index]; });
            }
            //OfficerInitiated
            showOfficerInitiated = showNonOfficerInitiated = true;
            {
                FilterElement fe = Instantiate(filterElementPrefab, filterUI).GetComponent<FilterElement>();
                fe.Initialize(2, 0, "ShowOfficerInitiated", PointsHolder.OfficerInitiatedCount, delegate { showOfficerInitiated = !showOfficerInitiated; });
            }
            //NonOfficerInitiated
            {
                FilterElement fe = Instantiate(filterElementPrefab, filterUI).GetComponent<FilterElement>();
                fe.Initialize(2, 1, "ShowNonOfficerInitiated", PointsHolder.NonOfficerInitiatedCount, delegate { showNonOfficerInitiated = !showNonOfficerInitiated; });
            }
        }
        bool CanRender(Point p) {
            //if (!TimeFilter.InRange(p.TotalTime)) return false;
            if (!priorityFilter[Mathf.Clamp(p.Priority, 0, 5)]) return false;
            if (!descriptionsToShow[p.DescriptionIndex]) return false;
            if (p.OfficerInitiated && !showOfficerInitiated) return false;
            if (!p.OfficerInitiated && !showNonOfficerInitiated) return false;
            return true;
        }
        public static int FilteredListCount() => instance.FilteredListCountInternal();
        int FilteredListCountInternal() {
            int totalPoints = PointsHolder.TotalPointCount;
            int filteredPointCount = 0;
            for (int i = 0; i < totalPoints; i++)
                if (CanRender(PointsHolder.Points[i]))
                    filteredPointCount++;
            return filteredPointCount;
        }
        public static IEnumerable<Point> FilteredList() => instance.FilteredListInternal();
        IEnumerable<Point> FilteredListInternal() {
            int totalPoints = PointsHolder.TotalPointCount;
            for (int i = 0; i < totalPoints; i++)
                if (CanRender(PointsHolder.Points[i]))
                    yield return PointsHolder.Points[i];
        }
    }
}