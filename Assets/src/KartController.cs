using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System;

// this class manages the mouvements

public class KartController : MonoBehaviour
{	
	public static bool stop = true;
	public float speedCoeff;
	public float turnCoeff;
	public float coeffInitSpeed;
	public Dictionary <string, float> speedDuration = new Dictionary <string,  float>();
	public Dictionary <string, float> speedToAdd = new Dictionary <string,  float>();
    private bool isGoingInAir = false;
	private bool inAirAfterJump=true;

	private Vector3 postForce;
	private Vector3 lowForce;
	public Vector3 forwardNormal;

	private Kart kart;
	private KartScript kart_script;

	public bool dansLesAirs = true;
	public Dictionary <string, Transform> wheels = new Dictionary <string, Transform>();
	private List<GameObject> smoke = new List<GameObject>();
	private float ky;

	private float ellapsedTime = 0f;
	private float currentTime = 0f;

	private float yTurn;
	public static bool IA_enabled = false;
	private bool forward = false;
	private bool backward = false;

	public int weaponSize = 1;
	private static int nControllers;

	private float accelerationTime = 0f;
	
	private float twTime = 0f;
	private float twLerp = 0f;

	private float twTimeWheels = 0f;
	private float twLerpWheels = 0f;

	public int numberOfJump = 0;
	private ControllerBase controller;
	
	// Use this for initialization
	void Start ()
	{
        coeffInitSpeed = speedCoeff;

		foreach (Transform child in transform)
		{
			if (child.name == "kartSmoke")
			{
				smoke.Add(child.gameObject);
				continue;
			}
			if (child.name != "steering")continue;
			wheels["steering"] = child;
			foreach (Transform w in child.transform)
				wheels[w.name] = w;
		}
		
		kart_script = GetComponent <KartScript> ();

		controller = ControllerManager.Instance.GetController(kart.numeroJoueur - 1);
	}
	
	void Update()
	{
		if (IA_enabled)
			return;
		yTurn = 0;
		if (Time.timeScale == 0)
			return;
		if (!stop && kart_script.AbleToMove())
		{
			forwardNormal = wheels["steering"].forward;
			//forwardNormal = transform.forward;
			forwardNormal.y = 0;
			forwardNormal = normalizeVector (forwardNormal);
			
			if (IA_enabled)
			{}
			//controlIA();
			else
			{
				controle ();
				/*
				if (hasAxis)
					controlPosition ();
				else
					controlKeyboard ();*/
			}
		}
		if (!IA_enabled)
			controlCamera ();
	}
	
	void FixedUpdate()
	{
	}

	float Slerp(float a, float b, float f)
	{
		return a + f*f * (b - a);
	}
	
	float Lerp(float a, float b, float f)
	{
		return a + f * (b - a);
	}

	void controlWheels(){
		float yTurnWheel = 0f;
		if(controller.GetKey("left"))
			yTurnWheel = -controller.GetAxis("left");
		else if(controller.GetKey("right"))
			yTurnWheel = controller.GetAxis("right");
		if (System.Math.Abs (yTurnWheel) < Game.Instance.thresholdAxis)
			yTurnWheel = 0;

		// WHEELS
		if (yTurnWheel==0){
			if (System.Math.Abs (twTimeWheels) < 0.1f)
				twTimeWheels = 0;
			if (twTimeWheels>0)
				twTimeWheels -= ellapsedTime;
			else if (twTimeWheels<0)
				twTimeWheels += ellapsedTime;
		}
		else{
			twTimeWheels += yTurnWheel*ellapsedTime;
		}
		
		twTimeWheels = System.Math.Max (twTimeWheels,-0.25f);
		twTimeWheels = System.Math.Min (twTimeWheels,0.25f);

		// STEERING WHEEL
		if (yTurnWheel==0 || System.Math.Abs(GetComponent<Rigidbody>().velocity.magnitude) < 1f){
			if (System.Math.Abs (twTime) < 0.01f)
				twTime = 0;
			if (twTime>0)
				twTime -= ellapsedTime;
			else if (twTime<0)
				twTime += ellapsedTime;
		}
		else{
			twTime += yTurnWheel*ellapsedTime;
		}

		twTime = System.Math.Max (twTime,-0.25f);
		twTime = System.Math.Min (twTime,0.25f);


		twLerpWheels = Lerp (0, 160f, twTimeWheels);
		twLerp = Lerp (0, 45f, twTime);


		Vector3 currentRot = new Vector3(wheels ["steering"].localRotation.eulerAngles.x,twLerp, wheels ["steering"].localRotation.eulerAngles.z);
		wheels ["steering"].localRotation = Quaternion.Euler (currentRot);
		wheels ["wheelAL"].localRotation = Quaternion.Euler (new Vector3 (0, 90f + twLerpWheels));
		wheels ["wheelAR"].localRotation = Quaternion.Euler (new Vector3 (0, 90f + twLerpWheels));
	}

