using Core.Points;
using General;
using UnityEngine;

namespace Core.Render.Points {
    public static class PointRenderer {
        static Material material;
        static Mesh mesh;
        static Bounds bounds;
        static ComputeBuffer argsBuffer;
        static readonly Color[] priorityColors = new Color[6] {
            Color.black, Color.Lerp(Color.green, Color.red, 4 / 4f), Color.Lerp(Color.green, Color.red, 3 / 4f), Color.Lerp(Color.green, Color.red, 2 / 4f),
            Color.Lerp(Color.green, Color.red, 1 / 4f), Color.Lerp(Color.green, Color.red, 0 / 4f)
        };
        public static void InitializePoints(Material m) {
            material = m;
            ReleaseBuffers();
            SetMesh();

            bounds = new Bounds(Vector3.zero, new Vector3(Dims.Width * 2, Dims.Height * 2));
            {
                int pointCount = PointsHolder.TotalPointCount;
                Debug.Log($"Points to Render: {pointCount}");
                //Argument buffer used by DrawMeshInstancedIndirect
                uint[] args = new uint[5] {mesh.GetIndexCount(0), (uint) pointCount, mesh.GetIndexStart(0), mesh.GetBaseVertex(0), 0};
                argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
                argsBuffer.SetData(args);
            }
            {
                material.SetColorArray(SPID._ColorArray, priorityColors);
            }
        }
        public static void Render() => Graphics.DrawMeshInstancedIndirect(mesh, 0, material, bounds, argsBuffer);
        //0, 1, 2, 3, 4, 5, NonOfficer, Officer
        public static void SetVarsToRender(ComputeBuffer drawVarsBuffer) => material.SetBuffer(SPID._VarsToRender, drawVarsBuffer);
        public static void SetDescriptionsToRender(ComputeBuffer descriptionBuffer) => material.SetBuffer(SPID._WriteDescription, descriptionBuffer);
        public static void SetPointsBuffer(ComputeBuffer pointsBuffer) => material.SetBuffer(SPID._Points, pointsBuffer);
        public static void ReleaseBuffers() {
            argsBuffer?.Release();
            argsBuffer = null;
        }
        public static void SetSize(float size) { SetMesh(size); }
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