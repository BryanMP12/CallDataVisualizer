using UnityEngine;

namespace Core.Render {
    public struct PointProperties {
        readonly Vector2 position;
        readonly int priority;
        readonly int description_index;
        readonly int officerInitiated;
        public PointProperties(Vector2 pos, int p, int d, bool oi) => (position, priority, description_index, officerInitiated) = (pos, p, d, oi ? 1 : 0);
        public const int Stride = sizeof(float) * 2 + sizeof(int) + sizeof(int) + sizeof(int);
    }
}