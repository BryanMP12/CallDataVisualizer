using System;
using General;
using UI.Gallery;
using UnityEngine;

namespace UI.DescriptionGallery {
    public class DescriptionDisplay : PooledGallery<DescriptionElementRep>, IAddon {
        Canvas canvas;
        void Awake() {
            canvas = GetComponent<Canvas>();
            Enabled = false;
        }
        public void EnableDisplay() {
            canvas.enabled = Enabled = true;
            MouseInput.ScrollDelta += MoveOffset;
            DescriptionElementRep.SetRectLimits(new Vector2(-parent.rect.height, 0));
            MoveOffset(0);
        }
        public void DisableDisplay() {
            canvas.enabled = Enabled = false;
            MouseInput.ScrollDelta -= MoveOffset;
        }
        public bool Enabled { get; set; }
    }
}