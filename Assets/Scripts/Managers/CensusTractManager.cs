using System;
using System.Collections;
using System.Collections.Generic;
using Core.CensusTracts;
using Core.Points;
using General;
using UnityEngine;
using Workers;
using Random = UnityEngine.Random;

namespace Managers {
    public class CensusTractManager : MonoBehaviour {
        [SerializeField] CensusTractHolder holder;
        [SerializeField] GameObject censusTractPrefab;
        [SerializeField] Transform parent;
        [SerializeField] Material censusTractMaterial;
        readonly List<CensusTractWorker> censusTractWorkers = new List<CensusTractWorker>();
        void Awake() { CreateTracts(); }
        void OnEnable() {
            //MouseInput.MouseClick += FindIntersectingCensusTract;
            PointsHolder.DataSet += OnDataSet;
        }
        void OnDisable() {
            //MouseInput.MouseClick -= FindIntersectingCensusTract;
            PointsHolder.DataSet -= OnDataSet;
        }
        void CreateTracts() {
            for (int i = 0; i < holder.CensusTracts.Count; i++) {
                CensusTractWorker worker = Instantiate(censusTractPrefab, parent).GetComponent<CensusTractWorker>();
                censusTractWorkers.Add(worker);
                worker.Initialize(holder.CensusTracts[i], i);
            }
        }
        void OnDataSet() {
            AssignPoints();
            SetCountColors();
        }
        void AssignPoints() {
            for (int i = 0; i < PointsHolder.Points.Length; i++) {
                Point point = PointsHolder.Points[i];
                if (!FindIntersectingCensusTract(point.MapPosition(), out CensusTractWorker worker)) continue;
                worker.AddPoint();
            }
        }
        void SetCountColors() {
            Color[] colors = new Color[censusTractWorkers.Count];
            for (int i = 0; i < censusTractWorkers.Count; i++) colors[i] = censusTractWorkers[i].CountColor();
            censusTractMaterial.SetColorArray(SPID._ColorArray, colors);
        }
        readonly RaycastHit[] hits = new RaycastHit[5];
        bool FindIntersectingCensusTract(Vector2 pos, out CensusTractWorker worker) {
            int count = Physics.RaycastNonAlloc(new Ray(new Vector3(pos.x, pos.y, -3), Vector3.forward), hits);
            if (count == 0) {
                worker = null;
                Debug.Log("No censusTractFound");
                return false;
            }
            if (count != 1) Debug.LogWarning("Found more than one census tracts");
            worker = hits[0].transform.GetComponent<CensusTractWorker>();
            return true;
        }
    }
}