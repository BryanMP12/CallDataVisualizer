using UnityEngine;

namespace Map {
    public class MapManager : MonoBehaviour {
        [SerializeField] Transform mapTransform;
        [SerializeField] Camera cam;
        void InitializeTransforms() {
            mapTransform.localScale = new Vector3(Dims.Width, Dims.Height, 0);
            mapTransform.position = new Vector3(Dims.Width, Dims.Height) / 2;
            cam.transform.position = new Vector3(Dims.Width, Dims.Height, -20) / 2;
            cam.orthographicSize = Dims.Height / 2f;
        }
    }
}