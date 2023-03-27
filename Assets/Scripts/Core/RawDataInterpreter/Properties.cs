namespace Core.RawDataInterpreter {
    public class Properties {
        // public ulong incident_id { get; set; }
        // public string agency { get; set; }
        // public string incident_address { get; set; }
        // public uint zip_code { get; set; }
        public string priority { get; set; }
        // public string callcode { get; set; }
        public string calldescription { get; set; }
        // public string category { get; set; }
        public string call_timestamp { get; set; }
        // public string precinct_sca { get; set; }
        // public string respondingunit { get; set; }
        public string officerinitiated { get; set; }
        // public double? intaketime { get; set; }
        // public double? dispatchtime { get; set; }
        // public double? traveltime { get; set; }
        // public double? totalresponsetime { get; set; }
        // public double? time_on_scene { get; set; }
        public double? totaltime { get; set; }
        // public string neighborhood { get; set; }
        // public string block_id { get; set; }
        // public string council_district { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
    }
}