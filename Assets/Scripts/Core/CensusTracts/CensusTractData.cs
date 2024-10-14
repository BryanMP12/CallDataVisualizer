using System;
using System.Collections.Generic;
using Core.RawDataInterpreter.TractData;

namespace Core.CensusTracts {
    [Serializable]
    public sealed class CensusTractData {
        public int[] BorderingCensuses;
        public int TotalPopulation; //P0010001
        //
        public int OneRacePopulation; //P0010002
        public int OneRaceWhite; //P0010003
        public int OneRaceBlack; //P0010004
        public int OneRaceAmericanIndian; //P0010005
        public int OneRaceAsian; //P0010006
        public int OneRaceNativeHawaiian; //P0010007
        public int OneRaceOther; //P0010008
        //
        public int TwoOrMoreRacePopulation; //P0010009
        public int HispanicOrLatinoPopulation; //P0020002
        public int NotHispanicOrLatinoPopulation; //P0020003
        public int EighteenYearsAndOverPopulation; //P0030001
        public int TotalHousingUnits; //H0010001
        public int OccupiedHousingUnits; //H0010002
        public int VacantHousingUnits; //H0010003
        public float OneRaceRatio() => HandleZeroDivision(OneRacePopulation, TotalPopulation);
        public float WhiteRatio() => HandleZeroDivision(OneRaceWhite, TotalPopulation);
        public float BlackRatio() => HandleZeroDivision(OneRaceBlack, TotalPopulation);
        public float AmericanIndianRatio() => HandleZeroDivision(OneRaceAmericanIndian, TotalPopulation);
        public float AsianRatio() => HandleZeroDivision(OneRaceAsian, TotalPopulation);
        public float NativeHawaiianRatio() => HandleZeroDivision(OneRaceNativeHawaiian, TotalPopulation);
        public float OtherRaceRatio() => HandleZeroDivision(OneRaceOther, TotalPopulation);
        public float TwoOrMoreRatio() => HandleZeroDivision(TwoOrMoreRacePopulation, TotalPopulation);
        public float HispanicOrLatinoRatio() => HandleZeroDivision(HispanicOrLatinoPopulation, TotalPopulation);
        public float NotHispanicOrLatinoRatio() => HandleZeroDivision(NotHispanicOrLatinoPopulation, TotalPopulation);
        public float EighteenYearsAndOverRatio() => HandleZeroDivision(EighteenYearsAndOverPopulation, TotalPopulation);
        public float OccupiedHousingUnitsRatio() => HandleZeroDivision(OccupiedHousingUnits, TotalHousingUnits);
        public float VacantHousingUnitsRatio() => HandleZeroDivision(VacantHousingUnits, TotalHousingUnits);
        float HandleZeroDivision(int a, int b) {
            if (b == 0) return 0;
            return a / (float) b;
        }
        public CensusTractData(List<int> borders, Models.Node node) {
            BorderingCensuses = borders.ToArray();
            TotalPopulation = node.P0010001;
            OneRacePopulation = node.P0010002;
            OneRaceWhite = node.P0010003;
            OneRaceBlack = node.P0010004;
            OneRaceAmericanIndian = node.P0010005;
            OneRaceAsian = node.P0010006;
            OneRaceNativeHawaiian = node.P0010007;
            OneRaceOther = node.P0010008;
            //
            TwoOrMoreRacePopulation = node.P0010009;
            HispanicOrLatinoPopulation = node.P0020002;
            NotHispanicOrLatinoPopulation = node.P0020003;
            EighteenYearsAndOverPopulation = node.P0030001;
            TotalHousingUnits = node.H0010001;
            OccupiedHousingUnits = node.H0010002;
            VacantHousingUnits = node.H0010003;
        }
    }
}