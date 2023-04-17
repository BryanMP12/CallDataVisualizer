using System;
using TMPro;
using UnityEngine;

namespace UI.CensusTracts {
    public class TractViewer : MonoBehaviour, IAddon {
        [SerializeField] TextMeshProUGUI tractNumberText;
        //
        [SerializeField] TractViewerElement totalPopulationText;
        [SerializeField] TractViewerElement oneRacePopulationText;
        [SerializeField] TractViewerElement oneRaceWhiteText;
        [SerializeField] TractViewerElement oneRaceBlackText;
        [SerializeField] TractViewerElement oneRaceAmericanIndianText;
        [SerializeField] TractViewerElement oneRaceAsianText;
        [SerializeField] TractViewerElement oneRaceNativeHawaiianText;
        [SerializeField] TractViewerElement oneRaceOtherText;
        //
        [SerializeField] TractViewerElement twoOrMoreRacePopText;
        [SerializeField] TractViewerElement hispanicOrLatinoPopText;
        [SerializeField] TractViewerElement notHispanicOrLatinoPopText;
        [SerializeField] TractViewerElement eighteenYearsAndOverPopText;
        [SerializeField] TractViewerElement totalHousingUnitsText;
        [SerializeField] TractViewerElement occupiedHousingUnitsText;
        [SerializeField] TractViewerElement vacantHousingUnitsText;
        //
        [SerializeField] TractViewerElement totalPointsText;
        [SerializeField] TractViewerElement filteredPointsText;
        //
        [SerializeField] TextMeshProUGUI moranValue;
        Canvas canvas;
        void Awake() { canvas = GetComponent<Canvas>(); }
        public bool Enabled { get; set; }
        public void SetMoranValue(float value) => moranValue.SetText(value > 0 ? $"Moran's I: {value:##.0000}" : "Moran's I: ---");
        public void EnableDisplay() => canvas.enabled = Enabled = true;
        public void DisableDisplay() => canvas.enabled = Enabled = false;
        void TurnOffAll() {
            totalPopulationText.SetVisual(0);
            oneRacePopulationText.SetVisual(0);
            oneRaceWhiteText.SetVisual(0);
            oneRaceBlackText.SetVisual(0);
            oneRaceAmericanIndianText.SetVisual(0);
            oneRaceAsianText.SetVisual(0);
            oneRaceNativeHawaiianText.SetVisual(0);
            oneRaceOtherText.SetVisual(0);
            twoOrMoreRacePopText.SetVisual(0);
            hispanicOrLatinoPopText.SetVisual(0);
            notHispanicOrLatinoPopText.SetVisual(0);
            eighteenYearsAndOverPopText.SetVisual(0);
            totalHousingUnitsText.SetVisual(0);
            occupiedHousingUnitsText.SetVisual(0);
            vacantHousingUnitsText.SetVisual(0);
            totalPointsText.SetVisual(0);
            filteredPointsText.SetVisual(0);
        }
        public void SetTractTitleClickActions(Func<int> totalPop, Func<int> oneRacePop, Func<int> oneRaceW, Func<int> oneRaceB, Func<int> oneRaceAI, Func<int> oneRaceA,
            Func<int> oneRaceNH, Func<int> oneRaceO, Func<int> twoOrMoreRacePop, Func<int> hOrLatinPop, Func<int> notHOrLatinPop, Func<int> eighteenOverPop, Func<int> totalHouse,
            Func<int> occupiedHouse, Func<int> vacantHouse, Func<int> totalPoints, Func<int> filteredPoints) {
            totalPopulationText.SetButtonAction(delegate {
                TurnOffAll();
                totalPopulationText.SetVisual(totalPop.Invoke());
            });
            oneRacePopulationText.SetButtonAction(delegate {
                TurnOffAll();
                oneRacePopulationText.SetVisual(oneRacePop.Invoke());
            });
            oneRaceWhiteText.SetButtonAction(delegate {
                TurnOffAll();
                oneRaceWhiteText.SetVisual(oneRaceW.Invoke());
            });
            oneRaceBlackText.SetButtonAction(delegate {
                TurnOffAll();
                oneRaceBlackText.SetVisual(oneRaceB.Invoke());
            });
            oneRaceAmericanIndianText.SetButtonAction(delegate {
                TurnOffAll();
                oneRaceAmericanIndianText.SetVisual(oneRaceAI.Invoke());
            });
            oneRaceAsianText.SetButtonAction(delegate {
                TurnOffAll();
                oneRaceAsianText.SetVisual(oneRaceA.Invoke());
            });
            oneRaceNativeHawaiianText.SetButtonAction(delegate {
                TurnOffAll();
                oneRaceNativeHawaiianText.SetVisual(oneRaceNH.Invoke());
            });
            oneRaceOtherText.SetButtonAction(delegate {
                TurnOffAll();
                oneRaceOtherText.SetVisual(oneRaceO.Invoke());
            });
            twoOrMoreRacePopText.SetButtonAction(delegate {
                TurnOffAll();
                twoOrMoreRacePopText.SetVisual(twoOrMoreRacePop.Invoke());
            });
            hispanicOrLatinoPopText.SetButtonAction(delegate {
                TurnOffAll();
                hispanicOrLatinoPopText.SetVisual(hOrLatinPop.Invoke());
            });
            notHispanicOrLatinoPopText.SetButtonAction(delegate {
                TurnOffAll();
                notHispanicOrLatinoPopText.SetVisual(notHOrLatinPop.Invoke());
            });
            eighteenYearsAndOverPopText.SetButtonAction(delegate {
                TurnOffAll();
                eighteenYearsAndOverPopText.SetVisual(eighteenOverPop.Invoke());
            });
            totalHousingUnitsText.SetButtonAction(delegate {
                TurnOffAll();
                totalHousingUnitsText.SetVisual(totalHouse.Invoke());
            });
            occupiedHousingUnitsText.SetButtonAction(delegate {
                TurnOffAll();
                occupiedHousingUnitsText.SetVisual(occupiedHouse.Invoke());
            });
            vacantHousingUnitsText.SetButtonAction(delegate {
                TurnOffAll();
                vacantHousingUnitsText.SetVisual(vacantHouse.Invoke());
            });
            //
            totalPointsText.SetButtonAction(delegate {
                TurnOffAll();
                totalPointsText.SetVisual(totalPoints.Invoke());
            });
            filteredPointsText.SetButtonAction(delegate {
                TurnOffAll();
                filteredPointsText.SetVisual(filteredPoints.Invoke());
            });
        }
        public void InitializeTotalAndAverageNumbers(int censusCount, int totalPop, int oneRacePop, int oneRaceW, int oneRaceB, int oneRaceAI, int oneRaceA, int oneRaceNH,
            int oneRaceO, int twoOrMoreRacePop, int hOrLatinPop, int notHOrLatinPop, int eighteenOverPop, int totalHouse, int occupiedHouse, int vacantHouse) {
            totalPopulationText.SetTotalAndAverage(totalPop, censusCount);
            //
            oneRacePopulationText.SetTotalAndAverage(oneRacePop, censusCount);
            oneRaceWhiteText.SetTotalAndAverage(oneRaceW, censusCount);
            oneRaceBlackText.SetTotalAndAverage(oneRaceB, censusCount);
            oneRaceAmericanIndianText.SetTotalAndAverage(oneRaceAI, censusCount);
            oneRaceAsianText.SetTotalAndAverage(oneRaceA, censusCount);
            oneRaceNativeHawaiianText.SetTotalAndAverage(oneRaceNH, censusCount);
            oneRaceOtherText.SetTotalAndAverage(oneRaceO, censusCount);
            //
            twoOrMoreRacePopText.SetTotalAndAverage(twoOrMoreRacePop, censusCount);
            //
            hispanicOrLatinoPopText.SetTotalAndAverage(hOrLatinPop, censusCount);
            notHispanicOrLatinoPopText.SetTotalAndAverage(notHOrLatinPop, censusCount);
            eighteenYearsAndOverPopText.SetTotalAndAverage(eighteenOverPop, censusCount);
            //
            totalHousingUnitsText.SetTotalAndAverage(totalHouse, censusCount);
            occupiedHousingUnitsText.SetTotalAndAverage(occupiedHouse, censusCount);
            vacantHousingUnitsText.SetTotalAndAverage(vacantHouse, censusCount);
        }
        public void InitializePointTotalAndAverage(int censusCount, int pointTotal) {
            totalPointsText.SetTotalAndAverage(pointTotal, censusCount);
            filteredPointsText.SetTotalAndAverage(pointTotal, censusCount);
        }
        public void SetFilteredPointTotalAndAverage(int censusCount, int filteredPointTotal) => filteredPointsText.SetTotalAndAverage(filteredPointTotal, censusCount);
        public void SetSelectedTract(string tractNumber, int totalPop, int oneRacePop, int oneRaceW, int oneRaceB, int oneRaceAI, int oneRaceA, int oneRaceNH, int oneRaceO,
            int twoOrMoreRacePop, int hOrLatinPop, int notHOrLatinPop, int eighteenOverPop, int totalHouse, int occupiedHouse, int vacantHouse,
            int pointCount, int filteredPointCount) {
            tractNumberText.SetText(tractNumber);
            totalPopulationText.SetSelectedValue(totalPop);
            //
            oneRacePopulationText.SetSelectedValue(oneRacePop);
            oneRaceWhiteText.SetSelectedValue(oneRaceW);
            oneRaceBlackText.SetSelectedValue(oneRaceB);
            oneRaceAmericanIndianText.SetSelectedValue(oneRaceAI);
            oneRaceAsianText.SetSelectedValue(oneRaceA);
            oneRaceNativeHawaiianText.SetSelectedValue(oneRaceNH);
            oneRaceOtherText.SetSelectedValue(oneRaceO);
            //
            twoOrMoreRacePopText.SetSelectedValue(twoOrMoreRacePop);
            //
            hispanicOrLatinoPopText.SetSelectedValue(hOrLatinPop);
            notHispanicOrLatinoPopText.SetSelectedValue(notHOrLatinPop);
            eighteenYearsAndOverPopText.SetSelectedValue(eighteenOverPop);
            //
            totalHousingUnitsText.SetSelectedValue(totalHouse);
            occupiedHousingUnitsText.SetSelectedValue(occupiedHouse);
            vacantHousingUnitsText.SetSelectedValue(vacantHouse);
            //
            totalPointsText.SetSelectedValue(pointCount);
            filteredPointsText.SetSelectedValue(filteredPointCount);
        }
    }
}