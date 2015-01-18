using UnityEngine;
using System.Collections;

public class GameplayGravity : MonoBehaviour {

	public Vector3 posCollided = new Vector3();
	private Vector3 initialPos;

	// Use this for initialization
	void Start () {
		initialPos = transform.localPosition + new Vector3(0,1);
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		gameObject.rigidbody.velocity = new Vector3 (0, -30f);
		
		if (transform.localPosition.y < -1.66f)
			// reset
			transform.localPosition += (initialPos-transform.localPosition);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.name != "Ground")
						return;
		posCollided = transform.localPosition;
	}
}
