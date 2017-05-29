using UnityEngine;
using System.Collections.Generic;

public class SlidingTrace : MonoBehaviour
{
    class TraceMesh
    {
        public TraceMesh()
        {
            m_index = 0;
            m_previousPts = new Vector3[2];
            m_currentPts = new Vector3[2];
            m_vertices = new List<Vector3[]>();
        }

        public int m_index;
        public Vector3[] m_previousPts, m_currentPts;

        public List<Vector3[]> m_vertices;
    }

    public Material TraceMaterial;
    public float WheelSize = 0.2f;

    public Transform[] m_points;
    public int length = 9;

    //public float SpaceBetween
    private List<Vector3> mVertices = new List<Vector3>(4 * 9);
    private List<Vector2> mUvs = new List<Vector2>(4 * 9);
    private List<int> mTriangles = new List<int>(4 * 9);

    // Use this for initialization
    void Start()
    {

        m_traceMeshs = new TraceMesh[m_points.Length];
        for(int i = 0; i< m_traceMeshs.Length;i++)
        {
            m_traceMeshs[i] = new TraceMesh();
        }

        m_slidingTraces = new GameObject("SlidingTraces");
        m_mf = m_slidingTraces.AddComponent<MeshFilter>();
        m_mesh = new Mesh();
        m_mf.mesh = m_mesh;
        m_renderer = m_slidingTraces.AddComponent<MeshRenderer>();
        m_renderer.material = TraceMaterial;
    }


    void Update()
    {
        for (int i = 0; i < m_points.Length; i++)
        {
            UpdatePoint(m_points[i], m_traceMeshs[i]);
        }
        
        m_mesh.vertices = mVertices.ToArray();
        m_mesh.uv = mUvs.ToArray();
        m_mesh.triangles = mTriangles.ToArray();
        m_mesh.RecalculateBounds();
        m_mesh.RecalculateNormals();

        mVertices.Clear();
        mUvs.Clear();
        mTriangles.Clear();
    }

    // Update is called once per frame
    void UpdatePoint(Transform _tr, TraceMesh _mesh)
    {
        Vector3 localXAxis = _tr.right * WheelSize;

        _mesh.m_previousPts[0] = _mesh.m_currentPts[0];
        _mesh.m_previousPts[1] = _mesh.m_currentPts[1];
        _mesh.m_currentPts[0] = _tr.position + localXAxis - _tr.up * WheelSize;
        _mesh.m_currentPts[1] = _tr.position - localXAxis - _tr.up * WheelSize;

        // Creation of a the vertice table
        Vector3[] vertices = new Vector3[4];
        vertices[0] = _mesh.m_previousPts[0];
        vertices[1] = _mesh.m_previousPts[1];
        vertices[2] = _mesh.m_currentPts[1];
        vertices[3] = _mesh.m_currentPts[0];

        if (_mesh.m_index < _mesh.m_vertices.Count)
        {
            _mesh.m_vertices[_mesh.m_index] = new Vector3[4];
            for (int i = 0; i < 4; i++)
                _mesh.m_vertices[_mesh.m_index][i] = vertices[i];
        }
        else
        {
            _mesh.m_vertices.Add(vertices);
        }

        int offset = mVertices.Count;

        for (int i = 0; i < _mesh.m_vertices.Count; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                mVertices.Add(_mesh.m_vertices[i][j]);
                mUvs.Add(c_uvs[j]);
            }
            for (int j = 0; j < 6; j++)
            {
                mTriangles.Add(offset + i * 4 + c_triangles[j]);
            }
        }

        _mesh.m_index++;
        if (_mesh.m_index > length)
            _mesh.m_index = 0;
    }

    public void RemoveAll()
    {
        m_mesh = new Mesh();
        m_mf.mesh = m_mesh;
        for(int i = 0; i < m_traceMeshs.Length; i++)
            m_traceMeshs[i].m_vertices.Clear();
    }

    private int[] c_triangles = new int[] { 0, 1, 2, 0, 2, 3 };
    private Vector2[] c_uvs = new Vector2[]
    {
        new Vector2(0,0),
        new Vector2(1,0),
        new Vector2(1,1),
        new Vector2(0,1)
    };

    private TraceMesh[] m_traceMeshs;

    private GameObject m_slidingTraces;

    private MeshFilter m_mf;
    private Mesh m_mesh;
    private MeshRenderer m_renderer;
}
