using System;
using System.Collections.Generic;

namespace Core.CensusTracts {
    [Serializable]
    public class CensusTract {
        public CensusNumber Number;
        public Coordinate Centroid;
        public List<Coordinate> Shape;
        public CensusTractData Data;
        public void SetData(CensusTractData d) => Data = d;
        public CensusTract(string censusNumber, List<Coordinate> shape, Coordinate center) {
            Number = new CensusNumber(censusNumber);
            Centroid = center;
            Shape = shape;
        }
    }
}