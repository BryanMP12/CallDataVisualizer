using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace General {
    public class InputManager : MonoBehaviour {
        [SerializeField] Camera cam;
        Vector2 initialLocation;
        public static event Action<Vector2> ClickNone;
        public static event Action<Vector2> ClickZ;
        //
        //public static event Action<Vector2> DragDeltaUI;
        public static event Action<Vector2> DragDelta;
        //
        public static event Action<float, Vector2> ScrollDeltaUI;
        public static event Action<float, Vector2> ScrollDeltaNone;
        void Update() {
            bool clickZ = Input.GetKey(KeyCode.Z);
            if (Input.GetMouseButtonDown(0)) Click(cam.ScreenToWorldPoint(Input.mousePosition), clickZ);
            if (Input.GetMouseButton(0)) Drag(cam.ScreenToWorldPoint(Input.mousePosition), clickZ);
            Scroll(Input.mouseScrollDelta, cam.ScreenToViewportPoint(Input.mousePosition));
        }
        bool uiMouseMode;
        void Click(Vector2 position, bool holdZ) {
            if (EventSystem.current.IsPointerOverGameObject()) {
                uiMouseMode = true;
                return;
            }
            uiMouseMode = false;
            initialLocation = position;
            if (holdZ) ClickZ?.Invoke(position);
            else ClickNone?.Invoke(position);
        }
        void Drag(Vector2 position, bool holdZ) {
            if (uiMouseMode) return;
            Vector2 difference = initialLocation - position;
            if (difference.magnitude < 0.1f) return;
            if (holdZ) return;
            DragDelta?.Invoke(difference);
        }
        void Scroll(Vector2 scrollDelta, Vector2 mousePos) {
            if (scrollDelta.magnitude < 0.01f) return;
            Vector2 vectorFromCenter = mousePos - new Vector2(0.5f, 0.5f);
            if (EventSystem.current.IsPointerOverGameObject()) ScrollDeltaUI?.Invoke(scrollDelta.y, vectorFromCenter);
            else ScrollDeltaNone?.Invoke(scrollDelta.y, vectorFromCenter);
        }
    }
}