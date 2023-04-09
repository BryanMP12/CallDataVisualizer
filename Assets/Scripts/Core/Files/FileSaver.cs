using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Core.Points;
using UnityEngine;

namespace Core.Files {
    public static class FileSaver {
        static readonly BinaryFormatter formatter = new BinaryFormatter();
        static string DatasetDirectory => $"{Application.persistentDataPath}/Datasets";
        public static string WritePoints(string fileName, SerializedPointHolder serializedPointHolder, bool overwrite) {
            if (!Directory.Exists(DatasetDirectory)) Directory.CreateDirectory(DatasetDirectory);
            string path = $"{DatasetDirectory}/{fileName}.data";
            if (File.Exists(path)) {
                if (overwrite) {
                    Debug.Log("File already exists. Overwriting");
                    File.Delete(path);
                } else {
                    Debug.Log("File already exists. Will not overwrite. Exiting");
                    return null;
                }
            }
            FileStream fileStream = new FileStream(path, FileMode.Create);
            formatter.Serialize(fileStream, serializedPointHolder);
            fileStream.Close();
            return path;
        }
        public static string[] GetDatasets() => Directory.GetFiles(DatasetDirectory);
        public static SerializedPointHolder LoadPointsWithPath(string path) {
            if (!File.Exists(path)) {
                Debug.LogError("Could not find path");
                return default;
            }
            FileStream fileStream = new FileStream(path, FileMode.Open);
            SerializedPointHolder serializedPointHolder = formatter.Deserialize(fileStream) as SerializedPointHolder;
            fileStream.Close();
            return serializedPointHolder;
        }
        public static void DeletePath(string path) {
            if (!File.Exists(path)) {
                Debug.LogError("Could not find path");
                return;
            }
            File.Delete(path);
        }
    }
}