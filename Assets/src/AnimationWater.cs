using UnityEngine;
using System.Collections;

public class AnimationWater : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		gameObject.transform.position += new Vector3 (0.1f, 0);
		if (gameObject.transform.position.x > 50f)
						gameObject.transform.position = new Vector3 ();
	}
}
