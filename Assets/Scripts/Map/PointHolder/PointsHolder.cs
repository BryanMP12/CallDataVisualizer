using System;

namespace Map.PointHolder {
    public static class PointsHolder {
        public static int TotalPointCount { get; private set; }
        public static Point[] Points { get; private set; }
        public static int[] PriorityCounts { get; private set; }
        public static int NonOfficerInitiatedCount => TotalPointCount - OfficerInitiatedCount;
        public static int OfficerInitiatedCount { get; private set; }
        public static NameCount[] Descriptions { get; private set; }
        public static void SetData(SerializedPointHolder sph) {
            PriorityCounts = sph.PriorityCounts;
            OfficerInitiatedCount = sph.OfficerInitiatedCount;
            TotalPointCount = sph.TotalPointCount;
            Descriptions = sph.Descriptions;
            Points = sph.Points;
        }
    }
    [Serializable]
    public struct NameCount {
        public string Name;
        public int TotalCount;
        public int[] PriorityCount;
        public int OfficerInitiatedCount;
        public void Increment(Point p) {
            TotalCount++;
            PriorityCount[p.Priority]++;
            if (p.OfficerInitiated) OfficerInitiatedCount++;
        }
        public NameCount(string n) {
            Name = n;
            TotalCount = 0;
            PriorityCount = new int[6];
            OfficerInitiatedCount = 0;
        }
    }
}