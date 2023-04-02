using System;
using System.Collections.Generic;

namespace Core.CensusTracts {
    [Serializable]
    public struct CensusTract {
        public CensusNumber Number;
        public Coordinate Centroid;
        public List<Coordinate> Shape;
        public CensusTract(string censusNumber, List<Coordinate> shape, Coordinate center) {
            Number = new CensusNumber(censusNumber);
            Centroid = center;
            Shape = shape;
        }
    }
}