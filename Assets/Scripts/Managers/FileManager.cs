using System.Collections.Generic;
using Core.CensusTracts;
using Core.Files;
using Core.MapDownloader;
using Core.Points;
using Core.RawDataInterpreter;
using Core.RawDataInterpreter.CallData;
using General;
using UI.FileSelectionGallery;
using UnityEditor;
using UnityEngine;

namespace Managers {
    public class FileManager : MonoBehaviour {
        FileSelectionDisplay fileSelectionDisplay;
        [SerializeField] string shapeFilePath;
        [SerializeField] CensusTractHolder censusTractHolder;
        void Awake() => fileSelectionDisplay = GetComponent<FileSelectionDisplay>();
        void Start() {
            string[] files = FileSaver.GetDatasets();
            foreach (string path in files) fileSelectionDisplay.AddRep(NewRep(path));
            fileSelectionDisplay.EnableDisplay();
            fileSelectionDisplay.AddDownloadListener(delegate { StartCoroutine(DetroitDatasetAPICaller.DownloadDataset(OnFinishDownload)); });
        }
        void OnFinishDownload(List<string> list, int count) {
            List<APIModels.Data> data = DataInterpreter.DeserializeAPIData(list);
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
        [ContextMenu("DeserializeShapefile")]
        public void DeserializeAndSaveShapefile() {
#if UNITY_EDITOR
            censusTractHolder.CensusTracts = ShapefileReader.ReadShapefile(shapeFilePath);
            EditorUtility.SetDirty(censusTractHolder);
#endif
        }
    }
}