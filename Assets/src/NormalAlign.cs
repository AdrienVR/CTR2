using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NormalAlign : MonoBehaviour {
	
	public Vector3 previous;
	public Vector3 upDir;


	private int equals = 0;
	private Transform steering;

	public static Quaternion upDirAngle;
	
	void Start ()
	{
		foreach (Transform child in transform){
			if (child.name == "steering")
			{
				steering = child;
				break;
			}
		}
	}

	void OnCollisionEnter(Collision collisionInfo)
	{
		if (collisionInfo.gameObject.name != "Ground")
			return;
		if (collisionInfo.contacts.Length>0)
			upDir = collisionInfo.contacts[0].normal;
	}
	
	void OnCollisionStay(Collision collisionInfo)
	{
		if (collisionInfo.gameObject.name != "Ground")
			return;
		if (collisionInfo.contacts.Length>0)
			upDir = collisionInfo.contacts[0].normal;
	}


	void Update () {
		
		//Debug.DrawRay(transform.position, upDir, Color.blue, 20, true);

		if(Vector3.Angle(upDir, previous)<10 && System.Math.Abs(upDir.y - previous.y)<0.1f)
			equals ++;
		else
			equals = 0;
		previous = upDir;

		if (equals>3){
			//steering.forward = transform.forward;
			//steering.up = upDir;
			//Debug.Log (System.Math.Sqrt(upDir.x*upDir.x+upDir.y*upDir.y+upDir.z*upDir.z));
			if (upDir.y<0.9f)
				upDir = new Vector3(upDir.x,0.9f, upDir.z);

			Vector3 hello = steering.up *0.75f + upDir*0.25f;
			//Vector3 hello = upDir;
			steering.rotation = Quaternion.FromToRotation(steering.up, hello) * steering.rotation;
			//upDirAngle = Quaternion.FromToRotation(steering.up, upDir);
			equals = 0;
		}
	}
}
