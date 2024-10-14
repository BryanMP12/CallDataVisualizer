using System.IO;
using TMPro;
using UI.Gallery;
using UnityEngine;
using UnityEngine.UI;

namespace UI.FileSelectionGallery {
    public sealed class FileSelectionElement : MonoBehaviour, IGalleryElement {
        [SerializeField] Image baseImage;
        [SerializeField] TextMeshProUGUI fileName;
        [SerializeField] TextMeshProUGUI filePath;
        [SerializeField] Button loadButton;
        [SerializeField] Image buttonImage;
        [SerializeField] TextMeshProUGUI buttonText;
        RectTransform rect;
        //[SerializeField] Button deleteButton;
        FileSelectionElementRep fileRep;
        public void InitializeDisplay() {
            loadButton.onClick.AddListener(() => { fileRep?.LoadPath(); });
            rect = GetComponent<RectTransform>();
            //deleteButton.onClick.AddListener(() => { fileRep?.DeletePath(); });
        }
        public void SetPosition(float position) => rect.anchoredPosition = new Vector2(0, position);
        public void SetVisualState(bool state) =>
            baseImage.enabled = fileName.enabled = filePath.enabled = 
                loadButton.enabled = buttonImage.enabled = buttonText.enabled /*= deleteButton.enabled*/ = state;
        public void SetRep(ElementRep rep) {
            fileRep = (FileSelectionElementRep) rep;
            fileName.SetText(FullPathToFileName(fileRep.FilePath));
            filePath.SetText(fileRep.FilePath);
            rect.anchoredPosition = new Vector2(0, rep.initialYPosition);
        }
        static string FullPathToFileName(string path) {
            string dirName = Path.GetDirectoryName(path);
            return path.Remove(0, dirName?.Length ?? 0);
        }
        public void RemoveRep() => fileRep = null;
    }
}