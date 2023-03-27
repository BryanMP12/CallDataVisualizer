using Core.PointHolder;
using Core.Render.Points;
using UnityEngine;

namespace Managers {
    public class PointRendererManager : MonoBehaviour {
        [SerializeField] Material material;
        [SerializeField] bool render;
        void OnEnable() { PointsHolder.DataSet += Initialize; }
        void OnDisable() {
            PointsHolder.DataSet -= Initialize;
            PointRenderer.ReleaseBuffers();
        }
        void Initialize() {
            render = true;
            PointRenderer.InitializePoints(material);
        }
        void Update() {
            if (render) PointRenderer.Render();
        }
    }
}