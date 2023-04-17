using System;
using General;
using UI.Gallery;
using UnityEngine;
using UnityEngine.UI;

namespace UI.FileSelectionGallery {
    public class FileSelectionDisplay : PooledGallery<FileSelectionElementRep>, IAddon {
        [SerializeField] Button downloadButton;
        Canvas canvas;
        Action downloadAction;
        public bool Enabled { get; set; }
        void Awake() {
            FileSelectionElementRep.SetRectLimits(new Vector2(-parent.rect.height, 0));
            MoveOffset(0);
            canvas = GetComponent<Canvas>();
            Enabled = false;
            downloadButton.onClick.AddListener(delegate { downloadAction?.Invoke(); });
        }
        public void AddDownloadListener(Action download) => downloadAction = download;
        public void EnableDisplay() {
            canvas.enabled = Enabled = true;
            InputManager.ScrollDeltaUI += MoveOffset;
        }
        public void DisableDisplay() {
            canvas.enabled = Enabled = false;
            InputManager.ScrollDeltaUI -= MoveOffset;
        }
    }
}