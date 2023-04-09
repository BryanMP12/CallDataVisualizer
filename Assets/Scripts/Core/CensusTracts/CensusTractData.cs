using System;
using System.Collections.Generic;
using Core.RawDataInterpreter.TractData;

namespace Core.CensusTracts {
    [Serializable]
    public class CensusTractData {
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
        public int TotalPopulationInGroupQuarters; //P0050001
        //
        public int InstitutionalizedPopulation; //P0050002
        public int CorrectionalFacilitiesForAdults; //P0050003
        public int JuvenileFacilities; //P0050004
        public int NursingFacilities; //P0050005
        public int OtherInstitutionalFacilities; //P0050006
        //
        public int NonInstitutionalizedPopulation; //P0050007
        public int CollegeUniversityStudentHousing; //P0050008
        public int MilitaryQuarters; //P0050009
        public int OtherNonInstitutionalizedFacilities; //P0050010
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
            TotalPopulationInGroupQuarters = node.P0050001;
            //
            InstitutionalizedPopulation = node.P0050002;
            CorrectionalFacilitiesForAdults = node.P0050003;
            JuvenileFacilities = node.P0050004;
            NursingFacilities = node.P0050005;
            OtherInstitutionalFacilities = node.P0050006;
            //
            NonInstitutionalizedPopulation = node.P0050007;
            CollegeUniversityStudentHousing = node.P0050008;
            MilitaryQuarters = node.P0050009;
            OtherNonInstitutionalizedFacilities = node.P0050010;
        }
    }
}