using System;
using System.Collections.Generic;
using Core.RawDataInterpreter.CallData;

namespace Core.Points {
    [Serializable]
    public class SerializedPointHolder {
        public string name;
        public int TotalPointCount { get; private set; }
        public Point[] Points { get; private set; }
        public int[] PriorityCounts { get; private set; }
        public int OfficerInitiatedCount { get; private set; }
        public DescriptionCount[] Descriptions { get; private set; }
        public SerializedPointHolder(int tc, Point[] p, int[] pc, int oic, DescriptionCount[] d) =>
            (TotalPointCount, Points, PriorityCounts, OfficerInitiatedCount, Descriptions) = (tc, p, pc, oic, d);
        public SerializedPointHolder(int count) {
            PriorityCounts = new int[6];
            OfficerInitiatedCount = 0;
            TotalPointCount = count;
            Descriptions = null;
            Points = new Point[TotalPointCount];
        }
        Point DeserializePropertiesToPoint(Properties properties, List<DescriptionCount> possibleDescriptions) {
            float totalTime = (float?) properties.totaltime ?? -1;
            if (!sbyte.TryParse(properties.priority, out sbyte priority)) priority = 0;
            if (priority is < 0 or > 5) priority = 0;

            int descriptionIndex = possibleDescriptions.FindIndex(x => x.Name == properties.calldescription);
            if (descriptionIndex < 0) {
                descriptionIndex = possibleDescriptions.Count;
                possibleDescriptions.Add(new DescriptionCount(properties.calldescription));
            }
            bool officerInitiated = properties.officerinitiated[0] == 'Y';

            Point point = new Point(properties.longitude, properties.latitude, totalTime, priority, descriptionIndex, officerInitiated);
            if (point.Priority is >= 0 and <= 5) PriorityCounts[point.Priority]++;
            if (point.OfficerInitiated) OfficerInitiatedCount++;
            possibleDescriptions[point.DescriptionIndex].Increment(point);
            return point;
        }
        static string ConstructFileName() {
            DateTime now = DateTime.Now;
            return $"detData-{now.Year}-{now.Month}-{now.Day}";
        }
        public static SerializedPointHolder APIDataToSerializedPointHolder(List<APIModels.Data> dataList, int totalCount) {
            SerializedPointHolder sph = new SerializedPointHolder(totalCount);
            List<DescriptionCount> possibleDescriptions = new List<DescriptionCount>();
            int pointCount = 0;
            for (int i = 0; i < dataList.Count; i++) {
                List<APIModels.Feature> features = dataList[i].features;
                for (int j = 0; j < features.Count; j++) {
                    sph.Points[pointCount] = sph.DeserializePropertiesToPoint(features[j].attributes, possibleDescriptions);
                    pointCount++;
                }
            }
            sph.Descriptions = possibleDescriptions.ToArray();
            sph.name = ConstructFileName();
            return sph;
        }
        public static SerializedPointHolder ManualDataToSerializedPointHolder(ManualModels.Data data) {
            SerializedPointHolder sph = new SerializedPointHolder(data.features.Count);
            List<DescriptionCount> possibleDescriptions = new List<DescriptionCount>();
            for (int i = 0; i < sph.TotalPointCount; i++) sph.Points[i] = sph.DeserializePropertiesToPoint(data.features[i].properties, possibleDescriptions);
            sph.Descriptions = possibleDescriptions.ToArray();
            sph.name = data.name;
            return sph;
        }
    }
}