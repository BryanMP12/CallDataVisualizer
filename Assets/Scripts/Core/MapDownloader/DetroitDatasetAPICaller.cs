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
        const int APIMaxRecordCount = 1000;
        public static IEnumerator DownloadDataset(Action<List<string>, int> finishDownloadAction) {
            int count;
            {
                UnityWebRequest www = JsonDownloader(GetCountOnlyURL);
                yield return SendRequest(www);
                string result = DownloadHandlerBuffer.GetContent(www);
                count = DataInterpreter.DeserializeCountResult(result);
                Debug.Log($"DataCount: {count}");
            }
            {
                int callsRequired = (count - 1) / APIMaxRecordCount + 1;
                List<string> jsonList = new List<string>();
                //callsRequired = 2; //temp for testing
                //count = 2000; //temp for testing
                for (int i = 0; i < callsRequired; i++)
                {
                    UnityWebRequest www = JsonDownloader(Last30DaysUrl(i * APIMaxRecordCount));
                    yield return SendRequest(www);
                    jsonList.Add(www.downloadHandler.text);
                }
                finishDownloadAction?.Invoke(jsonList, count);
            }
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