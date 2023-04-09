using System.Collections.Generic;
using Core;
using Core.CensusTracts;
using Core.Render.Tracts;
using UnityEngine;

namespace Workers {
    public class CensusTractWorker : MonoBehaviour {
        [SerializeField] LineRenderer borderOutline;
        [SerializeField] MeshFilter filter;
        [SerializeField] MeshCollider meshCollider;
        public int TractIndex { get; private set; }
        public readonly List<int> Points = new List<int>();
        static int MaxPointCount;
        public void Initialize(CensusTract tract, int index) {
            TractIndex = index;
            borderOutline.positionCount = tract.Shape.Count;
            for (int i = 0; i < tract.Shape.Count; i++) {
                Vector2 p = Dims.CoordToPort(tract.Shape[i].Lon, tract.Shape[i].Lat);
                borderOutline.SetPosition(i, new Vector3(p.x, p.y, -2));
            }
            Mesh mesh = MeshGenerator.MakeMesh(tract.Shape, index);
            filter.mesh = mesh;
            meshCollider.sharedMesh = mesh;
        }
        public void AddPoint(int pointIndex) {
            Points.Add(pointIndex);
        }
        public Color CountColor() {
            Color c = Color.Lerp(Color.green, Color.red, Points.Count / (float) MaxPointCount);
            c.a = 0.2f;
            return c;
        }
    }
}