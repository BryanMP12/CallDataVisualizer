using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Gallery {
    public class PooledGallery<T> : MonoBehaviour where T : ElementRep, new() {
        [SerializeField] GameObject elementPrefab;
        [SerializeField] protected RectTransform parent;
        [SerializeField] float scrollSpeed;
        readonly Stack<IGalleryElement> AvailableElements = new Stack<IGalleryElement>();
        readonly Stack<int> ReadyIndices = new Stack<int>();
        readonly List<T> ElementReps = new List<T>();
        protected IEnumerable<T> Reps => ElementReps;
        float GalleryHeight => parent.rect.height;
        readonly float ElementHeight = new T().YSize;
        Vector2 OffsetLimits = new Vector2(0, 0); //max, min
        float currentOffset;
        public void AddRep(T rep) {
            rep.SetYPosition(ElementReps.Count * -ElementHeight);
            ElementReps.Add(rep);
            ResetOffsetLimits();
            MoveOffset(0);
        }
        public void SortReps(Comparison<T> compare) {
            ElementReps.Sort(compare);
            for(int i = 0; i < ElementReps.Count; i++) ElementReps[i].SetYPosition(i * -ElementHeight);
            MoveReps(0);
        }
        public void RemoveRep(T rep) {
            ElementReps.Remove(rep);
            ResetOffsetLimits();
        }
        void ResetOffsetLimits() => OffsetLimits = new Vector2(0, Mathf.Max(0, (ElementReps.Count) * ElementHeight - GalleryHeight));
        public void ResetDisplayReps() {
            foreach (T t in ElementReps) AvailableElements.Push(t.TurnOffDisplay());
            ReadyIndices.Clear();
            ElementReps.Clear();
        }
        void AddAvailableDisplays(int count) {
            if (count < 0) return;
            for (int i = 0; i < count; i++) {
                IGalleryElement element = Instantiate(elementPrefab, parent).GetComponent<IGalleryElement>();
                element.InitializeDisplay();
                element.SetVisualState(false);
                AvailableElements.Push(element);
            }
        }
        protected void MoveOffset(float delta, Vector2 direction = default) {
            float offset = currentOffset - delta * scrollSpeed;
            if (offset < OffsetLimits.x || OffsetLimits.y < offset) return;
            MoveReps(offset);
        }
        void MoveReps(float offset) {
            currentOffset = offset;
            for (int i = 0; i < ElementReps.Count; i++) {
                int checkVal = ElementReps[i].SetOffset(offset);
                switch (checkVal) {
                    case 0: break;
                    case 1:
                        ReadyIndices.Push(i);
                        break;
                    case 2: {
                        AvailableElements.Push(ElementReps[i].TurnOffDisplay());
                        break;
                    }
                }
            }
            AddAvailableDisplays(ReadyIndices.Count - AvailableElements.Count);
            while (ReadyIndices.Count > 0) ElementReps[ReadyIndices.Pop()].SetDisplay(AvailableElements.Pop(), offset);
        }
    }
}