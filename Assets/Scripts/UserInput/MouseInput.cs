using System;
using UnityEngine;

namespace UserInput {
    public class MouseInput : MonoBehaviour {
        [SerializeField] Camera cam;
        [SerializeField] float zoomSpeed;
        [SerializeField] Vector2Int scrollLimits;
        Vector2 initialLocation;
        public static event Action<float> ScrollDelta;
        void Update() {
            if (Input.GetMouseButtonDown(0)) Click(cam.ScreenToWorldPoint(Input.mousePosition));
            if (Input.GetMouseButton(0)) Drag(cam.ScreenToWorldPoint(Input.mousePosition));
            Scroll(Input.mouseScrollDelta);
        }
        void Click(Vector2 position) => initialLocation = position;
        void Drag(Vector2 position) => cam.transform.position += (Vector3)(initialLocation - position);
        void Scroll(Vector2 scrollDelta) {
            if (scrollDelta.magnitude < 0.01f) return;
            //cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scrollDelta.y * zoomSpeed, scrollLimits.x, scrollLimits.y);
            ScrollDelta?.Invoke(scrollDelta.y);
        }
    }
}