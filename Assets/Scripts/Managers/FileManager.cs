using System;
using System.Collections.Generic;
using System.IO;
using Core.CensusTracts;
using Core.Files;
using Core.MapDownloader;
using Core.Points;
using Core.RawDataInterpreter;
using Core.RawDataInterpreter.CallData;
using Core.RawDataInterpreter.Shapefiles;
using Core.RawDataInterpreter.TractData;
using General;
using UI.DownloadStatus;
using UI.FileSelectionGallery;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Managers {
    public sealed class FileManager : MonoBehaviour {
        FileSelectionDisplay fileSelectionDisplay;
        [SerializeField] CensusTractHolder censusTractHolder;
        [SerializeField] DownloadStatusDisplay _downloadStatusDisplay;
        void Awake() {
            fileSelectionDisplay = GetComponent<FileSelectionDisplay>();
            DetroitDatasetAPICaller.DownloadProcessStarted += OnDownloadProcessStarted;
            DetroitDatasetAPICaller.DownloadProgressUpdated += OnDownloadProgressUpdated;
            DetroitDatasetAPICaller.DownloadCompleted += OnDownloadCompleted;
        }
        void Start() {
            string[] files = FileSaver.GetDatasets();
            foreach (string path in files) fileSelectionDisplay.AddRep(NewRep(path));
            fileSelectionDisplay.AddDownloadListener(delegate { StartCoroutine(DetroitDatasetAPICaller.DownloadLatestDataset()); });
        }
        void OnDownloadProcessStarted() {
            string start = DateTime.Now.Subtract(TimeSpan.FromDays(30)).ToString("MM/dd/yy");
            string end = DateTime.Now.ToString("MM/dd/yy");
            _downloadStatusDisplay.Initialize(start, end);
        }
        void OnDownloadProgressUpdated(int current, int total) {
            _downloadStatusDisplay.SetProgress(current, total);
        }
        void OnDownloadCompleted(List<string> list, int totalCount) {
            _downloadStatusDisplay.SetProgress(totalCount, totalCount);
            List<APIModels.Data> data = DataInterpreter.DeserializeAPIData(list);
            SerializedPointHolder sph = SerializedPointHolder.APIDataToSerializedPointHolder(data, totalCount);
            #if !UNITY_WEBGL
            string filePath = FileSaver.WritePoints(sph.name, sph, true);
            fileSelectionDisplay.AddRep(NewRep(filePath));
            #else
            PointsHolder.SetData(sph);
            CameraControl.SetCameraMovementState(true);
            #endif
            _downloadStatusDisplay.Close();
            GetComponent<Canvas>().enabled = false;
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
#if UNITY_EDITOR
        [ContextMenu("CreateCensusTractHolder")]
        public void CreateCensusTractHolder() {
            // Assets/RawData/tl_2020_26_tract/tl_2020_26_tract.cpg
            string tractDataPath = $"{Application.dataPath}/RawData/t/t/t_mi.json";
            string shapeFilePath = $"{Application.dataPath}/RawData/tl_2020_26_tract/tl_2020_26_tract";
            if (!File.Exists(tractDataPath)) {
                Debug.LogError($"{tractDataPath} does not exist. Returning");
                return;
            }
            if (!File.Exists($"{shapeFilePath}.shp")) {
                Debug.LogError($"{shapeFilePath} does not exist. Returning");
                return;
            }

            CensusTractHolder newCensusTrackHolder = ScriptableObject.CreateInstance<CensusTractHolder>();

            newCensusTrackHolder.CensusTracts = ShapefileReader.ReadShapefile(shapeFilePath);

            List<(Models.Node node, List<Models.Adjacency> adjacencies)> detroitNodes = TractDataReader.ReadTractData(tractDataPath);

            Dictionary<int, int> nodeIdToCensusIndex = new Dictionary<int, int>();

            for (int i = 0; i < newCensusTrackHolder.CensusTracts.Count; i++) {
                CensusTract ct = newCensusTrackHolder.CensusTracts[i];
                string tractNumber = ct.Number.ToString();
                int nodeId = detroitNodes.Find(x => x.node.BASENAME == tractNumber).node.id;
                nodeIdToCensusIndex.Add(nodeId, i);
            }

            foreach ((Models.Node node, List<Models.Adjacency> adjacencies) in detroitNodes) {
                if (!nodeIdToCensusIndex.TryGetValue(node.id, out int censusIndex)) {
                    Debug.LogError($"Could not find Node id of {node.id}. Continuing");
                    return;
                }
                List<int> borders = new List<int>();
                for (int i = 0; i < adjacencies.Count; i++) {
                    if (!nodeIdToCensusIndex.TryGetValue(adjacencies[i].id, out int id)) continue;
                    borders.Add(id);
                }
                newCensusTrackHolder.CensusTracts[censusIndex].SetData(new CensusTractData(borders, node));
            }

            AssetDatabase.CreateAsset(newCensusTrackHolder, "Assets/CensusTracts/NewDetroitCityCensusTracts.asset");
            AssetDatabase.SaveAssets();
        }
#endif
    }
}