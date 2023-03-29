using UnityEngine;

namespace UI.Gallery {
    public abstract class ElementRep {
        public float initialYPosition;
        float currentPosition;
        bool inUse;
        IGalleryElement poolElement;
        public abstract float YSize { get; }
        public abstract Vector2 PositionLimits { get; }
        public void SetYPosition(float yPos) => initialYPosition = yPos;
        public int SetOffset(float offset) {
            float position = initialYPosition + offset;
            if (inUse) poolElement.SetPosition(position);
            if (OutsideFully(position)) return inUse ? 2 : 0;
            return inUse ? 0 : 1;
        }
        public void SetDisplay(IGalleryElement element, float offset) {
            inUse = true;
            poolElement = element;
            poolElement.SetVisualState(true);
            poolElement.SetRep(this);
            poolElement.SetPosition(initialYPosition + offset);
        }
        public IGalleryElement TurnOffDisplay() {
            inUse = false;
            poolElement.SetVisualState(false);
            poolElement.RemoveRep();
            return poolElement;
        }
        bool OutsideFully(float position) => position + YSize / 2 < PositionLimits.x || PositionLimits.y < position - YSize / 2;
    }
}