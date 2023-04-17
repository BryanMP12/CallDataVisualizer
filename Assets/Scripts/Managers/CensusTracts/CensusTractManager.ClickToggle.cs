using System;

namespace Managers.CensusTracts {
    public partial class CensusTractManager {
        class ToggleCountRatioHolder {
            public bool ToggleVal;
            public readonly CensusDataCount CountType;
            public readonly CensusDataRatio RatioType;
            public ToggleCountRatioHolder(CensusDataCount c, CensusDataRatio r) => (CountType, RatioType) = (c, r);
        }
        class ToggleCountHolder {
            public readonly CensusDataCount CountType;
            public ToggleCountHolder(CensusDataCount c) => (CountType) = (c);
        }
        Func<int> ToggleFunc(ToggleCountRatioHolder tcr) {
            return delegate {
                if (tcr.ToggleVal) SetCountColors(tcr.CountType);
                else SetRatioColors(tcr.RatioType);
                tcr.ToggleVal = !tcr.ToggleVal;
                return tcr.ToggleVal ? 1 : 2;
            };
        }
        Func<int> ToggleFunc(ToggleCountHolder tcr) {
            return delegate {
                SetCountColors(tcr.CountType);
                return 1;
            };
        }
        ToggleCountHolder TogglePopulationTotal = new ToggleCountHolder(CensusDataCount.TotalPopulation);
        ToggleCountRatioHolder ToggleOneRacePopulation = new ToggleCountRatioHolder(CensusDataCount.OneRace, CensusDataRatio.OneRace);
        ToggleCountRatioHolder ToggleWhitePopulation = new ToggleCountRatioHolder(CensusDataCount.White, CensusDataRatio.White);
        ToggleCountRatioHolder ToggleBlackPopulation = new ToggleCountRatioHolder(CensusDataCount.Black, CensusDataRatio.Black);
        ToggleCountRatioHolder ToggleAmericanIndianPopulation = new ToggleCountRatioHolder(CensusDataCount.AmericanIndian, CensusDataRatio.AmericanIndian);
        ToggleCountRatioHolder ToggleAsianPopulation = new ToggleCountRatioHolder(CensusDataCount.Asian, CensusDataRatio.Asian);
        ToggleCountRatioHolder ToggleNativeHawaiianPopulation = new ToggleCountRatioHolder(CensusDataCount.NativeHawaiian, CensusDataRatio.NativeHawaiian);
        ToggleCountRatioHolder ToggleOtherPopulation = new ToggleCountRatioHolder(CensusDataCount.OtherRace, CensusDataRatio.OtherRace);

        //
        ToggleCountRatioHolder ToggleTwoRacePopulation = new ToggleCountRatioHolder(CensusDataCount.TwoOrMoreRace, CensusDataRatio.TwoOrMoreRace);
        ToggleCountRatioHolder ToggleHispanicOrLatinoPopulation = new ToggleCountRatioHolder(CensusDataCount.HispanicOrLatino, CensusDataRatio.HispanicOrLatino);
        ToggleCountRatioHolder ToggleNotHispanicOrLatinoPopulation = new ToggleCountRatioHolder(CensusDataCount.NotHispanicOrLatino, CensusDataRatio.NotHispanicOrLatino);
        ToggleCountRatioHolder ToggleEighteenYearsPopulation = new ToggleCountRatioHolder(CensusDataCount.EighteenYearsAndOver, CensusDataRatio.EighteenYearsAndOver);
        ToggleCountHolder ToggleHousingPopulation = new ToggleCountHolder(CensusDataCount.HousingUnits);
        ToggleCountRatioHolder ToggleOccupiedPopulation = new ToggleCountRatioHolder(CensusDataCount.OccupiedUnits, CensusDataRatio.OccupiedUnits);
        ToggleCountRatioHolder ToggleVacantPopulation = new ToggleCountRatioHolder(CensusDataCount.VacantUnits, CensusDataRatio.VacantUnits);
        //
        ToggleCountHolder TogglePoints = new ToggleCountHolder(CensusDataCount.Points);
        ToggleCountRatioHolder ToggleFilteredPoints = new ToggleCountRatioHolder(CensusDataCount.FilteredPoints, CensusDataRatio.FilteredPoints);
    }
}