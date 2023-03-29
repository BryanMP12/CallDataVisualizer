using Core.PointHolder;
using General;
using UnityEngine;

namespace Core.Render.Points {
    public static class PointRenderer {
        static Material material;
        static Mesh mesh;
        static Bounds bounds;
        static ComputeBuffer pointBuffer;
        static ComputeBuffer drawVarsBuffer; //0, 1, 2, 3, 4, 5, nonOfficer, officer
        static ComputeBuffer drawDescriptionsBuffer;
        static ComputeBuffer argsBuffer;
        static readonly Color[] priorityColors = new Color[6] {
            Color.black, Color.Lerp(Color.green, Color.red, 1 / 5f), Color.Lerp(Color.green, Color.red, 2 / 5f), Color.Lerp(Color.green, Color.red, 3 / 5f),
            Color.Lerp(Color.green, Color.red, 4 / 5f), Color.Lerp(Color.green, Color.red, 5 / 5f)
        };
        public static void InitializePoints(Material m) {
            material = m;
            ReleaseBuffers();
            SetMesh();

            bounds = new Bounds(Vector3.zero, new Vector3(Dims.Width * 2, Dims.Height * 2));

            int pointCount = PointsHolder.TotalPointCount;
            Debug.Log($"Points to Render: {pointCount}");

            {
                //Argument buffer used by DrawMeshInstancedIndirect
                uint[] args = new uint[5] {mesh.GetIndexCount(0), (uint) pointCount, mesh.GetIndexStart(0), mesh.GetBaseVertex(0), 0};
                argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
                argsBuffer.SetData(args);
            }
            {
                //Initialize buffer with the given population
                PointProperties[] properties = new PointProperties[pointCount];
                for (int i = 0; i < pointCount; i++) {
                    Point p = PointsHolder.Points[i];
                    properties[i] = new PointProperties(Dims.CoordToPort(p.Longitude, p.Latitude), p.Priority, p.DescriptionIndex, p.OfficerInitiated);
                }
                pointBuffer = new ComputeBuffer(pointCount, PointProperties.Stride);
                pointBuffer.SetData(properties);
                material.SetBuffer(SPID._Points, pointBuffer);
            }
            {
                material.SetColorArray(SPID._ColorArray, priorityColors);
            }
            {
                drawVarsBuffer = new ComputeBuffer(8, sizeof(int));
                SetVarsToRender(new bool[8] {true, true, true, true, true, true, true, true});
            }
            {
                drawDescriptionsBuffer = new ComputeBuffer(PointsHolder.Descriptions.Length, sizeof(int));
                bool[] properties = new bool[PointsHolder.Descriptions.Length];
                for (int i = 0; i < properties.Length; i++) properties[i] = true;
                SetDescriptionsToRender(properties);
            }
        }
        public static void Render() => Graphics.DrawMeshInstancedIndirect(mesh, 0, material, bounds, argsBuffer);
        //0, 1, 2, 3, 4, 5, NonOfficer, Officer
        public static void SetVarsToRender(bool[] vars) {
            int[] properties = new int[8];
            for (int i = 0; i < 8; i++) properties[i] = vars[i] ? 1 : 0;
            drawVarsBuffer.SetData(properties);
            material.SetBuffer(SPID._VarsToRender, drawVarsBuffer);
        }
        public static void SetDescriptionsToRender(bool[] descriptionsToRender) {
            int[] descriptions = new int[descriptionsToRender.Length];
            for (int i = 0; i < descriptions.Length; i++) descriptions[i] = descriptionsToRender[i] ? 1 : 0;
            drawDescriptionsBuffer.SetData(descriptions);
            material.SetBuffer(SPID._WriteDescription, drawDescriptionsBuffer);
        }
        public static void ReleaseBuffers() {
            pointBuffer?.Release();
            pointBuffer = null;
            drawVarsBuffer?.Release();
            drawVarsBuffer = null;
            drawDescriptionsBuffer?.Release();
            drawDescriptionsBuffer = null;
            argsBuffer?.Release();
            argsBuffer = null;
        }
        static void SetMesh(float s = 0.5f, float r = 45) {
            Mesh m = new Mesh();
            Vector3[] vertices = new Vector3[4] {new Vector3(-s, -s), new Vector3(s, -s), new Vector3(-s, s), new Vector3(s, s)};
            Matrix4x4 rotateMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 0, r));
            for (int i = 0; i < 4; i++) vertices[i] = rotateMatrix.MultiplyVector(vertices[i]);
            int[] tris = new int[6] {0, 2, 1, 2, 3, 1};
            Vector2[] uv = new Vector2[4] {Vector2.zero, new Vector2(1, 0), new Vector2(0, 1), Vector2.one};
            m.vertices = vertices;
            m.triangles = tris;
            m.uv = uv;
            mesh = m;
        }
    }
}