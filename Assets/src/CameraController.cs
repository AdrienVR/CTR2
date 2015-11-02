using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	
	private KartController kc;
	public float positionForward = 0.85f;
	public float reversed = 1f;
	public Vector3 backward;

	private float startTime;
	private float journeyLength;

	void Start() {
		startTime = Time.time;
		Vector3 objectiv  = kc.transform.position + backward - reversed*positionForward*6*kc.transform.forward+kc.transform.right*4;
		//Vector3 tr = (transform.position - objectiv)/20f;
		//while (Vector3.Distance(transform.position, objectiv)>100f){
		transform.position = 0.75f*objectiv;
		//}
		journeyLength = Vector3.Distance(transform.position, kc.transform.position);
		//Debug.Log ("duree de voyage " + journeyLength);

	}
		
	void FixedUpdate() {
		if (KartController.stop == true){
			float distCovered = (Time.time - startTime);
			float fracJourney = distCovered / journeyLength;
			Vector3 objectiv  = kc.transform.position + backward - reversed*positionForward*6*kc.transform.forward;
			transform.position = Vector3.Lerp(transform.position, objectiv, fracJourney*0.75f );
			transform.LookAt (kc.transform.position + new Vector3(0f,2f,0f));
		}
  	}
  
	public void SetKartController(KartController k)
	{
		kc = k;
	}
  
	// Update is called once per frame
	void Update ()
	{
		if (KartController.stop == false){
			transform.position = kc.transform.position + backward - reversed*positionForward*6*kc.transform.forward;
			transform.LookAt (kc.transform.position + new Vector3(0f,2f,0f));
		}
	}
	
}
