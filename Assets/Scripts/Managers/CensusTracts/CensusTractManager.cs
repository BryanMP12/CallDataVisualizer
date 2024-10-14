using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.CensusTracts;
using Core.Points;
using General;
using UI.CensusTracts;
using UI.LayerToolbar;
using UnityEngine;
using Workers;

namespace Managers.CensusTracts {
    public sealed partial class CensusTractManager : MonoBehaviour {
        [SerializeField] CensusTractHolder holder;
        [SerializeField] FilterManager filterManager;
        [SerializeField] TractViewer tractViewer;
        [SerializeField] Transform censusTractParent;
        [SerializeField] GameObject censusTractPrefab;
        [SerializeField] Transform adjacencyLineParent;
        [SerializeField] GameObject adjacencyLinePrefab;
        [SerializeField] Material censusTractMaterial;
        [SerializeField] LayerToggleDisplay layerToggleDisplay;
        readonly List<CensusTractWorker> censusTractWorkers = new List<CensusTractWorker>();
        List<CensusTract> Tracts => holder.CensusTracts; //Bandage used here just to make code shorter
        bool showTractFill = true;
        bool showTractOutline = true;
        bool showTractDigraph = false;
        void Awake() {
            CreateTracts();
            DrawDigraph();
            layerToggleDisplay.InitializeTractButtonAction(ToggleFill, ToggleTractOutline, ToggleDigraph);
            InitializeSliders();
        }
        bool ToggleFill() {
            showTractFill = !showTractFill;
            foreach (CensusTractWorker worker in censusTractWorkers) worker.SetFillRendererState(showTractFill);
            return showTractFill;
        }
        bool ToggleTractOutline() {
            showTractOutline = !showTractOutline;
            foreach (CensusTractWorker worker in censusTractWorkers) worker.SetBorderOutlineState(showTractOutline);
            return showTractOutline;
        }
        bool ToggleDigraph() {
            showTractDigraph = !showTractDigraph;
            adjacencyLineParent.gameObject.SetActive(showTractDigraph);
            return showTractDigraph;
        }
        void Start() {
            int totalPop = 0,
                oneRacePop = 0,
                oneRaceW = 0,
                oneRaceB = 0,
                oneRaceAI = 0,
                oneRaceA = 0,
                oneRaceNH = 0,
                oneRaceO = 0,
                twoOrMoreRacePop = 0,
                hOrLatinPop = 0,
                notHOrLatinPop = 0,
                eighteenOverPop = 0,
                totalHouse = 0,
                occupiedHouse = 0,
                vacantHouse = 0;
            for (int i = 0; i < Tracts.Count; i++) {
                CensusTract censusTract = Tracts[i];
                CensusTractData d = censusTract.Data;
                totalPop += d.TotalPopulation;
                oneRacePop += d.OneRacePopulation;
                oneRaceW += d.OneRaceWhite;
                oneRaceB += d.OneRaceBlack;
                oneRaceAI += d.OneRaceAmericanIndian;
                oneRaceA += d.OneRaceAsian;
                oneRaceNH += d.OneRaceNativeHawaiian;
                oneRaceO += d.OneRaceOther;
                twoOrMoreRacePop += d.TwoOrMoreRacePopulation;
                hOrLatinPop += d.HispanicOrLatinoPopulation;
                notHOrLatinPop += d.NotHispanicOrLatinoPopulation;
                eighteenOverPop += d.EighteenYearsAndOverPopulation;
                totalHouse += d.TotalHousingUnits;
                occupiedHouse += d.OccupiedHousingUnits;
                vacantHouse += d.VacantHousingUnits;
            }
            tractViewer.InitializeTotalAndAverageNumbers(Tracts.Count, totalPop, oneRacePop, oneRaceW, oneRaceB, oneRaceAI, oneRaceA, oneRaceNH, oneRaceO,
                twoOrMoreRacePop, hOrLatinPop, notHOrLatinPop, eighteenOverPop, totalHouse, occupiedHouse, vacantHouse);
            tractViewer.SetTractTitleClickActions(ToggleFunc(TogglePopulationTotal), ToggleFunc(ToggleOneRacePopulation), ToggleFunc(ToggleWhitePopulation),
                ToggleFunc(ToggleBlackPopulation), ToggleFunc(ToggleAmericanIndianPopulation), ToggleFunc(ToggleAsianPopulation), ToggleFunc(ToggleNativeHawaiianPopulation),
                ToggleFunc(ToggleOtherPopulation), ToggleFunc(ToggleTwoRacePopulation), ToggleFunc(ToggleHispanicOrLatinoPopulation),
                ToggleFunc(ToggleNotHispanicOrLatinoPopulation), ToggleFunc(ToggleEighteenYearsPopulation), ToggleFunc(ToggleHousingPopulation),
                ToggleFunc(ToggleOccupiedPopulation), ToggleFunc(ToggleVacantPopulation), ToggleFunc(TogglePoints), ToggleFunc(ToggleFilteredPoints)
            );
        }
        void OnEnable() {
            InputManager.TractClick += OnTractClick;
            FilterManager.FilterChanged += OnFilterChanged;
            PointsHolder.DataSet += OnDataSet;
        }
        void OnDisable() {
            InputManager.TractClick -= OnTractClick;
            FilterManager.FilterChanged -= OnFilterChanged;
            PointsHolder.DataSet -= OnDataSet;
        }
        void OnFilterChanged() {
            int filterCount = censusTractWorkers.Sum(worker => worker.FilteredPointCount(filterManager.PointIsValid));
            tractViewer.SetFilteredPointTotalAndAverage(censusTractWorkers.Count, filterCount);
        }
        float[] ratios;
        void CreateTracts() {
            for (int i = 0; i < Tracts.Count; i++) {
                CensusTractWorker worker = Instantiate(censusTractPrefab, censusTractParent).GetComponent<CensusTractWorker>();
                censusTractWorkers.Add(worker);
                worker.Initialize(Tracts[i], i);
            }
            ratios = new float[censusTractWorkers.Count];
        }
        void DrawDigraph() {
            for (int i = 0; i < Tracts.Count; i++) {
                CensusTract tract = Tracts[i];
                for (int j = 0; j < tract.Data.BorderingCensuses.Length; j++) {
                    int otherIndex = tract.Data.BorderingCensuses[j];
                    if (otherIndex > i) continue;
                    CensusTract otherTract = Tracts[otherIndex];
                    LineRenderer lr = Instantiate(adjacencyLinePrefab, adjacencyLineParent).GetComponent<LineRenderer>();
                    lr.positionCount = 2;
                    lr.SetPosition(0, Vec3(Dims.CoordToPort(tract.Centroid.Lon, tract.Centroid.Lat), -3));
                    lr.SetPosition(1, Vec3(Dims.CoordToPort(otherTract.Centroid.Lon, otherTract.Centroid.Lat), -3));
                }
            }
        }
        static Vector3 Vec3(Vector2 xy, float z) => new Vector3(xy.x, xy.y, z);
        void OnDataSet() {
            AssignPoints();
            int pointCount = censusTractWorkers.Sum(worker => worker.PointCount());
            tractViewer.InitializePointTotalAndAverage(Tracts.Count, pointCount);
        }
        void AssignPoints() {
            for (int i = 0; i < PointsHolder.Points.Length; i++) {
                Point point = PointsHolder.Points[i];
                if (!FindIntersectingCensusTract(point.MapPosition(), out CensusTractWorker worker)) continue;
                worker.AddPoint(i);
            }
        }
        void SetCountColors(CensusDataCount type) {
            Func<CensusTractWorker, float> func = GetCountFunc(type);
            float max = censusTractWorkers.Select(func).Max();
            for (int i = 0; i < censusTractWorkers.Count; i++) ratios[i] = func(censusTractWorkers[i]) / max;
            censusTractMaterial.SetFloatArray(SPID._RatioArray, ratios);
            tractViewer.SetMoranValue(null);
            UpdateColorRamp(ColorRampType.Count, max);
        }
        void SetRatioColors(CensusDataRatio type) {
            Func<CensusTractWorker, float> func = GetRatioFunc(type);
            for (int i = 0; i < censusTractWorkers.Count; i++) ratios[i] = func(censusTractWorkers[i]);
            censusTractMaterial.SetFloatArray(SPID._RatioArray, ratios);
            tractViewer.SetMoranValue(Moran(censusTractWorkers, Tracts, func));
            UpdateColorRamp(ColorRampType.Ratio, 1);
        }
        public enum CensusDataCount {
            TotalPopulation, OneRace, White, Black, AmericanIndian, Asian, NativeHawaiian, OtherRace, TwoOrMoreRace, HispanicOrLatino,
            NotHispanicOrLatino, EighteenYearsAndOver, HousingUnits, OccupiedUnits, VacantUnits, Points, FilteredPoints
        }
        public enum CensusDataRatio {
            OneRace, White, Black, AmericanIndian, Asian, NativeHawaiian, OtherRace, TwoOrMoreRace, HispanicOrLatino, NotHispanicOrLatino, EighteenYearsAndOver, OccupiedUnits,
            VacantUnits, FilteredPoints
        }
        Func<CensusTractWorker, float> GetCountFunc(CensusDataCount type) =>
            type switch {
                CensusDataCount.TotalPopulation      => w => Tracts[w.TractIndex].Data.TotalPopulation,
                CensusDataCount.OneRace              => w => Tracts[w.TractIndex].Data.OneRacePopulation,
                CensusDataCount.White                => w => Tracts[w.TractIndex].Data.OneRaceWhite,
                CensusDataCount.Black                => w => Tracts[w.TractIndex].Data.OneRaceBlack,
                CensusDataCount.AmericanIndian       => w => Tracts[w.TractIndex].Data.OneRaceAmericanIndian,
                CensusDataCount.Asian                => w => Tracts[w.TractIndex].Data.OneRaceAsian,
                CensusDataCount.NativeHawaiian       => w => Tracts[w.TractIndex].Data.OneRaceNativeHawaiian,
                CensusDataCount.OtherRace            => w => Tracts[w.TractIndex].Data.OneRaceOther,
                CensusDataCount.TwoOrMoreRace        => w => Tracts[w.TractIndex].Data.TwoOrMoreRacePopulation,
                CensusDataCount.HispanicOrLatino     => w => Tracts[w.TractIndex].Data.HispanicOrLatinoPopulation,
                CensusDataCount.NotHispanicOrLatino  => w => Tracts[w.TractIndex].Data.NotHispanicOrLatinoPopulation,
                CensusDataCount.EighteenYearsAndOver => w => Tracts[w.TractIndex].Data.EighteenYearsAndOverPopulation,
                CensusDataCount.HousingUnits         => w => Tracts[w.TractIndex].Data.TotalHousingUnits,
                CensusDataCount.OccupiedUnits        => w => Tracts[w.TractIndex].Data.OccupiedHousingUnits,
                CensusDataCount.VacantUnits          => w => Tracts[w.TractIndex].Data.VacantHousingUnits,
                CensusDataCount.Points               => w => w.PointCount(),
                CensusDataCount.FilteredPoints       => w => w.FilteredPointCount(filterManager.PointIsValid),
                _                                    => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        Func<CensusTractWorker, float> GetRatioFunc(CensusDataRatio type) =>
            type switch {
                CensusDataRatio.OneRace              => w => Tracts[w.TractIndex].Data.OneRaceRatio(),
                CensusDataRatio.White                => w => Tracts[w.TractIndex].Data.WhiteRatio(),
                CensusDataRatio.Black                => w => Tracts[w.TractIndex].Data.BlackRatio(),
                CensusDataRatio.AmericanIndian       => w => Tracts[w.TractIndex].Data.AmericanIndianRatio(),
                CensusDataRatio.Asian                => w => Tracts[w.TractIndex].Data.AsianRatio(),
                CensusDataRatio.NativeHawaiian       => w => Tracts[w.TractIndex].Data.NativeHawaiianRatio(),
                CensusDataRatio.OtherRace            => w => Tracts[w.TractIndex].Data.OtherRaceRatio(),
                CensusDataRatio.TwoOrMoreRace        => w => Tracts[w.TractIndex].Data.TwoOrMoreRatio(),
                CensusDataRatio.HispanicOrLatino     => w => Tracts[w.TractIndex].Data.HispanicOrLatinoRatio(),
                CensusDataRatio.NotHispanicOrLatino  => w => Tracts[w.TractIndex].Data.NotHispanicOrLatinoRatio(),
                CensusDataRatio.EighteenYearsAndOver => w => Tracts[w.TractIndex].Data.EighteenYearsAndOverRatio(),
                CensusDataRatio.OccupiedUnits        => w => Tracts[w.TractIndex].Data.OccupiedHousingUnitsRatio(),
                CensusDataRatio.VacantUnits          => w => Tracts[w.TractIndex].Data.VacantHousingUnitsRatio(),
                CensusDataRatio.FilteredPoints       => w => w.FilteredPointRatio(filterManager.PointIsValid),
                _                                    => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
    }
}