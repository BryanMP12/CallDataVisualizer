using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.RawDataInterpreter {
    public class DataInterpreterManager : MonoBehaviour {
        [SerializeField] TextAsset dataText;
        static void DeserializeAndLoadManualData(TextAsset text) {
            ManualModels.Data data = JsonConvert.DeserializeObject<ManualModels.Data>(text.text);
            Debug.Log($"{data.features.Count} features found");
        }
        public static List<APIModels.Data> DeserializeAPIData(List<string> json) {
            List<APIModels.Data> dataList = new List<APIModels.Data>();
            foreach (string j in json) dataList.Add(JsonConvert.DeserializeObject<APIModels.Data>(j));
            return dataList;
        }
        public static int DeserializeCountResult(string json) {
            APIModels.Count count = JsonConvert.DeserializeObject<APIModels.Count>(json);
            return count.count;
        }
    }
}