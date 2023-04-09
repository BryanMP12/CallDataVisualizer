using System;

namespace Core {
    public static class MapCalculations {
        static double MapLatLonDistance(double lon1Decimal, double lat1Decimal, double lon2Decimal, double lat2Decimal) {
            const double PI = Math.PI;
            double lat1 = lat1Decimal / 180 * PI;
            double lon1 = lon1Decimal / 180 * PI; 
            double lat2 = lat2Decimal / 180 * PI;
            double lon2 = lon2Decimal / 180 * PI; 
            
            return acos(sin(lat1) * sin(lat2) + cos(lat1) * cos(lat2) * cos(lon2 - lon1)) * 6371;
        }
        static double acos(double val) => Math.Acos(val);
        static double sin(double val) => Math.Sin(val);
        static double cos(double val) => Math.Cos(val);
        public static void CalculateDistance(double lon1, double lat1, double lon2, double lat2, out double width, out double height) {
            height = MapLatLonDistance(lon1, lat1, lon1, lat2);
            double bottom = MapLatLonDistance(lon1, lat1, lon2, lat1);
            double top = MapLatLonDistance(lon1, lat2, lon2, lat2);
            width = (top + bottom) / 2;
        }
    }
}