using System;
using System.Collections.Generic;
using Core.CensusTracts;
using UnityEngine;
using Workers;

namespace Managers.CensusTracts {
    public partial class CensusTractManager {
        static float Moran(IReadOnlyList<CensusTractWorker> workers, IReadOnlyList<CensusTract> tracts, Func<CensusTractWorker, float> dataRatio) {
            Debug.Log($"Workers: {workers.Count}, Tracts: {tracts.Count}"); //Counts should be same
            //n = Census Tract Count
            //m = Edge Count
            int n = tracts.Count;
            int m = 0;
            float[] ratio = new float[tracts.Count];
            float ratioAverage = 0;
            for (int i = 0; i < workers.Count; i++) {
                CensusTractWorker worker = workers[i];
                ratio[i] = dataRatio(worker);
                ratioAverage += ratio[i];
            }
            Debug.Log($"Ratio Total: {ratioAverage}, Ratio Count: {ratio.Length}");
            ratioAverage /= ratio.Length;
            for (int i = 0; i < ratio.Length; i++) ratio[i] -= ratioAverage;
            // for (int i = 0; i < ratio.Length; i++) Debug.Log(ratio[i]);

            float adjacencySum = 0;
            float squareSum = 0;
            for (int i = 0; i < tracts.Count; i++) {
                CensusTract tract = tracts[i];
                squareSum += ratio[i] * ratio[i];
                for (int j = 0; j < tract.Data.BorderingCensuses.Length; j++) {
                    if (i < tract.Data.BorderingCensuses[j]) continue;
                    m++;
                    adjacencySum += ratio[i] * ratio[tract.Data.BorderingCensuses[j]];
                }
            }

            Debug.Log($"CensusTractCount: {n}, EdgeCount: {m}, RatioAverage: {ratioAverage}");
            Debug.Log($"AdjacencySum: {adjacencySum}, SquareSum: {squareSum}");

            float moranValue = (n / (float) m) * (adjacencySum / squareSum);
            Debug.Log($"Moran's I: {moranValue}");
            return moranValue;
        }
    }
}