using System.Collections.Generic;
using Core.Files;
using Core.MapDownloader;
using Core.PointHolder;
using Core.RawDataInterpreter;
using Core;
using General;
using UI.FileSelectionGallery;
using UnityEngine;

namespace Managers {
    public class FileManager : MonoBehaviour {
        FileSelectionDisplay fileSelectionDisplay;
        void Awake() => fileSelectionDisplay = GetComponent<FileSelectionDisplay>();
        void Start() {
            string[] files = FileSaver.GetDatasets();
            foreach (string path in files) fileSelectionDisplay.AddRep(NewRep(path));
            fileSelectionDisplay.EnableDisplay();
            fileSelectionDisplay.AddDownloadListener(delegate { StartCoroutine(DetroitDatasetAPICaller.DownloadDataset(OnFinishDownload)); });
        }
        void OnFinishDownload(List<string> list, int count) {
            List<APIModels.Data> data = DataInterpreterManager.DeserializeAPIData(list);
            SerializedPointHolder sph = SerializedPointHolder.APIDataToSerializedPointHolder(data, count);
            string filePath = FileSaver.WritePoints(sph.name, sph, true);
            fileSelectionDisplay.AddRep(NewRep(filePath));
        }
        static void LoadFile(string filePath) {
            SerializedPointHolder sph = FileSaver.LoadPointsWithPath(filePath);
            PointsHolder.SetData(sph);
            CameraControl.SetCameraMovementState(true);
        }
        FileSelectionElementRep NewRep(string filePath) => new FileSelectionElementRep(filePath, delegate {
            LoadFile(filePath);
            fileSelectionDisplay.DisableDisplay();
        });
    }
}