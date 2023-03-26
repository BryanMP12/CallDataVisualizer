using System;
using System.Collections.Generic;
using Map.MapDownloader;
using Map.PointHolder;
using Map.Points;
using Map.RawDataInterpreter;
using UI.FileSelectionGallery;
using UnityEngine;

namespace Map.Files {
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
            PointRenderer.InitializePoints();
        }
        FileSelectionElementRep NewRep(string filePath) => new FileSelectionElementRep(filePath, delegate {
            LoadFile(filePath);
            fileSelectionDisplay.DisableDisplay();
        });
    }
}