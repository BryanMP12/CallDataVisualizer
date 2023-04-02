using System.Collections.Generic;
using UnityEngine;

namespace Core.CensusTracts {
    [CreateAssetMenu(fileName = "CensusTractHolder", menuName = "CensusTract/Holder", order = 0)]
    public class CensusTractHolder : ScriptableObject {
        public List<CensusTract> CensusTracts = new List<CensusTract>();
    }
}