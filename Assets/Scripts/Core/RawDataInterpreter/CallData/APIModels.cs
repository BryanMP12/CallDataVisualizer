﻿using System.Collections.Generic;

//These models are used for when you downloaded with the API
namespace Core.RawDataInterpreter.CallData {
    public static class APIModels {
        public sealed class Count {
            public int count;
        }
        public sealed class Data {
            public string objectIdFieldName { get; set; }
            public UniqueIdField uniqueIdField { get; set; }
            public string globalIdFieldName { get; set; }
            public string geometryType { get; set; }
            public SpatialReference spatialReference { get; set; }
            public List<Field> fields { get; set; }
            public bool exceededTransferLimit { get; set; }
            public List<Feature> features { get; set; }
        }
        public sealed class UniqueIdField {
            public string name { get; set; }
            public bool isSystemMaintained { get; set; }
        }
        public sealed class SpatialReference {
            public int wkid { get; set; }
            public int latestWkid { get; set; }
        }
        public sealed class Field {
            public string name;
            public string type;
            public string alias;
            public string sqlType;
            public int length;
            public string domain;
            public string defaultValue;
        }
        public sealed class Feature {
            public Properties attributes { get; set; }
        }
    }
}