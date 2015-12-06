using UnityEngine;
using System.Collections;

public class SkidTrace : MonoBehaviour
{

	public int NbTraces = 10;
	public GameObject traces;
	public Material TraceMaterial;

	void Start ()
	{
		m_index = 0;
		m_traces = new GameObject[NbTraces];
		GameObject traces = new GameObject();
		traces.AddComponent<Transform>();
	}

	// Update is called once per frame
	void Update ()
	{
		GameObject trace = GameObject.CreatePrimitive(PrimitiveType.Plane);
		trace.GetComponent<Renderer> ().material = TraceMaterial;
		if(!(m_index <NbTraces))
		{
			m_index=NbTraces-1;
			Destroy(m_traces[0]);
			for(int i =1;i<NbTraces;i++)
			{
				m_traces[i-1]=m_traces[i];
			}
		}
		if (m_index > 0)
		{
			float w = (gameObject.transform.rotation.w+ m_traces [m_index - 1].transform.rotation.w)/2;
			float x = (gameObject.transform.rotation.x+ m_traces [m_index - 1].transform.rotation.x)/2;
			float y = (gameObject.transform.rotation.y+ m_traces [m_index - 1].transform.rotation.y)/2;
			float z = (gameObject.transform.rotation.z+ m_traces [m_index - 1].transform.rotation.z)/2;
			trace.transform.rotation = new Quaternion(x,y,z,w);
		}
		else
			trace.transform.rotation = gameObject.transform.rotation;
		trace.transform.localScale=new Vector3(0.06f,0.01f,gameObject.transform.localScale.z/20f);
		trace.transform.position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y-gameObject.transform.localScale.y/3f,gameObject.transform.position.z);
		m_traces [m_index] = trace;
		m_index++;
	}

	private GameObject[] m_traces;
	private int m_index;

}
