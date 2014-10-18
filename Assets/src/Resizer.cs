using UnityEngine;
using System.Collections;

public class Resizer : MonoBehaviour {
	//
	public Rect rectCam;
	// Use this for initialization
	void Start () {
		Debug.Log ("screen : " + Screen.width+","+Screen.height);
		ResizeLocation ();
	}

	void ResizeLocation(){
		float rx = Screen.width / 816f * 459f / Screen.height;
		gameObject.transform.position = new Vector3 (gameObject.transform.position.x * rx,
		                                             gameObject.transform.position.y,
		                                             gameObject.transform.position.z);

	}
}
