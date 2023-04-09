using UnityEngine;

namespace General {
    public class CameraControl : MonoBehaviour {
        Camera cam;
        Transform camTransform;
        [SerializeField] float zoomSpeed;
        [SerializeField] float zoomTransformSpeed;
        [SerializeField] Vector2 scrollLimits;
        bool movementOn;
        static CameraControl instance;
        void Awake() {
            instance = this;
            cam = GetComponent<Camera>();
            camTransform = transform;
            SetCameraMovementState(true);
        }
        void OnEnable() {
            MouseInput.ScrollDelta += OnScroll;
            MouseInput.DragDelta += OnDrag;
        }
        void OnDisable() {
            MouseInput.ScrollDelta -= OnScroll;
            MouseInput.DragDelta -= OnDrag;
        }
        public static bool SetCameraMovementState(bool state) => instance.movementOn = state;
        public static bool ToggleCameraMovementState() => instance.movementOn = !instance.movementOn;
        void OnScroll(float scrollDelta, Vector2 direction) {
            if (!movementOn) return;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scrollDelta * zoomSpeed, scrollLimits.x, scrollLimits.y);
            OnDrag(new Vector2(direction.x, direction.y) * scrollDelta * zoomTransformSpeed); 
        }
        void OnDrag(Vector2 dragDelta) {
            if (!movementOn) return;
            Vector3 position = camTransform.position + new Vector3(dragDelta.x, dragDelta.y);
            camTransform.position = position;
        }
    }
}