using UnityEngine;

namespace Core {
    public static class Dims {
        public const int Width = 1024;
        public static readonly int Height = CalculateHeight();
        const double lon1 = -83.33;
        const double lat1 = 42.21;
        const double lon2 = -82.85;
        const double lat2 = 42.48;
        public static (double, double, double, double) Bbox => (lon1, lat1, lon2, lat2);
        public static (int, int) MapDimensions => (Width, Height);
        static int CalculateHeight() {
            MapCalculations.CalculateDistance(lon1, lat1, lon2, lat2, out double width, out double height);
            return (int) (Width * (height / width));
        }
        public static Vector2 CoordToPort(double lon, double lat) => new Vector2(
            (float) (Width * InverseLerp(lon1, lon2, lon)),
            (float) (Height * InverseLerp(lat1, lat2, lat)));
        public static double[] CoordToPortDouble(double lon, double lat) => new double[2] {
            Width * InverseLerp(lon1, lon2, lon),
            Height * InverseLerp(lat1, lat2, lat)
        };
        static double InverseLerp(double a, double b, double c) => (c - a) / (b - a);
    }
}