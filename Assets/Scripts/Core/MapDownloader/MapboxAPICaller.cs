using System;
using System.Collections;
using Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Core.MapDownloader {
    public static class MapboxAPICaller {
        const string ACCESS_TOKEN = "pk.eyJ1IjoiYnJ5YW5waSIsImEiOiJjbGV4dHVmNXgwMDRrM3Ztc3JybW52bDlvIn0.7TqnziSGXnoGv0DPjFyEBQ";
        static string GetURL() {
            double[] bbox = Dims.bbox;
            string url =
                $"https://api.mapbox.com/styles/v1/bryanpi/clfagfyl1000101qg2y2c1e8h/static/" +
                $"[{bbox[0]},{bbox[1]},{bbox[2]},{bbox[3]}]/{Dims.Width}x{Dims.Height}@2x?" +
                $"logo=false&attribution=false&" +
                $"access_token={ACCESS_TOKEN}";
            return url;
        }
        //Should probably move this somewhere else. I only want this as editor code
        public static void SaveTexture(Texture tex, string fileName) {
            if (fileName != null) AssetDatabase.CreateAsset(tex, $"Assets/Resources/Maps/{fileName}.asset");
        }
        static string MapPath(string fileName) => $"Assets/Resources/Maps/{fileName}.asset";
        //
        public static IEnumerator DownloadImage(Action<Texture> finishAction) {
            string url = GetURL();
            UnityWebRequest www = new UnityWebRequest(url, "GET");
            www.downloadHandler = new DownloadHandlerTexture();
            yield return www.SendWebRequest();
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
            Texture map = DownloadHandlerTexture.GetContent(www);
            finishAction?.Invoke(map);
        }
    }
}