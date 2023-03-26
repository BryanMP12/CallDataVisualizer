using System;

namespace Map {
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
        public static double[] CalculateDistance(double[] bbox) {
            double diagonal, left, right, bottom, top, middleHorizontal;
            diagonal = MapLatLonDistance(bbox[0], bbox[1], bbox[2], bbox[3]);
            left = MapLatLonDistance(bbox[0], bbox[1], bbox[0], bbox[3]);
            right = MapLatLonDistance(bbox[2], bbox[1], bbox[2], bbox[3]);
            bottom = MapLatLonDistance(bbox[0], bbox[1], bbox[2], bbox[1]);
            top = MapLatLonDistance(bbox[0], bbox[3], bbox[2], bbox[3]);
            middleHorizontal = (top + bottom) / 2;
            return new double[2] {middleHorizontal, left};
        }
    }
}