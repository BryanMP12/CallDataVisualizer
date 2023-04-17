using System;
using Core.Points;
using Core.Render;
using Core.Render.Heatmap;
using Core.Render.Points;
using General;
using UI.LayerToolbar;
using UnityEngine;

namespace Managers.RenderBuffers {
    public partial class RenderBufferManager : MonoBehaviour {
        [Header("Points")]
        //PointRenderer
        [SerializeField]
        Material pointRendererMaterial;
        [SerializeField] bool renderPoints;
        [Header("Heatmap")]
        //HeatmapRenderer
        [SerializeField]
        Material heatmapMaterial;
        [SerializeField] ComputeShader computeShader;
        [SerializeField] MeshRenderer mesh;

        //
        [SerializeField] LayerToggleDisplay layerToggleDisplay;
        void Awake() { InitializeSliders(); }
        void OnEnable() { PointsHolder.DataSet += OnDataSet; }
        void OnDisable() {
            PointsHolder.DataSet -= OnDataSet;
            PointRenderBuffer.ReleaseBuffers();
            PointRenderer.ReleaseBuffers();
            HeatmapGenerator.ReleaseBuffer();
        }
        void OnDataSet() {
            {
                renderPoints = true;
                PointRenderer.InitializePoints(pointRendererMaterial);
                layerToggleDisplay.InitializePointButtonAction(delegate { return renderPoints = !renderPoints; }, PointRenderer.SetSize);
            }
            {
                Texture heatTexture = HeatmapGenerator.InitializeHeatMap(computeShader);
                heatmapMaterial.SetTexture(SPID._InputTexture, heatTexture);
                layerToggleDisplay.InitializeHeatmapButtonAction(
                    delegate { return mesh.enabled = !mesh.enabled; },
                    HeatmapGenerator.RenderHeatmap, ChangeIntensity, delegate(float f) { heatmapMaterial.SetFloat(SPID._Threshold, f); });
            }
            PointRenderBuffer.InitializeBuffers();
        }
        void Update() {
            if (renderPoints) PointRenderer.Render();
        }
    }
}