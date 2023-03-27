using System.Collections.Generic;

//These models are used for when you manually downloaded the full dataset through the website, unfiltered.
//Not the API version.
namespace Core.RawDataInterpreter {
    public static class ManualModels {
        public class Data {
            public string type { get; set; }
            public string name { get; set; }
            public CRS crs { get; set; }
            public List<Feature> features { get; set; }
        }
        public class CRS {
            string type { get; set; }
            CRSProperties properties { get; set; }
        }
        public class CRSProperties {
            public string name { get; set; }
        }
        public class Feature {
            public string type { get; set; }
            public Properties properties { get; set; }
        }
    }
}