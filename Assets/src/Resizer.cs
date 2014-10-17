using UnityEngine;
using System.Collections;

public class Resizer : MonoBehaviour {

	public int nbX = 1;
	public int nbY = 2;

	// Use this for initialization
	void Start () {
		Debug.Log ("screen : " + Screen.width+","+Screen.height);
		ResizeLocation ();
	}

	void ResizeLocation(){
		//Designed for a 1600, 730)
		int sx = Screen.width / nbY * nbY;
		int sy = Screen.height / nbX * nbX;
		float ry = (float)sy / 730f;
		float rx = (float)sx / 1600f / ry;
		gameObject.transform.position = new Vector3 (gameObject.transform.position.x * rx,
		                                             gameObject.transform.position.y ,//* ry,
		                                             gameObject.transform.position.z);

	}
}
