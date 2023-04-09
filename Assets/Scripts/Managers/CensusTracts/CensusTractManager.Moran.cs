using System.Collections.Generic;
using System.Linq;
using Core.CensusTracts;
using UnityEngine;
using Workers;

namespace Managers.CensusTracts {
    public partial class CensusTractManager {
        [ContextMenu("Calculate Moran's I")]
        public void CalculateMoranI() { Moran(censusTractWorkers, holder.CensusTracts); }
        void Moran(List<CensusTractWorker> workers, List<CensusTract> tracts) {
            Debug.Log($"Workers: {workers.Count}, Tracts: {tracts.Count}"); //Counts should be same
            //n = Census Tract Count
            //m = Edge Count
            int n = tracts.Count;
            int m = 0;
            float[] ratio = new float[tracts.Count];
            for (int i = 0; i < workers.Count; i++) {
                CensusTractWorker worker = workers[i];
                int totalPointCount = worker.Points.Count;
                int validPointCount = worker.Points.Count(pointIndex => filterManager.PointIsValid(pointIndex));
                ratio[i] = validPointCount / (float) totalPointCount;
            }
            float ratioAverage = ratio.Sum() / ratio.Length;
            for (int i = 0; i < ratio.Length; i++) ratio[i] -= ratioAverage;
            for (int i = 0; i < ratio.Length; i++) Debug.Log(ratio[i]);

            float adjacencySum = 0;
            for (int i = 0; i < workers.Count; i++) {
                CensusTractWorker worker = workers[i];
                for (int j = 0; j < worker.Points.Count; j++) {
                    if (i < worker.Points[j]) continue;
                    m++;
                    adjacencySum += ratio[i] * ratio[worker.Points[j]];
                }
            }

            float squareSum = ratio.Sum(t => t * t);

            float moranValue = (n / (float) m) * (adjacencySum / squareSum);
            Debug.Log($"Moran's I: {moranValue}");
        }
    }
}