	void OnCollisionStay(Collision collision)
	{
		if(collision.gameObject.name=="Ground"){
			dansLesAirs = false;
			isGoingInAir = false;
			//Debug.Log("ground");
		}
		
		/*if(collision.gameObject.name=="accelerateur")
			rigidbody.velocity = new Vector3(rigidbody.velocity.x*3,rigidbody.velocity.y*3,rigidbody.velocity.z*3);*/
	}
	
	void OnCollisionExit(Collision collision)
	{
		if(collision.gameObject.name=="Ground") {
			if (!isGoingInAir){
				dansLesAirs = true;
				//Debug.Log("air");
			}
		}
	}

	public void setCoefficients(float speed, float turn){
		speedCoeff = speed;
		turnCoeff = turn;
	}
	
	public void AddSpeed(float duration, float addSpeed, string weapon)
	{
		if (speedDuration.ContainsKey(weapon) == false)
			speedDuration [weapon] = 0f;
		speedDuration [weapon] += duration;
		speedToAdd [weapon] = addSpeed;
	}

	void CheckSpeed()
	{
		speedCoeff = coeffInitSpeed;

		string [] copy = new string[speedDuration.Keys.Count];
		speedDuration.Keys.CopyTo(copy, 0);
		bool fire = false;

		foreach(string key in copy)
		{
			if (speedDuration [key] > 0f){
				if (key == "turbo")
					fire = true;
				speedDuration [key] -= ellapsedTime;
				speedCoeff *= speedToAdd[key];
			}
		}
		if (fire){
			foreach(GameObject w in smoke)
				w.GetComponent<ParticleRenderer>().material.SetColor ("_TintColor", Color.red);
		}
		else{
			float rgb = 0.1f;
			if (GetComponent<Rigidbody>().velocity.magnitude>1)
				rgb = 0.05f;
			else if (controller.GetKey("validate"))
				rgb = 0.5f;
			Color smokeColor = new Color(rgb,rgb,rgb);
			foreach(GameObject w in smoke)
				w.GetComponent<ParticleRenderer>().material.SetColor ("_TintColor", smokeColor);
		}
    }
	
	public Vector3 normalizeVector(Vector3 a)
	{
		float div = Mathf.Sqrt (a.x * a.x + a.y * a.y + a.z * a.z);
		a.x /= div;
		a.y /= div;
		a.z /= div;
		return a;
	}
	
	public void controle()
	{
		// wheels turning
		if ((controller.GetKey("stop") || controller.GetKey("validate")))
		{
			if(controller.GetKey("right"))
				yTurn = 0.5f*controller.GetAxis("right") * turnCoeff;
			else if(controller.GetKey("left"))
				yTurn = -0.5f*controller.GetAxis("left") * turnCoeff;
		}

		if (controller.GetKey("stop"))
	    {}
		else if(controller.GetKey("validate"))
		{
			postForce = forwardNormal*speedCoeff;
			forward = true;
		}
		else if(controller.GetKey("down"))
		{
			lowForce = -controller.GetAxis("down") * forwardNormal * speedCoeff;
			backward = true;
			if(controller.GetKey("right"))
				yTurn = -0.5f*controller.GetAxis("right") * turnCoeff;
			else if(controller.GetKey("left"))
				yTurn = 0.5f*controller.GetAxis("left") * turnCoeff;
		}
		
		if(controller.GetKeyDown("jump"))
		{
			if(dansLesAirs == false)
			{
				if (GetComponent<Rigidbody>().velocity.magnitude < 5 && inAirAfterJump)
				{
					GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + new Vector3(0,1.75f,0));
				}
				else 
				{
					float high = transform.localRotation.eulerAngles.x;
					if (high > 180)
						high = System.Math.Abs(high-360);
					else 
						high = 0;

					//Debug.Log("Angle : "+high);
					high = System.Math.Min(high, 9f);
					high = System.Math.Max(high, 1.75f);
					GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + new Vector3(0,high,0));
				}
				
				if (kart_script.tnt)
					numberOfJump++;
			}
		}

		if(controller.GetKey("jump") )//&& System.Math.Abs(Vector3.Angle(rigidbody.velocity,forwardNormal))>45)
		{
			//Debug.Log("Angle : "+transform.localRotation.eulerAngles.x+","+transform.localRotation.eulerAngles.z);
			//rigidbody.velocity = new Vector3(0,-26f,0);
		}

	}
	
	public void controlCamera()
	{
		if (controller.GetKeyDown ("inverseCamera")) 
		{
			kart.cm1c.Reversed = -1f ;
		}

		if (controller.GetKeyUp ("inverseCamera")) 
		{
			kart.cm1c.Reversed = 1f ;
		}

		if (controller.GetKeyDown ("switchCamera")) 
		{
			if (kart.cm1c.PositionForward == 1f)
				kart.cm1c.PositionForward = 0.85f ;
			else
				kart.cm1c.PositionForward = 1f ;
		}
	}
}
