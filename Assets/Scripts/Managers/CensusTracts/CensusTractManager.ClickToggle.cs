using System;

namespace Managers.CensusTracts {
    public sealed partial class CensusTractManager {
        sealed class ToggleCountRatioHolder {
            public bool ToggleVal;
            public readonly CensusDataCount CountType;
            public readonly CensusDataRatio RatioType;
            public ToggleCountRatioHolder(CensusDataCount c, CensusDataRatio r) => (CountType, RatioType) = (c, r);
        }
        sealed class ToggleCountHolder {
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
        readonly ToggleCountHolder TogglePopulationTotal = new ToggleCountHolder(CensusDataCount.TotalPopulation);
        readonly ToggleCountRatioHolder ToggleOneRacePopulation = new ToggleCountRatioHolder(CensusDataCount.OneRace, CensusDataRatio.OneRace);
        readonly ToggleCountRatioHolder ToggleWhitePopulation = new ToggleCountRatioHolder(CensusDataCount.White, CensusDataRatio.White);
        readonly ToggleCountRatioHolder ToggleBlackPopulation = new ToggleCountRatioHolder(CensusDataCount.Black, CensusDataRatio.Black);
        readonly ToggleCountRatioHolder ToggleAmericanIndianPopulation = new ToggleCountRatioHolder(CensusDataCount.AmericanIndian, CensusDataRatio.AmericanIndian);
        readonly ToggleCountRatioHolder ToggleAsianPopulation = new ToggleCountRatioHolder(CensusDataCount.Asian, CensusDataRatio.Asian);
        readonly ToggleCountRatioHolder ToggleNativeHawaiianPopulation = new ToggleCountRatioHolder(CensusDataCount.NativeHawaiian, CensusDataRatio.NativeHawaiian);
        readonly ToggleCountRatioHolder ToggleOtherPopulation = new ToggleCountRatioHolder(CensusDataCount.OtherRace, CensusDataRatio.OtherRace);
        //
        readonly ToggleCountRatioHolder ToggleTwoRacePopulation = new ToggleCountRatioHolder(CensusDataCount.TwoOrMoreRace, CensusDataRatio.TwoOrMoreRace);
        readonly ToggleCountRatioHolder ToggleHispanicOrLatinoPopulation = new ToggleCountRatioHolder(CensusDataCount.HispanicOrLatino, CensusDataRatio.HispanicOrLatino);
        readonly ToggleCountRatioHolder ToggleNotHispanicOrLatinoPopulation = new ToggleCountRatioHolder(CensusDataCount.NotHispanicOrLatino, CensusDataRatio.NotHispanicOrLatino);
        readonly ToggleCountRatioHolder ToggleEighteenYearsPopulation = new ToggleCountRatioHolder(CensusDataCount.EighteenYearsAndOver, CensusDataRatio.EighteenYearsAndOver);
        readonly ToggleCountHolder ToggleHousingPopulation = new ToggleCountHolder(CensusDataCount.HousingUnits);
        readonly ToggleCountRatioHolder ToggleOccupiedPopulation = new ToggleCountRatioHolder(CensusDataCount.OccupiedUnits, CensusDataRatio.OccupiedUnits);
        readonly ToggleCountRatioHolder ToggleVacantPopulation = new ToggleCountRatioHolder(CensusDataCount.VacantUnits, CensusDataRatio.VacantUnits);
        //
        readonly ToggleCountHolder TogglePoints = new ToggleCountHolder(CensusDataCount.Points);
        readonly ToggleCountRatioHolder ToggleFilteredPoints = new ToggleCountRatioHolder(CensusDataCount.FilteredPoints, CensusDataRatio.FilteredPoints);
    }
}