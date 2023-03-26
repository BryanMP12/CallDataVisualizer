namespace UI.Gallery {
    public interface IGalleryElement {
        public void InitializeDisplay();
        public void SetVisualState(bool state);
        public void SetRep(ElementRep elementRep);
        public void RemoveRep();
        public void SetPosition(float yOffset);
    }
}