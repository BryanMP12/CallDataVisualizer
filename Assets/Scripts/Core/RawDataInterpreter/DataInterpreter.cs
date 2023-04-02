using System.Collections.Generic;
using Core.RawDataInterpreter.CallData;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.RawDataInterpreter {
    public static class DataInterpreter {
        public static int DeserializeCountResult(string json) {
            APIModels.Count count = JsonConvert.DeserializeObject<APIModels.Count>(json);
            return count.count;
        }
        public static List<APIModels.Data> DeserializeAPIData(List<string> json) {
            List<APIModels.Data> dataList = new List<APIModels.Data>();
            foreach (string j in json) dataList.Add(JsonConvert.DeserializeObject<APIModels.Data>(j));
            return dataList;
        }
        public static void DeserializeAndLoadManualData(TextAsset text) {
            ManualModels.Data data = JsonConvert.DeserializeObject<ManualModels.Data>(text.text);
            Debug.Log($"{data.features.Count} features found");
        }
    }
}