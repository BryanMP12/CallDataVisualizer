using Core;
using Core.CensusTracts;
using UnityEngine;
using Workers;

namespace Managers.CensusTracts {
    public partial class CensusTractManager {
        
        static readonly RaycastHit[] hits = new RaycastHit[5];
        bool FindIntersectingCensusTract(Vector2 pos, out CensusTractWorker worker) {
            int count = Cast(pos);
            if (count == 0) {
                worker = null;
                Debug.Log("No tract found");
                return false;
            }
            int hitIndex = 0;
            if (count != 1) {
                Debug.LogWarning($"Found more than one census tracts: {count}");
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