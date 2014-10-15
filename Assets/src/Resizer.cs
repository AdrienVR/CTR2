using UnityEngine;
using System.Collections;

public class Resizer : MonoBehaviour {

	public int nb = 2;

	// Use this for initialization
	void Start () {
		//Designed for a 1600, 730)
		Debug.Log ("screen : " + Screen.width+","+Screen.height);
		int sx = Screen.width;
		int sy = Screen.height;
		float r = (float)sx / (float)sy;
		gameObject.transform.position = new Vector3 (gameObject.transform.position.x,
		                                            gameObject.transform.position.y,
		                                            gameObject.transform.position.z);
	}
}
