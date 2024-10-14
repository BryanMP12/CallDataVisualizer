using UnityEngine;

namespace General {
    public sealed class CameraControl : MonoBehaviour {
        Camera cam;
        Transform camTransform;
        [SerializeField] float zoomSpeed;
        [SerializeField] float zoomTransformSpeed;
        [SerializeField] Vector2 scrollLimits;
        [SerializeField] AnimationCurve zoomCurve;
        [SerializeField] [Range(50, 200)] int frameRate;
        bool movementOn;
        static CameraControl instance;
        void Awake() {
            instance = this;
            cam = GetComponent<Camera>();
            camTransform = transform;
            SetCameraMovementState(true);

            Application.targetFrameRate = frameRate;
        }
        void OnEnable() {
            InputManager.ScrollDeltaNone += OnScroll;
            InputManager.DragDelta += OnDrag;
        }
        void OnDisable() {
            InputManager.ScrollDeltaNone -= OnScroll;
            InputManager.DragDelta -= OnDrag;
        }
        public static bool SetCameraMovementState(bool state) => instance.movementOn = state;
        public static bool ToggleCameraMovementState() => instance.movementOn = !instance.movementOn;
        float scrollRatio = 1;
        void OnScroll(float scrollDelta, Vector2 direction) {
            if (!movementOn) return;
            scrollRatio = Mathf.Clamp01(scrollRatio - scrollDelta * zoomSpeed);
            cam.orthographicSize = Mathf.Lerp(scrollLimits.x, scrollLimits.y, zoomCurve.Evaluate(scrollRatio));
            OnDrag(new Vector2(direction.x, direction.y) * scrollDelta * zoomTransformSpeed);
        }
        void OnDrag(Vector2 dragDelta) {
            if (!movementOn) return;
            Vector3 position = camTransform.position + new Vector3(dragDelta.x, dragDelta.y);
            camTransform.position = position;
        }
    }
}