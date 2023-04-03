using UnityEngine;

namespace Core {
    public static class Dims {
        public const int Width = 1024;
        public static readonly int Height = CalculateHeight();
        const double lon1 = -83.33;
        const double lat1 = 42.214;
        const double lon2 = -82.905913;
        const double lat2 = 42.47971;
        public static double[] bbox => new double[4] {lon1, lat1, lon2, lat2};
        static int CalculateHeight() {
            double[] dimensions = MapCalculations.CalculateDistance(bbox);
            return (int) (Width * (dimensions[1] / dimensions[0]));
        }
        public static Vector2 CoordToPort(double lon, double lat) => new Vector2(
            (float) (Width * InverseLerp(bbox[0], bbox[2], lon)),
            (float) (Height * InverseLerp(bbox[1], bbox[3], lat)));
        public static double[] CoordToPortDouble(double lon, double lat) => new double[2] {
            Width * InverseLerp(bbox[0], bbox[2], lon),
            Height * InverseLerp(bbox[1], bbox[3], lat)
        };
        static double InverseLerp(double a, double b, double c) => (c - a) / (b - a);
    }
}