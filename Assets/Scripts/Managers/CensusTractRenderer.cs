using System;
using Core;
using Core.CensusTracts;
using UnityEngine;

namespace Managers {
    public class CensusTractRenderer : MonoBehaviour {
        [SerializeField] CensusTractHolder holder;
        [SerializeField] GameObject censusTractPrefab;
        void Awake() {
            foreach (CensusTract censusTract in holder.CensusTracts) {
                LineRenderer line = Instantiate(censusTractPrefab).GetComponent<LineRenderer>();
                line.positionCount = censusTract.Shape.Count;
                for (int i = 0; i < censusTract.Shape.Count; i++) {
                    Coordinate coord = censusTract.Shape[i];
                    line.SetPosition(i, Dims.CoordToPort(coord.Lon, coord.Lat));
                }
            }
        }
    }
}