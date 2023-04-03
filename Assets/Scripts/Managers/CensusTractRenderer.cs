using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Core.CensusTracts;
using TriangleNet;
using TriangleNet.Geometry;
using TriangleNet.Meshing;
using TriangleNet.Meshing.Algorithm;
using TriangleNet.Topology;
using UnityEngine;

namespace Managers {
    public class CensusTractRenderer : MonoBehaviour {
        [SerializeField] CensusTractHolder holder;
        [SerializeField] GameObject censusTractPrefab;
        [SerializeField] MeshFilter filter;
        [SerializeField] [Range(0, 250)] int val;
        void Awake() {
            foreach (CensusTract tract in holder.CensusTracts) {
                LineRenderer lr = Instantiate(censusTractPrefab).GetComponent<LineRenderer>();
                lr.positionCount = tract.Shape.Count;
                for (int i = 0; i < tract.Shape.Count; i++) {
                    Vector2 p = Dims.CoordToPort(tract.Shape[i].Lon, tract.Shape[i].Lat);
                    lr.SetPosition(i, new Vector3(p.x, p.y, -2));
                }
            }
        }
        void MakeMesh(int index) {
            //Dwyer
            //Incremental
            //SweepLine

            List<Coordinate> coords = holder.CensusTracts[index].Shape;

            int coordCount = coords.Count;
            Vertex[] vertices = new Vertex[coordCount];
            for (int i = 0; i < coordCount; i++) {
                double[] point = Dims.CoordToPortDouble(coords[i].Lon, coords[i].Lat);
                vertices[i] = new Vertex(point[0], point[1]);
            }

            Polygon p = new Polygon();
            p.Add(new Contour(vertices), 1);
            ConstraintOptions co = new ConstraintOptions() {
                ConformingDelaunay = true,
                Convex = false
            };
            QualityOptions qo = new QualityOptions() { };
            IMesh iMesh = p.Triangulate(co, qo);

            UnityEngine.Mesh mesh = new UnityEngine.Mesh() {
                vertices = GetVertices(iMesh.Vertices),
                triangles = GetTriangleIds(iMesh.Triangles)
            };
            filter.mesh = mesh;
        }
        void OnValidate() { MakeMesh(val); }
        static Vector3[] GetVertices(ICollection<Vertex> vertices) {
            Vector3[] verts = new Vector3[vertices.Count];
            int index = 0;
            foreach (Vertex vert in vertices) verts[index++] = new Vector3((float) vert.X, (float) vert.Y, 0);
            return verts;
        }
        static int[] GetTriangleIds(ICollection<Triangle> triangles) {
            int[] tris = new int[triangles.Count * 3];
            int index = 0;
            foreach (Triangle tri in triangles) {
                tris[index++] = tri.GetVertexID(1);
                tris[index++] = tri.GetVertexID(0);
                tris[index++] = tri.GetVertexID(2);
            }
            return tris;
        }
    }
}