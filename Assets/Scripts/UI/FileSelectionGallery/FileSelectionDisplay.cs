using System;
using UI.Gallery;
using UnityEngine;
using UnityEngine.UI;
using UserInput;

namespace UI.FileSelectionGallery {
    public class FileSelectionDisplay : PooledGallery<FileSelectionElementRep> {
        [SerializeField] Button downloadButton;
        Canvas canvas;
        Action downloadAction;
        void Awake() {
            FileSelectionElementRep.SetRectLimits(new Vector2(-rectTransform.rect.height, 0));
            MoveOffset(0);
            canvas = GetComponent<Canvas>();
            downloadButton.onClick.AddListener(delegate { downloadAction?.Invoke(); });
        }
        public void AddDownloadListener(Action download) => downloadAction = download;
        public void EnableDisplay() {
            canvas.enabled = true;
            MouseInput.ScrollDelta += MoveOffset;
        }
        public void DisableDisplay() {
            canvas.enabled = false;
            MouseInput.ScrollDelta -= MoveOffset;
        }
    }
}