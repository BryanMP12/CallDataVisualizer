using System;

namespace Core.CensusTracts {
    [Serializable]
    public struct Coordinate {
        public double Lon;
        public double Lat;
        public Coordinate(double lon, double lat) => (Lon, Lat) = (lon, lat);
    }
}