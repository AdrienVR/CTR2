using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gameplay : MonoBehaviour {

	public Dictionary <string, Transform> wheels = new Dictionary <string, Transform>();

	public Transform kart;
	private Transform kartSteering;

	// Use this for initialization
	void Start () {
		foreach (Transform child in transform){
			wheels[child.name] = child;
		}
		kartSteering = kart.FindChild("steering");
		//SetPhysics (false);
	}

	public void SetKart(Transform k){
		kart = k;
	}
	
	public void SetPhysics(bool a){
		foreach(Transform t in wheels.Values){
			t.gameObject.collider.enabled = a;
		}
	}

	private void ResetBalls(){
		foreach(Transform t in wheels.Values)
			t.transform.localPosition += (new Vector3(t.transform.localPosition.x,0f, t.transform.localPosition.z) - t.transform.localPosition);
	}

	private void CheckDistance(){
		
		foreach(Transform t in wheels.Values){
			if (t.transform.localPosition.y < -1.66f)
				ResetBalls();/*
			if (t.transform.localPosition.y < -1.66f)
				t.transform.localPosition += new Vector3(0,1f);
			if (t.transform.localPosition.y > 1)
				t.transform.localPosition += new Vector3(0,-1);*/
		}
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
		float coeffSmooth = 0.5f;
		smooth.x = System.Math.Min (smooth.x, coeffSmooth);
		smooth.z = System.Math.Min (smooth.z, coeffSmooth);

		smooth.x = System.Math.Max (smooth.x, -coeffSmooth);
		smooth.z = System.Math.Max (smooth.z, -coeffSmooth);
		Vector3 new_smooth = kart.transform.localRotation.eulerAngles + smooth;
		/*new_smooth.x = System.Math.Min (new_smooth.x, 45);
		new_smooth.z = System.Math.Min (new_smooth.z, 20);
		new_smooth.x = System.Math.Max (new_smooth.x, -45);
		new_smooth.z = System.Math.Max (new_smooth.z, -20);*/
		//kartSteering.localRotation = Quaternion.Euler (new_smooth);
		//kart.transform.localRotation = Quaternion.Euler (new_smooth);

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//copy parent
		CheckDistance ();
		transform.localPosition = kart.transform.position + Vector3.up * 0.2f;

		Vector3 nextRot = kart.transform.localRotation.eulerAngles;
		nextRot.x = Vector3.Angle(Vector3.forward, wheels["forward"].localPosition - wheels["backward"].localPosition);
		nextRot.z = Vector3.Angle(Vector3.right,wheels["right"].localPosition - wheels["left"].localPosition);

		if (wheels ["forward"].localPosition.y > wheels ["backward"].localPosition.y)
			nextRot.x *= -1;
		if (wheels ["left"].localPosition.y > wheels ["right"].localPosition.y)
			nextRot.z *= -1;
		//if (kart.rigidbody.velocity.magnitude>2 && (kart.transform.localRotation.eulerAngles-nextRot).magnitude<2)
			SmoothApply (nextRot);
		transform.Rotate (new Vector3 (0, nextRot.y-transform.rotation.eulerAngles.y));
	}
}
