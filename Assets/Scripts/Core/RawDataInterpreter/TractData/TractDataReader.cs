using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.RawDataInterpreter.TractData {
    public static class TractDataReader {
        public static List<(Models.Node, List<Models.Adjacency>)> ReadTractData(string path) {
            StreamReader reader = new StreamReader(path);
            Models.Data data = JsonConvert.DeserializeObject<Models.Data>(reader.ReadToEnd());
            Debug.Log($"{data.nodes.Count} nodes found");
            Debug.Log($"{data.adjacency.Count} edges found");
            Debug.Log($"Detroit Tract Count: {CensusTracts.DetroitCity.Length}");

            List<(Models.Node, List<Models.Adjacency>)> detroitNodes = new List<(Models.Node, List<Models.Adjacency>)>();
            for (int i = 0; i < data.nodes.Count; i++) {
                Models.Node n = data.nodes[i];
                if (n.COUNTYFP20 != "163") continue;
                if (CensusTracts.DetroitCity.Contains(n.BASENAME)) 
                    detroitNodes.Add((n, data.adjacency[n.id]));
            }
            Debug.Log($"Detroit Node Count: {detroitNodes.Count}");
            return detroitNodes;
        }
    }
}