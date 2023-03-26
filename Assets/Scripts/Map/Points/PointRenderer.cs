using Map.PointHolder;
using UnityEngine;

namespace Map.Points {
    public class PointRenderer : MonoBehaviour {
        [SerializeField] Material material;
        [SerializeField] bool render;
        [SerializeField] [Range(0.1f, 2)] float s;
        [SerializeField] [Range(0, 360)] float r;
        static PointRenderer instance;
        void Awake() => instance = this;
        Mesh mesh;
        Bounds bounds;
        ComputeBuffer meshPropertiesBuffer;
        ComputeBuffer argsBuffer;
        struct PointProperties {
            readonly Vector2 position;
            readonly int priority;
            readonly int description_index;
            public PointProperties(Vector2 pos, int p, int d) => (position, priority, description_index) = (pos, p, d);
            public const int Stride = sizeof(float) * 2 + sizeof(int) + sizeof(int);
        };
        public static void InitializePoints() => instance.InitializePointsInternal();
        void InitializePointsInternal() {
            ReleaseBuffers();

            mesh = CreateQuad();
            bounds = new Bounds(Vector3.zero, new Vector3(Dims.Width * 2, Dims.Height * 2));

            int pointCount = PointsHolder.TotalPointCount;
            Debug.Log($"Points: {pointCount}");

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
                    properties[i] = new PointProperties(point, p.Priority, p.DescriptionIndex);
                }
                meshPropertiesBuffer = new ComputeBuffer(pointCount, PointProperties.Stride);
                meshPropertiesBuffer.SetData(properties);
                material.SetBuffer("_Points", meshPropertiesBuffer);
            }
            {
                Color[] colors = new Color[6];
                colors[0] = Color.black;
                for (int i = 1; i < 6; i++) colors[i] = Color.Lerp(Color.green, Color.red, i / 5f);
                material.SetColorArray("_ColorArray", colors);
            }
            {
                float[] descriptions = new float[PointsHolder.Descriptions.Length];
                for (int i = 0; i < descriptions.Length; i++) descriptions[i] = 1f;
                material.SetFloatArray("_WriteDescription", descriptions);
            }
            render = true;
        }
        void Update() {
            if (render) Graphics.DrawMeshInstancedIndirect(mesh, 0, material, bounds, argsBuffer);
        }
        void ReleaseBuffers() {
            meshPropertiesBuffer?.Release();
            meshPropertiesBuffer = null;
            argsBuffer?.Release();
            argsBuffer = null;
        }
        void OnDisable() => ReleaseBuffers();
        Mesh CreateQuad() {
            Mesh m = new Mesh();
            Vector3[] vertices = new Vector3[4] {new Vector3(-s, -s), new Vector3(s, -s), new Vector3(-s, s), new Vector3(s, s)};
            Matrix4x4 rotateMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 0, r));
            for (int i = 0; i < 4; i++) vertices[i] = rotateMatrix.MultiplyVector(vertices[i]);
            int[] tris = new int[6] {0, 2, 1, 2, 3, 1};
            Vector2[] uv = new Vector2[4] {Vector2.zero, new Vector2(1, 0), new Vector2(0, 1), Vector2.one};
            m.vertices = vertices;
            m.triangles = tris;
            m.uv = uv;
            return m;
        }
#if UNITY_EDITOR
        void OnValidate() { mesh = CreateQuad(); }
#endif
    }
}