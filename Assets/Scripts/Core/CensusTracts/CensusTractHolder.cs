using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Core.CensusTracts {
    [CreateAssetMenu(fileName = "CensusTractHolder", menuName = "CensusTract/Holder", order = 0)]
    public sealed class CensusTractHolder : ScriptableObject {
        public List<CensusTract> CensusTracts = new List<CensusTract>();
    }
    
    #if UNITY_EDITOR
    [CustomEditor(typeof(CensusTractHolder))]
    public sealed class CensusTractHolderEditor : Editor {
        public override void OnInspectorGUI() {
            GUI.enabled = false;
            base.OnInspectorGUI();
            GUI.enabled = true;
        }
    }
    #endif
}