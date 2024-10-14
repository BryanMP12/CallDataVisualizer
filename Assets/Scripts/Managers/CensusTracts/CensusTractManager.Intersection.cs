using Core;
using Core.CensusTracts;
using UnityEngine;
using Workers;

namespace Managers.CensusTracts {
    public sealed partial class CensusTractManager {
        static readonly RaycastHit[] hits = new RaycastHit[5];
        CensusTractWorker selectedTractWorker;
        void OnTractClick(Vector2 pos) {
            if(selectedTractWorker != null) selectedTractWorker.Deselect();
            selectedTractWorker = null;
            if (!FindIntersectingCensusTract(pos, out CensusTractWorker worker)) return;
            selectedTractWorker = worker;
            worker.Select();
            CensusTract tract = holder.CensusTracts[worker.TractIndex];
            CensusTractData d = tract.Data;
            tractViewer.SetSelectedTract(tract.Number.ToString(), d.TotalPopulation, d.OneRacePopulation, d.OneRaceWhite, d.OneRaceBlack, d.OneRaceAmericanIndian, d.OneRaceAsian,
                d.OneRaceNativeHawaiian, d.OneRaceOther, d.TwoOrMoreRacePopulation, d.HispanicOrLatinoPopulation, d.NotHispanicOrLatinoPopulation, d.EighteenYearsAndOverPopulation,
                d.TotalHousingUnits, d.OccupiedHousingUnits, d.VacantHousingUnits, worker.PointCount(), worker.FilteredPointCount(filterManager.PointIsValid));
        }
        bool FindIntersectingCensusTract(Vector2 pos, out CensusTractWorker worker) {
            int count = Cast(pos);
            if (count == 0) {
                worker = null;
                //Debug.Log("No tract found");
                return false;
            }
            int hitIndex = 0;
            if (count != 1) {
                //Debug.LogWarning($"Found more than one census tracts: {count}");
                float currentDistance = (pos - GetCentroidFromHit(hits[0])).sqrMagnitude;
                for (int i = 1; i < count; i++) {
                    float distance = (pos - GetCentroidFromHit(hits[i])).sqrMagnitude;
                    if (distance > currentDistance) continue;
                    hitIndex = i;
                    currentDistance = distance;
                }
            }
            worker = hits[hitIndex].transform.GetComponent<CensusTractWorker>();
            return true;
        }
        Vector2 GetCentroidFromHit(RaycastHit hit) {
            Coordinate c = holder.CensusTracts[hit.transform.GetComponent<CensusTractWorker>().TractIndex].Centroid;
            return Dims.CoordToPort(c.Lon, c.Lat);
        }
        static int Cast(Vector2 pos) {
            int count = Physics.RaycastNonAlloc(new Ray(new Vector3(pos.x, pos.y, -3), Vector3.forward), hits);
            if (count > 0) return count;
            return Physics.SphereCastNonAlloc(new Ray(new Vector3(pos.x, pos.y, -3), Vector3.forward), 15, hits);
        }
    }
}