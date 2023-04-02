using System;

namespace Core.Points {
    [Serializable]
    public struct Point {
        public double Longitude;
        public double Latitude;
        public float TotalTime;
        public sbyte Priority;
        public int DescriptionIndex;
        public bool OfficerInitiated;
        public Point(double lon, double lat, float tt, sbyte p, int di, bool oi) {
            Longitude = lon;
            Latitude = lat;
            TotalTime = tt;
            Priority = p;
            DescriptionIndex = di;
            OfficerInitiated = oi;
        }
    }
}