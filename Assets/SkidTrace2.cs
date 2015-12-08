using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkidTrace2 : MonoBehaviour
{
	public Transform SymetricalWheel;
	public Material TraceMaterial;
	public float WheelSize=0.2f;
	//public float SpaceBetween

	// Use this for initialization
	void Start ()
	{
		m_index = 0;
		m_vertices = new List<Vector3[]>();
		m_previousPts = new Vector3[2];
		m_currentPts = new Vector3[2];
		
		m_plan = new GameObject ();
		m_mf = m_plan.AddComponent<MeshFilter>();
		m_mesh = new Mesh();
		m_mf.mesh = m_mesh;
		m_renderer = m_plan.AddComponent<MeshRenderer>();
		m_renderer.material = TraceMaterial;
		
		Vector3 localXAxis = (transform.position - SymetricalWheel.transform.position).normalized*WheelSize;
		m_currentPts [0] = transform.position + localXAxis - transform.up * WheelSize;
		m_currentPts[1]=transform.position - localXAxis - transform.up * WheelSize;
	}

	
	// Update is called once per frame
	void Update ()
	{
		Vector3 localXAxis = (transform.position - SymetricalWheel.transform.position).normalized*WheelSize;

		m_previousPts [0] = m_currentPts [0];
		m_previousPts [1] = m_currentPts [1];
		m_currentPts[0]=transform.position + localXAxis - transform.up*0.3f;
		m_currentPts[1]=transform.position - localXAxis - transform.up*0.3f;

		// Creation of a the vertice table
		Vector3[] vertices = new Vector3[4];
		vertices [0] = m_previousPts [0];
		vertices [1] = m_previousPts [1];
		vertices [2] = m_currentPts [1];
		vertices [3] = m_currentPts [0];

		Vector3[] mVertices = new Vector3[4 * (m_vertices.Count + 1)];
		Vector2[] mUv = new Vector2[4 * (m_vertices.Count + 1)];
		int[] mTriangles = new int[6 * (m_vertices.Count + 1)];

		if (m_index < m_vertices.Count)
		{
			m_vertices [m_index] = new Vector3[4];
			for (int i = 0;i<4;i++)
				m_vertices [m_index][i] = vertices[i];
		}
		else
		{
			m_vertices.Add(vertices);
		}

		int index = 0;
		for(int i = 0;i <m_vertices.Count;i++)
		{
			for (int j = 0 ; j < 4 ; j++)
			{
				int index44 = i * 4 + j;
				mVertices[index44] = m_vertices[i][j];
				mUv[index44] = c_uvs[j];
			}
			for (int j = 0 ; j < 6 ; j++)
			{
				int index66 = i * 6 + j;
				mTriangles[index66] = i * 4 + c_triangles[j];
			}
		}

		m_mesh.vertices = mVertices;
		m_mesh.uv = mUv;
		m_mesh.triangles = mTriangles;
		// Creation of the normals
		m_mesh.RecalculateBounds();
		m_mesh.RecalculateNormals();

		m_index++;
		if (m_index > 9)
			m_index = 0;
	}

	public void RemoveAll()
	{
		m_mesh = new Mesh();
		m_mf.mesh = m_mesh;
		m_vertices = new List<Vector3[]>();
	}

	private int[] c_triangles = new int[] {0,1,2,0,2,3};
	private Vector2[] c_uvs  = new Vector2[]
	{
		new Vector2(0,0),
		new Vector2(1,0),
		new Vector2(1,1),
		new Vector2(0,1)
	};

	private List<Vector3[]> m_vertices;

	private int m_index;
	private float m_deltaX;
	private Vector3[] m_previousPts, m_currentPts;

	private GameObject m_plan;
	private MeshFilter m_mf;
	private Mesh m_mesh;
	private MeshRenderer m_renderer;
}
