using System;
using System.Collections;
using System.Collections.Generic;
using Core.RawDataInterpreter;
using UnityEngine;
using UnityEngine.Networking;

namespace Core.MapDownloader {
    public static class DetroitDatasetAPICaller {
        static string Last30DaysUrl(int offset) =>
            $"https://services2.arcgis.com/qvkbeam7Wirps6zC/arcgis/rest/services/cad_30d_lookback/FeatureServer/0/" +
            $"query?where=1%3D1&outFields=priority,calldescription,call_timestamp,officerinitiated,totaltime,longitude,latitude&" +
            $"returnGeometry=false&resultOffset={offset}&outSR=4326&f=json";
        const string GetCountOnlyURL =
            "https://services2.arcgis.com/qvkbeam7Wirps6zC/arcgis/rest/services/cad_30d_lookback/FeatureServer/0/query?where=1%3D1&outFields=*&returnGeometry=false&returnCountOnly=true&outSR=4326&f=json";
        const int APIMaxRecordCount = 1000; //This is a hard limit set by the data provider
        public static event Action DownloadProcessStarted;
        //Current, Total
        public static event Action<int, int> DownloadProgressUpdated;
        //Calls, Total
        public static event Action<List<string>, int> DownloadCompleted;
        public static IEnumerator DownloadLatestDataset() {
            UnityWebRequest initialRequest = JsonDownloader(GetCountOnlyURL);
            DownloadProcessStarted?.Invoke();
            yield return SendRequest(initialRequest);
            string result = DownloadHandlerBuffer.GetContent(initialRequest);
            int totalCount = DataInterpreter.DeserializeCountResult(result);

            int callsRequired = (totalCount - 1) / APIMaxRecordCount + 1;
            List<string> jsonList = new List<string>();
            for (int i = 0; i < callsRequired; i++) {
                UnityWebRequest downloadRequest = JsonDownloader(Last30DaysUrl(i * APIMaxRecordCount));
                yield return SendRequest(downloadRequest);
                //
                DownloadProgressUpdated?.Invoke(Mathf.Min((i + 1) * APIMaxRecordCount, totalCount), totalCount);
                jsonList.Add(downloadRequest.downloadHandler.text);
            }
            DownloadCompleted?.Invoke(jsonList, totalCount);
        }
        static UnityWebRequest JsonDownloader(string url) => new UnityWebRequest(url, "GET") {downloadHandler = new DownloadHandlerBuffer()};
        static IEnumerator SendRequest(UnityWebRequest www) {
            yield return www.SendWebRequest();
            Debug.Log(www.result);
            switch (www.result) {
                case UnityWebRequest.Result.InProgress: break;
                case UnityWebRequest.Result.Success:    break;
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.ProtocolError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(www.error);
                    yield break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}