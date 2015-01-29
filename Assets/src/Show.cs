using UnityEngine;
using System.Collections;

public class Show : MonoBehaviour {
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0,1));
	}
}
