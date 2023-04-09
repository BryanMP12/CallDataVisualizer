using System.Collections.Generic;
using Core;
using Core.CensusTracts;
using Core.Points;
using General;
using UnityEngine;
using Workers;

namespace Managers.CensusTracts {
    public partial class CensusTractManager : MonoBehaviour {
        [SerializeField] CensusTractHolder holder;
        [SerializeField] FilterManager filterManager;
        [SerializeField] Transform censusTractParent;
        [SerializeField] GameObject censusTractPrefab;
        [SerializeField] Transform adjacencyLineParent;
        [SerializeField] GameObject adjacencyLinePrefab;
        [SerializeField] Material censusTractMaterial;
        readonly List<CensusTractWorker> censusTractWorkers = new List<CensusTractWorker>();
        void Awake() {
            CreateTracts();
            DrawDigraph();
        }
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
                CensusTractWorker worker = Instantiate(censusTractPrefab, censusTractParent).GetComponent<CensusTractWorker>();
                censusTractWorkers.Add(worker);
                worker.Initialize(holder.CensusTracts[i], i);
            }
        }
        void DrawDigraph() {
            for (int i = 0; i < holder.CensusTracts.Count; i++) {
                CensusTract tract = holder.CensusTracts[i];
                for (int j = 0; j < tract.Data.BorderingCensuses.Length; j++) {
                    int otherIndex = tract.Data.BorderingCensuses[j];
                    if (otherIndex > i) continue;
                    CensusTract otherTract = holder.CensusTracts[otherIndex];
                    LineRenderer lr = Instantiate(adjacencyLinePrefab, adjacencyLineParent).GetComponent<LineRenderer>();
                    lr.positionCount = 2;
                    lr.SetPosition(0, Vec3(Dims.CoordToPort(tract.Centroid.Lon, tract.Centroid.Lat), -3));
                    lr.SetPosition(1, Vec3(Dims.CoordToPort(otherTract.Centroid.Lon, otherTract.Centroid.Lat), -3));
                }
            }
        }
        static Vector3 Vec3(Vector2 xy, float z) => new Vector3(xy.x, xy.y, z);
        void OnDataSet() {
            AssignPoints();
            SetCountColors();
        }
        void AssignPoints() {
            for (int i = 0; i < PointsHolder.Points.Length; i++) {
                Point point = PointsHolder.Points[i];
                if (!FindIntersectingCensusTract(point.MapPosition(), out CensusTractWorker worker)) continue;
                worker.AddPoint(i);
            }
        }
        void SetCountColors() {
            Color[] colors = new Color[censusTractWorkers.Count];
            for (int i = 0; i < censusTractWorkers.Count; i++) colors[i] = censusTractWorkers[i].CountColor();
            censusTractMaterial.SetColorArray(SPID._ColorArray, colors);
        }
    }
}