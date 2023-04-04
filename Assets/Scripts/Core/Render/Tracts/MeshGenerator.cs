using System.Collections.Generic;
using Core.CensusTracts;
using TriangleNet.Geometry;
using TriangleNet.Meshing;
using TriangleNet.Topology;
using UnityEngine;

namespace Core.Render.Tracts {
    public static class MeshGenerator {
        public static Mesh MakeMesh(IReadOnlyList<Coordinate> coords, int index) {
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

            Vector2[] uvs = new Vector2[iMesh.Vertices.Count];
            for (int i = 0; i < uvs.Length; i++) uvs[i] = new Vector2(index, index);
            
            return new Mesh() {
                vertices = GetVertices(iMesh.Vertices),
                triangles = GetTriangleIds(iMesh.Triangles),
                uv = uvs
            };
        }
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