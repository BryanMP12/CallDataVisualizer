using Core.Points;
using Core.Render.Points;
using UI.RenderLayerToolbar;
using UnityEngine;

namespace Managers {
    public class PointRendererManager : MonoBehaviour {
        [SerializeField] Material material;
        [SerializeField] bool render;
        [SerializeField] RenderDisplay renderDisplay;
        void OnEnable() { PointsHolder.DataSet += Initialize; }
        void OnDisable() {
            PointsHolder.DataSet -= Initialize;
            PointRenderer.ReleaseBuffers();
        }
        void Initialize() {
            render = true;
            PointRenderer.InitializePoints(material);
            renderDisplay.InitializePointButtonAction(delegate { return render = !render; }, PointRenderer.SetSize); }
        void Update() {
            if (render) PointRenderer.Render();
        }
    }
}