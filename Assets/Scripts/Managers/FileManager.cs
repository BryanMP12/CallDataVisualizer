using System.Collections.Generic;
using Core.CensusTracts;
using Core.Files;
using Core.MapDownloader;
using Core.Points;
using Core.RawDataInterpreter;
using Core.RawDataInterpreter.CallData;
using Core.RawDataInterpreter.Shapefiles;
using Core.RawDataInterpreter.TractData;
using General;
using UI.FileSelectionGallery;
using UnityEditor;
using UnityEngine;

namespace Managers {
    public class FileManager : MonoBehaviour {
        FileSelectionDisplay fileSelectionDisplay;
        [SerializeField] string shapeFilePath;
        [SerializeField] CensusTractHolder censusTractHolder;
        [SerializeField] string tractDataFilePath;
        void Awake() => fileSelectionDisplay = GetComponent<FileSelectionDisplay>();
        void Start() {
            string[] files = FileSaver.GetDatasets();
            foreach (string path in files) fileSelectionDisplay.AddRep(NewRep(path));
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
        [ContextMenu("DeserializeTractFile")]
        public void DeserializeTractDataFile() {
            List<(Models.Node node, List<Models.Adjacency> adjacencies)> detroitNodes = TractDataReader.ReadTractData(tractDataFilePath);

            Dictionary<int, int> nodeIdToCensusIndex = new Dictionary<int, int>();
            for (int i = 0; i < censusTractHolder.CensusTracts.Count; i++) {
                CensusTract ct = censusTractHolder.CensusTracts[i];
                string tractNumber = ct.Number.ToString();
                int nodeId = detroitNodes.Find(x => x.node.BASENAME == tractNumber).node.id;
                nodeIdToCensusIndex.Add(nodeId, i);
            }

            foreach ((Models.Node node, List<Models.Adjacency> adjacencies) in detroitNodes) {
                int censusIndex = nodeIdToCensusIndex[node.id];
                List<int> borders = new List<int>();
                for (int i = 0; i < adjacencies.Count; i++) {
                    if (!nodeIdToCensusIndex.TryGetValue(adjacencies[i].id, out int id)) continue;
                    borders.Add(id);
                }
                censusTractHolder.CensusTracts[censusIndex].SetData(new CensusTractData(borders, node));
            }
            EditorUtility.SetDirty(censusTractHolder);
        }
    }
}