using System;
using UI.Gallery;
using UnityEngine;

namespace UI.FileSelectionGallery {
    public class FileSelectionElementRep : ElementRep {
        public readonly string filePath;
        readonly Action LoadButtonClicked;
        public FileSelectionElementRep(string path, Action loadButtonAction) => (filePath, LoadButtonClicked) = (path, loadButtonAction);
        public void LoadPath() => LoadButtonClicked?.Invoke();
        public void DeletePath() { }

        //Dimensions
        static Vector2 RectLimits;
        public static void SetRectLimits(Vector2 limits) => RectLimits = new Vector2(25 + limits.x, 25 + limits.y);
        public override Vector2 PositionLimits => RectLimits;
        public override float YSize => 50;
        public FileSelectionElementRep() { filePath = null; }
        //
    }
}