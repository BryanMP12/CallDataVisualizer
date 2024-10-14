using Core;
using Core.MapDownloader;
using General;
using UnityEngine;

namespace Managers {
    public sealed class BaseMapManager : MonoBehaviour {
        [SerializeField] Transform mapTransform;
        [SerializeField] Camera cam;
        [SerializeField] Material material;
        [SerializeField] Texture2D[] textures;
        public void SetTexture() {
            material.SetTexture(SPID._Map, textures[0]);
        }
        
        #if UNITY_EDITOR
        void InitializeTransforms() {
            mapTransform.localScale = new Vector3(Dims.Width, Dims.Height, 0);
            mapTransform.position = new Vector3(Dims.Width, Dims.Height) / 2;
            cam.transform.position = new Vector3(Dims.Width, Dims.Height, -20) / 2;
            cam.orthographicSize = Dims.Height / 2f;
        }
        [SerializeField] MapStyle style;
        [ContextMenu("DownloadMaps")]
        public void DownloadMaps() {
            MapboxAPICaller.DownloadMaps(this, style);
        }
        [ContextMenu("WriteMapsToArray")]
        public void WriteMapsToArray() {
            MapboxAPICaller.SetTextureArray(style);
        }
        #endif
    }
}