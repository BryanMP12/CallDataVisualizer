#if UNITY_EDITOR
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Core.MapDownloader {
    public enum MapStyle { DarkNoLabels, Satellite }
    public static class MapboxAPICaller {
        const string ACCESS_TOKEN = "pk.eyJ1IjoiYnJ5YW5waSIsImEiOiJjbGV4dHVmNXgwMDRrM3Ztc3JybW52bDlvIn0.7TqnziSGXnoGv0DPjFyEBQ";
        static string GetURL(double lon1, double lat1, double lon2, double lat2, int width, int height, MapStyle style) {
            string url =
                $"https://api.mapbox.com/styles/v1/bryanpi/{Styles[(int) style]}/static/" +
                $"[{lon1},{lat1},{lon2},{lat2}]/{width}x{height}@2x?" +
                $"logo=false&attribution=false&" +
                $"access_token={ACCESS_TOKEN}";
            return url;
        }
        static readonly string[] Styles = new string[2] {"clg55yp0y009501ppc0bbuoyj", "clg56pmlt000c01o66zot4qir"};
        //"clfagfyl1000101qg2y2c1e8h", 
        public static void DownloadMaps(MonoBehaviour mb, MapStyle style) {
            (double lon1, double lat1, double lon2, double lat2) = Dims.Bbox;
            (int width, int height) = Dims.MapDimensions;
            Debug.Log($"Map width: {width} Map height: {height}");

            mb.StartCoroutine(DownloadImage(lon1, lat1, lon2, lat2, width, height, $"{style.ToString()}", style));
        }
        public static void SetTextureArray(MapStyle style) {
            const int n = 4;
            Texture2DArray array = new Texture2DArray(Dims.Width, Dims.Height, 16, TextureFormat.RGB24, false);
            for (int i = 0; i < 16; i++) {
                Texture2D texture = Resources.Load<Texture2D>($"Maps/{style.ToString()}/{n}x{n}/map{i}");
                array.SetPixels(texture.GetPixels(0), i);
            }
            AssetDatabase.CreateAsset(array, $"Assets/Resources/Maps/{style.ToString()}/Map.asset");
        }
        static IEnumerator DownloadImage(double lon1, double lat1, double lon2, double lat2, int width, int height, string fileName, MapStyle style) {
            string url = GetURL(lon1, lat1, lon2, lat2, width, height, style);
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
            Texture2D map = DownloadHandlerTexture.GetContent(www);
            AssetDatabase.CreateAsset(map, $"Assets/Resources/Maps/{fileName}.asset");
            AssetDatabase.SaveAssets();
        }
    }
}
#endif