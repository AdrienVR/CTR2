using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gameplay : MonoBehaviour {

	public Dictionary <string, GameplayGravity> wheels = new Dictionary <string, GameplayGravity>();

	public Transform kart;

	// Use this for initialization
	void Start () {
		foreach (Transform child in transform){
			wheels[child.name] = child.GetComponent<GameplayGravity>();
		}
	}

	public void SetKart(Transform k){
		kart = k;
	}

	private void SmoothApply(Vector3 newAngles){
		Vector3 smooth = newAngles-kart.transform.localRotation.eulerAngles;
		smooth.x %= 360;
		smooth.z %= 360;
		if (smooth.x > 180)
			smooth.x -= 360;
		else if (smooth.x < -180)
			smooth.x += 360;
		if (smooth.z > 180)
			smooth.z -= 360;
		else if (smooth.z < -180)
			smooth.z += 360;
		//Debug.Log ( smooth.x+",z:" +smooth.z);
		float coeffSmooth = 2f;
		smooth.x = System.Math.Min (smooth.x, coeffSmooth);
		smooth.z = System.Math.Min (smooth.z, coeffSmooth);

		smooth.x = System.Math.Max (smooth.x, -coeffSmooth);
		smooth.z = System.Math.Max (smooth.z, -coeffSmooth);
		Vector3 new_smooth = kart.transform.localRotation.eulerAngles + smooth;
		/*new_smooth.x = System.Math.Min (new_smooth.x, 45);
		new_smooth.z = System.Math.Min (new_smooth.z, 20);
		new_smooth.x = System.Math.Max (new_smooth.x, -45);
		new_smooth.z = System.Math.Max (new_smooth.z, -20);*/
		kart.transform.localRotation = Quaternion.Euler (new_smooth);

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//copy parent
		//CheckDistance ();
		transform.localPosition = kart.transform.position + Vector3.up * 0.2f;

		Vector3 nextRot = kart.transform.localRotation.eulerAngles;
		nextRot.x = Vector3.Angle(Vector3.forward, wheels["forward"].posCollided - wheels["backward"].posCollided);
		nextRot.z = 0;//Vector3.Angle(Vector3.right,wheels["right"].posCollided - wheels["left"].posCollided);

		if (wheels ["forward"].posCollided.y > wheels ["backward"].posCollided.y)
			nextRot.x *= -1;
		if (wheels ["left"].posCollided.y > wheels ["right"].posCollided.y)
			nextRot.z *= -1;
		//if (kart.rigidbody.velocity.magnitude>2 && (kart.transform.localRotation.eulerAngles-nextRot).magnitude<2)
			SmoothApply (nextRot);
		transform.Rotate (new Vector3 (0, nextRot.y-transform.rotation.eulerAngles.y));
	}
}
