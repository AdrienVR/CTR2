using UnityEngine;
using System.Collections;

public class GameplayGravity : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		gameObject.rigidbody.velocity = new Vector3 (0, -13f);
	}
}
