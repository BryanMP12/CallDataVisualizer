using System;
using TMPro;
using UnityEngine;

namespace UI.DownloadStatus {
    public sealed class DownloadStatusDisplay : MonoBehaviour {
        [SerializeField] TextMeshProUGUI _timelineText;
        [SerializeField] TextMeshProUGUI _progressText;
        Canvas _canvas;
        void Awake() {
            _canvas = GetComponent<Canvas>();
        }
        public void Initialize(string start, string end) {
            _canvas.enabled = true;
            _timelineText.SetText($"Downloading data from {start} to {end}");
            _progressText.SetText("");
        }
        public void SetProgress(int count, int total) {
            _progressText.SetText($"Progress: {count}/{total}");
        }
        public void Close() {
            _canvas.enabled = false;
        }
    }
}