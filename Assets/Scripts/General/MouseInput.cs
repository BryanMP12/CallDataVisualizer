using System;
using UnityEngine;

namespace General {
    public class MouseInput : MonoBehaviour {
        [SerializeField] Camera cam;
        Vector2 initialLocation;
        public static event Action<float, Vector2> ScrollDelta;
        public static event Action<Vector2> DragDelta;
        void Update() {
            if (Input.GetMouseButtonDown(0)) Click(cam.ScreenToWorldPoint(Input.mousePosition));
            if (Input.GetMouseButton(0)) Drag(cam.ScreenToWorldPoint(Input.mousePosition));
            Scroll(Input.mouseScrollDelta, cam.ScreenToViewportPoint(Input.mousePosition));
        }
        void Click(Vector2 position) => initialLocation = position;
        void Drag(Vector2 position) {
            Vector2 difference = initialLocation - position;
            if (difference.magnitude < 0.1f) return;
            DragDelta?.Invoke(difference); 
        }
        void Scroll(Vector2 scrollDelta, Vector2 mousePos) {
            if (scrollDelta.magnitude < 0.01f) return;
            Vector2 vectorFromCenter = mousePos - new Vector2(0.5f, 0.5f);
            ScrollDelta?.Invoke(scrollDelta.y, vectorFromCenter);
        }
    }
}