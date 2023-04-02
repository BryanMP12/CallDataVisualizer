using Core.Points;
using UnityEditor;
using UnityEngine;

namespace Editor {
    [CustomEditor(typeof(PointsHolder))]
    public class PointsHolderEditor : UnityEditor.Editor {
        SerializedProperty totalCount, priorityCounts, officerInitiatedCount, Descriptions;
        void OnEnable() {
            totalCount = serializedObject.FindProperty("totalCount");
            priorityCounts = serializedObject.FindProperty("priorityCounts");
            officerInitiatedCount = serializedObject.FindProperty("officerInitiatedCount");
            Descriptions = serializedObject.FindProperty("Descriptions");
        }
        public override void OnInspectorGUI() {
            GUI.enabled = false;
            EditorGUILayout.PropertyField(totalCount);
            EditorGUILayout.PropertyField(priorityCounts);
            EditorGUILayout.PropertyField(officerInitiatedCount);
            EditorGUILayout.PropertyField(Descriptions);
            GUI.enabled = true;
        }
    }
}