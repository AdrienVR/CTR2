using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System;

// this class manages the mouvements

public class KartControllerRay : MonoBehaviour
{	
	public static bool stop = true;
	public float speedCoeff = 20;
	public float turnCoeff = 45;
	public float timeMaxAcceleration = 1;

    private ControllerBase controller;

	private Vector3 postForce;

	// Use this for initialization
	void Start ()
	{
		controller = ControllerManager.Instance.GetController(0);
	}

	public void UpdateGameplay()
	{
		Vector3 localForward = Vector3.forward;
		if (controller.GetKey("stop"))
		{}
		else if(controller.GetKey("validate"))
		{
			m_accelerationTime += 2 * Time.deltaTime;
			postForce = localForward*speedCoeff;
		}
		else if(controller.GetKey("down"))
		{
			m_accelerationTime += 2 * Time.deltaTime;
			postForce = -localForward*speedCoeff;
		}
		// wheels turning
		//if ((controller.GetKey("stop") || controller.GetKey("validate")))
		{
			if(controller.GetKey("right"))
			{
				m_angle += 1;
			}
			else if(controller.GetKey("left"))
			{
				m_angle -= 1;
			}
		}
		
		if(controller.GetKey("jump") )//&& System.Math.Abs(Vector3.Angle(rigidbody.velocity,forwardNormal))>45)
		{
			//Debug.Log("Angle : "+transform.localRotation.eulerAngles.x+","+transform.localRotation.eulerAngles.z);
			//rigidbody.velocity = new Vector3(0,-26f,0);
		}
		
	}

	private float m_angle;
	
	void FixedUpdate()
	{

		Debug.DrawRay(transform.position, -transform.up * 2);
		RaycastHit hit;
		if (Physics.Raycast(transform.position, -transform.up, out hit, 2f)){
			transform.up = Vector3.Slerp(transform.up, hit.normal, 0.05f);
		}
		transform.Rotate(Vector3.up, m_angle);

		m_accelerationTime -= Time.deltaTime;
		UpdateGameplay();
		transform.Rotate(Vector3.up, m_angle);

		m_accelerationTime = Mathf.Clamp(m_accelerationTime, 0, timeMaxAcceleration);

		if (m_accelerationTime > 0)
			GetComponent<Rigidbody>().velocity = Vector3.Slerp(Vector3.zero,postForce,m_accelerationTime/timeMaxAcceleration);
	}

	private float m_accelerationTime;
}
