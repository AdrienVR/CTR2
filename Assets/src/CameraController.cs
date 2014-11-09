using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	
	private KartController kc;
	public float positionForward = 0.85f;
	public float reversed = 1f;
	public Vector3 backward;
	
	public float speed = 1.0F;
	private float startTime;
	private float journeyLength;

	void Start() {
		startTime = Time.time;
		journeyLength = Vector3.Distance(transform.position, kc.transform.position);
		Vector3 objectiv  = kc.transform.position + backward - reversed*positionForward*6*kc.transform.forward;
		transform.position = Vector3.Lerp (transform.position,objectiv, 0.75f);
	}
		
	void FixedUpdate() {
		if (KartController.stop == true){
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			Vector3 objectiv  = kc.transform.position + backward - reversed*positionForward*6*kc.transform.forward;
			transform.position = Vector3.Lerp(transform.position, objectiv, fracJourney );
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
