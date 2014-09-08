using UnityEngine;
using System.Collections;

public class Camera1Controller : MonoBehaviour
{

	public KartController kc;

	// Use this for initialization
	void Start () { 
		for(int i=1;i<200;i++)
		{
			Debug.Log("PD!"); // t'es content ?
		}
	}
	

	// Update is called once per frame
	void Update ()
	{
		Vector3 t= kc.transform.position+ new Vector3(0,5,0) + 12*kc.transform.forward;
		transform.position = t;
		transform.LookAt (kc.transform);
	}

}
