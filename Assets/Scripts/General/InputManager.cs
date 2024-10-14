using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace General {
    public sealed class InputManager : MonoBehaviour {
        [SerializeField] Camera cam;
        Vector2 initialLocation;
        public static event Action<Vector2> TractClick;
        public static event Action<Vector2> DragDelta;
        //
        public static event Action<float, Vector2> ScrollDeltaUI;
        public static event Action<float, Vector2> ScrollDeltaNone;
        void Update() {
            if (Input.GetMouseButtonDown(0)) LeftClick(cam.ScreenToWorldPoint(Input.mousePosition));
            if (Input.GetMouseButton(0)) LeftDrag(cam.ScreenToWorldPoint(Input.mousePosition));
            if (Input.GetMouseButtonDown(1)) RightClick(cam.ScreenToWorldPoint(Input.mousePosition));
            Scroll(Input.mouseScrollDelta, cam.ScreenToViewportPoint(Input.mousePosition));
        }
        bool uiMouseMode;
        void LeftClick(Vector2 position) {
            if (EventSystem.current.IsPointerOverGameObject()) {
                uiMouseMode = true;
                return;
            }
            uiMouseMode = false;
            initialLocation = position;
        }
        void LeftDrag(Vector2 position) {
            if (uiMouseMode) return;
            Vector2 difference = initialLocation - position;
            if (difference.magnitude < 0.1f) return;
            DragDelta?.Invoke(difference);
        }
        void RightClick(Vector2 position) { TractClick?.Invoke(position); }
        void Scroll(Vector2 scrollDelta, Vector2 mousePos) {
            if (scrollDelta.magnitude < 0.01f) return;
            Vector2 vectorFromCenter = mousePos - new Vector2(0.5f, 0.5f);
            if (EventSystem.current.IsPointerOverGameObject()) ScrollDeltaUI?.Invoke(scrollDelta.y, vectorFromCenter);
            else ScrollDeltaNone?.Invoke(scrollDelta.y, vectorFromCenter);
        }
    }
}