using System.Collections.Generic;

//These models are used for when you manually downloaded the full dataset through the website, unfiltered.
//Not the API version.
namespace Core.RawDataInterpreter.CallData {
    public static class ManualModels {
        public sealed class Data {
            public string type { get; set; }
            public string name { get; set; }
            public CRS crs { get; set; }
            public List<Feature> features { get; set; }
        }
        public sealed class CRS {
            string type { get; set; }
            CRSProperties properties { get; set; }
        }
        public sealed class CRSProperties {
            public string name { get; set; }
        }
        public sealed class Feature {
            public string type { get; set; }
            public Properties properties { get; set; }
        }
    }
}