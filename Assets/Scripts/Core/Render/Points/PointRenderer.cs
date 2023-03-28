using Core.PointHolder;
using UnityEngine;

namespace Core.Render.Points {
    public static class PointRenderer {
        static Material material;
        static Mesh mesh;
        static Bounds bounds;
        static ComputeBuffer meshPropertiesBuffer;
        static ComputeBuffer argsBuffer;
        static readonly Color[] priorityColors = new Color[6] {
            Color.black, Color.Lerp(Color.green, Color.red, 1 / 5f), Color.Lerp(Color.green, Color.red, 2 / 5f), Color.Lerp(Color.green, Color.red, 3 / 5f),
            Color.Lerp(Color.green, Color.red, 4 / 5f), Color.Lerp(Color.green, Color.red, 5 / 5f)
        };
        struct PointProperties {
            readonly Vector2 position;
            readonly int priority;
            readonly int description_index;
            readonly int officerInitiated;
            public PointProperties(Vector2 pos, int p, int d, bool oi) => (position, priority, description_index, officerInitiated) = (pos, p, d, oi ? 1 : 0);
            public const int Stride = sizeof(float) * 2 + sizeof(int) + sizeof(int) + sizeof(int);
        }
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
                    Vector2 point = Dims.CoordToPort(p.Longitude, p.Latitude);
                    properties[i] = new PointProperties(point, p.Priority, p.DescriptionIndex, p.OfficerInitiated);
                }
                meshPropertiesBuffer = new ComputeBuffer(pointCount, PointProperties.Stride);
                meshPropertiesBuffer.SetData(properties);
                material.SetBuffer("_Points", meshPropertiesBuffer);
            }
            {
                bool[] priorityToRender = new bool[6] {true, true, true, true, true, true};
                SetPrioritiesToRender(priorityToRender);
                material.SetColorArray("_ColorArray", priorityColors);
            }
            {
                SetRenderOfficerInitiatedState(true);
                SetRenderNonOfficerInitiatedState(true);
            }
            {
                bool[] descriptionsToRender = new bool[PointsHolder.Descriptions.Length];
                for (int i = 0; i < descriptionsToRender.Length; i++) descriptionsToRender[i] = true;
                SetDescriptions(descriptionsToRender);
            }
        }
        public static void Render() => Graphics.DrawMeshInstancedIndirect(mesh, 0, material, bounds, argsBuffer);
        public static void SetDescriptions(bool[] descriptionsToRender) {
            float[] descriptions = new float[descriptionsToRender.Length];
            for (int i = 0; i < descriptions.Length; i++) descriptions[i] = descriptionsToRender[i] ? 1 : 0;
            material.SetFloatArray("_WriteDescription", descriptions);
        }
        public static void SetPrioritiesToRender(bool[] priorityToRender) {
            float[] priorities = new float[6];
            for (int i = 0; i < 6; i++) priorities[i] = priorityToRender[i] ? 1 : 0;
            material.SetFloatArray("_RenderPriority", priorities);
        }
        public static void SetRenderNonOfficerInitiatedState(bool b) => material.SetFloat("_RenderNonOfficerInitiated", b ? 1 : 0);
        public static void SetRenderOfficerInitiatedState(bool b) => material.SetFloat("_RenderOfficerInitiated", b ? 1 : 0);
        public static void ReleaseBuffers() {
            meshPropertiesBuffer?.Release();
            meshPropertiesBuffer = null;
            argsBuffer?.Release();
            argsBuffer = null;
        }
        static void SetMesh(float s = 1, float r = 45) {
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