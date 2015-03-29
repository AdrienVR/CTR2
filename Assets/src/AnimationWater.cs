using UnityEngine;
using System.Collections;

public class AnimationWater : MonoBehaviour {
	
	// Update is called once per frame
	void FixedUpdate () {
		gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles+new Vector3 (0, 0.06f));
	}
}
