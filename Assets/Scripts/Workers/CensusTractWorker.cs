using System;
using System.Collections.Generic;
using Core;
using Core.CensusTracts;
using Core.Render.Tracts;
using UnityEngine;
using System.Linq;

namespace Workers {
    public sealed class CensusTractWorker : MonoBehaviour {
        [SerializeField] LineRenderer borderOutline;
        [SerializeField] MeshFilter filter;
        [SerializeField] MeshRenderer meshRenderer;
        [SerializeField] MeshCollider meshCollider;
        static readonly Gradient WhiteGradient = new Gradient() {
            colorKeys = new GradientColorKey[1] {new GradientColorKey(Color.white, 0)},
            alphaKeys = new GradientAlphaKey[1] {new GradientAlphaKey(1, 0)},
            mode = GradientMode.Fixed
        };
        static readonly Gradient RedGradient = new Gradient() {
            colorKeys = new GradientColorKey[1] {new GradientColorKey(Color.red, 0)},
            alphaKeys = new GradientAlphaKey[1] {new GradientAlphaKey(1, 0)},
            mode = GradientMode.Fixed
        };
        public int TractIndex { get; private set; }
        readonly List<int> Points = new List<int>();
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
        public void AddPoint(int pointIndex) { Points.Add(pointIndex); }
        public void Select() {
            borderOutline.sortingOrder = 1;
            borderOutline.colorGradient = RedGradient;
            borderOutline.startWidth = 1;
        }
        public void Deselect() {
            borderOutline.sortingOrder = 0;
            borderOutline.colorGradient = WhiteGradient;
            borderOutline.startWidth = 0.5f;
        }
        public int PointCount() => Points.Count;
        public void SetFillRendererState(bool state) => meshRenderer.enabled = state;
        public void SetBorderOutlineState(bool state) => borderOutline.enabled = state;
        public int FilteredPointCount(Func<int, bool> pointFilter) { return Points.Count == 0 ? 0 : Points.Count(pointFilter); }
        public float FilteredPointRatio(Func<int, bool> pointFilter) {
            if (Points.Count == 0) return 0;
            return FilteredPointCount(pointFilter) / (float) PointCount();
        }
    }
}