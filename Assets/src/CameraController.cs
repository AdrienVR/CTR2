using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	
	private KartController kc;
	public float positionForward = 0.85f;
	public float reversed = 1f;
	public Vector3 backward;

	// Use this for initialization
	void Start () {
	}

	public void SetKartController(KartController k)
	{
		kc = k;
	}
	
	
	// Update is called once per frame
	void Update ()
	{
		transform.position = kc.transform.position + backward - reversed*positionForward*6*kc.transform.forward;
		transform.LookAt (kc.transform.position + new Vector3(0f,2f,0f));
	}

	
	void FixedUpdate ()
	{
		//transform.LookAt (kc.transform.position + new Vector3(0f,2f,0f));
	}
	
}
