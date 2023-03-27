using UnityEngine;

namespace General {
    public class CameraControl : MonoBehaviour {
        Camera cam;
        [SerializeField] float zoomSpeed;
        [SerializeField] float zoomTransformSpeed;
        [SerializeField] Vector2Int scrollLimits;
        bool movementOn;
        static CameraControl instance;
        void Awake() {
            instance = this;
            cam = GetComponent<Camera>();
        }
        void OnEnable() {
            MouseInput.ScrollDelta += OnScroll;
            MouseInput.DragDelta += OnDrag;
        }
        void OnDisable() {
            MouseInput.ScrollDelta -= OnScroll;
            MouseInput.DragDelta -= OnDrag;
        }
        public static void SetCameraMovementState(bool state) => instance.movementOn = state;
        void OnScroll(float scrollDelta, Vector2 direction) {
            if (!movementOn) return;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scrollDelta * zoomSpeed, scrollLimits.x, scrollLimits.y);
            if (scrollDelta > 0) cam.transform.position += new Vector3(direction.x, direction.y, 0) * scrollDelta * zoomTransformSpeed;
        }
        void OnDrag(Vector2 dragDelta) {
            if (!movementOn) return;
            cam.transform.position += new Vector3(dragDelta.x, dragDelta.y, 0);
        }
    }
}