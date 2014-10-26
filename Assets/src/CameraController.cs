using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	
	private KartController kc;
	public float positionForward = 1f;
	public Vector3 backward;
	//private bool started = false;

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
		//if (!started)
		//	StartCoroutine (WaitAndMove ());
		Vector3 t= kc.transform.position + backward - positionForward*6*kc.transform.forward;
		transform.position = t;
		transform.LookAt (kc.transform.position + new Vector3(0f,2f,0f));//*/
	}

	
	/*IEnumerator WaitAndMove() {
		started = true;
		yield return new WaitForSeconds(0.1f);
		Vector3 t= kc.transform.position + backward - positionForward*6*kc.transform.forward;
		transform.position = t;
		transform.LookAt (kc.transform.position + new Vector3(0f,2f,0f));
		started = false;
	}*/
	
}
