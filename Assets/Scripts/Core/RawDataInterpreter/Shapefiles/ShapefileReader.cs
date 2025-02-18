﻿using System.Collections.Generic;
using System.Linq;
using Core.CensusTracts;
using NetTopologySuite.Features;
using NetTopologySuite.IO.Esri;
using UnityEngine;
using Point = NetTopologySuite.Geometries.Point;

namespace Core.RawDataInterpreter.Shapefiles {
    public static class ShapefileReader {
        public static List<CensusTract> ReadShapefile(string path) {
            List<CensusTract> censusTracts = new List<CensusTract>();
            foreach (Feature feature in Shapefile.ReadAllFeatures(path)) {
                if ((string) feature.Attributes["COUNTYFP"] != "163") continue; //Check if in Wayne County

                string tractName = feature.Attributes["NAMELSAD"].ToString(); //Census Tract XXXX
                string number = tractName.Remove(0, 13);
                if (!CensusTracts.DetroitCity.Contains(number)) continue;
                Debug.Log(number);
                Debug.Log($"Geometry: {feature.Geometry}");

                List<Coordinate> shape = feature.Geometry.Coordinates.Select(coord => new Coordinate(coord.X, coord.Y)).ToList();
                Point p = feature.Geometry.Centroid;
                Coordinate centroid = new Coordinate(p.X, p.Y);
                censusTracts.Add(new CensusTract(number, shape, centroid));
            }
            Debug.Log($"Total: {censusTracts.Count}");
            Debug.Log($"Detroit Census Tract count: {CensusTracts.DetroitCity.Length}");
            return censusTracts;
        }
    }
}